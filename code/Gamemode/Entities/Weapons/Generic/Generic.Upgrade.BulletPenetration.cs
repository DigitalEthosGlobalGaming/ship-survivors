using Sandbox;

namespace ShipSurvivors
{

	public partial class GenericWeaponUpgradeBulletPenetration : WeaponUpgrade 
	{
		public override void OnOwnerStatsUpdate()
		{
			base.OnOwnerStatsUpdate();
			var weapon = GetWeapon<PelletWeapon>();
			if ( weapon?.IsValid() ?? false )
			{
				weapon.AttackBulletPenetration = weapon.AttackBulletPenetration + (1 * (int)Level);
			}
		}

	}
}
