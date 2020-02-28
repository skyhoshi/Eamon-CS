
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

			if (gDobjArtifact != null && gDobjArtifact.DoorGate == null)
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!gActorMonster.CheckNBTLHostility())
			{
				PrintCalmDown();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (gDobjArtifact == null)
			{
				var numExits = 0L;

				gEngine.CheckNumberOfExits(gActorRoom, gActorMonster, true, ref numExits);

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

			if (gDobjArtifact == null)
			{
				if (Direction == 0)
				{
					Direction direction = 0;

					gEngine.GetRandomMoveDirection(gActorRoom, gActorMonster, true, ref direction);

					Direction = direction;
				}

				Debug.Assert(Enum.IsDefined(typeof(Direction), Direction));
			}

			if (gDobjArtifact != null)
			{
				Globals.Buf.SetFormat("{0}", gDobjArtifact.GetDoorGateFleeDesc());
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

			gGameState.R2 = gDobjArtifact != null ? 0 : gActorRoom.GetDirs(Direction);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
				{
					x.Direction = Direction;

					x.Artifact = gDobjArtifact;

					x.Fleeing = true;
				});
			}
		}

		public override void MonsterExecute()
		{
			Debug.Assert(Direction == 0);

			if (gActorMonster.ShouldFleeRoom())
			{
				gEngine.MoveMonsterToRandomAdjacentRoom(gActorRoom, gActorMonster, true, true);
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public override void PlayerFinishParsing()
		{
			if (gCommandParser.CurrToken < gCommandParser.Tokens.Length)
			{
				Direction = gEngine.GetDirection(gCommandParser.Tokens[gCommandParser.CurrToken]);

				if (Direction != 0)
				{
					gCommandParser.CurrToken++;
				}
				else if (gActorRoom.IsLit())
				{
					gCommandParser.ParseName();

					gCommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
					{
						a => a.IsInRoom(gActorRoom),
						a => a.IsEmbeddedInRoom(gActorRoom),
						a => a.IsCarriedByContainerContainerTypeExposedToRoom(gActorRoom, gEngine.ExposeContainersRecursively)
					};

					gCommandParser.ObjData.ArtifactNotFoundFunc = PrintNothingHereByThatName;

					PlayerResolveArtifact();
				}
				else
				{
					gCommandParser.NextState = Globals.CreateInstance<IStartState>();
				}
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
