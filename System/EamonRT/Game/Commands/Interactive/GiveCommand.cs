
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

			Debug.Assert(GoldAmount > 0 || DobjArtifact != null);

			Debug.Assert(IobjMonster != null);

			if (DobjArtifact != null)
			{
				if (!DobjArtifact.IsCarriedByCharacter())
				{
					if (!GetCommandCalled)
					{
						RedirectToGetCommand<IGiveCommand>(DobjArtifact);
					}
					else if (DobjArtifact.DisguisedMonster == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (IobjMonster.ShouldRefuseToAcceptGift(DobjArtifact))
				{
					gEngine.MonsterEmotes(IobjMonster);

					gOut.WriteLine();

					goto Cleanup;
				}

				var artCount = 0L;

				var artWeight = DobjArtifact.Weight;

				if (DobjArtifact.GeneralContainer != null)
				{
					rc = DobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = DobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				if (gEngine.EnforceMonsterWeightLimits)
				{
					var monWeight = 0L;

					rc = IobjMonster.GetFullInventoryWeight(ref monWeight, recurse: true);

					Debug.Assert(gEngine.IsSuccess(rc));

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

				if (DobjArtifact.DeadBody != null && IobjMonster.ShouldRefuseToAcceptDeadBody(DobjArtifact))
				{
					PrintPolitelyRefuses(IobjMonster);

					goto Cleanup;
				}

				if (gGameState.Ls == DobjArtifact.Uid)
				{
					Debug.Assert(DobjArtifact.LightSource != null);

					gEngine.LightOut(DobjArtifact);
				}

				if (ActorMonster.Weapon == DobjArtifact.Uid)
				{
					Debug.Assert(DobjArtifact.GeneralWeapon != null);

					rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetReadyWeaponDesc());

					Debug.Assert(gEngine.IsSuccess(rc));

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
					var monsterName = IobjMonster.EvalPlural(IobjMonster.GetTheName(true), IobjMonster.GetArticleName(true, true, false, true, Globals.Buf01));

					Globals.Buf01.Clear();

					if (!ac.IsOpen())
					{
						Globals.Buf01.SetFormat(" opens {0}", DobjArtifact.GetTheName());

						ac.SetOpen(true);
					}

					if (ac.Field2 != Constants.InfiniteDrinkableEdible)
					{
						ac.Field2--;
					}

					rc = DobjArtifact.SyncArtifactCategories(ac);

					Debug.Assert(gEngine.IsSuccess(rc));

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

							Debug.Assert(gEngine.IsSuccess(rc));

							Globals.Buf.SetPrint("{0}{1}{2} drinks {3} all and hands {4} back.",
								monsterName,
								Globals.Buf01,
								Globals.Buf01.Length > 0 ? "," : "",
								DobjArtifact.EvalPlural("it", "them"),
								DobjArtifact.EvalPlural("it", "them"));
						}
					}

					gOut.Write("{0}", Globals.Buf);

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
						IobjMonster.GetTheName(true, true, false, true, Globals.Buf01));

					IobjMonster.AddHealthStatus(Globals.Buf);

					gOut.Write("{0}", Globals.Buf);

					if (IobjMonster.IsDead())
					{
						gEngine.MonsterDies(ActorMonster, IobjMonster);
					}
				}
				else
				{
					DobjArtifact.SetCarriedByMonster(IobjMonster);

					if (Globals.IsRulesetVersion(5))
					{
						IobjMonster.CalculateGiftFriendlinessPct(DobjArtifact.Value);

						IobjMonster.ResolveFriendlinessPct(gCharacter);
					}
					else
					{
						if (IobjMonster.Friendliness == Friendliness.Neutral)
						{
							IobjMonster.Friendliness = Friendliness.Friend;

							IobjMonster.OrigFriendliness = (Friendliness)200;

							gEngine.MonsterEmotes(IobjMonster);

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
					IobjMonster.GetTheName(buf: Globals.Buf01));

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
					IobjMonster.GetTheName(true),
					IobjMonster.EvalPlural("s", ""));

				gCharacter.HeldGold -= GoldAmount;

				if (Globals.IsRulesetVersion(5))
				{
					IobjMonster.CalculateGiftFriendlinessPct(GoldAmount);

					IobjMonster.ResolveFriendlinessPct(gCharacter);
				}
				else
				{
					if (IobjMonster.Friendliness == Friendliness.Neutral && GoldAmount > 4999)
					{
						IobjMonster.Friendliness = Friendliness.Friend;

						IobjMonster.OrigFriendliness = (Friendliness)200;

						gEngine.MonsterEmotes(IobjMonster);

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
