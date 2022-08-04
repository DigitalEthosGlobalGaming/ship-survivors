
using Degg.Cameras;
using Degg.Core;
using Degg.Entities;
using Degg.Util;
using Degg.Util.CurrencySystem;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

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


		public bool IsShootingPrimary { get; set; }
		public bool IsShootingSecondary { get; set; }
		public bool IsUsingSpecial { get; set; }

		public bool IsAccountSetup = false;

		[Net]
		public DeggCurrencyAccount Account { get; set; }

		[Net]
		public ShipResource Resource { get; set; }


		public override void Spawn()
		{
			base.Spawn();
			Entity.SetSpritesheet( "assets/player_ships.sprite" );
			Entity.SetupPhysics();
			Tags.Add( "player" );
		}


		public void LoadFromResource()
		{
			var resource = ShipResource.GetResourceForShipPlayer( this );
			Resource = resource;
			if (resource != null)
			{
				Log.Info( resource.Sprite );
				Entity.SetSprite( resource.Sprite );
				if ( resource.StartingUpgrades != null )
				{
					foreach ( var item in resource.StartingUpgrades )
					{
						var upgrade = UpgradeResource.Get( item );
						BuyUpgrade( upgrade );
					}
				}
			}

		}

		public void Init(ShipResource ship)
		{
			if (IsClient)
			{
				return;
			}
			Resource = ship;
			MaxHealth = 10f;
			Health = 10f;

			LoadFromResource();
			SetupStats();
			if ( DeggGame.IsDevelopment() )
			{
				Health = 5000f;
			}
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

		public void PrimaryAttackDown()
		{
			if (IsServer)
			{
				foreach (var i in Upgrades)
				{
					i.PrimaryAttack();
				}
			}
		}

		public void SecondaryAttackDown()
		{
			if ( IsServer )
			{
				foreach ( var i in Upgrades )
				{
					i.SecondaryAttack();
				}
			}
		}
		public void SpecialDown()
		{
			if ( IsServer )
			{
				foreach ( var i in Upgrades )
				{
					i.Special();
				}
			}
		}



		public override void BuildInput( InputBuilder input )
		{
			base.BuildInput( input );
			input.Position = Cursor;
		}

		public override void ServerTick()
		{
			base.ServerTick();
			foreach ( var i in Upgrades )
			{
				i.ServerTick();
			}
			if (!IsAccountSetup)
			{
				if (Client?.IsValid() ?? false) {
					Account = CurrencySystem.Current?.GetAccount( "cores", this.Client );
					if ( Account?.IsValid() ?? false )
					{
						IsAccountSetup = true;
					}
				}
			}
		}



		public override void ClientTick()
		{
			base.ClientTick();

			foreach ( var i in Upgrades )
			{
				if ( i?.IsValid() ?? false )
				{
					i.ClientTick();
				}
			}

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
				var round = MyGame.GetRoundManager();
				if ( round.IsEnding)
				{
					DebugOverlay.Sphere( Position, (Time.Now - round.RoundEndStartTime) * 100, Color.Blue );
				}
				if ( Input.Down( InputButton.PrimaryAttack ) )
				{
					IsShootingPrimary = true;
					PrimaryAttackDown();
				} else
				{
					IsShootingPrimary = false;
				}

				if ( Input.Down( InputButton.PrimaryAttack ) )
				{
					IsShootingSecondary = true;
					SecondaryAttackDown();
				}
				else
				{
					IsShootingSecondary = false;
				}
				if ( Input.Down( InputButton.Jump ) )
				{
					IsShootingSecondary = true;
					SpecialDown();
				}
				else
				{
					IsShootingSecondary = false;
				}

				if ( MyGame.IsDevelopment() )
				{

					if ( Input.Pressed( InputButton.Slot7 ) )
					{
						AddCores( 1 );
					}

					if ( Input.Pressed( InputButton.Slot9 ) )
					{
						round.EndRound();
					}

					if ( Input.Pressed( InputButton.Slot0 ) )
					{
						round.IsEnding = true;
						round.RoundEndStartTime = Time.Now;
					}
					if ( Input.Pressed( InputButton.Slot8 ) )
					{
						TakeDamage( new DamageInfo()
						{
							Damage = 100000
						} );
					}
				}

			}

			if ( IsServer )
			{
				var currentSpeed = Velocity.Distance( Vector3.Zero );
				var isMoving = false;
				var phys = Entity?.PhysicsBody;

				if ( !(phys?.IsValid() ?? false))
				{
					return;
				}
				var isShooting = IsShootingSecondary || IsShootingPrimary;
				if ( currentSpeed < MaxSpeed )
				{
					var force = Vector3.Zero;
					var forward = Vector3.Forward;
					var backward = Vector3.Backward;
					var left = Vector3.Left;
					var right = Vector3.Right;
					var localMovementSpeed = MovementSpeed;
					if ( isShooting )
					{
						localMovementSpeed = localMovementSpeed / 5;
					}

					if ( Input.Down( InputButton.Forward ) )
					{
						isMoving = true;
						force += forward * 1000f * localMovementSpeed;
					}
					if ( Input.Down( InputButton.Back ) )
					{
						isMoving = true;
						force += backward * 1000f * localMovementSpeed;
					}
					if ( Input.Down( InputButton.Left ) )
					{
						isMoving = true;
						force += left * 1000f * localMovementSpeed;
					}
					if ( Input.Down( InputButton.Right ) )
					{
						isMoving = true;
						force += right * 1000f * localMovementSpeed;
					}
					force = SimulateMove( force );
					phys.ApplyForce( force );
				}
				if ( isMoving == false )
				{
					var vel = Velocity;
					var localSlowdown = 1000f;
					if ( isShooting )
					{
						localSlowdown = localSlowdown * 2;
					}
					phys.ApplyForce( -vel * localSlowdown * Time.Delta );
				}
			}
		}

		public virtual Vector3 SimulateMove( Vector3 force)
		{
			return force;
		}

		public virtual void OnRoundEnd()
		{
			foreach ( var i in Upgrades )
			{
				i.OnRoundEnd();
			}
			GiveRandomUpgrades();
			GetCoresAccount()?.Save();
		}

		public virtual void OnRoundStart()
		{
			foreach ( var i in Upgrades )
			{
				i.OnRoundStart();
			}
			GetCoresAccount()?.Save();
		}

		public virtual List<UpgradeResource> GetUpgradeResources()
		{
			List<UpgradeResource> res = new List<UpgradeResource>();
			if ( Resource?.Upgrades != null )
			{
				foreach ( var upgradePath in Resource?.Upgrades )
				{
					var upgrade = UpgradeResource.Get( upgradePath );
					if ( upgrade != null)
					{
						res.Add( upgrade );
					}
				}
			}

			return res;
		}

		public virtual List<string> GetUpgradeClassNames()
		{
			List<UpgradeResource> upgrades = GetUpgradeResources();
			var res = new List<string>();
			foreach ( var item in upgrades )
			{
				res.Add( item.ClassName );
			}

			return res;
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

		public override void OnTriggerTouch( BaseTrigger trigger )
		{
			base.OnTriggerTouch( trigger );
			if ( trigger.Tags.Has( "out_of_bounds" ) )
			{
			}
		}

		public void AddCores(float amount)
		{
			var account = GetCoresAccount();
			account.AddAmount( amount );
		}

		public DeggCurrencyAccount GetCoresAccount()
		{
			return Account;
		}
	}
}
