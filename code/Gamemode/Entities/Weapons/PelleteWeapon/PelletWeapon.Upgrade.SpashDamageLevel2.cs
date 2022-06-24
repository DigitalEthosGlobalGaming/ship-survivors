using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeBulletSplashDamageLevel2 : WeaponUpgrade 
	{

		public override string ParentUpgradeClassName { get; set; } = "PelletWeaponUpgradeBulletSplashDamage";
		public override string UpgradeName { get; set; } = "Explosive Fragments";
		public override string Description { get; set; } = "Extra damage from fragmenting shells.";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair048.png";
		public override float Rarity { get; set; } = 1;
	}
}
