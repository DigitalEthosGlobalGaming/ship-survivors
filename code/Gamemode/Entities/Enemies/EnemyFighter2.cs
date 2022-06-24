using Sandbox;

namespace ShipSurvivors
{
	public class EnemyFighterV2: EnemyShip
	{

		public EnemyShip ClosestAlly { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			EntityMaterial = "materials/ships/enemy_fighter_2.vmat";

			Scale = 1f;
			MaxSpeed = 50f;
			Accelleration = 2f;
			Health = 10f;
			AttackSpeed = 2.5f;
		}

		public override Bullet GetBullet()
		{
			var bullet = new Bullet
			{
				Owner = this,
				Position = this.Position + this.Rotation.Forward * 5f,
				Scale = 0.35f
			};

			var velocity = (Rotation.Forward * 80f);
			bullet.PhysicsBody.Velocity = velocity;
			bullet.RenderColor = Color.Gray.WithRed( 0.9f ).WithBlue(0.7f);
			bullet.Strength = 2;

			return bullet;
		}

		public override void ExpensiveTick()
		{
			base.ExpensiveTick();
			var closest = GetClosest<EnemyShip>( 0, 30f );
			if ( closest?.IsValid() ?? false )
			{				
				ClosestAlly = closest;

				var inverse = closest.Position - Position;
				// Todo, move this to the move step function
				PhysicsBody.Velocity = -inverse * 25 * Time.Delta;
				if ( NetworkIdent % 2 == 0 )
				{
					PhysicsBody.Velocity = PhysicsBody.Velocity + (Rotation.Left * Time.Delta * 12);
				} else
				{
					PhysicsBody.Velocity = PhysicsBody.Velocity + (Rotation.Right * Time.Delta * 10);
				}
			}
		}

		public override void MoveStep()
		{
			base.MoveStep();
			if ( PhysicsBody?.IsValid() ?? false )
			{
				if ( NetworkIdent % 2 == 0 )
				{
					PhysicsBody.Velocity = PhysicsBody.Velocity + (Rotation.Left * Time.Delta * 50);
				}
				else
				{
					PhysicsBody.Velocity = PhysicsBody.Velocity + (Rotation.Right * Time.Delta * 50);

				}
				var amount = PhysicsBody.Velocity.Distance( Vector3.Zero );
				if ( amount > 150 )
				{
					PhysicsBody.Velocity = PhysicsBody.Velocity.Normal * 150;
				}
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
