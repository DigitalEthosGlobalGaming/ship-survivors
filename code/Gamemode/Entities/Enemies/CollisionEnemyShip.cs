using Sandbox;

namespace ShipSurvivors
{
	public class CollisionEnemyShip: EnemyShip
	{

		public override void Spawn()
		{
			base.Spawn();
			EntityMaterial = "materials/ships/enemies/enemy_collision_1.vmat";
		}


		public override void MoveStep()
		{
			if ( Target?.IsValid ?? false )
			{
				var targetV = Target.Position;

				var targetPosition = targetV;
				var forceDirection = GetRotationLookingAt( targetPosition ).Forward;
				Accelleration = 30f;
				if (targetPosition.Distance(Position) > 200)
				{
					Accelleration = targetPosition.Distance( Position );
				}


				PhysicsBody.Velocity = forceDirection * Accelleration;
			}
			else
			{
				FindTarget();
			}
		}

		public override void OnShipPlayerCollision(ShipPlayer player)
		{
			this.Delete();
			var damage = new DamageInfo();
			damage.Attacker = Owner;
			damage.Damage = 1f;
			damage.Position = Position;
			player.TakeDamage( damage );
			EmitSound( "player.ship.damage" );
		}

	}
}
