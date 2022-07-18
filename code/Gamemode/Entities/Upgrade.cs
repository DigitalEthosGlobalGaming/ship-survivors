using Degg.Entities;
using Degg.Util;
using Sandbox;
using System;
using System.Collections.Generic;

namespace ShipSurvivors
{


	public partial class Upgrade : Entity 
	{

		public static string[] Upgrades { get; set; } =
		{
			"",""
		};

		[Net]
		public bool IsSpecial { get; set; }
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
		public UpgradeResource Resource { get; set; }

		[Net]
		public bool Active { get; set; }

		public virtual void ServerTick()
		{

		}

		public void ClientTick()
		{

		}

		public override void Spawn()
		{
			base.Spawn();
			ClientOrServerSpawn();
			Transmit = TransmitType.Owner;
			Level = 1;
			Active = false;
			
		}

		public void LoadResource()
		{
			Resource = UpgradeResource.GetResourceForUpgrade( this );
			if ( Resource != null )
			{
				Image = "/" + Resource.Image;
				UpgradeName = Resource.UpgradeName;
				Description = Resource.Description;
			}
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();
			ClientOrServerSpawn();
		}

		public virtual void ClientOrServerSpawn()
		{
			LoadResource();
		}

		public virtual void PrimaryAttack()
		{
			OnPrimaryAttack();
		}

		public virtual void OnPrimaryAttack()
		{

		}

		public virtual void SecondaryAttack()
		{
			OnSecondaryAttack();
		}

		public virtual void OnSecondaryAttack()
		{

		}

		public virtual void Special()
		{
			OnSpecial();
		}

		public virtual void OnSpecial()
		{

		}
		public virtual List<UpgradeResource> GetChildrenUpgradeResources()
		{
			var results = new List<UpgradeResource>();
			if ( Resource?.ChildrenUpgrades != null )
			{
				foreach ( var item in Resource?.ChildrenUpgrades )
				{
					var res = UpgradeResource.Get( item );
					if (res != null)
					{
						results.Add( res );
					}
				}
			}

			return results;
		}
		public virtual List<string> GetUpgradeClassNames()
		{
			var children = GetChildrenUpgradeResources();
			var results = new List<string>();

			foreach ( var item in children )
			{
				results.Add( item.ClassName );
			}

			return results;
		}



		public virtual List<Upgrade> GetUpgrades()
		{
			List<Upgrade> upgrades = new List<Upgrade>();
			var classNames = GetUpgradeClassNames();
			foreach(var className in classNames)
			{
				try
				{
					var entity = CreateByName<Upgrade>( className );
					if ( !(entity.ParentUpgrade?.IsValid() ?? false) )
					{
						entity.ParentUpgrade = this;
					}

					upgrades.Add( entity );
				} catch(Exception)
				{
					Log.Warning( "Error loading upgrade " + className );
				}
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

		public virtual void OnRoundStart()
		{

		}

		public virtual void OnRoundEnd()
		{

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
