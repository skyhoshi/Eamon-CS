
// FleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : Command, IFleeCommand
	{
		/// <summary>
		/// An event that fires after checking whether exits are available for fleeing, and it resolves that there are.
		/// </summary>
		public const long PpeAfterNumberOfExitsCheck = 1;

		public virtual Direction Direction { get; set; }

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
				var numExits = 0L;

				gEngine.CheckNumberOfExits(ActorRoom, ActorMonster, true, ref numExits);

				if (numExits == 0)
				{
					PrintNoPlaceToGo();

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}
			}

			PlayerProcessEvents(PpeAfterNumberOfExitsCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjArtifact == null)
			{
				if (Direction == 0)
				{
					Direction direction = 0;

					gEngine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref direction);

					Direction = direction;
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

					x.Artifact = DobjArtifact;

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
