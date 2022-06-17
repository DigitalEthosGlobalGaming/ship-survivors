using Sandbox;

namespace ShipSurvivors
{
	public class CollisionEnemyShip: EnemyShip
	{

		public override void Spawn()
		{
			base.Spawn();
			EntityMaterial = "materials/ships/star_large.vmat";
		}


		public override void MoveStep()
		{
			if ( Target?.IsValid ?? false )
			{
				var targetV = Target.Position;

				var targetPosition = targetV;
				var forceDirection = GetRotationLookingAt( targetPosition ).Forward;
				Accelleration = 20f;

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
			damage.Damage = 1f;
			damage.Position = Position;
			player.TakeDamage( damage );
			EmitSound( "player.ship.damage" );
		}

	}
}
