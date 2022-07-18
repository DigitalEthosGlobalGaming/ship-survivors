using Sandbox;

namespace ShipSurvivors
{
	public partial class GenericWeaponUpgradeBulletSplashDamage : WeaponUpgrade 
	{
		public void CreateBullets(Entity source, Vector3 position)
		{
			var player = GetShipPlayer();
			var damage = ((player?.GetUpgradeLevel( "GenericWeaponUpgradeBulletSplashDamageLevel2" ) ?? 0) / 2) + 1;
			var bullet = new Bullet();
			bullet.Owner = source.Owner;
			bullet.Position = position;
			bullet.Damage = damage;
			bullet.Scale = 0.2f;
			bullet.DeathTime = Time.Now + 2f;
			bullet.Strength = 0;
			bullet.SetVelocityFromAngle( Rand.Float( -180, 180 ), 50f );
			bullet.EntityMaterial = "materials/bullets/bullet_player_1.vmat";
			PlaySoundOnClient( "ship.weapon.fire" );
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

		public override bool CanBuyUpgrade()
		{
			var player = GetShipPlayer();
			var currentAmount = player?.GetUpgradeLevel( "GenericWeaponUpgradeBulletSplashDamage" ) ?? 0;
			return currentAmount < 10;
		}

		public override void OnEnemyDamaged( Entity b, EnemyShip e, bool didKill )
		{
			base.OnEnemyDamaged( b, e, didKill );
			if (didKill)
			{
				for ( int i = 0; i < Level; i++ )
				{
					CreateBullets( b, e.Position );
				}
			}
		}
	}
}
