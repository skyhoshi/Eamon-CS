
// AttackCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : Command, IAttackCommand
	{
		public virtual bool BlastSpell { get; set; }

		public virtual bool CheckAttack { get; set; }

		public virtual long MemberNumber { get; set; }

		protected override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!BlastSpell && ActorMonster.Weapon <= 0)
			{
				PrintMustFirstReadyWeapon();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtifact != null)
			{
				var artClasses = new Enums.ArtifactType[] { Enums.ArtifactType.DeadBody, Enums.ArtifactType.DisguisedMonster, Enums.ArtifactType.Container, Enums.ArtifactType.DoorGate };

				var ac = DobjArtifact.GetArtifactClass(artClasses, false);

				if (ac != null)
				{
					if (ac.Type == Enums.ArtifactType.DeadBody)
					{
						if (BlastSpell)
						{
							Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.Engine.BlastDesc);
						}

						DobjArtifact.SetInLimbo();

						Globals.Out.Write("{0}You {1} {2} to bits!{0}",
							Environment.NewLine,
							BlastSpell ? "blast" : "hack",
							DobjArtifact.EvalPlural("it", "them"));

						goto Cleanup;
					}

					if (ac.Type == Enums.ArtifactType.DisguisedMonster)
					{
						Globals.RtEngine.RevealDisguisedMonster(DobjArtifact);

						var monster = Globals.MDB[ac.Field5];

						Debug.Assert(monster != null);

						ICommand command = null;

						if (BlastSpell)
						{
							command = Globals.CreateInstance<IBlastCommand>(x =>
							{
								x.CastSpell = false;
							});
						}
						else
						{
							command = Globals.CreateInstance<IAttackCommand>();
						}

						CopyCommandData(command);

						command.DobjArtifact = null;

						command.DobjMonster = monster;

						NextState = command;

						goto Cleanup;
					}

					/*
						Damage it...
					*/

					var keyUid = ac.GetKeyUid();

					if (keyUid == -2)
					{
						Globals.Out.Write("{0}You already broke {1}!{0}", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"));

						goto Cleanup;
					}

					var breakageStrength = ac.GetBreakageStrength();

					if (breakageStrength < 1000)
					{
						Globals.Out.WriteLine("{0}Nothing happens.", Environment.NewLine);

						goto Cleanup;
					}

					var d = 0L;

					var s = 0L;

					if (BlastSpell)
					{
						d = 2;

						s = 5;

						Globals.Buf.SetFormat("{0}{1}{0}", Environment.NewLine, Globals.Engine.BlastDesc);
					}
					else
					{
						var weapon = Globals.ADB[ActorMonster.Weapon];

						Debug.Assert(weapon != null);

						var weaponAc = weapon.GetArtifactClass(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon });

						Debug.Assert(weaponAc != null);

						d = weaponAc.Field7;

						s = weaponAc.Field8;

						Globals.Buf.SetFormat("{0}Wham! You hit {1}!{0}", Environment.NewLine, DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf01));
					}

					Globals.Out.Write("{0}", Globals.Buf);

					var rl = 0L;

					rc = Globals.Engine.RollDice(d, s, 0, ref rl);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					breakageStrength -= rl;

					if (breakageStrength > 1000)
					{
						ac.SetBreakageStrength(breakageStrength);

						rc = DobjArtifact.SyncArtifactClasses(ac);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						goto Cleanup;
					}

					/*
						Broken!
					*/

					ac.SetOpen(true);

					ac.SetKeyUid(-2);

					ac.Field8 = 0;

					rc = DobjArtifact.SyncArtifactClasses(ac);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					DobjArtifact.Value = 0;

					rc = DobjArtifact.AddStateDesc(Globals.Engine.BrokenDesc);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Buf.SetFormat("{0}{1} {2} to pieces", 
						Environment.NewLine, 
						DobjArtifact.GetDecoratedName03(true, true, false, false, Globals.Buf01),
						DobjArtifact.EvalPlural("smashes", "smash"));

					if (ac.Type == Enums.ArtifactType.Container)
					{
						var artifactList = DobjArtifact.GetContainedList();

						foreach (var artifact in artifactList)
						{
							artifact.SetInRoom(ActorRoom);
						}

						if (artifactList.Count > 0)
						{
							Globals.Buf.AppendFormat("; {0} contents spill to the {1}",
								DobjArtifact.EvalPlural("its", "their"),
								ActorRoom.Type == Enums.RoomType.Indoors ? "floor" : "ground");
						}

						ac.Field7 = 0;
					}

					Globals.Buf.AppendFormat("!{0}", Environment.NewLine);

					Globals.Out.Write("{0}", Globals.Buf);
				}
				else
				{
					PrintWhyAttack(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}
			}
			else
			{
				if (!CheckAttack && DobjMonster.Friendliness != Enums.Friendliness.Enemy)
				{
					Globals.Out.Write("{0}Attack non-enemy (Y/N): ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
					{
						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					CheckAttack = true;

					Globals.RtEngine.MonsterGetsAggravated(DobjMonster);
				}

				var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.OfMonster = ActorMonster;

					x.DfMonster = DobjMonster;

					x.MemberNumber = MemberNumber;

					x.BlastSpell = BlastSpell;
				});

				combatSystem.ExecuteAttack();
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void MonsterExecute()
		{
			Debug.Assert(DobjMonster != null);

			var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.OfMonster = ActorMonster;

				x.DfMonster = DobjMonster;

				x.MemberNumber = MemberNumber;
			});

			combatSystem.ExecuteAttack();

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
			CommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch03;

			CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom)
			};

			CommandParser.ObjData.ArtifactNotFoundFunc = PrintNobodyHereByThatName;

			PlayerResolveMonster();
		}

		public AttackCommand()
		{
			SortOrder = 250;

			Name = "AttackCommand";

			Verb = "attack";

			Type = Enums.CommandType.Interactive;

			MemberNumber = 1;
		}
	}
}
