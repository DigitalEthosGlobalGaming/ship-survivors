using Sandbox;
using System;

namespace ShipSurvivors
{
	public partial class PelletWeapon : ShipWeapon
	{
		public override string UpgradeName { get; set; } = "Pellet Weapon";
		public override string Description { get; set; } = "Basic Pellet Weapon";
		public float AttackBulletSize { get; set; }
		public int AttackBulletPenetration { get; set; }
		public float AttackBulletDamage { get; set; }
		public float AttackBulletSpeed { get; set; }
		public override void ResetStats()
		{
			base.ResetStats();
			AttackBulletDamage = 0.5f;
			AttackBulletPenetration = 1;
			AttackBulletSize = 0.2f;
			AttackBulletSpeed = 1f;
		}
		public Bullet CreateBullet()
		{
			var bullet = new Bullet();
			bullet.Owner = GetShipPlayer();
			bullet.Scale = AttackBulletSize;
			bullet.Strength = AttackBulletPenetration;
			bullet.Damage = AttackBulletDamage;


			var velocity = (Rotation.Forward * (50f + 50));
			bullet.PhysicsBody.Velocity = velocity;
			bullet.EntityMaterial = "materials/bullets/bullet_player_1.vmat";
			return bullet;
		}
		public override void OnPrimaryAttack()
		{
			var player = GetShipPlayer();

			var bullet = CreateBullet();
			bullet.Position = Position + (Rotation.Forward * 5f);

			PlaySoundOnClient( "ship.weapon.fire" );

			if (player != null)
			{
				var homingBulletChange = player.GetUpgradeLevel( "PelleteWeaponUpgradeSideGunner" );
				if ( homingBulletChange > 0 )
				{
					for ( int i = 0; i < homingBulletChange; i++ )
					{
						var rnd = Rand.Float( 0, 100 ) <= 25;
						if (rnd)
						{
							var enemies = MyGame.GetRoundManager().Enemies;
							var enemy = Rand.FromList( enemies );
							if ( enemy?.IsValid() ?? false )
							{
								var extraBullet = CreateBullet();
								extraBullet.RenderColor = Color.White.WithGreen(0.5f);
								extraBullet.Position = Position;
								extraBullet.LookAt( enemy.Position );
								extraBullet.Velocity = extraBullet.Rotation.Forward * (100f);
							}
						}
					}					
				}
				player.ScreenShakeOnClient( 0.5f );
			}
		}
		private Vector3 Vector2( float x, object y )
		{
			throw new NotImplementedException();
		}
	}
}
