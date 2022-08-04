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

			var Ships = ShipResource.GetAll();
			ShipResource selectedShip = null;
			foreach ( var ship in Ships )
			{
				if (ship.ResourcePath == SelectedShip)
				{
					selectedShip = ship;
				}
			}

			var player = ClientUtil.GetCallingPawn<GameLoadingPawn>();
			if ( selectedShip?.ShipClassName != null && (player?.IsValid() ?? false) )
			{
				player.EntityName = selectedShip.ShipClassName;
				var ent = player.OnJoin();
				if ( ent is ShipPlayer playerShip)
				{
					playerShip.Init(selectedShip);
				}

			}
		}
	}
}
