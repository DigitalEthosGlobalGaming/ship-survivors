using Sandbox;
using System;

namespace Degg.Entities
{
	public enum Entity2DShapes {
		Circle,
		Square,
		Other
	}
	public partial class Entity2D : ModelEntity
	{
		public const float DefaultEntityMass = 10f;
		public const float DefaultEntitySize = 10f;
		[Net,Change]
		public string EntityMaterial { get; set; }

		public string ClientEntityMaterial { get; set; }

		public float RotationDegrees { get; set; }

		public float ZIndex { get; set; }

		public void OnEntityMaterialChanged( string before, string after)
		{
			var mat = Material.Load( after );		
			
			SetMaterialOverride( mat );
		}

		/// <summary>
		/// Called when the entity is first created 
		/// </summary>
		public override void Spawn()
		{
			base.Spawn();
			SetModel( "models/base/mesh_10x10.vmdl" );
		}

		public void SetShape( Entity2DShapes shape, float scale = 1f)
		{
			switch ( shape )
			{
				case Entity2DShapes.Square:
					var a = DefaultEntitySize / 2;
					a = a * scale;
					SetupPhysicsFromOBB( PhysicsMotionType.Dynamic, -a, a );
					break;
				case Entity2DShapes.Circle:
					SetupPhysicsFromSphere( PhysicsMotionType.Dynamic, Vector3.Zero, 2.5f * scale );
					break;
				case Entity2DShapes.Other:
					break;
			}
			if ( PhysicsBody != null )
			{
				PhysicsBody.Mass = DefaultEntityMass;
				PhysicsBody.GravityEnabled = false;
			}
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();
			if ( EntityMaterial != null )
			{
				SetMaterialOverride( EntityMaterial );
			}
		}

		[Event.Tick]
		public void Tick()
		{
			if ( IsServer )
			{
				ServerTick();
			}
			if ( IsClient )
			{
				ClientTick();
			}
		}
		public virtual void ClientTick()
		{
			if ( ClientEntityMaterial != EntityMaterial && EntityMaterial != null)
			{
				SetMaterialOverride( EntityMaterial );
				EntityMaterial = ClientEntityMaterial;
			}
		}

		public virtual void ServerTick()
		{
			if ( Position.z != ZIndex )
			{
				Position = Position.WithZ( ZIndex );
			}

			var rotation = Rotation.FromAxis( Vector3.Up, RotationDegrees );
			Rotation = rotation;
		}

		public void LookAt( Vector3 position, float? rotateAmount = null)
		{
			LookAt( position.x, position.y, rotateAmount );

		}

		public void LookAt( Vector2 position, float? rotateAmount = null )
		{
			LookAt( position.x, position.y, rotateAmount );
		}

		public void LookAt( float x, float y, float? rotateAmount = null)
		{
			Rotation = GetRotationLookingAt( x, y, rotateAmount );
		}

		public Rotation GetRotationLookingAt( Vector3 pos, float? rotateAmount = null, float degreeOffset = 90f )
		{
			return GetRotationLookingAt( pos.x, pos.y, rotateAmount, degreeOffset );
		}

		public Rotation GetRotationLookingAt( float x, float y, float? rotateAmount, float degreeOffset = 90f )
		{
			x = x - Position.x;
			y = y - Position.y;
			float rad = (float)Math.Atan2( -x, y );
			float deg = (float)(rad * (180 / Math.PI)) + degreeOffset;

			var rotation = Rotation.FromAxis( Vector3.Up, deg );
			if ( !rotateAmount.HasValue )
			{
				return rotation;
			}
			else
			{
				var difference = Rotation.Distance( rotation );
				return Rotation.Slerp( Rotation, rotation, (RealTime.Delta * rotateAmount.Value * 100f) / difference );
			}
		}

		[ClientRpc]
		public void EmitSound( string name )
		{
			PlaySound( name );
		}
	}
}
