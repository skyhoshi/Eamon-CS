
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
using Classes = Eamon.Framework.Primitive.Classes;
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

		public virtual long AttackNumber { get; set; }

		public virtual void PrintHackToBits()
		{
			Globals.Out.Print("You {0} {1} to bits!", BlastSpell ? "blast" : "hack", DobjArtifact.EvalPlural("it", "them"));
		}

		public virtual void BuildWhamHitObj()
		{
			Globals.Buf.SetPrint("Wham!  You hit {0}!", DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf01));
		}

		public virtual void BuildSmashesToPieces()
		{
			Globals.Buf.SetFormat("{0}{1} {2} to pieces", Environment.NewLine, DobjArtifact.GetDecoratedName03(true, true, false, false, Globals.Buf01), DobjArtifact.EvalPlural("smashes", "smash"));
		}

		public virtual void BuildContentsSpillToFloor()
		{
			Globals.Buf.AppendFormat("; {0} contents spill to the {1}", DobjArtifact.EvalPlural("its", "their"), ActorRoom.EvalRoomType("floor", "ground"));
		}

		public override void PlayerExecute()
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
				Classes.IArtifactCategory ac = null;

				if (DobjArtifact.IsAttackable01(ref ac))
				{
					Debug.Assert(ac != null);

					if (ac.Type == Enums.ArtifactType.DeadBody)
					{
						if (BlastSpell)
						{
							Globals.Out.Print("{0}", Globals.Engine.GetBlastDesc());
						}

						DobjArtifact.SetInLimbo();

						PrintHackToBits();

						goto Cleanup;
					}

					if (ac.Type == Enums.ArtifactType.DisguisedMonster)
					{
						Globals.Engine.RevealDisguisedMonster(DobjArtifact);

						var monster = Globals.MDB[ac.Field1];

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

						command.Dobj = monster;

						NextState = command;

						goto Cleanup;
					}

					/*
						Damage it...
					*/

					var keyUid = ac.GetKeyUid();

					if (keyUid == -2)
					{
						PrintAlreadyBrokeIt(DobjArtifact);

						goto Cleanup;
					}

					var breakageStrength = ac.GetBreakageStrength();

					if (breakageStrength < 1000)
					{
						Globals.Out.Print("Nothing happens.");

						goto Cleanup;
					}

					var d = 0L;

					var s = 0L;

					if (BlastSpell)
					{
						d = 2;

						s = 5;

						Globals.Buf.SetPrint("{0}", Globals.Engine.GetBlastDesc());
					}
					else
					{
						var weapon = Globals.ADB[ActorMonster.Weapon];

						Debug.Assert(weapon != null);

						var weaponAc = weapon.GetArtifactCategory(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon });

						Debug.Assert(weaponAc != null);

						d = weaponAc.Field3;

						s = weaponAc.Field4;

						BuildWhamHitObj();
					}

					Globals.Out.Write("{0}", Globals.Buf);

					var rl = Globals.Engine.RollDice01(d, s, 0);

					breakageStrength -= rl;

					if (breakageStrength > 1000)
					{
						ac.SetBreakageStrength(breakageStrength);

						rc = DobjArtifact.SyncArtifactCategories(ac);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						goto Cleanup;
					}

					/*
						Broken!
					*/

					ac.SetOpen(true);

					ac.SetKeyUid(-2);

					ac.Field4 = 0;

					rc = DobjArtifact.SyncArtifactCategories(ac);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					DobjArtifact.Value = 0;

					rc = DobjArtifact.AddStateDesc(DobjArtifact.GetBrokenDesc());

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					BuildSmashesToPieces();

					if (ac.Type == Enums.ArtifactType.Container)
					{
						var artifactList = DobjArtifact.GetContainedList();

						foreach (var artifact in artifactList)
						{
							artifact.SetInRoom(ActorRoom);
						}

						if (artifactList.Count > 0)
						{
							BuildContentsSpillToFloor();
						}

						ac.Field3 = 0;
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

					Globals.Engine.MonsterGetsAggravated(DobjMonster);
				}

				var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.OfMonster = ActorMonster;

					x.DfMonster = DobjMonster;

					x.MemberNumber = MemberNumber;

					x.AttackNumber = AttackNumber;

					x.BlastSpell = BlastSpell;

					x.OmitSkillGains = !BlastSpell && !ShouldAllowSkillGains();
				});

				combatSystem.ExecuteAttack();
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void MonsterExecute()
		{
			Debug.Assert(DobjMonster != null);

			var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.OfMonster = ActorMonster;

				x.DfMonster = DobjMonster;

				x.MemberNumber = MemberNumber;

				x.AttackNumber = AttackNumber;
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

		public override void PlayerFinishParsing()
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

			AttackNumber = 1;
		}
	}
}

/* EamonCsCodeTemplate

// AttackCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{

	}
}
EamonCsCodeTemplate */
