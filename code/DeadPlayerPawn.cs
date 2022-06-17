using Degg;
using Degg.Core;
using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class DeadPlayerPawn: DeggLoadingPawn
	{

		[Net]
		public float RoundsSurvived { get; set; }
		public bool ReadyToReset { get; set; }
		public void Reset()
		{
			Delete();
			Client.Pawn = CreateByName( "GameLoadingPawn" );
		}



		public override void HudSetup()
		{
			base.HudSetup();
			Hud.AddPanel<EndGamePanel>();
		}

		[ConCmd.Server( "ss.client.ready" )]
		public static void OnReady()
		{
			var player = ClientUtil.GetCallingPawn<DeadPlayerPawn>();
			if ( player?.IsValid() ?? false )
			{
				player.ReadyToReset = true;
			}
		}
	}
}
