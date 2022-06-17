using Sandbox;
using System;

namespace ShipSurvivors
{
	public class EnemyShipLevel3: EnemyShip
	{

		public override void Spawn()
		{
			base.Spawn();
			EntityMaterial = "materials/ships/enemy_ship_3.vmat";

			Scale = 1f;
			MaxSpeed = 50f;
			Accelleration = 2f;
			Health = 10f;
			RenderColor = Color.Black.WithRed( 0.3f );
			AttackSpeed = 2.5f;
		}

		public override Bullet GetBullet()
		{
			var bullet = new Bullet
			{
				Owner = this,
				Position = this.Position,
				Scale = 0.25f
			};


			bullet.RenderColor = Color.Red;

			return bullet;
		}

		public override Vector3 GetLookAtPosition()
		{
			float rad = (float) (Time.Tick % 360 * Math.PI) / 180;
			var position = Vector2.FromRadian( rad );

			return Position + (Vector3.Zero.WithX(position.x).WithY(position.y));
		}

		public override void TryShoot()
		{
			if ( NextAttackTime < Time.Now )
			{
				var velocity = (Rotation.Forward * (40f));
				var bullet = GetBullet();
				OnShoot( bullet );				
				bullet.PhysicsBody.Velocity = velocity;

				velocity = (Rotation.Left * (40f));
				bullet = GetBullet();
				OnShoot( bullet );
				bullet.PhysicsBody.Velocity = velocity;

				velocity = (Rotation.Right * (40f));
				bullet = GetBullet();
				OnShoot( bullet );
				bullet.PhysicsBody.Velocity = velocity;

				velocity = (Rotation.Backward * (40f));
				bullet = GetBullet();
				OnShoot( bullet );
				bullet.PhysicsBody.Velocity = velocity;

				EmitSound( "enemy.weapon.fire1" );
				NextAttackTime = GetNextAttackTime();
			}
		}


	}
}
