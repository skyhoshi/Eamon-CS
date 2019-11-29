
// FleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
		/// <summary></summary>
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

				Globals.Engine.CheckNumberOfExits(ActorRoom, ActorMonster, true, ref numExits);

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

					Globals.Engine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref direction);

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

			Globals.Out.Print("Attempting to flee{0}.", Globals.Buf);

			Globals.GameState.R2 = DobjArtifact != null ? 0 : ActorRoom.GetDirs(Direction);

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
			RetCode rc;

			Debug.Assert(Direction == 0);

			if (ActorMonster.ShouldFleeRoom())
			{
				var charMonster = Globals.MDB[Globals.GameState.Cm];

				Debug.Assert(charMonster != null);

				var numExits = 0L;

				Globals.Engine.CheckNumberOfExits(ActorRoom, ActorMonster, true, ref numExits);

				var rl = ActorMonster.GetFleeingMemberCount();

				var monster = Globals.CloneInstance(ActorMonster);

				Debug.Assert(monster != null);

				monster.GroupCount = rl;

				var monsterName = monster.EvalInRoomLightLevel(rl > 1 ? "Unseen entities" : "An unseen entity",
						monster.InitGroupCount > rl ? monster.GetDecoratedName02(true, true, false, false, Globals.Buf) : monster.GetDecoratedName03(true, true, false, false, Globals.Buf));

				if (numExits == 0)
				{
					if (charMonster.IsInRoom(ActorRoom))
					{
						Globals.Out.Print("{0} {1} to flee, but can't find {2}!", monsterName, rl > 1 ? "try" : "tries", ActorRoom.EvalRoomType("an exit", "a path"));

						Globals.Thread.Sleep(Globals.GameState.PauseCombatMs);
					}

					goto Cleanup;
				}

				if (rl < ActorMonster.GroupCount)
				{
					ActorMonster.GroupCount -= rl;

					if (charMonster.IsInRoom(ActorRoom))
					{
						Globals.Out.Print("{0} {1}!", monsterName, rl > 1 ? "flee" : "flees");

						Globals.Thread.Sleep(Globals.GameState.PauseCombatMs);
					}

					if (Globals.Engine.EnforceMonsterWeightLimits)
					{
						rc = ActorMonster.EnforceFullInventoryWeightLimits(recurse: true);

						Debug.Assert(Globals.Engine.IsSuccess(rc));
					}
				}
				else
				{
					Direction direction = 0;

					var found = false;

					var roomUid = 0L;

					Globals.Engine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref direction, ref found, ref roomUid);

					Debug.Assert(Enum.IsDefined(typeof(Direction), direction));

					Debug.Assert(roomUid > 0);

					if (charMonster.IsInRoom(ActorRoom))
					{
						if (direction > Direction.West && direction < Direction.Northeast)
						{
							Globals.Buf.SetFormat(" {0}ward", direction.ToString().ToLower());
						}
						else
						{
							Globals.Buf.SetFormat(" to the {0}", direction.ToString().ToLower());
						}

						Globals.Out.Print("{0} {1}{2}!", monsterName, rl > 1 ? "flee" : "flees", Globals.Buf);

						Globals.Thread.Sleep(Globals.GameState.PauseCombatMs);
					}

					ActorMonster.Location = roomUid;
				}
			}

		Cleanup:

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
			if (CommandParser.CurrToken < CommandParser.Tokens.Length)
			{
				Direction = Globals.Engine.GetDirection(CommandParser.Tokens[CommandParser.CurrToken]);

				if (Direction != 0)
				{
					CommandParser.CurrToken++;
				}
				else if (ActorRoom.IsLit())
				{
					CommandParser.ParseName();

					CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
					{
						a => a.IsInRoom(ActorRoom),
						a => a.IsEmbeddedInRoom(ActorRoom),
						a => a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, Globals.Engine.ExposeContainersRecursively)
					};

					CommandParser.ObjData.ArtifactNotFoundFunc = PrintNothingHereByThatName;

					PlayerResolveArtifact();
				}
				else
				{
					CommandParser.NextState = Globals.CreateInstance<IStartState>();
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
