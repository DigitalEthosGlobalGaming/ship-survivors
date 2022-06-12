using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class AttackSpeedUpgrade : Upgrade
	{
		public override void Spawn()
		{
			base.Spawn();

		}	

		public override void OnOwnerStatsUpdate() {
			var p = GetShipPlayer();
			p.AttackSpeed = p.AttackSpeed + 0.1f;
		}

	}
}
