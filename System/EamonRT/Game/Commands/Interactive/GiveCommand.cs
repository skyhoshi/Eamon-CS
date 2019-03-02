
// GiveCommand.cs

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
	public class GiveCommand : Command, IGiveCommand
	{
		/// <summary></summary>
		public const long PpeAfterEnforceMonsterWeightLimitsCheck = 1;

		/// <summary></summary>
		public const long PpeAfterPlayerGivesReadiedWeaponCheck = 2;

		/// <summary></summary>
		public const long PpeBeforeMonsterTakesGold = 3;

		public virtual bool GetCommandCalled { get; set; }

		public virtual long GoldAmount { get; set; }

		public virtual bool MonsterRefusesToAccept()
		{
			return !Globals.IsRulesetVersion(5) && (IobjMonster.Friendliness == Friendliness.Enemy || (IobjMonster.Friendliness == Friendliness.Neutral && DobjArtifact.Value < 3000));
		}

		public override void PlayerExecute()
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
					if (DobjArtifact.DisguisedMonster == null || !GetCommandCalled)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (MonsterRefusesToAccept())
				{
					Globals.Engine.MonsterSmiles(IobjMonster);

					Globals.Out.WriteLine();

					goto Cleanup;
				}

				var artCount = 0L;

				var artWeight = DobjArtifact.Weight;

				rc = DobjArtifact.GetContainerInfo(ref artCount, ref artWeight, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (Globals.Engine.EnforceMonsterWeightLimits)
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

				PlayerProcessEvents(PpeAfterEnforceMonsterWeightLimitsCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (DobjArtifact.DeadBody != null)
				{
					PrintPolitelyRefuses(IobjMonster);

					goto Cleanup;
				}

				if (Globals.GameState.Ls == DobjArtifact.Uid)
				{
					Debug.Assert(DobjArtifact.LightSource != null);

					Globals.Engine.LightOut(DobjArtifact);
				}

				if (ActorMonster.Weapon == DobjArtifact.Uid)
				{
					Debug.Assert(DobjArtifact.GeneralWeapon != null);

					rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetReadyWeaponDesc());

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					ActorMonster.Weapon = -1;
				}

				PlayerProcessEvents(PpeAfterPlayerGivesReadiedWeaponCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				PrintGiveObjToActor(DobjArtifact, IobjMonster);

				var ac = DobjArtifact.GetArtifactCategory(new ArtifactType[] { ArtifactType.Drinkable, ArtifactType.Edible });

				if (!Globals.IsRulesetVersion(5) && ac != null && ac.Field2 > 0)
				{
					var monsterName = IobjMonster.EvalPlural(IobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf), IobjMonster.GetDecoratedName02(true, true, false, true, Globals.Buf01));

					Globals.Buf01.Clear();

					if (!ac.IsOpen())
					{
						Globals.Buf01.SetFormat(" opens {0}", DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf));

						ac.SetOpen(true);
					}

					if (ac.Field2 != Constants.InfiniteDrinkableEdible)
					{
						ac.Field2--;
					}

					rc = DobjArtifact.SyncArtifactCategories(ac);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (ac.Field2 > 0)
					{
						Globals.Buf.SetPrint("{0}{1}{2} takes a {3} and hands {4} back.",
							monsterName,
							Globals.Buf01,
							Globals.Buf01.Length > 0 ? "," : "",
							ac.Type == ArtifactType.Edible ? "bite" : "drink",
							DobjArtifact.EvalPlural("it", "them"));
					}
					else
					{
						DobjArtifact.Value = 0;

						if (ac.Type == ArtifactType.Edible)
						{
							Globals.GameState.Wt -= artWeight;

							DobjArtifact.SetInLimbo();

							Globals.Buf.SetPrint("{0}{1}{2} eats {3} all.",
								monsterName,
								Globals.Buf01,
								Globals.Buf01.Length > 0 ? " and" : "",
								DobjArtifact.EvalPlural("it", "them"));
						}
						else
						{
							rc = DobjArtifact.AddStateDesc(DobjArtifact.GetEmptyDesc());

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Buf.SetPrint("{0}{1}{2} drinks {3} all and hands {4} back.",
								monsterName,
								Globals.Buf01,
								Globals.Buf01.Length > 0 ? "," : "",
								DobjArtifact.EvalPlural("it", "them"),
								DobjArtifact.EvalPlural("it", "them"));
						}
					}

					Globals.Out.Write("{0}", Globals.Buf);

					if (ac.Field1 == 0)
					{
						goto Cleanup;
					}

					IobjMonster.DmgTaken -= ac.Field1;

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
						Globals.Engine.MonsterDies(ActorMonster, IobjMonster);
					}
				}
				else
				{
					Globals.GameState.Wt -= artWeight;

					DobjArtifact.SetCarriedByMonster(IobjMonster);

					if (Globals.IsRulesetVersion(5))
					{
						IobjMonster.CalculateGiftFriendlinessPct(DobjArtifact.Value);
					}
					else
					{
						if (IobjMonster.Friendliness == Friendliness.Neutral)
						{
							IobjMonster.Friendliness = Friendliness.Friend;

							IobjMonster.OrigFriendliness = (Friendliness)200;

							Globals.Engine.MonsterSmiles(IobjMonster);

							Globals.Out.WriteLine();
						}
					}
				}
			}
			else
			{
				Globals.Out.Print("Give {0} gold piece{1} to {2}.",
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
					PrintNotEnoughGold();

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				PlayerProcessEvents(PpeBeforeMonsterTakesGold);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				Globals.Out.Print("{0} take{1} the money.",
					IobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf),
					IobjMonster.EvalPlural("s", ""));

				Globals.Character.HeldGold -= GoldAmount;

				if (Globals.IsRulesetVersion(5))
				{
					IobjMonster.CalculateGiftFriendlinessPct(GoldAmount);
				}
				else
				{
					if (IobjMonster.Friendliness == Friendliness.Neutral && GoldAmount > 4999)
					{
						IobjMonster.Friendliness = Friendliness.Friend;

						IobjMonster.OrigFriendliness = (Friendliness)200;

						Globals.Engine.MonsterSmiles(IobjMonster);

						Globals.Out.WriteLine();
					}
				}
			}

			Globals.Engine.CheckEnemies();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
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

								CommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
							else if (CommandParser.ObjData.FilterArtifactList.Count < 1)
							{
								CommandParser.ObjData.ArtifactNotFoundFunc();

								CommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
							else
							{
								PrintWearingRemoveFirst(CommandParser.ObjData.FilterArtifactList[0]);

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

			IsIobjEnabled = true;

			Name = "GiveCommand";

			Verb = "give";

			Type = CommandType.Interactive;
		}
	}
}
