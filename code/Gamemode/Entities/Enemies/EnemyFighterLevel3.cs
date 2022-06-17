using Sandbox;
using System;

namespace ShipSurvivors
{
	public class EnemyFighterLevel3 : EnemyShip
	{
		public float MoveDirection { get; set; }

		public float NextMoveChange { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			EntityMaterial = "materials/ships/enemy_fighter_1.vmat";

			Scale = 1f;
			MaxSpeed = 50f;
			Accelleration = 2f;
			Health = 50f;
			RenderColor = Color.Black.WithRed( 0.3f ).WithBlue(0.3f);
			AttackSpeed = 5f;
			NextMoveChange = Time.Now + 5f;
		}

		public override Bullet GetBullet()
		{
			var bullet = new Bullet
			{
				Owner = this,
				Position = this.Position + this.Rotation.Forward * 5f,
				Scale = 0.35f
			};

			var velocity = (Rotation.Forward * (60f));
			bullet.PhysicsBody.Velocity = velocity;
			bullet.RenderColor = Color.Red.WithBlue( 0.5f );
			bullet.Damage = 2;
			bullet.Strength = 2;

			return bullet;
		}

		public override void MoveStep()
		{
			base.MoveStep();
			if ( NextMoveChange < Time.Now)
			{
				NextMoveChange = Time.Now + Rand.Float(2.5f,5);
				MoveDirection = Rand.Float( -50f, 50f );
			}
			if ( PhysicsBody?.IsValid() ?? false )
			{
				PhysicsBody.Velocity = PhysicsBody.Velocity + (Rotation.Left * Time.Delta * MoveDirection);
			}
		}

		public override void TryShoot()
		{
			if ( NextAttackTime < Time.Now )
			{
				var bullet = GetBullet();
				OnShoot( bullet );
				EmitSound( "enemy.weapon.fire1" );
				NextAttackTime = GetNextAttackTime();
			}
		}


	}
}
