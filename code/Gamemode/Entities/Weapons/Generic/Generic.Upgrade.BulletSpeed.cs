
using Sandbox;

namespace ShipSurvivors
{

	public partial class GenericWeaponUpgradeBulletSpeed : WeaponUpgrade 
	{
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
