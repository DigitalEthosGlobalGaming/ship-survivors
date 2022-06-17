
using Degg.UI.Elements.SimpleGameMenu;
using Sandbox.UI;

namespace ShipSurvivors
{
	public class EndGamePanelScore : SimpleGameMenuItem
	{
		public Label EndGameScore {get;set;}

		public EndGamePanelScore()
		{
			EndGameScore = AddChild<Label>();
		}

		public override void SetText( string text )
		{
			EndGameScore.Text = text;
		}
	}
}
