using Sandbox;

namespace ShipSurvivors
{
	public class EnemyFighter: EnemyShip
	{


		public EnemyShip ClosestAlly { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			EntityMaterial = "materials/ships/enemy_fighter_1.vmat";

			if ( MyGame.GetDifficulty() > 5 )
			{
				AttackSpeed = AttackSpeed * 0.75f;
			}
			if ( MyGame.GetDifficulty() > 15 )
			{
				AttackSpeed = AttackSpeed * 0.5f;
			}
		}

		public override Bullet GetBullet()
		{
			var bullet = new Bullet
			{
				Owner = this,
				Position = this.Position + this.Rotation.Forward * 5f,
				Scale = 0.25f
			};

			var velocity = (Rotation.Forward * 80f);
			bullet.PhysicsBody.Velocity = velocity;
			bullet.RenderColor = Color.Gray.WithRed( 0.9f );

			return bullet;
		}

		public override void ExpensiveTick()
		{
			base.ExpensiveTick();
			var closest = GetClosest<EnemyShip>(0,10f);
			if ( closest?.IsValid() ?? false )
			{
				ClosestAlly = closest;

				// Todo, move this to the move step function
				var inverse = closest.Position - Position;
				PhysicsBody.Velocity = -inverse * 25 * Time.Delta;
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
