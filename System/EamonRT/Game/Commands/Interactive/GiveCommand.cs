
// GiveCommand.cs

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
	public class GiveCommand : Command, IGiveCommand
	{
		/// <summary>
		/// An event that fires after limits are enforced on the weight a <see cref="IMonster">Monster</see> can carry.
		/// </summary>
		public const long PpeAfterEnforceMonsterWeightLimitsCheck = 1;

		/// <summary>
		/// An event that fires after checking whether the player is giving away a readied weapon.
		/// </summary>
		public const long PpeAfterPlayerGivesReadiedWeaponCheck = 2;

		/// <summary>
		/// An event that fires before a <see cref="IMonster">Monster</see> takes the gold offered by the player.
		/// </summary>
		public const long PpeBeforeMonsterTakesGold = 3;

		public virtual long GoldAmount { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(GoldAmount > 0 || gDobjArtifact != null);

			Debug.Assert(gIobjMonster != null);

			if (gDobjArtifact != null)
			{
				if (!gDobjArtifact.IsCarriedByCharacter())
				{
					if (!GetCommandCalled)
					{
						RedirectToGetCommand<IGiveCommand>(gDobjArtifact);
					}
					else if (gDobjArtifact.DisguisedMonster == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (gIobjMonster.ShouldRefuseToAcceptGift(gDobjArtifact))
				{
					gEngine.MonsterEmotes(gIobjMonster);

					gOut.WriteLine();

					goto Cleanup;
				}

				var artCount = 0L;

				var artWeight = gDobjArtifact.Weight;

				if (gDobjArtifact.GeneralContainer != null)
				{
					rc = gDobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = gDobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				if (gEngine.EnforceMonsterWeightLimits)
				{
					var monWeight = 0L;

					rc = gIobjMonster.GetFullInventoryWeight(ref monWeight, recurse: true);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (artWeight > gIobjMonster.GetWeightCarryableGronds() || artWeight + monWeight > gIobjMonster.GetWeightCarryableGronds() * gIobjMonster.GroupCount)
					{
						PrintTooHeavy(gDobjArtifact);

						goto Cleanup;
					}
				}

				PlayerProcessEvents(PpeAfterEnforceMonsterWeightLimitsCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (gDobjArtifact.DeadBody != null && gIobjMonster.ShouldRefuseToAcceptDeadBody(gDobjArtifact))
				{
					PrintPolitelyRefuses(gIobjMonster);

					goto Cleanup;
				}

				if (gGameState.Ls == gDobjArtifact.Uid)
				{
					Debug.Assert(gDobjArtifact.LightSource != null);

					gEngine.LightOut(gDobjArtifact);
				}

				if (gActorMonster.Weapon == gDobjArtifact.Uid)
				{
					Debug.Assert(gDobjArtifact.GeneralWeapon != null);

					rc = gDobjArtifact.RemoveStateDesc(gDobjArtifact.GetReadyWeaponDesc());

					Debug.Assert(gEngine.IsSuccess(rc));

					gActorMonster.Weapon = -1;
				}

				PlayerProcessEvents(PpeAfterPlayerGivesReadiedWeaponCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				PrintGiveObjToActor(gDobjArtifact, gIobjMonster);

				var ac = gDobjArtifact.GetArtifactCategory(new ArtifactType[] { ArtifactType.Drinkable, ArtifactType.Edible });

				if (!Globals.IsRulesetVersion(5) && ac != null && ac.Field2 > 0)
				{
					var monsterName = gIobjMonster.EvalPlural(gIobjMonster.GetTheName(true), gIobjMonster.GetArticleName(true, true, false, true, Globals.Buf01));

					Globals.Buf01.Clear();

					if (!ac.IsOpen())
					{
						Globals.Buf01.SetFormat(" opens {0}", gDobjArtifact.GetTheName());

						ac.SetOpen(true);
					}

					if (ac.Field2 != Constants.InfiniteDrinkableEdible)
					{
						ac.Field2--;
					}

					rc = gDobjArtifact.SyncArtifactCategories(ac);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (ac.Field2 > 0)
					{
						Globals.Buf.SetPrint("{0}{1}{2} takes a {3} and hands {4} back.",
							monsterName,
							Globals.Buf01,
							Globals.Buf01.Length > 0 ? "," : "",
							ac.Type == ArtifactType.Edible ? "bite" : "drink",
							gDobjArtifact.EvalPlural("it", "them"));
					}
					else
					{
						gDobjArtifact.Value = 0;

						if (ac.Type == ArtifactType.Edible)
						{
							gDobjArtifact.SetInLimbo();

							Globals.Buf.SetPrint("{0}{1}{2} eats {3} all.",
								monsterName,
								Globals.Buf01,
								Globals.Buf01.Length > 0 ? " and" : "",
								gDobjArtifact.EvalPlural("it", "them"));
						}
						else
						{
							rc = gDobjArtifact.AddStateDesc(gDobjArtifact.GetEmptyDesc());

							Debug.Assert(gEngine.IsSuccess(rc));

							Globals.Buf.SetPrint("{0}{1}{2} drinks {3} all and hands {4} back.",
								monsterName,
								Globals.Buf01,
								Globals.Buf01.Length > 0 ? "," : "",
								gDobjArtifact.EvalPlural("it", "them"),
								gDobjArtifact.EvalPlural("it", "them"));
						}
					}

					gOut.Write("{0}", Globals.Buf);

					if (ac.Field1 == 0)
					{
						goto Cleanup;
					}

					gIobjMonster.DmgTaken -= ac.Field1;

					if (gIobjMonster.DmgTaken < 0)
					{
						gIobjMonster.DmgTaken = 0;
					}

					Globals.Buf.SetFormat("{0}{1} is ",
						Environment.NewLine,
						gIobjMonster.GetTheName(true, true, false, true, Globals.Buf01));

					gIobjMonster.AddHealthStatus(Globals.Buf);

					gOut.Write("{0}", Globals.Buf);

					if (gIobjMonster.IsDead())
					{
						gEngine.MonsterDies(gActorMonster, gIobjMonster);
					}
				}
				else
				{
					gDobjArtifact.SetCarriedByMonster(gIobjMonster);

					if (Globals.IsRulesetVersion(5))
					{
						gIobjMonster.CalculateGiftFriendlinessPct(gDobjArtifact.Value);

						gIobjMonster.ResolveFriendlinessPct(gCharacter);
					}
					else
					{
						if (gIobjMonster.Friendliness == Friendliness.Neutral)
						{
							gIobjMonster.Friendliness = Friendliness.Friend;

							gIobjMonster.OrigFriendliness = (Friendliness)200;

							gEngine.MonsterEmotes(gIobjMonster);

							gOut.WriteLine();
						}
					}
				}
			}
			else
			{
				gOut.Print("Give {0} gold piece{1} to {2}.",
					gEngine.GetStringFromNumber(GoldAmount, false, Globals.Buf),
					GoldAmount > 1 ? "s" : "",
					gIobjMonster.GetTheName(buf: Globals.Buf01));

				gOut.Write("{0}Are you sure (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
				{
					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (gCharacter.HeldGold < GoldAmount)
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

				gOut.Print("{0} take{1} the money.",
					gIobjMonster.GetTheName(true),
					gIobjMonster.EvalPlural("s", ""));

				gCharacter.HeldGold -= GoldAmount;

				if (Globals.IsRulesetVersion(5))
				{
					gIobjMonster.CalculateGiftFriendlinessPct(GoldAmount);

					gIobjMonster.ResolveFriendlinessPct(gCharacter);
				}
				else
				{
					if (gIobjMonster.Friendliness == Friendliness.Neutral && GoldAmount > 4999)
					{
						gIobjMonster.Friendliness = Friendliness.Friend;

						gIobjMonster.OrigFriendliness = (Friendliness)200;

						gEngine.MonsterEmotes(gIobjMonster);

						gOut.WriteLine();
					}
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			long i;

			gCommandParser.ParseName();

			if (long.TryParse(gCommandParser.ObjData.Name, out i) && i > 0)
			{
				GoldAmount = i;
			}

			if (GoldAmount == 0)
			{
				gCommandParser.ObjData.ArtifactMatchFunc = () =>
				{
					if (gCommandParser.ObjData.FilterArtifactList.Count > 1)
					{
						PrintDoYouMeanObj1OrObj2(gCommandParser.ObjData.FilterArtifactList[0], gCommandParser.ObjData.FilterArtifactList[1]);

						gCommandParser.NextState = Globals.CreateInstance<IStartState>();
					}
					else if (gCommandParser.ObjData.FilterArtifactList.Count < 1)
					{
						gCommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
						{
							a => a.IsWornByCharacter()
						};

						gCommandParser.ObjData.ArtifactMatchFunc = () =>
						{
							if (gCommandParser.ObjData.FilterArtifactList.Count > 1)
							{
								PrintDoYouMeanObj1OrObj2(gCommandParser.ObjData.FilterArtifactList[0], gCommandParser.ObjData.FilterArtifactList[1]);

								gCommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
							else if (gCommandParser.ObjData.FilterArtifactList.Count < 1)
							{
								gCommandParser.ObjData.ArtifactNotFoundFunc();

								gCommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
							else
							{
								PrintWearingRemoveFirst(gCommandParser.ObjData.FilterArtifactList[0]);

								gCommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
						};

						PlayerResolveArtifact();
					}
					else
					{
						gCommandParser.ObjData.RevealEmbeddedArtifactFunc(gActorRoom, gCommandParser.ObjData.FilterArtifactList[0]);

						gCommandParser.SetArtifact(gCommandParser.ObjData.FilterArtifactList[0]);
					}
				};

				PlayerResolveArtifact();
			}

			if (GoldAmount > 0 || gDobjArtifact != null)
			{
				gCommandParser.ObjData = gCommandParser.IobjData;

				gCommandParser.ObjData.QueryDescFunc = () => string.Format("{0}To whom? ", Environment.NewLine);

				PlayerResolveMonster();
			}
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			var prepNames = new string[] { "to" };

			return prepNames.FirstOrDefault(pn => string.Equals(prep.Name, pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

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
