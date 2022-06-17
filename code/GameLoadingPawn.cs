using Degg;
using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class GameLoadingPawn: DeggLoadingPawn
	{

		public override void HudSetup()
		{
			base.HudSetup();
			Hud.AddPanel<GameMenuPanel>();
		}

		[ConCmd.Server( "ss.client.loaded" )]
		public static void OnLoad(string SelectedShip)
		{
			var player = ClientUtil.GetCallingPawn<GameLoadingPawn>();
			if ( player?.IsValid() ?? false )
			{
				player.EntityName = SelectedShip;
				player.OnJoin();
			}
		}
	}
}
