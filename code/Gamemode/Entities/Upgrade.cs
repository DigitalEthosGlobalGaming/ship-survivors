using Degg.Entities;
using Degg.Util;
using Sandbox;
using System.Collections.Generic;

namespace ShipSurvivors
{

	public partial class UpgradeStoreItem {
		public string Name { get; set; }
		public string UpgradeClassName { get; set; }
		public string Description { get; set; }
		public float Rarity { get; set; }
		public string Image { get; set; }

	

		public static UpgradeStoreItem CreateItem(string name, string className, string description, float rarity, string image = "" )
		{
			var item = new UpgradeStoreItem();

			item.Name = name;
			item.UpgradeClassName = className;
			item.Description = description;
			item.Image = image;
			item.Rarity = rarity;
			return item;
		}

		public Upgrade Create()
		{
			var upgrade = TypeLibrary.Create<Upgrade>( UpgradeClassName );
			upgrade.Name = Name;
			upgrade.Description = Description;
			upgrade.Rarity = Rarity;
			upgrade.Image = Image;

			return upgrade;
		}

	}

	public partial class Upgrade : Entity 
	{
		public static string[] Upgrades =  {

			"degg/models/simple/platform_1x1.vmdl",
			"degg/models/simple/platform_1x1_thin.vmdl",
			"degg/models/simple/platform_2x1.vmdl",
			"degg/models/simple/platform_2x1_thin.vmdl",
			"degg/models/simple/platform_2x2.vmdl",
			"degg/models/simple/platform_2x2_thin.vmdl"
		};

		[Net]
		public Upgrade ParentUpgrade { get; set; }

		[Net]
		public string Image { get; set; }
		[Net]
		public string Description { get; set; }
		[Net]
		public float Rarity { get; set; }

		[Net]
		public string ParentUpgradeClassName { get; set; }

		[Net]
		public float Level { get; set; }

		[Net]
		public bool Active { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Owner;
			Rarity = 1;
			Level = 1;
			Description = "Upgrade";
			Image = "";
			Active = false;
		}


		public virtual void Fire()
		{
			OnFire();
		}

		public virtual void OnFire()
		{

		}

		public virtual List<Upgrade> GetUpgrades()
		{
			return new List<Upgrade>();
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


	}
}
