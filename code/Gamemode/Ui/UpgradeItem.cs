using Degg.UI.Elements;
using Sandbox.UI;

namespace ShipSurvivors
{
	public class UpgradeItemElement : DeggCard
	{
		public Panel Base { get; set; }

		public Upgrade Upgrade { get; set; }
		public int UpgradeIndex { get; set; }

		public Upgrade ParentUpgrade { get; set; }

		public UpgradeItemElement() : base()
		{
			AddClass( "upgrade-icon-card" );
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

		public override void OnDeleted()
		{
			base.OnDeleted();
			if ( ParentUpgrade?.IsValid ?? false)
			{
				ParentUpgrade.Delete();	
			}
		}

		public override DeggCardFooter CreateFooterElement()
		{
			return Inner.AddChild<UpgradeCardFooter>();
		}

		public void SetElementsInformation()
		{
			if ( Upgrade != null )
			{
				if ( ParentUpgrade == null )
				{
					var parent = Upgrade.ParentUpgradeClassName;
					if ( parent != null && parent != "" )
					{
						ParentUpgrade = TypeLibrary.Create<Upgrade>( parent );
					}
				}

				if ( Header != null )
				{
					Header.SetText( Upgrade.UpgradeName );
				}
				if ( Body != null )
				{
					
					Body.SetText( Upgrade.Description );
				}

				if ( Image != null )
				{
					Image.SetImage( Upgrade.Image );
				}
				if (Footer != null && Footer is UpgradeCardFooter ucf)
				{
					if ( ParentUpgrade != null)
					{
						ucf.SetUpgrade( Upgrade );
					}

				}
			}
		}
		public override void Tick()
		{
			SetElementsInformation();
		}
	}
}
