using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class MaxHealthUpgrade : Upgrade
	{
		public override void OnOwnerStatsUpdate()
		{
			var p = GetShipPlayer();
			p.MaxHealth = p.MaxHealth + 10f;
		}
	}
}
