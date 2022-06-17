using Sandbox.UI;

namespace Degg.UI.Elements
{
	public partial class FullScreenPanel : DeggPanel {

		public Panel Inner { get; set; }

		public FullScreenPanel()
		{
			AddClass( "degg-fullscreen" );
			Inner = AddChild<Panel>( "degg-fullscreen-inner" );
			StyleSheet.Load( "/Degg/Ui/Styles/base.scss" );
		}

		public void SetCursorActive(bool val)
		{
			if ( val )
			{
				Style.PointerEvents = "all";
			} else
			{
				Style.PointerEvents = "none";
			}
		}
	}
}
