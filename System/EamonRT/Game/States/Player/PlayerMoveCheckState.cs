
// PlayerMoveCheckState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

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
		protected bool _found;

		protected long _roomUid;

		protected virtual IRoom Room { get; set; }

		protected virtual IMonster Monster { get; set; }

		protected virtual long ArtUid { get; set; }

		public virtual Enums.Direction Direction { get; set; }

		public virtual IArtifact Artifact { get; set; }

		public virtual bool Fleeing { get; set; }

		protected virtual void ProcessEvents()
		{

		}

		protected virtual void ProcessEvents01()
		{
			RetCode rc;

			if (Globals.GameState.R2 == Constants.DirectionExit)
			{
				PrintRideOffIntoSunset();

				Globals.Out.Write("{0}Leave this adventure (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					Globals.GameState.Die = 0;

					Globals.ExitType = Enums.ExitType.FinishAdventure;

					Globals.MainLoop.ShouldShutdown = true;
				}
			}
			else
			{
				PrintCantGoThatWay();
			}
		}

		public override void Execute()
		{
			Debug.Assert(Enum.IsDefined(typeof(Enums.Direction), Direction) || Artifact != null);

			Monster = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(Monster != null);

			ProcessEvents();

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
						Globals.Out.Write("{0}{1} block{2} the way!{0}", Environment.NewLine, Artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), Artifact.EvalPlural("s", ""));

						goto Cleanup;
					}
				}
			}

			ProcessEvents01();

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
