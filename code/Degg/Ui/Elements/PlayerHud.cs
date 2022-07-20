using Degg.Core;
using Degg.UI.Elements;
using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace Degg.Ui
{
	public partial class PlayerHud : HudEntity<RootPanel>
	{
		public static PlayerHud Current { get; set; }
		public List<Panel> Panels { get; set; }
		public Panel Container { get; set; }
		public DeggPlayer Player { get; set; }
		public bool NeedsReset { get; set; }
		public PlayerHud( DeggPlayer player )
		{
			Container = RootPanel.AddChild<Panel>();
			Current = this;
			Player = player;
			Panels = new List<Panel>();
			NeedsReset = true;
		}

		[Event.Tick]
		public void Tick()
		{
			if ( NeedsReset == true)
			{
				NeedsReset = false;
				Setup();
			}
		}

		[Event.Hotload]
		public void OnHotload()
		{
			NeedsReset = true;
		}
		public void Setup()
		{
			NeedsReset = false;
			if ( Panels != null )
			{
				foreach ( var i in Panels )
				{
					i.Delete( true );
				}
			}

			OnSetup();
		}

		public virtual void OnSetup()
		{
			Container.StyleSheet.Load( "/Degg/Ui/Styles/base.scss" );
			Container.AddClass( "degg-root" );
			Container.AddChild<ChatBox>();
		}

		public T AddPanel<T>() where T : Panel, new()
		{
			var newElement = RootPanel.AddChild<T>();
			if ( newElement is PlayerPanel<DeggPlayer> playerPanel )
			{
				playerPanel.SetPlayer( Player );
			}
			Panels.Add( newElement );
			return newElement;
		}


		protected override void OnDestroy()
		{
			base.OnDestroy();
			foreach ( var item in Children )
			{
				if ( item.IsValid() )
				{
					item.Delete();
				}
			}
		}
	}
}
