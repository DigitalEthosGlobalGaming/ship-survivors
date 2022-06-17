using Degg.Entities;
using Degg.Util;
using Sandbox;

namespace ShipSurvivors
{

	public partial class PelletWeaponUpgradeBulletSplashDamage : WeaponUpgrade 
	{

		public override string ParentUpgradeClassName { get; set; } = "PelletWeapon";
		public override string UpgradeName { get; set; } = "Fragmenting Shells";
		public override string Description { get; set; } = "Creates a smaller bullet in a random direction when killing an enemy";
		public override string Image { get; set; } = "/raw/crosshairs/green/crosshair161.png";
		public override float Rarity { get; set; } = 1;
		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Owner;
			Active = false;
		}

		public void CreateBullets(Entity source, Vector3 position)
		{
			var bullet = new Bullet();
			bullet.Owner = source.Owner;
			bullet.Position = position;
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

		public override void OnEnemyDamaged( Entity b, EnemyShip e, bool didKill )
		{
			base.OnEnemyDamaged( b, e, didKill );
			if (didKill)
			{
				CreateBullets(b, e.Position);
			}
		}

	}
}
