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

		public override void OnDeleted()
		{
			base.OnDeleted();
			if ( ParentUpgrade?.IsValid ?? false)
			{
				ParentUpgrade.Delete();	
			}
		}

		public void SetElementsInformation()
		{
			if ( Upgrade != null )
			{
				if ( ParentUpgrade == null )
				{
					var parent = Upgrade.ParentUpgradeClassName;
					if ( parent != null )
					{
						ParentUpgrade = TypeLibrary.Create<Upgrade>( parent );
					}
				}

				if ( Header != null )
				{
					Log.Info( ParentUpgrade.Image );
					Header.SetText( Upgrade.UpgradeName );
				}
				if ( Body != null )
				{
					Body.SetText( Upgrade.Description );
				}
				if (Footer != null)
				{				

					if ( ParentUpgrade != null)
					{
						Footer.SetText( "Upgrade for " + ParentUpgrade.UpgradeName );
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
