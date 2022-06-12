using Degg.UI.Elements;
using Sandbox.UI;

namespace ShipSurvivors
{
	public class UpgradeItemElement : DeggCard
	{
		public Panel Base { get; set; }

		public Upgrade Upgrade { get; set; }
		public int UpgradeIndex { get; set; }

		public UpgradeItemElement() : base()
		{

		}

		public void SetUpgrade( Upgrade upgrade )
		{
			Upgrade = upgrade;
			CreateSubElements();
		}

		public override void OnClickInner( MousePanelEvent e )
		{
			ShipPlayer.BuyUpgradeConCommand( Upgrade.ClassName );
		}

		public override void CreateSubElements()
		{
			base.CreateSubElements();

		}

		public void SetElementsInformation()
		{
			if ( Upgrade != null )
			{
				if ( Header != null )
				{
					Header.SetText( Upgrade.Name );
				}
				if ( Body != null )
				{
					Body.SetText( Upgrade.Description );
				}
			}
		}
		public override void Tick()
		{
			SetElementsInformation();
		}
	}
}
