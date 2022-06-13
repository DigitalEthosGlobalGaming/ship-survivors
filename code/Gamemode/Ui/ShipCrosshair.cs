using Degg;
using Degg.UI.Elements;
using Degg.Util;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;
using static Degg.Util.RoundSystem.Round;

namespace ShipSurvivors
{
	public class ShipCrosshair : DeggPanel
	{
		public Cursor Crosshair { get; set; }
		public ShipCrosshair()
		{
			Style.Width = Length.ViewWidth(100);
			SetupElements();
		}

		public void SetupElements()
		{
			Crosshair = AddChild<Cursor>("ship-crosshair");
			Crosshair.SetTexture( "/raw/crosshairs/green/crosshair177.png" );
			Crosshair.ImageElement.Style.Width = Length.Pixels( 50 );
			Crosshair.ImageElement.Style.Height = Length.Pixels( 50 );
		}

		public override void Tick()
		{

			base.Tick();
			var player = ClientUtil.GetPawn<ShipPlayer>();
			if ( player?.IsValid() ?? false )
			{
				Crosshair.Position = player.Position + player.Cursor;
			}
		}

	}
}
