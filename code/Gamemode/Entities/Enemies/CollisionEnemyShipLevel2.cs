using Sandbox;

namespace ShipSurvivors
{
	public class CollisionEnemyShipLevel2 : EnemyShip
	{

		public override void Spawn()
		{
			base.Spawn();
			EntityMaterial = "materials/ships/enemy_collision_ship_2.vmat";

			Scale = 1f;
			Accelleration = 40f;
			Health = 10f;
		}


		public override void MoveStep()
		{
			if ( Target?.IsValid ?? false )
			{
				var targetV = Target.Position;

				var targetPosition = targetV;
				var forceDirection = GetRotationLookingAt( targetPosition ).Forward;
				LookAt( targetPosition );

				PhysicsBody.Velocity =  forceDirection * Accelleration;
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
			damage.Damage = 5f;
			damage.Position = Position;
			player.TakeDamage( damage );
			EmitSound( "player.ship.damage" );
		}

	}
}
