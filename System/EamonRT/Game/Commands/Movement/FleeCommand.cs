
// FleeCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
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
		public virtual Enums.Direction Direction { get; set; }

		protected virtual void PlayerProcessEvents()
		{

		}

		protected override void PlayerExecute()
		{
			Debug.Assert(Direction == 0 || Enum.IsDefined(typeof(Enums.Direction), Direction));

			if (!Globals.RtEngine.CheckNBTLHostility(ActorMonster))
			{
				PrintCalmDown();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			var numExits = 0L;

			Globals.RtEngine.CheckNumberOfExits(ActorRoom, ActorMonster, true, ref numExits);
			
			if (numExits == 0)
			{
				PrintNoPlaceToGo();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			PlayerProcessEvents();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (Direction == 0)
			{
				Enums.Direction direction = 0;

				Globals.RtEngine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref direction);

				Direction = direction;
			}

			Debug.Assert(Enum.IsDefined(typeof(Enums.Direction), Direction));

			if (Direction > Enums.Direction.West && Direction < Enums.Direction.Northeast)
			{
				Globals.Buf.SetFormat("{0}ward", Direction.ToString().ToLower());
			}
			else
			{
				Globals.Buf.SetFormat("to the {0}", Direction.ToString().ToLower());
			}

			Globals.Out.Write("{0}Attempting to flee {1}.{0}", Environment.NewLine, Globals.Buf);

			Globals.GameState.R2 = ActorRoom.GetDirs(Direction);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
				{
					x.Direction = Direction;

					x.Fleeing = true;
				});
			}
		}

		protected override void MonsterExecute()
		{
			RetCode rc;

			Debug.Assert(Direction == 0);

			if (Globals.RtEngine.CheckNBTLHostility(ActorMonster))
			{
				var charMonster = Globals.MDB[Globals.GameState.Cm];

				Debug.Assert(charMonster != null);

				var monsters = Globals.Engine.GetMonsterList(() => true, m => m.IsInRoom(ActorRoom) && m != ActorMonster);

				var numExits = 0L;

				Globals.RtEngine.CheckNumberOfExits(ActorRoom, ActorMonster, true, ref numExits);

				var rl = 0L;

				rc = Globals.Engine.RollDice(1, ActorMonster.GroupCount, 0, ref rl);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var monster = Globals.CloneInstance(ActorMonster);

				Debug.Assert(monster != null);

				monster.GroupCount = rl;

				var monsterName = monster.EvalInRoomLightLevel(rl > 1 ? "Unseen entities" : "An unseen entity",
						monster.InitGroupCount > rl ? monster.GetDecoratedName02(true, true, false, false, Globals.Buf) : monster.GetDecoratedName03(true, true, false, false, Globals.Buf));

				if (numExits == 0)
				{
					var roomType = Globals.Engine.GetRoomTypes(ActorRoom.Type);

					Debug.Assert(roomType != null);

					if (monsters.Contains(charMonster))
					{
						Globals.Out.Write("{0}{1} {2} to flee, but can't find {3}!{0}", Environment.NewLine, monsterName, rl > 1 ? "try" : "tries", roomType.FleeDesc);
					}

					goto Cleanup;
				}

				Globals.GameState.ModNBTL(ActorMonster.Friendliness, -ActorMonster.Hardiness * rl);

				if (rl < ActorMonster.GroupCount)
				{
					ActorMonster.GroupCount -= rl;

					if (monsters.Contains(charMonster))
					{
						Globals.Out.Write("{0}{1} {2}!{0}", Environment.NewLine, monsterName, rl > 1 ? "flee" : "flees");
					}

					if (Globals.RtEngine.EnforceMonsterWeightLimits)
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

					Globals.RtEngine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref direction, ref found, ref roomUid);

					Debug.Assert(Enum.IsDefined(typeof(Enums.Direction), direction));

					Debug.Assert(roomUid > 0);

					if (monsters.Contains(charMonster))
					{
						if (direction > Enums.Direction.West && direction < Enums.Direction.Northeast)
						{
							Globals.Buf.SetFormat("{0}ward", direction.ToString().ToLower());
						}
						else
						{
							Globals.Buf.SetFormat("to the {0}", direction.ToString().ToLower());
						}

						Globals.Out.Write("{0}{1} {2} {3}!{0}", Environment.NewLine, monsterName, rl > 1 ? "flee" : "flees", Globals.Buf);
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

				CommandParser.CurrToken++;
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
