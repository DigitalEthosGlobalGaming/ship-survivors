using Degg.Ui;
using Degg.UI.Elements.SimpleGameMenu;
using Sandbox.UI;

namespace ShipSurvivors
{
	public partial class HelpScreen: SimpleGameMenuScreen
	{

		public HelpScreen():base()
		{
			AddClass( "help-screen" );
			var label = AddChild<Label>("help-header");
			label.SetText( "How to play" );

			AddChild<Label>( "help-text" ).SetText( "Welcome to Sky Survivors" );
			AddChild<Label>( "help-text" ).SetText( "Every wave you will get an upgrade to choose from." );
			AddChild<Label>( "help-text" ).SetText( "Survive for as long as possible." );
			AddChild<Label>( "help-text" ).SetText( "" );

			AddChild<Label>( "help-text" ).SetText( "ASDW keys to move" );
			AddChild<Label>( "help-text" ).SetText( "Use the mouse to aim and left click to shoot" );

			AddChild<Label>( "help-text" ).SetText( "Back button is in the top left corner." );

		}
	}
}
