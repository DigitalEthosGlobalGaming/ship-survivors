using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeBulletSize : WeaponUpgrade 
	{

		public override string ParentUpgradeClassName { get; set; } = "PelletWeapon";
		public override string UpgradeName { get; set; } = "Pim Rounds";
		public override string Description { get; set; } = "Increases Projectile Size";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair186.png";
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
				weapon.AttackBulletSize = weapon.AttackBulletSize + 0.25f;
			}
		}

	}
}
