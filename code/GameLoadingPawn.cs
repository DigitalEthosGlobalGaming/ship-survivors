using Degg;
using Degg.Entities;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipSurvivors
{
	public partial class GameLoadingPawn: DeggLoadingPawn
	{

		public override void HudSetup()
		{
			base.HudSetup();
			Hud.AddPanel<ShipSelectorPanel>();
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
