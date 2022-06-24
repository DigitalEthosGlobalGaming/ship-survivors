using Degg.Core;
using Degg.Util.RoundSystem;
using Degg.Utils;
using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShipSurvivors
{

	public partial class RoundManager : Round
	{
		public DifficultySystem Difficulty { get; set;}

		public List<EnemyShip> Enemies { get; set; }
		public List<string> EnemiesThatCanSpawn { get; set; }

		public int CurrentExpensiveTickIndex { get; set; }

		public Dictionary<int,string> BossRounds { get; set; }

		public float NextEnemySpawn { get; set; }
		public float SpawnRate { get; set; }
		public float SpawnAmount { get; set; }
		public int MaxEnemiesAlive { get; set; }

		public Timer CheckEnemiesTimer { get; set; }

		[Net]
		public float EnemiesToKill { get; set; }

		public bool HasStarted { get; set; }

		public float NextRoundStartTime { get; set; }

		public bool IsEnding { get; set; }
		public float RoundEndStartTime { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Always;
			if ( IsServer )
			{
				Reset();
			}
		}

		public void UpdateEnemiesThatCanSpawn()
		{
			int d = (int)Difficulty.Difficulty;
			var spawnCombinations = new Dictionary<int, string>();

			var isTesting = false;

			if ( isTesting )
			{
				spawnCombinations[2] = "BossLevel1";
				
			} else {
				spawnCombinations[1] = "CollisionEnemyShip";
				spawnCombinations[2] = "EnemyFighter";
				spawnCombinations[3] = "EnemyFighter";
				spawnCombinations[4] = "CollisionEnemyShip";
				spawnCombinations[5] = "CollisionEnemyShip";
				spawnCombinations[6] = "EnemyFighter";
				spawnCombinations[7] = "CollisionEnemyShip";
				spawnCombinations[8] = "EnemyFighterV2";
				spawnCombinations[9] = "EnemyFighter";
				spawnCombinations[10] = "CollisionEnemyShip";
				spawnCombinations[11] = "EnemyFighterV2";
				spawnCombinations[12] = "EnemyFighterV2";
				spawnCombinations[14] = "EnemyFighterV2";
				spawnCombinations[15] = "EnemyShipLevel3";
				spawnCombinations[16] = "EnemyFighterV2";
				spawnCombinations[17] = "EnemyShipLevel3";
				spawnCombinations[18] = "EnemyFighterV2";
				spawnCombinations[19] = "EnemyFighterV2";
				spawnCombinations[20] = "CollisionEnemyShipLevel2";
				spawnCombinations[21] = "EnemyFighterV2";
				spawnCombinations[25] = "CollisionEnemyShipLevel2";
				spawnCombinations[26] = "CollisionEnemyShipLevel2";
				spawnCombinations[27] = "CollisionEnemyShipLevel2";
				spawnCombinations[28] = "EnemyFighterLevel3";
				spawnCombinations[29] = "EnemyFighterLevel3";
				spawnCombinations[30] = "EnemyFighterLevel3";
				spawnCombinations[31] = "EnemyFighterLevel3";
				spawnCombinations[32] = "EnemyFighterLevel3";
				spawnCombinations[33] = "EnemyFighterLevel3";
				spawnCombinations[34] = "EnemyFighterLevel3";
				spawnCombinations[35] = "EnemyFighterLevel3";
			}


			if (spawnCombinations.ContainsKey(d))
			{
				EnemiesThatCanSpawn.Add( spawnCombinations[d] );
			}
		}

		public bool AreAllPlayersDead()
		{
			var shipPlayer = ShipPlayer.GetAllPlayers<ShipPlayer>();
			var deadPlayers = ShipPlayer.GetAllPlayers<DeadPlayerPawn>();
			if ( shipPlayer.Count() == 0 && deadPlayers.Count() > 0)
			{
				return true;
			}
			return false;
		}
		public bool AreAllPlayersDeadAndReady()
		{

			if ( AreAllPlayersDead())
			{
				var areAllReady = true;
				var deadPlayers = ShipPlayer.GetAllPlayers<DeadPlayerPawn>();
				if ( deadPlayers.Count > 0 )
				{
					foreach ( var i in deadPlayers )
					{
						if ( !i.ReadyToReset )
						{
							areAllReady = false;
						}
					}
				}
				return areAllReady;
			}
			return false;
		}

		public void CheckGameState( Timer t )
		{
			if ( IsServer )
			{
				if (AreAllPlayersDead())
				{
					EndRound();
					State = RoundState.Warmup;
					if (AreAllPlayersDeadAndReady())
					{
						var deadPlayers = ShipPlayer.GetAllPlayers<DeadPlayerPawn>();
						foreach ( var i in deadPlayers )
						{
							i.Reset();
						}

						Reset();
					}
				}

				if (State == RoundState.Warmup)
				{
					CheckRoundStart();
				}
				UpdateEnemiesAlive();
			}
		}

		public void Reset()
		{
			if ( Difficulty == null)
			{
				Difficulty = new DifficultySystem();
			}
			if ( CheckEnemiesTimer == null )
			{
				CheckEnemiesTimer = new Timer( CheckGameState, 1000f );
			}

			Difficulty.SetDifficulty( 0 );
			
			Difficulty.AddItem( "MaxEnemiesAlive", 1.25f );
			Difficulty.AddItem( "SpawnAmount", 1 );

			Enemies = new List<EnemyShip>();

			CheckEnemiesTimer.Start();

			if ( BossRounds == null )
			{
				BossRounds = new Dictionary<int, string>();
				BossRounds[30] = "BossLevel1";
			}
			EnemiesThatCanSpawn = new List<string>();

		}

		public void DeleteBullets()
		{
			var bullets = All.OfType<Bullet>();
			foreach ( var item in bullets )
			{
				item.Delete();
			}
		}

		public void DeleteEnemies()
		{
			var entities = All.OfType<EnemyShip>();
			foreach ( var item in entities )
			{
				item.Delete();
			}
			Enemies = new List<EnemyShip>();
		}

		public void CheckRoundStart()
		{
			var players = DeggPlayer.GetAllPlayers<ShipPlayer>();
			var canStart = true;
			var isPlayers = false;
			foreach ( var item in players )
			{
				isPlayers = true;
				if (item.UpgradesToBuy.Count != 0)
				{
					canStart = false;
				}
			}

			if ( canStart && isPlayers )
			{
				StartRound();
			}
		}

		public override void OnRoundStart()
		{
			NextEnemySpawn = 0;
			IsEnding = false;
			Difficulty.UpdateDifficulty( 1 );
			var d = (int) Difficulty.Difficulty;
			MaxEnemiesAlive = (int) Difficulty.GetValue( "MaxEnemiesAlive", 2);
			if ( MaxEnemiesAlive < 2)
			{
				MaxEnemiesAlive = 2;
			}
			SpawnAmount =  Difficulty.GetValue( "SpawnAmount", 0.5f );
			SpawnRate = Difficulty.GetValue( "EnemySpawnRate", 2f );
			EnemiesToKill = MaxEnemiesAlive * 2;
			UpdateEnemiesThatCanSpawn();
			base.OnRoundStart();
			if ( BossRounds.ContainsKey( d ))
			{
				var bossSpawn = GetValidSpawnPoint();
				if ( bossSpawn.HasValue )
				{
					var boss = CreateByName<EnemyShip>( BossRounds[d] );
					boss.Position = bossSpawn.Value;
					Enemies.Add( boss );
				}
			}

			var players = DeggPlayer.GetAllPlayers<ShipPlayer>();
			foreach ( var player in players )
			{
				player.OnRoundStart();
				player.ClientOnRoundStart();
			}
		}

		public override void OnRoundEnd()
		{
			base.OnRoundEnd();
			DeleteBullets();
			DeleteEnemies();
			var players = DeggPlayer.GetAllPlayers<ShipPlayer>();
			foreach(var player in players)
			{
				player.OnRoundEnd();
				player.ClientOnRoundEnd();
			}
			NextRoundStartTime = Time.Now + 3000f;
		}

		public void UpdateEnemiesAlive()
		{
			var newEnemies = new List<EnemyShip>();
			var isBossAlive = false;
			foreach ( var enemy in Enemies )
			{
				if ( !(enemy?.IsValid() ?? false) )
				{
					EnemiesToKill = EnemiesToKill - 1;
				} else
				{
					if (enemy.IsBoss)
					{
						isBossAlive = true;
					}
					newEnemies.Add( enemy );
				}
			}
			Enemies = newEnemies;
			if ( isBossAlive )
			{
				return;
			}
			if (EnemiesToKill <= 0 && IsEnding == false)
			{
				RoundEndStartTime = Time.Now;
				IsEnding = true;
			}
		}

		public override bool CanRoundEnd()
		{
			if ( Enemies.Count() <= 0 && IsEnding)
			{
				return true;
			}
			return false;
		}

		public override void OnEndTick()
		{
			if ( NextRoundStartTime < Time.Now )
			{
				StartRound();
			}
		}

		public override void InProgressTick()
		{
			if ( CurrentExpensiveTickIndex >= Enemies.Count)
			{
				CurrentExpensiveTickIndex = 0;
			}
			if ( Enemies.Count > 0 ) {
				var enemy = Enemies[CurrentExpensiveTickIndex];
				if ( enemy?.IsValid ?? false )
				{
					enemy.ExpensiveTick();
				}
				CurrentExpensiveTickIndex = CurrentExpensiveTickIndex + 1;
			}
			
			if (NextEnemySpawn < Time.Now)
			{
				var spawnRate = 5f;
				NextEnemySpawn = Time.Now + spawnRate;
				
				var spawnAmount = (int) Math.Ceiling(SpawnAmount);
				if ( spawnAmount < 1)
				{
					spawnAmount = 1;
				}
				for ( int i = 0; i < spawnAmount; i++ )
				{
					SpawnEnemy();
				}
			}
		}

		public Vector3? GetValidSpawnPoint()
		{
			var maxLoop = 1000;
			for ( int i = 0; i < maxLoop; i++ )
			{
				var player = DeggPlayer.GetRandomPlayer<ShipPlayer>();
				if ( player?.IsValid() ?? false )
				{
					var randomDirection = Rand.Float( 0, 360 );
					var distance = Rand.Float( 150, 200 );
					var position = player.Position + Rotation.FromAxis( Vector3.Up, randomDirection ).Forward * distance;

					var closestPlayer = DeggPlayer.GetClosestPlayer<ShipPlayer>( position );
					if ( closestPlayer.Key > 100 )
					{
						return position;
					}
				}
			}
			return null;
		}
		public void SpawnEnemy()
		{
			if ( IsEnding )
			{
				return;
			}
			var spawnPosition = GetValidSpawnPoint();
			if ( spawnPosition.HasValue)
			{
				if ( Enemies.Count() <= MaxEnemiesAlive )
				{
					string enemyClassName = Rand.FromList( EnemiesThatCanSpawn );
					var enemy = CreateByName<EnemyShip>( enemyClassName );
					enemy.Position = spawnPosition.Value;
					Enemies.Add( enemy );
				}
			}
		}
	}

}
