using Degg;
using Degg.UI.Elements;
using Sandbox.UI;

namespace ShipSurvivors
{
	public class PlayerHealthElement: DeggPanel
	{
		public Label Health { get; set; }
		public Image Icon { get; set; }
		public int Amount { get; set; }
		public string Texture { get; set; }

		public PlayerHealthElement(): base()
		{
			var Base = AddChild<Panel>( "player-health-element" );
			var Inner = Base.AddChild<Panel>();
			Health = Inner.AddChild<Label>( "player-health-element-label" );
			Health.SetText( "10/10" );
		}

		public void SetElementsInformation()
		{
			if ( Health != null)
			{
				var player = ClientUtil.GetPawn<ShipPlayer>();
				if (player?.IsValid ?? false)
				{
					Health.SetText( $"{player.Health}/{player.MaxHealth}" );
				}
			}
		}

		public override void Tick()
		{
			SetElementsInformation();
		}



	}
}
