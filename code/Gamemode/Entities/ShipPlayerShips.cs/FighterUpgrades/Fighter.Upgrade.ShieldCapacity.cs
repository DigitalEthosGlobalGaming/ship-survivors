using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class FigherUpgradeShieldCapacity : WeaponUpgrade 
	{
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
