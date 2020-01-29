
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : State, IPlayerMoveCheckState
	{
		public const long PeBeforeCanMoveToRoomCheck = 1;

		public const long PeAfterBlockingArtifactCheck = 2;

		public bool _found;

		public long _roomUid;

		public virtual IRoom Room { get; set; }

		public virtual IMonster Monster { get; set; }

		public virtual long ArtUid { get; set; }

		public virtual Direction Direction { get; set; }

		public virtual IArtifact Artifact { get; set; }

		public virtual bool Fleeing { get; set; }

		public override void ProcessEvents(long eventType)
		{
			RetCode rc;

			var gameState = gEngine.GetGameState();

			Debug.Assert(gameState != null);

			if (eventType == PeBeforeCanMoveToRoomCheck)
			{
				// Special blocking artifacts

				var artifact = gEngine.GetBlockedDirectionArtifact(gameState.Ro, gameState.R2, Direction);

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

					gOut.Write("{0}Leave this adventure (Y/N): ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						gameState.Die = 0;

						Globals.ExitType = ExitType.FinishAdventure;

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
			Debug.Assert(Enum.IsDefined(typeof(Direction), Direction) || Artifact != null);

			Monster = gMDB[gGameState.Cm];

			Debug.Assert(Monster != null);

			ProcessEvents(PeBeforeCanMoveToRoomCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (gGameState.R2 > 0 && gRDB[gGameState.R2] != null)
			{
				if (Monster.CanMoveToRoomUid(gGameState.R2, Fleeing))
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

			Room = gRDB[gGameState.Ro];

			Debug.Assert(Room != null);

			ArtUid = Artifact != null ? Artifact.Uid : Room.GetDirectionDoorUid(Direction);

			if (ArtUid > 0)
			{
				if (Artifact == null)
				{
					Artifact = gADB[ArtUid];

					Debug.Assert(Artifact != null);
				}

				gEngine.CheckDoor(Room, Artifact, ref _found, ref _roomUid);

				if (_found)
				{
					if (Room.IsLit())
					{
						gEngine.RevealEmbeddedArtifact(Room, Artifact);
					}

					gGameState.R2 = _roomUid;

					if (gGameState.R2 > 0 && gRDB[gGameState.R2] != null)
					{
						NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
						{
							x.Direction = Direction;

							x.Artifact = Artifact;

							x.Fleeing = Fleeing;
						});

						goto Cleanup;
					}
					else if (gGameState.R2 == 0 && Room.IsLit())
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
