﻿
using Sandbox;
using System;

namespace Degg.Cameras {
	public class FollowCamera : CameraMode
	{
		// should only need TargetRotation but I'm shit
		public Angles TargetAngles;
		Rotation TargetRotation;


		private float Distance = 150.0f;
		private float TargetDistance = 150.0f;
		public float MinDistance => 100.0f;
		public float MaxDistance => 300.0f;
		public float DistanceStep => 10.0f;

		public override void Build( ref CameraSetup camSetup )
		{
			base.Build( ref camSetup );
			camSetup.Position = Position;
			camSetup.Rotation = Rotation;
		}

		public override void Update()
		{
			Vector3 position = Vector3.Zero;
			var scale = 1f;
			if ( Entity?.IsValid() ?? false )
			{
				position = Entity.Position;
			}

			Position = position + Vector3.Up * (24 + (scale));
			TargetRotation = Rotation.From( TargetAngles );

			Rotation = Rotation.Slerp( Rotation, TargetRotation, RealTime.Delta * 10.0f );
			TargetDistance = TargetDistance.LerpTo( Distance, RealTime.Delta * 5.0f );
			Position += Rotation.Backward * TargetDistance;

			FieldOfView = 80.0f;
		}

		public override void BuildInput( InputBuilder input )
		{

			Distance = Math.Clamp( Distance + (-input.MouseWheel * DistanceStep), MinDistance, MaxDistance );

			TargetAngles.yaw += input.AnalogLook.yaw;

			TargetAngles.pitch += input.AnalogLook.pitch;

			TargetAngles = TargetAngles.Normal;

			if ( !input.Down( InputButton.PrimaryAttack ) )
				TargetAngles.pitch = TargetAngles.pitch.Clamp( 0, 89 );
		}
	}

}
