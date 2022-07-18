using Sandbox;

namespace ShipSurvivors
{

	public partial class GenericWeaponUpgradeBulletDamage : WeaponUpgrade 
	{
		public override void OnOwnerStatsUpdate()
		{
			base.OnOwnerStatsUpdate();
			var weapon = GetWeapon<PelletWeapon>();
			if ( weapon?.IsValid() ?? false )
			{
				weapon.AttackBulletDamage = weapon.AttackBulletDamage + (0.5f * Level);
			}
		}

	}
}
