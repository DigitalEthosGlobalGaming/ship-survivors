using Degg.Ui;
using Sandbox;

namespace Degg.Core
{
	public partial class DeggPlayer: Player
	{
		public PlayerHud Hud { get; set; }
		[Event.Hotload]
		public virtual void HudHotload()
		{
			if ( IsClient )
			{
				HudSetup();
			}
		}

		public virtual void DeleteHud()
		{
			if ( IsClient )
			{
				if ( Hud != null )
				{
					Hud.Delete();
					Hud = null;
				}
			}
		}

		public virtual void HudSetup()
		{
			if ( IsClient )
			{
				DeleteHud();
				Hud = new PlayerHud( this );
			}
		}
	}
}
