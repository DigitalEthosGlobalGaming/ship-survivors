using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeBulletSpeed : WeaponUpgrade 
	{
		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Owner;
			Rarity = 1;
			Name = "Electrified Barrels";
			ParentUpgradeClassName = "PelletWeapon";
			Description = "Increases Projectile Speed";
			Image = "";
			Active = false;
		}

		public override void OnOwnerStatsUpdate()
		{
			base.OnOwnerStatsUpdate();
			var weapon = GetWeapon<PelletWeapon>();
			if ( weapon?.IsValid() ?? false )
			{
				weapon.AttackBulletSpeed = weapon.AttackBulletSpeed + 10f;
			}
		}

	}
}
