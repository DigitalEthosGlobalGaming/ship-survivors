using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelleteWeaponUpgradeSideGunner : WeaponUpgrade 
	{

		public override string ParentUpgradeClassName { get; set; } = "PelletWeapon";
		public override string UpgradeName { get; set; } = "Side Gunner";
		public override string Description { get; set; } = "On fire 25% Chance to fire another bullet at a random enemy.";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair080.png";
		public override float Rarity { get; set; } = 1;
	}
}
