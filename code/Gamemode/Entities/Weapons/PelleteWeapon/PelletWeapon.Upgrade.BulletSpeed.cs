using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeBulletSpeed : WeaponUpgrade 
	{
		public override string ParentUpgradeClassName { get; set; } = "PelletWeapon";
		public override string UpgradeName { get; set; } = "Electrified Barrels";
		public override string Description { get; set; } = "Increases Projectile Speed";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair020.png";
		public override float Rarity { get; set; } = 1;
		public override void OnOwnerStatsUpdate()
		{
			base.OnOwnerStatsUpdate();
			var weapon = GetWeapon<PelletWeapon>();
			if ( weapon?.IsValid() ?? false )
			{
				weapon.AttackBulletSpeed = weapon.AttackBulletSpeed + (10f * Level);
			}
		}

	}
}
