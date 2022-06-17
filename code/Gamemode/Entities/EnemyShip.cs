﻿using Degg.Entities;
using Sandbox;
using System.Linq;

namespace ShipSurvivors
{
	public partial class EnemyShip : Entity2D
	{
		public float TurnSpeed { get; set; }
		public float MaxSpeed { get; set; }
		public float Accelleration { get; set; }
		public float NextAttackTime { get; set; }
		public float AttackSpeed { get; set; }
		public ShipPlayer Target { get; set; }
		public override void Spawn()
		{
			base.Spawn();
			SetShape( Entity2DShapes.Square, 0.5f );
			Scale = 0.5f;
			MaxSpeed = 50f;
			Accelleration = 1f;
			Health = 3f;
			RenderColor = Color.Black.WithRed(0.3f);
			NextAttackTime = GetNextAttackTime();
			AttackSpeed = 5f;
		}

		public void FindTarget()
		{
			var target = Rand.FromList( All.OfType<ShipPlayer>().ToList() );
			Target = target;
		}

		public virtual Bullet GetBullet()
		{
			return null;
		}

		public virtual float GetNextAttackTime()
		{
			return Time.Now + (float)Rand.Float(AttackSpeed * 0.75f, AttackSpeed * 1.25f );
		}

		public virtual void TryShoot()
		{

		}

		public virtual void OnShoot(Bullet b)
		{

		}

		public override void ServerTick()
		{
			base.ServerTick();
			MoveStep();
			TryShoot();
		}

		public virtual Vector3 GetLookAtPosition()
		{
			if ( Target?.IsValid() ?? false )
			{
				return Target.Position;
			}
			return this.Position;
		}
		public virtual void MoveStep()
		{
			if ( Target?.IsValid() ?? false ) {
				var targetDistance = 75f;
				var falloffDistance = targetDistance / 2;
				var targetV = Target.Position;
				var targetRelativePosition = GetRotationLookingAt( targetV ).Backward;

				var targetPosition = targetRelativePosition * targetDistance + targetV;
				var forceDirection = GetRotationLookingAt( targetPosition ).Forward;
				var distance = targetPosition.Distance( Position );

				LookAt( GetLookAtPosition() );
		
				if ( distance < falloffDistance )
				{
					PhysicsBody.Velocity = PhysicsBody.Velocity.LerpTo(Vector3.Zero, Accelleration * Time.Delta);
				}
				else
				{
					PhysicsBody.Velocity = PhysicsBody.Velocity + forceDirection * Accelleration;
				}

			} else
			{
				FindTarget();
			}	
		}

		public override void StartTouch( Entity other )
		{
			base.StartTouch( other );
			if (other is ShipPlayer player )
			{
				if ( IsServer )
				{
					OnShipPlayerCollision( player );
				}
			}
		}

		public virtual void OnShipPlayerCollision(ShipPlayer player)
		{

		}

		public override void TakeDamage( DamageInfo info )
		{
			base.TakeDamage( info );
		}

		public override void OnKilled()
		{
			base.OnKilled();
			EmitSound( "enemy.ship.destroy" );
			Delete();

		}


	}
}
