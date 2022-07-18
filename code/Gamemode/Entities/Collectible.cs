using Degg.Entities;
using Sandbox;
using System;

namespace ShipSurvivors
{
	public partial class Collectible : Entity2D
	{

		public float TimeToLive { get; set; }
		public override void Spawn()
		{
			base.Spawn();
			Tags.Add( "collectible" );
			EntityMaterial = "materials/ships/other/collectables.vmat";			
			SetShape(Entity2DShapes.Circle, 0.1f);
			TimeToLive = Time.Now + 25f;
		}

		public override void StartTouch( Entity other )
		{
			base.StartTouch( other );
			if ( other is ShipPlayer player )
			{
				if ( IsServer )
				{
					OnPickup( player );
					player.GetCoresAccount().AddAmount( 1 );
					Delete();
				}
			}
		}

		public virtual void OnPickup( ShipPlayer player )
		{

		}



		public override void ServerTick()
		{
			base.ServerTick();
			float rad = (float)(Time.Tick % 360 * Math.PI) / 180;
			var position = Vector2.FromRadians( rad );

			LookAt( Position + (Vector3.Zero.WithX( position.x ).WithY( position.y )) );

			if ( TimeToLive < Time.Now)
			{
				Delete();
			}

			var closestPlayer = GetClosestPawn<ShipPlayer>();
			if ( closestPlayer?.IsValid() ?? false )
			{
				var maxDistance = 15;
				var distance = closestPlayer.Position.Distance( Position );
				if ( distance < maxDistance )
				{
					PhysicsBody.Velocity = (closestPlayer.Position - Position) * Time.Delta * ((maxDistance / (distance + 0.01f)) * 250);
				}
			}
		}
	}
}
