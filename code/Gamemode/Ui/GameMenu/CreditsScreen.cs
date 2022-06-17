using Degg.Ui;
using Degg.UI.Elements.SimpleGameMenu;
using Sandbox.UI;

namespace ShipSurvivors
{
	public partial class CreditsScreen: SimpleGameMenuScreen
	{

		public CreditsScreen():base()
		{
			AddClass( "help-screen" );
			var label = AddChild<Label>("help-header");
			label.SetText( "Credits" );

			AddChild<Label>( "help-text" ).SetText( "Thank you for playing." );
			AddChild<Label>( "help-text" ).SetText( "// Digital Ethos Global Gaming" );
			AddChild<Label>( "help-text" ).SetText( "DEGG = {};" );
			AddChild<Label>( "help-text" ).SetText( "DEGG.Developers.Lead = 'stinkfire';" );

		}
	}
}
