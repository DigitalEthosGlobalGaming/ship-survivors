using Degg.UI.Elements;
using Sandbox.UI;

namespace ShipSurvivors
{
	public class UpgradeIconElement: DeggPanel
	{
		public Label AmountLabel { get; set; }
		public int Amount { get; set; }

		public UpgradeIconElement(): base()
		{
			StyleSheet.Load( "/Gamemode/Ui/UpgradeIconElement.scss" );
			var Base = AddChild<Panel>( "upgrade-icon-container" );
			var Inner = Base.AddChild<Panel>();

			Inner.AddClass( "upgrade-icon" );
			AmountLabel = Inner.AddChild<Label>();
		}

		public void SetElementsInformation()
		{
			if ( AmountLabel != null)
			{
				AmountLabel.SetText( Amount.ToString() );
			}
		}

		public override void Tick()
		{
			SetElementsInformation();
		}



	}
}
