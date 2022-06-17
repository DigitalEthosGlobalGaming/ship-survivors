using Degg.UI.Elements;
using Sandbox.UI;

namespace ShipSurvivors
{
	public class UpgradeIconElement: DeggPanel
	{
		public Label AmountLabel { get; set; }
		public Image Icon { get; set; }
		public int Amount { get; set; }
		public string Texture { get; set; }

		public UpgradeIconElement(): base()
		{
			
			StyleSheet.Load( "/Gamemode/Ui/UpgradeIconElement.scss" );
			var Base = AddChild<Panel>( "upgrade-icon-container" );
			var Inner = Base.AddChild<Panel>();
			Icon = Inner.AddChild<Image>("upgrade-icon-image");
			Icon.Style.Width = Length.Pixels( 75 );
			Icon.Style.Height = Length.Pixels(75 );

			Inner.AddClass( "upgrade-icon" );
			AmountLabel = Inner.AddChild<Label>( "upgrade-icon-amount" );
		}

		public void SetElementsInformation()
		{
			if ( AmountLabel != null)
			{
				AmountLabel.SetText( Amount.ToString() );
			}
			if ( Icon != null && Texture != null && Texture != "" )
			{
				Icon.SetTexture( Texture );
			}
		}

		public override void Tick()
		{
			SetElementsInformation();
		}



	}
}
