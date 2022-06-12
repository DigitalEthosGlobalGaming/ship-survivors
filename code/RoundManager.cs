using Degg.Core;
using Degg.Util.RoundSystem;
using Degg.Utils;
using Sandbox;
using System;
using System.Collections.Generic;

namespace ShipSurvivors
{

	public partial class RoundManager : Round
	{
		public DifficultySystem Difficulty { get; set;}

		public List<EnemyShip> Enemies { get; set; }

		public float NextEnemySpawn { get; set; }
		public float SpawnRate { get; set; }
		public float SpawnAmount { get; set; }
		public int EnemiesSpawned { get; set; }

		public Timer CheckEnemiesTimer { get; set; }

		public float EnemiesAlive { get; set; }

		public bool HasStarted { get; set; }

		public float NextRoundStartTime { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			Transmit = TransmitType.Always;
			if ( IsServer )
			{
				Difficulty = new DifficultySystem();
				Difficulty.AddItem( "EnemiesSpawned", 2 );
				Difficulty.AddItem( "SpawnAmount", 1 );
				Enemies = new List<EnemyShip>();
				CheckEnemiesTimer = new Timer( CheckEnemies, 1000f );
				CheckEnemiesTimer.Start();
			}
		}

		public void CheckEnemies( Timer t )
		{
			if ( IsServer )
			{
				var enemies = 0;
				foreach ( var item in Enemies )
				{
					if ( item?.IsValid() ?? false )
					{
						enemies = enemies + 1;
					}
				}
				EnemiesAlive = enemies;
			}
		}

		public void CheckRoundStart()
		{
			var players = DeggPlayer.GetAllPlayers<ShipPlayer>();
			var canStart = true;
			foreach ( var item in players )
			{
				if (item.UpgradesToBuy.Count != 0)
				{
					canStart = false;
				}
			}

			if ( canStart )
			{
				StartRound();
			}
		}

		public override void OnRoundStart()
		{
			EnemiesSpawned = 0;
			NextEnemySpawn = 0;
			Difficulty.UpdateDifficulty( 1 );
			EnemiesSpawned = (int) Difficulty.GetValue( "EnemiesSpawned", 1);
			if ( EnemiesSpawned  < 2)
			{
				EnemiesSpawned = 2;
			}
			SpawnAmount =  Difficulty.GetValue( "SpawnAmount", 0.5f );
			SpawnRate = Difficulty.GetValue( "EnemySpawnRate", 2f );
			EnemiesAlive = EnemiesSpawned;
			base.OnRoundStart();

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
			var players = DeggPlayer.GetAllPlayers<ShipPlayer>();
			foreach(var player in players)
			{
				player.OnRoundEnd();
				player.ClientOnRoundEnd();
			}
			NextRoundStartTime = Time.Now + 3000f;
		}

		public override bool CanRoundEnd()
		{
			if ( EnemiesSpawned == 0 && EnemiesAlive == 0 )
			{
				return true;
			}
			return false;
		}

		public override void Tick()
		{
			base.Tick();
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
			if (NextEnemySpawn < Time.Now)
			{
				var spawnRate = 5f / Difficulty.Difficulty;
				NextEnemySpawn = Time.Now + spawnRate;
				var spawnAmount = (int) Math.Ceiling(SpawnAmount);
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
					var distance = Rand.Float( 200, 300 );
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
			var spawnPosition = GetValidSpawnPoint();
			if ( spawnPosition.HasValue)
			{
				if ( EnemiesSpawned > 0 )
				{
					EnemiesSpawned = EnemiesSpawned - 1;
					var enemy = new EnemyShip();
					enemy.Position = spawnPosition.Value;
					Enemies.Add( enemy );
				}
			}
		}

	}

}
