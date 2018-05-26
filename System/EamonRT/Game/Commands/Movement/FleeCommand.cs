
// FleeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : Command, IFleeCommand
	{
		/// <summary>
		/// This event fires after a check has been made to see if exits are available for fleeing,
		/// and it resolves that there are.
		/// </summary>
		protected const long PpeAfterNumberOfExitsCheck = 1;

		public virtual Enums.Direction Direction { get; set; }

		protected virtual void SetDoorGateFleeDesc()
		{
			Globals.Buf.Clear();
		}

		protected virtual bool ShouldMonsterFlee()
		{
			return Globals.Engine.CheckNBTLHostility(ActorMonster);
		}

		protected virtual long GetMonsterFleeingMemberCount()
		{
			return Globals.Engine.RollDice01(1, ActorMonster.GroupCount, 0);
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(Direction == 0 || Enum.IsDefined(typeof(Enums.Direction), Direction));

			if (DobjArtifact != null && !DobjArtifact.IsDoorGate())
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!Globals.Engine.CheckNBTLHostility(ActorMonster))
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
					Enums.Direction direction = 0;

					Globals.Engine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref direction);

					Direction = direction;
				}

				Debug.Assert(Enum.IsDefined(typeof(Enums.Direction), Direction));
			}

			if (DobjArtifact != null)
			{
				SetDoorGateFleeDesc();
			}
			else if (Direction > Enums.Direction.West && Direction < Enums.Direction.Northeast)
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

		protected override void MonsterExecute()
		{
			RetCode rc;

			Debug.Assert(Direction == 0);

			if (ShouldMonsterFlee())
			{
				var charMonster = Globals.MDB[Globals.GameState.Cm];

				Debug.Assert(charMonster != null);

				var monsters = Globals.Engine.GetMonsterList(() => true, m => m.IsInRoom(ActorRoom) && m != ActorMonster);

				var numExits = 0L;

				Globals.Engine.CheckNumberOfExits(ActorRoom, ActorMonster, true, ref numExits);

				var rl = GetMonsterFleeingMemberCount();

				var monster = Globals.CloneInstance(ActorMonster);

				Debug.Assert(monster != null);

				monster.GroupCount = rl;

				var monsterName = monster.EvalInRoomLightLevel(rl > 1 ? "Unseen entities" : "An unseen entity",
						monster.InitGroupCount > rl ? monster.GetDecoratedName02(true, true, false, false, Globals.Buf) : monster.GetDecoratedName03(true, true, false, false, Globals.Buf));

				if (numExits == 0)
				{
					if (monsters.Contains(charMonster))
					{
						Globals.Out.Print("{0} {1} to flee, but can't find {2}!", monsterName, rl > 1 ? "try" : "tries", ActorRoom.EvalRoomType("an exit", "a path"));
					}

					goto Cleanup;
				}

				Globals.GameState.ModNBTL(ActorMonster.Friendliness, -ActorMonster.Hardiness * rl);

				if (Globals.IsRulesetVersion(5))
				{
					Globals.GameState.ModDTTL(ActorMonster.Friendliness, -ActorMonster.DmgTaken);
				}

				if (rl < ActorMonster.GroupCount)
				{
					ActorMonster.GroupCount -= rl;

					if (monsters.Contains(charMonster))
					{
						Globals.Out.Print("{0} {1}!", monsterName, rl > 1 ? "flee" : "flees");
					}

					if (Globals.Engine.EnforceMonsterWeightLimits)
					{
						rc = ActorMonster.EnforceFullInventoryWeightLimits(recurse: true);

						Debug.Assert(Globals.Engine.IsSuccess(rc));
					}
				}
				else
				{
					Enums.Direction direction = 0;

					var found = false;

					var roomUid = 0L;

					Globals.Engine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref direction, ref found, ref roomUid);

					Debug.Assert(Enum.IsDefined(typeof(Enums.Direction), direction));

					Debug.Assert(roomUid > 0);

					if (monsters.Contains(charMonster))
					{
						if (direction > Enums.Direction.West && direction < Enums.Direction.Northeast)
						{
							Globals.Buf.SetFormat(" {0}ward", direction.ToString().ToLower());
						}
						else
						{
							Globals.Buf.SetFormat(" to the {0}", direction.ToString().ToLower());
						}

						Globals.Out.Print("{0} {1}{2}!", monsterName, rl > 1 ? "flee" : "flees", Globals.Buf);
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

		protected override void PlayerFinishParsing()
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
						a => a.IsEmbeddedInRoom(ActorRoom)
					};

					CommandParser.ObjData.ArtifactNotFoundFunc = PrintNothingHereByThatName;

					PlayerResolveArtifact();
				}
				else
				{
					CommandParser.NextState.Discarded = true;

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

			Type = Enums.CommandType.Movement;
		}
	}
}
