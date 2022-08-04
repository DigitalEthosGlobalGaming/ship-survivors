using Degg.Cameras;
using Degg.Core;
using Sandbox;
using System;

namespace Degg.Entities
{
	public partial class Pawn2D : DeggPlayer
	{
		[Net]
		public Entity2D Entity { get; set; }


		public float ZIndex { get; set; }
		/// <summary>
		/// Called when the entity is first created 
		/// </summary>
		public override void Spawn()
		{
			base.Spawn();
			SetCamera<TopdownCamera>();
			EnableDrawing = true;
			Entity = new Entity2D();
			Entity.Owner = this;
			Entity.Parent = this;
		}


		public override void Simulate( Client cl )
		{
			if ( Position.z != ZIndex )
			{
				Position = Position.WithZ( ZIndex );
			}
			base.Simulate( cl );

		}


		public void LookAt( float x, float y, float rotateAmount = 1f , float degreeOffset = 90f )
		{
			Entity?.LookAt( x, y, rotateAmount, degreeOffset );
		}

		public Rotation GetRotationLookingAt( Vector3 pos, float rotateAmount, float degreeOffset = 90f )
		{
			return GetRotationLookingAt( pos.x, pos.y, rotateAmount, degreeOffset );
		}

		public Rotation GetRotationLookingAt(float x, float y, float rotateAmount, float degreeOffset = 90f )
		{
			return Entity?.GetRotationLookingAt( x, y, rotateAmount, degreeOffset) ?? new Rotation();
		}

		public override void Touch( Entity other )
		{
			base.Touch( other );
			if (other is BaseTrigger trigger)
			{
				OnTriggerTouch( trigger );
			}
		}

		public virtual void OnTriggerTouch(BaseTrigger trigger)
		{

		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			if ( IsServer )
			{
				Log.Info( "Entity" );
				Entity?.Delete();
			}
		}
	}
}
