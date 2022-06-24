using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class FigherUpgradeShieldRegeneration : WeaponUpgrade 
	{
		public override string UpgradeName { get; set; } = "Solar Charging";
		public override string Description { get; set; } = "At the start of every regenerate a shield.";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair002.png";
		public override float Rarity { get; set; } = 1;
	}
}
