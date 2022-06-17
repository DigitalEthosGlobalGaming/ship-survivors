using Degg.UI.Elements;
using Sandbox.UI;


namespace ShipSurvivors
{
	public partial class UpgradeCardFooter: DeggCardFooter
	{
		public Label LeftLabel { get; set; }
		public Label RightLabel { get; set; }
		public Upgrade Upgrade { get; set; }
		public UpgradeCardFooter()
		{
			AddClass( "ss-card-footer" );
			LeftLabel = AddChild<Label>( "card-footer-left" );
			RightLabel = AddChild<Label>( "card-footer-right" );
		}
		public void SetUpgrade(Upgrade upgrade)
		{
			Upgrade = upgrade;
			if (LeftLabel == null)
			{
				LeftLabel = AddChild<Label>("card-footer-left");
			}

			if ( RightLabel == null )
			{
				RightLabel = AddChild<Label>( "card-footer-right" );
			}

			LeftLabel.SetText( Upgrade?.ParentUpgrade?.UpgradeName ?? "" );
			RightLabel.SetText( "Upgrade" );
		}
	}
}
