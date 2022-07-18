using Degg.Ui;
using Degg.UI.Elements;
using Degg.UI.Elements.SimpleGameMenu;
using Sandbox.UI;
using System.Collections.Generic;

namespace ShipSurvivors
{

	public class StoreOption
	{
		public string Name { get; set; }
		public string ClassName { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public bool Active { get; set; }
	}
	public class StoreScreen : SimpleGameMenuScreen
	{

		public Panel StoreItemsPanel { get; set; }
		public List<StoreOption> StoreItems = new List<StoreOption>();

		public StoreScreen(): base() { 
			StyleSheet.Load( "/Gamemode/Ui/styles.scss" );
			StyleSheet.Load( "/Gamemode/Ui/UpgradePanel.scss" );

			var label = AddChild<Label>();
			label.SetText( "Coming Soon" );

			UpdateItems();

			StoreItemsPanel.AddClass( "w-100 flex-column center" );

		}

		public void UpdateItems()
		{
			StoreItemsPanel?.Delete();
			StoreItemsPanel = AddChild<Panel>();

			foreach ( var item in StoreItems )
			{
				AddStoreOption( item );				
			}
		}

		public void AddStoreOption(StoreOption item )
		{
			var button = StoreItemsPanel.AddChild<DeggCard>( "ship-button" );
			button.SetClass( "ship-button active", item.Active );
			button.Image.SetImage( item.Image );
			button.Image.AddClass( "ship-selector" );
			button.Header.SetText( item.Name );
			button.Body.SetText( item.Description );
			if ( item.Active )
			{
				button.Inner.AddEventListener( "OnClick", () =>
				{
					Sandbox.ConsoleSystem.Run( "ss.client.loaded", item.ClassName );
				} );
			}
			else
			{
				button.Header.SetText( "Unidentified" );
				button.Body.SetText( "Coming soon" );
			}
		}
	}
}
