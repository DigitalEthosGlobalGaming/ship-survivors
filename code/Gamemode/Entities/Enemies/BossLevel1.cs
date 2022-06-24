using Sandbox;

namespace ShipSurvivors
{
	public class BossLevel1: EnemyShip
	{

		public float ChargeDuration { get; set; }
		public override void Spawn()
		{
			base.Spawn();
			EntityMaterial = "materials/ships/enemy_boss_level1.vmat";
			Scale = 2.5f;
			Health = 1500f;
			IsBoss = true;

			AttackSpeed = 0.5f;
		}

		public override BossLevel1Bullet GetBullet()
		{
			var bullet = new BossLevel1Bullet
			{
				Owner = this,
				Position = this.Position,
			};

			bullet.RenderColor = Color.Red;
			bullet.Target = Target;

			return bullet;
		}

		public override void TryShoot()
		{
			if ( NextAttackTime < Time.Now && ChargeDuration > 2.5f )
			{
				var bullet = GetBullet();
				OnShoot( bullet );
				EmitSound( "enemy.weapon.fire1" );
				NextAttackTime = GetNextAttackTime();
			}
		}

		public override void MoveStep()
		{
			base.MoveStep();
			if ( Health < 750)
			{
				ChargeDuration = ChargeDuration - Time.Delta;
				AttackSpeed = 0.25f;
			}
			ChargeDuration = ChargeDuration - Time.Delta;
			if ( ChargeDuration  < 0)
			{
				ChargeDuration = 5f;
				PhysicsBody.Velocity = PhysicsBody.Velocity + (Rotation.Forward * 150f);
			}
		}


	}
}
