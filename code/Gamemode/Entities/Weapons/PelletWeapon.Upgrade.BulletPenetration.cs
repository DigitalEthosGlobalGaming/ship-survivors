using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeBulletPenetration : WeaponUpgrade 
	{

		public override string ParentUpgradeClassName { get; set; } = "PelletWeapon";
		public override string UpgradeName { get; set; } = "Hardened Casings";
		public override string Description { get; set; } = "Increases bullet strength, allowing them to pass through an additional enemy bullet.";
		public override string Image { get; set; } = "";
		public override float Rarity { get; set; } = 1;
		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Owner;
			Active = false;
		}

		public override void OnOwnerStatsUpdate()
		{
			base.OnOwnerStatsUpdate();
			var weapon = GetWeapon<PelletWeapon>();
			if ( weapon?.IsValid() ?? false )
			{
				weapon.AttackBulletPenetration = weapon.AttackBulletPenetration + 1;
			}
		}

	}
}
