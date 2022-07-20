
using Degg;
using Degg.Core;
using Degg.Util;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipSurvivors
{
	public partial class ShipPlayer
	{
		public float Damage { get; set; }
		public float MovementSpeed { get; set; }
		public float AttackSpeed { get; set; }
		[Net]
		public float MaxHealth { get; set; }
		public float UpgradeAmount { get; set; }

		[Net, Change]
		public List<Upgrade> Upgrades { get; set; }

		[Net, Change]
		public List<Upgrade> UpgradesToBuy { get; set; }

		public void OnUpgradesToBuyChanged( List<Upgrade> before, List<Upgrade> next )
		{
			UpgradesToBuy = next;
			Event.Run( "ss.upgrades-to-buy.change" );
		}


		public float GetUpgradeLevel(string className)
		{
			var upgrades = GetUpgrades();
			float level = 0;

			foreach(var i in upgrades)
			{
				if (i.ClassName == className )
				{
					level = level + i.Level;
				}
			}
			return level;
		}

		public bool HasUpgrade(string className)
		{
			return GetUpgradeLevel( className ) > 0;
		}
		public void OnUpgradesChanged( List<Upgrade> before, List<Upgrade> next)
		{
			Upgrades = next;
			Event.Run( "ss.upgrades.change" );
		}

		public void DeleteNonActiveUpgrades()
		{
			if ( UpgradesToBuy != null )
			{
				foreach(var upgrade in UpgradesToBuy )
				{
					if ( upgrade.Owner == null )
					{
						upgrade.Delete();
					}
				}
			}
			UpgradesToBuy = new List<Upgrade>();
		}

		public void GiveRandomUpgrades()
		{
			DeleteNonActiveUpgrades();
			var upgrades = GetUpgradesToBuy();
			if ((upgrades?.Count ?? 0) == 0)
			{
				return;
			}
			var amountAdded = UpgradeAmount;
			var maxTries = 1000;

			var newUpgrades = new List<Upgrade>(UpgradesToBuy);
			while ( amountAdded > 0 && maxTries > 0 )
			{
				maxTries = maxTries - 1;
				var newUpgrade = Rand.FromList( upgrades );
				newUpgrade.Owner = this;
				if ( newUpgrade.CanBuyUpgrade() )
				{
					var isExistingUpgrade = newUpgrades.Find( ( item ) => item.ClassName == newUpgrade.ClassName )?.IsValid ?? false;

					if ( !isExistingUpgrade )
					{
						amountAdded = amountAdded - 1;
						newUpgrade.Owner = this;
						newUpgrade.Active = true;
						newUpgrades.Add( newUpgrade );
					}
				}
			}

			UpgradesToBuy = newUpgrades;


		}

		public virtual List<Upgrade> GetUpgradesToBuy()
		{
			var upgrades = new List<Upgrade>();
			var classNames = GetUpgradeClassNames();
			foreach ( var className in classNames )
			{
				try
				{
					var entity = CreateByName<Upgrade>( className );
					if ( !(entity.ParentUpgrade?.IsValid() ?? false) )
					{
						entity.ParentUpgrade = null;
					}

					upgrades.Add( entity );
				} catch(Exception e)
				{
					Log.Warning("Error loading " + className );
					Log.Warning( e );
				}
			}

			foreach ( var upgrade in Upgrades )
			{
				var upgradeList = upgrade.GetUpgrades();

				foreach ( var item in upgradeList )
				{
					upgrades.Add( item );
				}
			}
			
			return upgrades;
		}

		[ConCmd.Server( "ss.upgrades.buy", Help = "Buys the upgrade" )]
		public static void BuyUpgradeConCommand( string ClassName )
		{
			var player = ClientUtil.GetCallingPawn<ShipPlayer>();
			if ( player?.IsValid() ?? false )
			{
				var playerUpgrades = player.GetUpgrades();
				var upgradesToBuy = new List<Upgrade>( player.UpgradesToBuy );

				var isValidUpgrade = upgradesToBuy.Exists( ( item ) => item.ClassName == ClassName);

				if ( isValidUpgrade )
				{
					player.BuyUpgrade( ClassName );
				}
			}

			MyGame.GetRoundManager().CheckRoundStart();
		}

		public List<Upgrade> GetUpgrades()
		{
			List<Upgrade> upgrades = new List<Upgrade>(Upgrades);
			return upgrades;
		}

		public Upgrade GetUpgradeByClassName(string className)
		{
			var upgrades = GetUpgrades();
			foreach(var upgrade in upgrades)
			{
				if (upgrade.ClassName == className)
				{
					return upgrade;
				}
			}

			return null;
		}

		public List<ShipWeapon> GetWeapons()
		{
			List<ShipWeapon> weapons = new List<ShipWeapon>( );
			var upgrades = GetUpgrades();

			foreach(var upgrade in upgrades )
			{
				if (upgrade is ShipWeapon weapon)
				{
					weapons.Add( weapon );
				}
			}

			return weapons;
		}

		public Upgrade BuyUpgrade(Upgrade upgrade)
		{

			var existingUpgrade = GetUpgrades().Find( ( item ) =>
			{
				return item.ClassName == upgrade.ClassName;
			});

			if ( existingUpgrade?.IsValid() ?? false)
			{
				existingUpgrade.Level = existingUpgrade.Level + 1;
				DeleteNonActiveUpgrades();
				UpdateStats();

				return existingUpgrade;
			}
			upgrade.Active = true;
			upgrade.Owner = this;
			upgrade.Equip(this );

			DeleteNonActiveUpgrades();
			UpdateStats();

			return upgrade;
		}

		public Upgrade BuyUpgrade<T>() where T: Upgrade, new()
		{
			var upgrade = new T();
			return BuyUpgrade( upgrade );
		}

		public Upgrade BuyUpgrade(UpgradeResource upgrade)
		{
			if ( upgrade != null )
			{
				return BuyUpgrade( upgrade.ClassName );
			} else
			{
				Log.Warning( "Error trying to buy upgrade " + upgrade );
				return null;
			}
		}
		public Upgrade BuyUpgrade(string className)
		{
			var entity = CreateByName( className );
			if ( entity is Upgrade upgrade)
			{
				return BuyUpgrade( upgrade );
			}

			return null;
		}

		public void OnEnemyDamaged(Entity source, EnemyShip enemy, bool didKill)
		{
			if ( Upgrades == null )
			{
				Upgrades = new List<Upgrade>();
			}
			
			foreach ( var upgrade in Upgrades )
			{
				if ( upgrade?.IsValid() ?? false )
				{
					if ( upgrade.Active )
					{
						upgrade.OnEnemyDamaged( source, enemy, didKill );
					}
				}
			}
		}

		public void UpdateStats()
		{
			if ( Upgrades  == null)
			{
				Upgrades = new List<Upgrade>();
			}

			Damage = 0.5f;
			if ( DeggGame.IsDevelopment() )
			{
				MaxHealth = 1000;
				Health = 1000f;
			} else
			{
				MaxHealth = 10;
				Health = 10f;
			}
			MovementSpeed = 1;
			TurnSpeed = 1f;
			MaxSpeed = 50f;
			AttackSpeed = 0;
			UpgradeAmount = 3;

			foreach ( var upgrade in Upgrades )
			{
				if ( upgrade?.IsValid() ?? false )
				{
					if ( upgrade.Active )
					{
						upgrade.Owner = this;
						upgrade.ResetStats();
					}
				}
			}

			foreach (var upgrade in Upgrades)
			{
				if ( upgrade?.IsValid() ?? false )
				{
					if ( upgrade.Active )
					{
						upgrade.Owner = this;
						upgrade.OnOwnerStatsUpdate();
					}
				}
			}
		}		
	}
}
