using Degg;
using Degg.UI.Elements;
using Sandbox.UI;

namespace ShipSurvivors
{
	public class PlayerCoresElement: DeggPanel
	{
		public Label Health { get; set; }
		public int Amount { get; set; }

		public PlayerCoresElement(): base()
		{
			var Base = AddChild<Panel>( "player-cores-element" );
			var Inner = Base.AddChild<Panel>();
			Health = Inner.AddChild<Label>( "player-cores-element-label" );
			Health.SetText( "0" );
		}

		public void SetElementsInformation()
		{
			if ( Health != null)
			{
				var player = ClientUtil.GetPawn<ShipPlayer>();
				
				if (player?.IsValid ?? false)
				{
					Health.SetText( $"{player.GetCoresAccount()?.Amount ?? 0} cores" );
				}
			}
		}

		public override void Tick()
		{
			SetElementsInformation();
		}



	}
}
