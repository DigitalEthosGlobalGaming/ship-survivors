﻿using Degg.Core;
using Degg.Entities;
using Degg.Util;
using ShipSurvivors;
using System;
using System.Linq;

//
// You don't need to put things in a namespace, but it doesn't hurt.
//
namespace Sandbox
{

	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class MyGame : DeggGame
	{
		public static RoundManager GetRoundManager()
		{
			var game = Current;
			if (game is MyGame myGame)
			{
				return myGame.Rounds;
			}
			return null;
		}

		[Net]
		public RoundManager Rounds { get; set; }
		public MyGame()
		{
			AdvLog.Info( "TESTING" );
			Rounds = new RoundManager();
		}



		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );
			

			// Create a pawn for this client to play with
			var pawn = new GameLoadingPawn();
			client.Pawn = pawn;
			pawn.Position = Vector3.Zero;
		}


		[Event.Tick.Server]
		public void OnTick()
		{
			var clients = Client.All.Any();

			if ( clients )
			{
				if ( Rounds != null )
				{
					if ( !Rounds.HasStarted )
					{
						Rounds.HasStarted = true;
						Rounds.StartRound();
					}
					Rounds.Tick();
				}
			}
		}
	}

}
