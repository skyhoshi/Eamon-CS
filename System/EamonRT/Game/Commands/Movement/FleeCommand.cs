
// FleeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : Command, IFleeCommand
	{
		public long _numExits;

		public Direction _randomDirection;

		public virtual Direction Direction { get; set; }

		/// <summary></summary>
		public virtual long NumExits
		{
			get
			{
				return _numExits;
			}

			set
			{
				_numExits = value;
			}
		}

		/// <summary></summary>
		public virtual Direction RandomDirection
		{
			get
			{
				return _randomDirection;
			}

			set
			{
				_randomDirection = value;
			}
		}

		public override void PlayerExecute()
		{
			Debug.Assert(Direction == 0 || Enum.IsDefined(typeof(Direction), Direction));

			if (DobjArtifact != null && DobjArtifact.DoorGate == null)
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!ActorMonster.CheckNBTLHostility())
			{
				PrintCalmDown();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtifact == null)
			{
				NumExits = 0;

				gEngine.CheckNumberOfExits(ActorRoom, ActorMonster, true, ref _numExits);

				if (NumExits == 0)
				{
					PrintNoPlaceToGo();

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}
			}

			PlayerProcessEvents(EventType.AfterNumberOfExitsCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjArtifact == null)
			{
				if (Direction == 0)
				{
					RandomDirection = 0;

					gEngine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref _randomDirection);

					Direction = RandomDirection;
				}

				Debug.Assert(Enum.IsDefined(typeof(Direction), Direction));
			}

			if (DobjArtifact != null)
			{
				Globals.Buf.SetFormat("{0}", DobjArtifact.GetDoorGateFleeDesc());
			}
			else if (Direction > Direction.West && Direction < Direction.Northeast)
			{
				Globals.Buf.SetFormat(" {0}ward", Direction.ToString().ToLower());
			}
			else
			{
				Globals.Buf.SetFormat(" to the {0}", Direction.ToString().ToLower());
			}

			gOut.Print("Attempting to flee{0}.", Globals.Buf);

			gGameState.R2 = DobjArtifact != null ? 0 : ActorRoom.GetDirs(Direction);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
				{
					x.Direction = Direction;

					x.DoorGateArtifact = DobjArtifact;

					x.Fleeing = true;
				});
			}
		}

		public override void MonsterExecute()
		{
			Debug.Assert(Direction == 0);

			if (ActorMonster.ShouldFleeRoom())
			{
				gEngine.MoveMonsterToRandomAdjacentRoom(ActorRoom, ActorMonster, true, true);
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public FleeCommand()
		{
			SortOrder = 100;

			IsDarkEnabled = true;

			Name = "FleeCommand";

			Verb = "flee";

			Type = CommandType.Movement;
		}
	}
}
