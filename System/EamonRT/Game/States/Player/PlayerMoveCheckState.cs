
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : State, IPlayerMoveCheckState
	{
		/// <summary>
		/// This event fires before a check is made to see if the player can move to a room.
		/// </summary>
		public const long PeBeforeCanMoveToRoomCheck = 1;

		/// <summary>
		/// This event fires after a check is made to see if a blocking artifact (for example,
		/// a door) prevents the player's movement.
		/// </summary>
		public const long PeAfterBlockingArtifactCheck = 2;

		public bool _found;

		public long _roomUid;

		public virtual IRoom Room { get; set; }

		public virtual IMonster Monster { get; set; }

		public virtual long ArtUid { get; set; }

		public virtual Enums.Direction Direction { get; set; }

		public virtual IArtifact Artifact { get; set; }

		public virtual bool Fleeing { get; set; }

		public override void ProcessEvents(long eventType)
		{
			RetCode rc;

			var gameState = Globals.Engine.GetGameState();

			Debug.Assert(gameState != null);

			if (eventType == PeBeforeCanMoveToRoomCheck)
			{
				// Special blocking artifacts

				var artifact = Globals.Engine.GetBlockedDirectionArtifact(gameState.Ro, gameState.R2, Direction);

				if (artifact != null)
				{
					PrintObjBlocksTheWay(artifact);

					GotoCleanup = true;
				}
			}
			else if (eventType == PeAfterBlockingArtifactCheck)
			{
				if (gameState.R2 == Constants.DirectionExit)
				{
					PrintRideOffIntoSunset();

					Globals.Out.Write("{0}Leave this adventure (Y/N): ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						gameState.Die = 0;

						Globals.ExitType = Enums.ExitType.FinishAdventure;

						Globals.MainLoop.ShouldShutdown = true;
					}
				}
				else
				{
					PrintCantGoThatWay();
				}
			}
		}

		public override void Execute()
		{
			Debug.Assert(Enum.IsDefined(typeof(Enums.Direction), Direction) || Artifact != null);

			Monster = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(Monster != null);

			ProcessEvents(PeBeforeCanMoveToRoomCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (Globals.GameState.R2 > 0 && Globals.RDB[Globals.GameState.R2] != null)
			{
				if (Monster.CanMoveToRoomUid(Globals.GameState.R2, Fleeing))
				{
					NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
					{
						x.Direction = Direction;

						x.Artifact = Artifact;
					});
				}
				else
				{
					PrintCantVerbThere(Fleeing ? "flee" : "move");
				}

				goto Cleanup;
			}

			Room = Globals.RDB[Globals.GameState.Ro];

			Debug.Assert(Room != null);

			ArtUid = Artifact != null ? Artifact.Uid : Room.GetDirectionDoorUid(Direction);

			if (ArtUid > 0)
			{
				if (Artifact == null)
				{
					Artifact = Globals.ADB[ArtUid];

					Debug.Assert(Artifact != null);
				}

				Globals.Engine.CheckDoor(Room, Artifact, ref _found, ref _roomUid);

				if (_found)
				{
					if (Room.IsLit())
					{
						Globals.Engine.RevealEmbeddedArtifact(Room, Artifact);
					}

					Globals.GameState.R2 = _roomUid;

					if (Globals.GameState.R2 > 0 && Globals.RDB[Globals.GameState.R2] != null)
					{
						NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
						{
							x.Direction = Direction;

							x.Artifact = Artifact;

							x.Fleeing = Fleeing;
						});

						goto Cleanup;
					}
					else if (Globals.GameState.R2 == 0 && Room.IsLit())
					{
						PrintObjBlocksTheWay(Artifact);

						goto Cleanup;
					}
				}
			}

			ProcessEvents(PeAfterBlockingArtifactCheck);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

			Globals.NextState = NextState;
		}

		public PlayerMoveCheckState()
		{
			Name = "PlayerMoveCheckState";
		}
	}
}

/* EamonCsCodeTemplate

// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{

	}
}
EamonCsCodeTemplate */
