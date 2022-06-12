using Degg.Ui;
using Degg.UI.Elements;
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

	}
	public class ShipSelectorPanel: WelcomePanel
	{

		public override void SetupWelcomeElements()
		{
			StyleSheet.Load( "/Gamemode/Ui/styles.scss" );
			StyleSheet.Load( "/Gamemode/Ui/UpgradePanel.scss" );
			var Ships = new List<ShipSelectOption>();
			Ships.Add( new ShipSelectOption()
			{
				ClassName = "ShipPlayerFighter",
				Name = "Testing this",
				Description="Test ship, please ignore",
				Image= "/raw/spaceshooter/ships/ship_j.png",
			} ); ;
			Ships.Add( new ShipSelectOption()
			{
				ClassName = "ShipPlayerFighter",
				Name = "Fighter",
				Description = "Base ship with a standard pellet weapon.",
				Image= "raw/spaceshooter/src/ship_sidesd.png"
			} ); ;

			foreach (var ship in Ships)
			{
				var button = Inner.AddChild<DeggCard>( "button" );
				button.Image.SetImage( ship.Image );
				button.Image.AddClass( "ship-selector" );
				button.Header.SetText( "SHIP | " + ship.Name );
				button.Body.SetText( ship.Description );
				button.Inner.AddEventListener("OnClick", () => { 
					Sandbox.ConsoleSystem.Run( "ss.client.loaded",  ship.ClassName );
				} );
			}

			

		}
	}
}
