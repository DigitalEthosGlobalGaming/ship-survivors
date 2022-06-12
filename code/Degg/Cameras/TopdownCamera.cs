using Sandbox;
using System;

namespace Degg.Cameras {
	public partial class TopdownCamera : CameraMode
	{

		// should only need TargetRotation but I'm shit
		public Angles TargetAngles;
		Rotation TargetRotation;

		public float Angle { get; set; }


		private float Distance = 150.0f;
		private float TargetDistance = 150.0f;
		public float MinDistance => 20.0f;
		public float MaxDistance => 150.0f;
		public float DistanceStep => 10.0f;

		public bool MatchRotation { get; set; }

		public override void Build( ref CameraSetup camSetup )
		{
			base.Build( ref camSetup );
			camSetup.Position = Position;
			camSetup.Rotation = Rotation;
		}

		public override void Update()
		{
			Vector3 position = Vector3.Zero;

			if ( Entity?.IsValid() ?? false )
			{


				position = Entity.Position;

			}
			Position = position + Vector3.Up * Distance;
			TargetRotation = Rotation.FromPitch( 90 );


			Rotation = Rotation.Slerp( Rotation, TargetRotation, RealTime.Delta * 10.0f );
			TargetDistance = TargetDistance.LerpTo( Distance, RealTime.Delta * 5.0f );

			FieldOfView = 80.0f;
		}

		public override void BuildInput( InputBuilder input )
		{

			Distance = Math.Clamp( Distance + (-input.MouseWheel * DistanceStep), MinDistance, MaxDistance );
		}

	}
}
