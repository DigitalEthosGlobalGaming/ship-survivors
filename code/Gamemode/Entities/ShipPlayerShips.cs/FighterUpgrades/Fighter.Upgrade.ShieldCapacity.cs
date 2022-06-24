using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class FigherUpgradeShieldCapacity : WeaponUpgrade 
	{
		public override string UpgradeName { get; set; } = "Flux Capacitors";
		public override string Description { get; set; } = "Increase max shield capacity";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair181.png";
		public override float Rarity { get; set; } = 1;
		public override void OnOwnerStatsUpdate()
		{
			base.OnOwnerStatsUpdate();
			var player = GetShipPlayer();
			if ( player?.IsValid() ?? false )
			{
				player.MaxHealth = player.MaxHealth + (10 * this.Level);
			}
		}
	}
}
