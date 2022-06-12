
namespace ShipSurvivors
{
	public partial class ShipPlayer
	{
		public override void HudSetup()
		{
			base.HudSetup();
			Hud.AddPanel<UpgradePanel>();
		}
	}
}
