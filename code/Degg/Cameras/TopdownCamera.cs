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

		public float ShakeAmount { get; set; }

		public float NextShake { get; set; }

		public Vector3 TargetPosition { get; set; }

		public bool MatchRotation { get; set; }

		public override void Build( ref CameraSetup camSetup )
		{
			base.Build( ref camSetup );
			camSetup.Position = Position;
			camSetup.Rotation = Rotation;
		}

		public void Shake( float amount )
		{
			ShakeAmount = amount;
			Position = Position.WithX( Position.x + Rand.Float( -amount, amount ) ).WithY( Position.y +  Rand.Float( -amount, amount ) );
		}
		public override void Update()
		{
			if ( Entity?.IsValid() ?? false )
			{
				TargetPosition = Entity.Position;
			}

			TargetPosition = TargetPosition + Vector3.Up * Distance;
			Position = Position.LerpTo( TargetPosition, 15f * Time.Delta );
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
