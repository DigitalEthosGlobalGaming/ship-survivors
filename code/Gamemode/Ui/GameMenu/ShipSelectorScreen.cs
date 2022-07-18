using Degg.Ui;
using Degg.UI.Elements;
using Degg.UI.Elements.SimpleGameMenu;

namespace ShipSurvivors
{
	public class ShipSelectorScreen : SimpleGameMenuScreen
	{

		public ShipSelectorScreen(): base() { 
			StyleSheet.Load( "/Gamemode/Ui/styles.scss" );
			StyleSheet.Load( "/Gamemode/Ui/UpgradePanel.scss" );
			var Ships = ShipResource.GetAll();			

			foreach (var ship in Ships)
			{
				var button = AddChild<DeggCard>( "ship-button" );
				button.SetClass( "ship-button active", ship.Active );
				button.Image.SetImage( ship.Image );
				button.Image.AddClass( "ship-selector" );
				button.Header.SetText( ship.ShipName );
				button.Body.SetText( ship.Description );
				if ( ship.Active )
				{
					button.Inner.AddEventListener( "OnClick", () =>
					{
						Sandbox.ConsoleSystem.Run( "ss.client.loaded", ship.ResourcePath );
					} );
				} else
				{
					button.Header.SetText( "Unidentified" );
					button.Body.SetText( "Coming soon" );
				}
			}
		}
	}
}
