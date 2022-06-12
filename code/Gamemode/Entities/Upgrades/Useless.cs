using Degg.Entities;
using Sandbox;

namespace ShipSurvivors
{
	public partial class UselessUpgrade : Upgrade
	{
		public override void Spawn()
		{
			base.Spawn();
			Name = "Useless Upgrade";
		}
	

		public override void OnEquip() {
			base.OnEquip();

		}

		public override void OnUnEquip()
		{
			base.OnUnEquip();
		}



	}
}
