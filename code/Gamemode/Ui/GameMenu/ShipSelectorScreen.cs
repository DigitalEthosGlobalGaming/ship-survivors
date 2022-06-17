using Degg.Ui;
using Degg.UI.Elements;
using Degg.UI.Elements.SimpleGameMenu;
using Sandbox.UI;
using System.Collections.Generic;

namespace ShipSurvivors
{

	class ShipSelectOption
	{
		public string Name { get; set; }
		public string ClassName { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public bool Active { get; set; }
	}
	public class ShipSelectorScreen : SimpleGameMenuScreen
	{

		public ShipSelectorScreen(): base() { 
			StyleSheet.Load( "/Gamemode/Ui/styles.scss" );
			StyleSheet.Load( "/Gamemode/Ui/UpgradePanel.scss" );
			var Ships = new List<ShipSelectOption>();
			Ships.Add( new ShipSelectOption()
			{
				ClassName = "ShipPlayerFighter",
				Name = "Fighter",
				Description="Basic Fighter Ship",
				Active = true,
				Image= "/raw/spaceshooter/ships/player_fighter.png",
			} );

			Ships.Add( new ShipSelectOption()
			{
				ClassName = "ShipPlayerDual",
				Name = "Dual Fighter",
				Active = false,
				Description = "Shoots 2 missles but with reduced damage.",
				Image= "raw/spaceshooter/ships/player_dual.png"
			} ); ;

			foreach (var ship in Ships)
			{
				var button = AddChild<DeggCard>( "ship-button" );
				button.SetClass( "ship-button active", ship.Active );
				button.Image.SetImage( ship.Image );
				button.Image.AddClass( "ship-selector" );
				button.Header.SetText( ship.Name );
				button.Body.SetText( ship.Description );
				if ( ship.Active )
				{
					button.Inner.AddEventListener( "OnClick", () =>
					{
						Sandbox.ConsoleSystem.Run( "ss.client.loaded", ship.ClassName );
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
