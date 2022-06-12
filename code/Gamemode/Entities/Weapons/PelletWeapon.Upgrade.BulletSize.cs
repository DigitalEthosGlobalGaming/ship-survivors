using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeBulletSize : WeaponUpgrade 
	{
		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Owner;
			Rarity = 1;
			Name = "Pim Rounds";
			ParentUpgradeClassName = "PelletWeapon";
			Description = "Increases Projectile Size";
			Image = "";
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
