
// GiveCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : Command, IGiveCommand
	{
		public virtual bool GetCommandCalled { get; set; }

		public virtual long GoldAmount { get; set; }

		protected virtual void PlayerProcessEvents()
		{

		}

		protected virtual void PlayerProcessEvents01()
		{

		}

		protected virtual void PlayerProcessEvents02()
		{

		}

		protected virtual bool MonsterRefusesToAccept()
		{
			return IobjMonster.Friendliness == Enums.Friendliness.Enemy || (IobjMonster.Friendliness == Enums.Friendliness.Neutral && DobjArtifact.Value < 3000);
		}

		protected override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(GoldAmount > 0 || DobjArtifact != null);

			Debug.Assert(IobjMonster != null);

			if (DobjArtifact != null)
			{
				if (!DobjArtifact.IsCarriedByCharacter() && !GetCommandCalled)
				{
					PrintTakingFirst(DobjArtifact);

					NextState = Globals.CreateInstance<IGetCommand>(x =>
					{
						x.PreserveNextState = true;
					});

					CopyCommandData(NextState as ICommand, false);

					NextState.NextState = Globals.CreateInstance<IGiveCommand>(x =>
					{
						x.GetCommandCalled = true;
					});

					CopyCommandData(NextState.NextState as ICommand);

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByCharacter())
				{
					if (!DobjArtifact.IsDisguisedMonster() || !GetCommandCalled)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (MonsterRefusesToAccept())
				{
					Globals.RtEngine.MonsterSmiles(IobjMonster);

					Globals.Out.WriteLine();

					goto Cleanup;
				}

				var artCount = 0L;

				var artWeight = DobjArtifact.Weight;

				rc = DobjArtifact.GetContainerInfo(ref artCount, ref artWeight, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.RtEngine.EnforceMonsterWeightLimits)
				{
					var monWeight = 0L;

					rc = IobjMonster.GetFullInventoryWeight(ref monWeight, recurse: true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (artWeight > IobjMonster.GetWeightCarryableGronds() || artWeight + monWeight > IobjMonster.GetWeightCarryableGronds() * IobjMonster.GroupCount)
					{
						PrintTooHeavy(DobjArtifact);

						goto Cleanup;
					}
				}

				PlayerProcessEvents();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (DobjArtifact.IsDeadBody())
				{
					PrintPolitelyRefuses(IobjMonster);

					goto Cleanup;
				}

				if (Globals.GameState.Ls == DobjArtifact.Uid)
				{
					Debug.Assert(DobjArtifact.IsLightSource());

					Globals.RtEngine.LightOut(DobjArtifact);
				}

				if (ActorMonster.Weapon == DobjArtifact.Uid)
				{
					Debug.Assert(DobjArtifact.IsWeapon01());

					rc = DobjArtifact.RemoveStateDesc(Globals.Engine.ReadyWeaponDesc);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					ActorMonster.Weapon = -1;
				}

				PlayerProcessEvents01();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				PrintGiveObjToActor(DobjArtifact, IobjMonster);

				var ac = DobjArtifact.GetArtifactClass(new Enums.ArtifactType[] { Enums.ArtifactType.Drinkable, Enums.ArtifactType.Edible });

				if (ac != null && ac.Field6 > 0)
				{
					var monsterName = IobjMonster.EvalPlural(IobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf), IobjMonster.GetDecoratedName02(true, true, false, true, Globals.Buf01));

					Globals.Buf01.Clear();

					if (!ac.IsOpen())
					{
						Globals.Buf01.SetFormat(" opens {0}", DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf));

						ac.SetOpen(true);
					}

					if (ac.Field6 != Constants.InfiniteDrinkableEdible)
					{
						ac.Field6--;
					}

					rc = DobjArtifact.SyncArtifactClasses(ac);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (ac.Field6 > 0)
					{
						Globals.Buf.SetFormat("{0}{1}{2}{3} takes a {4} and hands {5} back.{0}",
							Environment.NewLine,
							monsterName,
							Globals.Buf01,
							Globals.Buf01.Length > 0 ? "," : "",
							ac.Type == Enums.ArtifactType.Edible ? "bite" : "drink",
							DobjArtifact.EvalPlural("it", "them"));
					}
					else
					{
						DobjArtifact.Value = 0;

						if (ac.Type == Enums.ArtifactType.Edible)
						{
							Globals.GameState.Wt -= artWeight;

							DobjArtifact.SetInLimbo();

							Globals.Buf.SetFormat("{0}{1}{2}{3} eats {4} all.{0}",
								Environment.NewLine,
								monsterName,
								Globals.Buf01,
								Globals.Buf01.Length > 0 ? " and" : "",
								DobjArtifact.EvalPlural("it", "them"));
						}
						else
						{
							rc = DobjArtifact.AddStateDesc(Globals.Engine.EmptyDesc);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Buf.SetFormat("{0}{1}{2}{3} drinks {4} all and hands {5} back.{0}",
								Environment.NewLine,
								monsterName,
								Globals.Buf01,
								Globals.Buf01.Length > 0 ? "," : "",
								DobjArtifact.EvalPlural("it", "them"),
								DobjArtifact.EvalPlural("it", "them"));
						}
					}

					Globals.Out.Write("{0}", Globals.Buf);

					if (ac.Field5 == 0)
					{
						goto Cleanup;
					}

					IobjMonster.DmgTaken -= ac.Field5;

					if (IobjMonster.DmgTaken < 0)
					{
						IobjMonster.DmgTaken = 0;
					}

					Globals.Buf.SetFormat("{0}{1} is ",
						Environment.NewLine,
						IobjMonster.GetDecoratedName03(true, true, false, true, Globals.Buf01));

					IobjMonster.AddHealthStatus(Globals.Buf);

					Globals.Out.Write("{0}", Globals.Buf);

					if (IobjMonster.IsDead())
					{
						Globals.RtEngine.MonsterDies(ActorMonster, IobjMonster);
					}
				}
				else
				{
					Globals.GameState.Wt -= artWeight;

					DobjArtifact.SetCarriedByMonster(IobjMonster);

					if (IobjMonster.Friendliness == Enums.Friendliness.Neutral)
					{
						IobjMonster.Friendliness = Enums.Friendliness.Friend;

						Globals.RtEngine.MonsterSmiles(IobjMonster);

						Globals.Out.WriteLine();
					}
				}
			}
			else
			{
				Globals.Out.Write("{0}Give {1} gold piece{2} to {3}.{0}",
					Environment.NewLine,
					Globals.Engine.GetStringFromNumber(GoldAmount, false, Globals.Buf),
					GoldAmount > 1 ? "s" : "",
					IobjMonster.GetDecoratedName03(false, true, false, false, Globals.Buf01));

				Globals.Out.Write("{0}Are you sure (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
				{
					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (Globals.Character.HeldGold < GoldAmount)
				{
					Globals.Out.Write("{0}You only have {1} gold piece{2}.{0}",
						Environment.NewLine,
						Globals.Engine.GetStringFromNumber(Globals.Character.HeldGold, false, Globals.Buf),
						Globals.Character.HeldGold != 1 ? "s" : "");

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				PlayerProcessEvents02();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				Globals.Out.Write("{0}{1} take{2} the money.{0}",
					Environment.NewLine,
					IobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf),
					IobjMonster.EvalPlural("s", ""));

				Globals.Character.HeldGold -= GoldAmount;

				if (IobjMonster.Friendliness == Enums.Friendliness.Neutral && GoldAmount > 4999)
				{
					IobjMonster.Friendliness = Enums.Friendliness.Friend;

					Globals.RtEngine.MonsterSmiles(IobjMonster);

					Globals.Out.WriteLine();
				}
			}

			Globals.RtEngine.CheckEnemies();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			long i;

			CommandParser.ParseName();

			if (long.TryParse(CommandParser.ObjData.Name, out i) && i > 0)
			{
				GoldAmount = i;
			}

			if (GoldAmount == 0)
			{
				CommandParser.ObjData.ArtifactMatchFunc = () =>
				{
					if (CommandParser.ObjData.FilterArtifactList.Count > 1)
					{
						PrintDoYouMeanObj1OrObj2(CommandParser.ObjData.FilterArtifactList[0], CommandParser.ObjData.FilterArtifactList[1]);

						CommandParser.NextState.Dispose();

						CommandParser.NextState = Globals.CreateInstance<IStartState>();
					}
					else if (CommandParser.ObjData.FilterArtifactList.Count < 1)
					{
						CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
						{
							a => a.IsWornByCharacter()
						};

						CommandParser.ObjData.ArtifactMatchFunc = () =>
						{
							if (CommandParser.ObjData.FilterArtifactList.Count > 1)
							{
								PrintDoYouMeanObj1OrObj2(CommandParser.ObjData.FilterArtifactList[0], CommandParser.ObjData.FilterArtifactList[1]);

								CommandParser.NextState.Dispose();

								CommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
							else if (CommandParser.ObjData.FilterArtifactList.Count < 1)
							{
								CommandParser.ObjData.ArtifactNotFoundFunc();

								CommandParser.NextState.Dispose();

								CommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
							else
							{
								PrintWearingRemoveFirst(CommandParser.ObjData.FilterArtifactList[0]);

								CommandParser.NextState.Dispose();

								CommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
						};

						PlayerResolveArtifact();
					}
					else
					{
						CommandParser.ObjData.RevealEmbeddedArtifactFunc(ActorRoom, CommandParser.ObjData.FilterArtifactList[0]);

						CommandParser.SetArtifact(CommandParser.ObjData.FilterArtifactList[0]);
					}
				};

				PlayerResolveArtifact();
			}

			if (GoldAmount > 0 || DobjArtifact != null)
			{
				CommandParser.ObjData = CommandParser.IobjData;

				CommandParser.ObjData.QueryDesc = string.Format("{0}To whom? ", Environment.NewLine);

				PlayerResolveMonster();
			}
		}

		public GiveCommand()
		{
			SortOrder = 280;

			Name = "GiveCommand";

			Verb = "give";

			Type = Enums.CommandType.Interactive;
		}
	}
}
