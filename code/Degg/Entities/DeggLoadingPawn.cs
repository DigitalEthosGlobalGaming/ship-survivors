using Degg.Cameras;
using Degg.Core;
using Degg.Ui;
using Sandbox;
using System;

namespace Degg.Entities
{
	public partial class DeggLoadingPawn: DeggPlayer
	{

		public string EntityName { get; set; }
		public override void Spawn()
		{
			base.Spawn();
		}

		public virtual void OnJoin()
		{
			Delete();
			Client.Pawn = CreateByName( EntityName );
		}

		public override void HudSetup()
		{
			base.HudSetup();
		}

		
		[ConCmd.Server("degg.client.loaded")]
		public static void OnLoad()
		{
			var player = ClientUtil.GetCallingPawn<DeggLoadingPawn>();
			if (player?.IsValid() ?? false)
			{
				player.OnJoin();
			}
		}
	}
}
