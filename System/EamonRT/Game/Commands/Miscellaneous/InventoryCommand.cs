
// InventoryCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
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
	public class InventoryCommand : Command, IInventoryCommand
	{
		public virtual bool AllowExtendedContainers { get; set; }

		public virtual bool OmitHealthStatus { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(gDobjArtifact != null || gDobjMonster != null);

			if (!gActorRoom.IsLit())
			{
				Debug.Assert(gDobjMonster != null && gDobjMonster.IsCharacterMonster());
			}

			if (gDobjArtifact != null)
			{
				var ac = gDobjArtifact.InContainer;

				if (ac == null)
				{
					ac = gDobjArtifact.OnContainer;
				}

				if (ac == null && AllowExtendedContainers)
				{
					ac = gDobjArtifact.UnderContainer;
				}

				if (ac == null && AllowExtendedContainers)
				{
					ac = gDobjArtifact.BehindContainer;
				}

				if (ac != null)
				{
					var containerType = gEngine.GetContainerType(ac.Type);

					if (gDobjArtifact.IsEmbeddedInRoom(gActorRoom))
					{
						gDobjArtifact.SetInRoom(gActorRoom);
					}

					if (ac == gDobjArtifact.InContainer && !ac.IsOpen())
					{
						PrintMustFirstOpen(gDobjArtifact);

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					var artifactList = gDobjArtifact.GetContainedList(containerType: containerType);

					var showCharOwned = !gDobjArtifact.IsCarriedByCharacter() && !gDobjArtifact.IsWornByCharacter();

					if (artifactList.Count > 0)
					{
						Globals.Buf.SetFormat("{0}{1} {2} you see ",
							Environment.NewLine,
							gEngine.EvalContainerType(containerType, "Inside", "On", "Under", "Behind"),
							gDobjArtifact.GetTheName(false, showCharOwned, false, false, Globals.Buf01));

						rc = gEngine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), ArticleType.A, showCharOwned, StateDescDisplayCode.None, false, false, Globals.Buf);

						Debug.Assert(gEngine.IsSuccess(rc));
					}
					else
					{
						Globals.Buf.SetFormat("{0}There's nothing {1} {2}",
							Environment.NewLine,
							gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"),
							gDobjArtifact.GetTheName(false, showCharOwned, false, false, Globals.Buf01));
					}

					Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

					gOut.Write("{0}", Globals.Buf);
				}
				else
				{
					PrintCantVerbObj(gDobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}
			}
			else
			{
				IArtifact goldArtifact = null;

				var isCharMonster = gDobjMonster.IsCharacterMonster();

				if (!isCharMonster && gDobjMonster.Friendliness < Friendliness.Friend)
				{
					gEngine.MonsterSmiles(gDobjMonster);

					gOut.WriteLine();

					goto Cleanup;
				}

				if (gDobjMonster.HasWornInventory())
				{
					var artifactList = gDobjMonster.GetWornList();

					if (artifactList.Count > 0)
					{
						Globals.Buf.SetFormat("{0}{1} {2} {3}",
							Environment.NewLine,
							isCharMonster ? "You" : gDobjMonster.EvalPlural(gDobjMonster.GetTheName(true, true, false, true, Globals.Buf01), "They"),
							isCharMonster ? "are" : gDobjMonster.EvalPlural("is", "are"),
							isCharMonster ? "wearing " : gDobjMonster.EvalPlural("wearing ", "wearing among them "));

						rc = gEngine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), ArticleType.A, isCharMonster ? false : true, isCharMonster ? StateDescDisplayCode.AllStateDescs : StateDescDisplayCode.SideNotesOnly, isCharMonster ? true : false, false, Globals.Buf);

						Debug.Assert(gEngine.IsSuccess(rc));

						Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

						gOut.Write("{0}", Globals.Buf);
					}
				}

				if (gDobjMonster.HasCarriedInventory())
				{
					var artifactList = gDobjMonster.GetCarriedList();

					if (isCharMonster)
					{
						// use total debt for characters with no assets; otherwise use HeldGold (which may be debt or asset)

						var totalGold = gCharacter.HeldGold < 0 && gCharacter.BankGold < 0 ? gCharacter.HeldGold + gCharacter.BankGold : gCharacter.HeldGold;

						if (totalGold != 0)
						{
							goldArtifact = Globals.CreateInstance<IArtifact>(x =>
							{
								x.Name = string.Format("{0}{1} gold piece{2}",
												totalGold < 0 ? "a debt of " : "",
												gEngine.GetStringFromNumber(Math.Abs(totalGold), false, Globals.Buf),
												Math.Abs(totalGold) != 1 ? "s" : "");
							});

							artifactList.Add(goldArtifact);
						}
					}

					Globals.Buf.SetFormat("{0}{1} {2} {3}",
						Environment.NewLine,
						isCharMonster ? "You" : gDobjMonster.EvalPlural(gDobjMonster.GetTheName(true, true, false, true, Globals.Buf01), "They"),
						isCharMonster ? "are" : gDobjMonster.EvalPlural("is", "are"),
						artifactList.Count == 0 ? "" :
						isCharMonster ? "carrying " : gDobjMonster.EvalPlural("carrying ", "carrying among them "));

					if (artifactList.Count > 0)
					{
						rc = gEngine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), ArticleType.A, isCharMonster ? false : true, isCharMonster ? StateDescDisplayCode.AllStateDescs : StateDescDisplayCode.SideNotesOnly, isCharMonster ? true : false, false, Globals.Buf);

						Debug.Assert(gEngine.IsSuccess(rc));
					}
					else
					{
						Globals.Buf.Append("empty handed");
					}

					Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

					gOut.Write("{0}", Globals.Buf);
				}

				if (!OmitHealthStatus)
				{
					var isUninjuredGroup = gDobjMonster.GroupCount > 1 && gDobjMonster.DmgTaken == 0;

					Globals.Buf.SetFormat("{0}{1} {2} ",
						Environment.NewLine,
						isCharMonster ? "You" :
						isUninjuredGroup ? "They" :
						gDobjMonster.GetTheName(true, true, false, true, Globals.Buf01),
						isCharMonster || isUninjuredGroup ? "are" : "is");

					gDobjMonster.AddHealthStatus(Globals.Buf);

					gOut.Write("{0}", Globals.Buf);
				}

				if (goldArtifact != null)
				{
					goldArtifact.Dispose();

					goldArtifact = null;
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
			if (gCommandParser.CurrToken < gCommandParser.Tokens.Length)
			{
				if (gActorRoom.IsLit())
				{
					if (Globals.IsRulesetVersion(5))
					{
						PlayerResolveArtifact();
					}
					else
					{
						gCommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch03;

						PlayerResolveMonster();
					}
				}
				else
				{
					gCommandParser.NextState = Globals.CreateInstance<IStartState>();
				}
			}
			else
			{
				Dobj = gMDB[gGameState.Cm];
			}
		}

		public InventoryCommand()
		{
			SortOrder = 320;

			IsDarkEnabled = true;

			Name = "InventoryCommand";

			Verb = "inventory";

			Type = CommandType.Miscellaneous;
		}
	}
}
