using Sandbox;

namespace ShipSurvivors
{

	public partial class GenericWeaponUpgradeBulletSize : WeaponUpgrade 
	{
		public override void OnOwnerStatsUpdate()
		{
			base.OnOwnerStatsUpdate();
			var weapon = GetWeapon<PelletWeapon>();
			if ( weapon?.IsValid() ?? false )
			{
				weapon.AttackBulletSize = weapon.AttackBulletSize + (0.25f * Level);
			}
		}

	}
}
