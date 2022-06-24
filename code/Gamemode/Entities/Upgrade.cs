using Degg.Entities;
using Degg.Util;
using Sandbox;
using System.Collections.Generic;

namespace ShipSurvivors
{
	
	public partial class Upgrade : Entity 
	{
		public static string[] Upgrades { get; set; } =
		{
			"",""
		};
		public virtual float Rarity { get; set; } = 1;
		public virtual string Image { get; set; } = "";
		public virtual string UpgradeName { get; set; } = "";
		public virtual string Description { get; set; } = "";
		public virtual string ParentUpgradeClassName { get; set; } = "";

		[Net]
		public Upgrade ParentUpgrade { get; set; }

		[Net]
		public float Level { get; set; }

		[Net]
		public bool Active { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			ClientOrServerSpawn();
			Transmit = TransmitType.Owner;
			Level = 1;
			Active = false;
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();
			ClientOrServerSpawn();
		}

		public virtual void ClientOrServerSpawn()
		{

		}


		public virtual void Fire()
		{
			OnFire();
		}

		public virtual void OnFire()
		{

		}

		public virtual string[] GetUpgradeClassNames()
		{
			return new string[0];
		}

		public virtual List<Upgrade> GetUpgrades()
		{
			List<Upgrade> upgrades = new List<Upgrade>();
			var classNames = GetUpgradeClassNames();
			foreach(var className in classNames)
			{
				var entity = CreateByName<Upgrade>( className );
				if ( !(entity.ParentUpgrade?.IsValid() ?? false))
				{
					entity.ParentUpgrade = this;
				}

				upgrades.Add( entity );
			}

			return upgrades;
		}


		public virtual T GetFirstParentUpgrade<T>() where T: Upgrade
		{
			var parent = GetParentUpgrade();
			if (parent == null)
			{
				return null;
			}
			if (parent is T t)
			{
				return t;
			}
			return parent.GetFirstParentUpgrade<T>();
		}
		public ShipPlayer GetShipPlayer()
		{
			if (Owner is ShipPlayer player)
			{
				if ( player?.IsValid() ?? false )
				{
					return player;
				}
			}
			return null;
		}

		public virtual bool CanBuyUpgrade()
		{
			return true;
		}


		public void Equip(ShipPlayer player)
		{
			Owner = player;
			this.Parent = player;
			if (!player.Upgrades.Contains(this))
			{
				player.Upgrades.Add( this );
			}

			player.UpdateStats();
		}
		public void Unequip()
		{
			var player = GetShipPlayer();
			if (!(player?.IsValid() ?? false))
			{
				return;
			}
			player.Upgrades.Remove( this );
			Owner = null;
			player.UpdateStats();
			Delete();
		}

		public virtual void ResetStats()
		{

		}

		public virtual void OnOwnerStatsUpdate()
		{

		}


		public Upgrade GetParentUpgrade()
		{
			if (ParentUpgrade?.IsValid() ?? false)
			{
				return ParentUpgrade;
			}
			if (GetShipPlayer() == null)
			{
				return null;
			}

			return GetShipPlayer()?.GetUpgradeByClassName(ParentUpgradeClassName ?? "") ?? null;
		}
		public virtual void OnEquip() { }

		public virtual void OnUnEquip() { }

		public override string ToString()
		{
			return $"{Name}";
		}

		[ClientRpc]
		public void PlaySoundOnClient( string name )
		{
			PlaySound( name );
		}

		public virtual void OnEnemyDamaged( Entity b, EnemyShip e, bool didKill )
		{

		}
	}
}
