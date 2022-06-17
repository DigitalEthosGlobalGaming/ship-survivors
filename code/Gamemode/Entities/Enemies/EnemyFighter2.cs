using Sandbox;

namespace ShipSurvivors
{
	public class EnemyFighterV2: EnemyShip
	{

		public override void Spawn()
		{
			base.Spawn();
			EntityMaterial = "materials/ships/enemy_fighter_2.vmat";

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
				Position = this.Position + this.Rotation.Forward * 5f,
				Scale = 0.35f
			};

			var velocity = (Rotation.Forward * (40f));
			bullet.PhysicsBody.Velocity = velocity;
			bullet.RenderColor = Color.Red.WithBlue(0.5f);
			bullet.Strength = 2;

			return bullet;
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
