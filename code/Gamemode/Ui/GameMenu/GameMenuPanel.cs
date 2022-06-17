using Degg.Ui;
using Degg.UI.Elements.SimpleGameMenu;

namespace ShipSurvivors
{

	public partial class GameMenuPanel: SimpleGameMenu
	{

		public GameMenuPanel() :base()
		{
			StyleSheet.Load( "/Gamemode/Ui/styles.scss" );
		}

		public override void SetupMenuItems()
		{
			base.SetupMenuItems();
			SetTitle( "Sky Survivors" );
			AddMenuItemScreen<ShipSelectorScreen>( "Play" ).AddClass( "game-menu-item" );
			AddMenuItemScreen<HelpScreen>( "Help", () =>
			{
				Log.Info( "Yo" );
			} ).AddClass( "game-menu-item" );
			AddMenuItemScreen<CreditsScreen>( "Credits" ).AddClass( "game-menu-item" );
		}
	}
}
