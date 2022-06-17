﻿
using Degg.Cameras;
using Degg.Entities;
using Degg.Util;
using Sandbox;
using System;

namespace ShipSurvivors
{
	public partial class ShipPlayer: Pawn2D
	{
		[Net]
		public float Coins { get; set; }

		public float MaxSpeed { get; set; }

		[Net]
		public float TurnSpeed { get; set; }

		public Vector3 Cursor { get; set; }

		public float NextShootTime { get; set; }

		public float MouseSensitivity { get; set; }

		[Net]
		public Rotation TargetRotation { get; set; }


		public override void Spawn()
		{
			base.Spawn();
			Init();
			MaxHealth = 10f;
			Health = 10f;
		}

		public override void Respawn()
		{
			base.Respawn();
			Init();
		}

		public void Init()
		{
			SetShape( Entity2DShapes.Square, 0.5f );
			CollisionGroup = CollisionGroup.Trigger;
			SetupStats();
			Health = 5000f;
			Coins = 0f;
		}

		public override void ClientSpawn()
		{
			base.ClientSpawn();
			MouseSensitivity = 10f;
		}

		public override void OnKilled()
		{
			base.OnKilled();
			Delete();
			var pawn = CreateByName<DeadPlayerPawn>( "DeadPlayerPawn" );
			Client.Pawn = pawn;
			pawn.RoundsSurvived = MyGame.GetRoundManager().Difficulty.Difficulty;
		}


		public void SetupStats()
		{
			UpdateStats();
		}

		public void FireBullet()
		{
			if (IsServer)
			{
				foreach (var i in Upgrades)
				{
					i.Fire();
				}
			}
		}



		public override void BuildInput( InputBuilder input )
		{
			base.BuildInput( input );
			input.Position = Cursor;
		}

		public override void ClientTick()
		{
			base.ClientTick();
			if ( MouseSensitivity <= 0 )
			{
				MouseSensitivity = 1f;
			}

			var temp = Input.MouseDelta;
			Cursor = Cursor + (new Vector3( -temp.y, -temp.x, 0 ) * Time.Delta * MouseSensitivity);

			var maxDistance = 150f;
			if ( Cursor.Distance( Vector3.Zero ) > maxDistance )
			{
				Cursor = Cursor.Normal * maxDistance;
			}
		}


		public override void Simulate( Client cl )
		{
			base.Simulate( cl );
			
			var cursor = Input.Position;

			LookAt( cursor.x, cursor.y, 0 );

			if ( IsServer )
			{
				if ( Input.Down( InputButton.PrimaryAttack ) )
				{
					FireBullet();
				}

				if ( Input.Pressed( InputButton.Slot9 ) )
				{
					var round = MyGame.GetRoundManager();
					round.EndRound();
				}
				if ( Input.Pressed( InputButton.Slot8 ) )
				{					
					TakeDamage( new DamageInfo()
					{
						Damage = 100000
					} );
				}

			}

			if ( IsServer )
			{
				var currentSpeed = Velocity.Distance( Vector3.Zero );
				var isMoving = false;

				if ( !(PhysicsBody?.IsValid() ?? false))
				{
					return;
				}
				if ( currentSpeed < MaxSpeed )
				{
					var force = Vector3.Zero;
					var forward = Vector3.Forward;
					var backward = Vector3.Backward;
					var left = Vector3.Left;
					var right = Vector3.Right;

					if ( Input.Down( InputButton.Forward ) )
					{
						isMoving = true;
						force += forward * 1000f * MovementSpeed;
					}
					if ( Input.Down( InputButton.Back ) )
					{
						isMoving = true;
						force += backward * 1000f * MovementSpeed;
					}
					if ( Input.Down( InputButton.Left ) )
					{
						isMoving = true;
						force += left * 1000f * MovementSpeed;
					}
					if ( Input.Down( InputButton.Right ) )
					{
						isMoving = true;
						force += right * 1000f * MovementSpeed;
					}
					PhysicsBody.ApplyForce( force );
				}
				if ( isMoving == false )
				{
					var vel = Velocity;
					PhysicsBody.ApplyForce( -vel * 1000f * Time.Delta );
				}
			}
		}

		public void OnRoundEnd()
		{
			GiveRandomUpgrades();
		}

		public void OnRoundStart()
		{
			// GiveRandomUpgrades();
		}

		[ClientRpc]
		public void ClientOnRoundEnd()
		{
			Event.Run( "ss.rounds.end" );
		}

		[ClientRpc]
		public void ClientOnRoundStart()
		{
			Event.Run( "ss.rounds.start" );
		}

		[ClientRpc]
		public void PlaySoundOnClient(string name)
		{ 
			PlaySound( name );
		}

		[ClientRpc]
		public void ScreenShakeOnClient( float amount )
		{
			var camera = GetCamera<TopdownCamera>();
			if (camera != null)
			{
				camera.Shake( amount );
			}
		}





	}
}
