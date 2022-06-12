
using Degg;
using Degg.Util;
using Sandbox;
using System.Collections.Generic;

namespace ShipSurvivors
{
	public partial class ShipPlayer
	{
		public float Damage { get; set; }
		public float MovementSpeed { get; set; }
		public float AttackSpeed { get; set; }
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
			
			for ( int i = 0; i < UpgradeAmount; i++ )
			{
				var newUpgrade = Rand.FromList(upgrades);
				newUpgrade.Owner = this;
				newUpgrade.Active = true;

				UpgradesToBuy.Add( newUpgrade );
			}
		}

		public virtual List<Upgrade> GetUpgradesToBuy()
		{
			var upgrades = new List<Upgrade>();

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
		public Upgrade BuyUpgrade(string className)
		{
			var entity = CreateByName( className );
			if ( entity is Upgrade upgrade)
			{
				return BuyUpgrade( upgrade );
			}

			return null;
		}

		public void UpdateStats()
		{
			if ( Upgrades  == null)
			{
				Upgrades = new List<Upgrade>();
			}

			Damage = 1;
			MaxHealth = 100;
			MovementSpeed = 1;
			TurnSpeed = 1f;
			MaxSpeed = 50f;
			AttackSpeed = 0;
			UpgradeAmount = 5;

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
