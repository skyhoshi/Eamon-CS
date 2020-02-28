
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : Command, IRemoveCommand
	{
		/// <summary></summary>
		protected virtual bool OmitWeightCheck { get; set; }

		/// <summary></summary>
		public const long PpeAfterWornArtifactRemove = 1;

		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			if (gIobjArtifact != null)
			{
				if (!GetCommandCalled)
				{
					RedirectToGetCommand<IRemoveCommand>(gDobjArtifact, false);

					goto Cleanup;
				}

				if (!gDobjArtifact.IsCarriedByCharacter())
				{
					if (gDobjArtifact.DisguisedMonster == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (gActorMonster.Weapon <= 0 && gDobjArtifact.IsReadyableByCharacter() && NextState == null)
				{
					NextState = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(NextState as ICommand, false);
				}
			}
			else
			{
				var arArtifact = gADB[gGameState.Ar];

				var shArtifact = gADB[gGameState.Sh];

				var arAc = arArtifact != null ? arArtifact.Wearable : null;

				var shAc = shArtifact != null ? shArtifact.Wearable : null;

				if (gDobjArtifact.Uid == gGameState.Sh)
				{
					gActorMonster.Armor = arAc != null ? (arAc.Field1 / 2) + ((arAc.Field1 / 2) >= 3 ? 2 : 0) : 0;

					gGameState.Sh = 0;
				}

				if (gDobjArtifact.Uid == gGameState.Ar)
				{
					gActorMonster.Armor = shAc != null ? shAc.Field1 : 0;

					gGameState.Ar = 0;
				}

				gDobjArtifact.SetCarriedByCharacter();

				PrintRemoved(gDobjArtifact);

				PlayerProcessEvents(PpeAfterWornArtifactRemove);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void MonsterExecute()
		{
			RetCode rc;

			Debug.Assert(gDobjArtifact != null && gIobjArtifact != null && Prep != null && Enum.IsDefined(typeof(ContainerType), Prep.ContainerType));

			Debug.Assert(gDobjArtifact.IsCarriedByContainer(gIobjArtifact) && gDobjArtifact.GetCarriedByContainerContainerType() == Prep.ContainerType);

			var artTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

			var ac = gDobjArtifact.GetArtifactCategory(artTypes, false);

			if (ac == null)
			{
				ac = gDobjArtifact.GetCategories(0);
			}

			if (ac != null && ac.Type != ArtifactType.DisguisedMonster && gDobjArtifact.Weight <= 900 && !gDobjArtifact.IsUnmovable01() && (ac.Type != ArtifactType.DeadBody || ac.Field1 == 1) && ac.Type != ArtifactType.BoundMonster)
			{
				OmitWeightCheck = gDobjArtifact.IsCarriedByMonster(gActorMonster, true);

				var artCount = 0L;

				var artWeight = gDobjArtifact.Weight;

				if (gDobjArtifact.GeneralContainer != null)
				{
					rc = gDobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = gDobjArtifact.GetContainerInfo(ref artCount, ref artWeight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				var monWeight = 0L;

				rc = gActorMonster.GetFullInventoryWeight(ref monWeight, recurse: true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (!gEngine.EnforceMonsterWeightLimits || OmitWeightCheck || (artWeight <= gActorMonster.GetWeightCarryableGronds() && artWeight + monWeight <= gActorMonster.GetWeightCarryableGronds() * gActorMonster.GroupCount))
				{
					gDobjArtifact.SetCarriedByMonster(gActorMonster);

					var charMonster = gMDB[gGameState.Cm];

					Debug.Assert(charMonster != null);

					if (charMonster.IsInRoom(gActorRoom))
					{
						if (gActorRoom.IsLit())
						{
							var monsterName = gActorMonster.EvalPlural(gActorMonster.GetTheName(true), gActorMonster.GetArticleName(true, true, false, true, Globals.Buf01));

							gOut.Print("{0} removes {1} from {2} {3}.", monsterName, gDobjArtifact.GetArticleName(), gEngine.EvalContainerType(Prep.ContainerType, "inside", "on", "under", "behind"), OmitWeightCheck ? gIobjArtifact.GetArticleName(buf: Globals.Buf01) : gIobjArtifact.GetTheName(buf: Globals.Buf01));
						}
						else
						{
							var monsterName = string.Format("An unseen {0}", gActorMonster.CheckNBTLHostility() ? "offender" : "entity");

							gOut.Print("{0} picks up {1}.", monsterName, "a weapon");
						}
					}

					// when a weapon is picked up all monster affinities to that weapon are broken

					var fumbleMonsters = gEngine.GetMonsterList(m => m.Weapon == -gDobjArtifact.Uid - 1 && m != gActorMonster);

					foreach (var monster in fumbleMonsters)
					{
						monster.Weapon = -1;
					}
				}
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
			gCommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsWornByCharacter()
			};

			gCommandParser.ObjData.ArtifactMatchFunc = () =>
			{
				ContainerType = Prep != null ? Prep.ContainerType : (ContainerType)(-1);

				if (gCommandParser.ObjData.FilterArtifactList.Count > 1)
				{
					PrintDoYouMeanObj1OrObj2(gCommandParser.ObjData.FilterArtifactList[0], gCommandParser.ObjData.FilterArtifactList[1]);

					gCommandParser.NextState = Globals.CreateInstance<IStartState>();
				}
				else if (gCommandParser.ObjData.FilterArtifactList.Count < 1)
				{
					gCommandParser.ObjData = gCommandParser.IobjData;

					gCommandParser.ObjData.QueryDescFunc = () => string.Format("{0}From {1}what? ", Environment.NewLine, Enum.IsDefined(typeof(ContainerType), ContainerType) ? gEngine.EvalContainerType(ContainerType, "inside ", "on ", "under ", "behind ") : "");

					PlayerResolveArtifact();

					if (gIobjArtifact != null)
					{
						if (!Enum.IsDefined(typeof(ContainerType), ContainerType))
						{
							var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer };

							var defaultAc = gIobjArtifact.GetArtifactCategory(artTypes);

							ContainerType = defaultAc != null ? gEngine.GetContainerType(defaultAc.Type) : ContainerType.In;
						}

						var ac = gEngine.EvalContainerType(ContainerType, gIobjArtifact.InContainer, gIobjArtifact.OnContainer, gIobjArtifact.UnderContainer, gIobjArtifact.BehindContainer);

						if (ac != null)
						{
							if (ac != gIobjArtifact.InContainer || ac.IsOpen())
							{
								gCommandParser.ObjData = gCommandParser.DobjData;

								gCommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
								{
									a => a.IsCarriedByContainer(gIobjArtifact) && a.GetCarriedByContainerContainerType() == ContainerType
								};

								gCommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch;

								gCommandParser.ObjData.ArtifactNotFoundFunc = PrintDontFollowYou;

								PlayerResolveArtifact();
							}
							else
							{
								PrintMustFirstOpen(gIobjArtifact);

								gCommandParser.NextState = Globals.CreateInstance<IStartState>();
							}
						}
						else
						{
							PrintDontFollowYou();

							gCommandParser.NextState = Globals.CreateInstance<IStartState>();
						}
					}
				}
				else
				{
					gCommandParser.ObjData.RevealEmbeddedArtifactFunc(gActorRoom, gCommandParser.ObjData.FilterArtifactList[0]);

					gCommandParser.SetArtifact(gCommandParser.ObjData.FilterArtifactList[0]);
				}
			};

			PlayerResolveArtifact();
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			return gCommandParser.ObjData == gCommandParser.IobjData || artifact.IsWornByCharacter();
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			var prepNames = new string[] { "in", "fromin", "on", "fromon", "under", "fromunder", "behind", "frombehind", "from" };

			return prepNames.FirstOrDefault(pn => string.Equals(prep.Name, pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public RemoveCommand()
		{
			SortOrder = 220;

			IsIobjEnabled = true;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "RemoveCommand";

			Verb = "remove";

			Type = CommandType.Manipulation;
		}
	}
}
