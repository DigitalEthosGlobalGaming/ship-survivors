using Degg;
using Degg.Ui;
using Degg.UI.Elements.SimpleGameMenu;

namespace ShipSurvivors
{

	public partial class EndGamePanel: SimpleGameMenu
	{

		EndGamePanelScore ScoreMenuItem { get; set; }
		SimpleGameMenuButtonItem ReadyMenuItem { get; set; }
		public EndGamePanel() :base()
		{
			StyleSheet.Load( "/Gamemode/Ui/styles.scss" );

			SetTitle( "Ship Destroyed" );
		}


		public override void SetupMenuItems()
		{
			base.SetupMenuItems();

			ScoreMenuItem = AddMenuItem<EndGamePanelScore>( "Your Score" );
			ScoreMenuItem.AddClass( "game-menu-item" );

			ReadyMenuItem = AddMenuItem<SimpleGameMenuButtonItem>( "New Game", () =>
			{
				Sandbox.ConsoleSystem.Run( "ss.client.ready" );
				ReadyMenuItem.SetText( "Ready" );
			} );
		}

		public override void Tick()
		{
			base.Tick();

			var deadPlayer = ClientUtil.GetPawn<DeadPlayerPawn>();
			if ( deadPlayer != null )
			{
				ScoreMenuItem.SetText( $"Rounds Survived: {deadPlayer.RoundsSurvived}" );
			}

		}
	}
}
