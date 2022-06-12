using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class ShipSpeedUpgrade : Upgrade
	{
		public override void Spawn()
		{
			base.Spawn();

		}
		public override void OnOwnerStatsUpdate()
		{
			var p = GetShipPlayer();
			p.MovementSpeed = p.MovementSpeed + 0.1f;
		}
	}
}
