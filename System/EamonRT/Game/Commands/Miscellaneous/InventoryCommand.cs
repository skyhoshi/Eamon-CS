
// InventoryCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

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

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!ActorRoom.IsLit())
			{
				Debug.Assert(DobjMonster != null && DobjMonster.IsCharacterMonster());
			}

			if (DobjArtifact != null)
			{
				var ac = DobjArtifact.InContainer;

				if (ac == null)
				{
					ac = DobjArtifact.OnContainer;
				}

				if (ac == null && AllowExtendedContainers)
				{
					ac = DobjArtifact.UnderContainer;
				}

				if (ac == null && AllowExtendedContainers)
				{
					ac = DobjArtifact.BehindContainer;
				}

				if (ac != null)
				{
					var containerType = gEngine.GetContainerType(ac.Type);

					if (DobjArtifact.IsEmbeddedInRoom(ActorRoom))
					{
						DobjArtifact.SetInRoom(ActorRoom);
					}

					if (ac == DobjArtifact.InContainer && !ac.IsOpen())
					{
						PrintMustFirstOpen(DobjArtifact);

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					var artifactList = DobjArtifact.GetContainedList(containerType: containerType);

					var showCharOwned = !DobjArtifact.IsCarriedByCharacter() && !DobjArtifact.IsWornByCharacter();

					if (artifactList.Count > 0)
					{
						Globals.Buf.SetFormat("{0}{1} {2} you see ",
							Environment.NewLine,
							gEngine.EvalContainerType(containerType, "Inside", "On", "Under", "Behind"),
							DobjArtifact.GetTheName(false, showCharOwned, false, false, Globals.Buf01));

						rc = gEngine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), ArticleType.A, showCharOwned, StateDescDisplayCode.None, false, false, Globals.Buf);

						Debug.Assert(gEngine.IsSuccess(rc));
					}
					else
					{
						Globals.Buf.SetFormat("{0}There's nothing {1} {2}",
							Environment.NewLine,
							gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"),
							DobjArtifact.GetTheName(false, showCharOwned, false, false, Globals.Buf01));
					}

					Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

					gOut.Write("{0}", Globals.Buf);
				}
				else
				{
					PrintCantVerbObj(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}
			}
			else
			{
				IArtifact goldArtifact = null;

				var isCharMonster = DobjMonster.IsCharacterMonster();

				if (!isCharMonster && DobjMonster.Friendliness < Friendliness.Friend)
				{
					gEngine.MonsterEmotes(DobjMonster);

					gOut.WriteLine();

					goto Cleanup;
				}

				var hasWornInventory = DobjMonster.HasWornInventory();

				if (hasWornInventory)
				{
					var artifactList = DobjMonster.GetWornList();

					if (artifactList.Count > 0)
					{
						Globals.Buf.SetFormat("{0}{1} {2} {3}",
							Environment.NewLine,
							isCharMonster ? "You" : DobjMonster.EvalPlural(DobjMonster.GetTheName(true, true, false, true, Globals.Buf01), "They"),
							isCharMonster ? "are" : DobjMonster.EvalPlural("is", "are"),
							isCharMonster ? "wearing " : DobjMonster.EvalPlural("wearing ", "wearing among them "));

						rc = gEngine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), ArticleType.A, isCharMonster ? false : true, isCharMonster ? StateDescDisplayCode.AllStateDescs : StateDescDisplayCode.SideNotesOnly, isCharMonster ? true : false, false, Globals.Buf);

						Debug.Assert(gEngine.IsSuccess(rc));

						Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

						gOut.Write("{0}", Globals.Buf);
					}
				}

				var hasCarriedInventory = DobjMonster.HasCarriedInventory();

				if (hasCarriedInventory)
				{
					var artifactList = DobjMonster.GetCarriedList();

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
						isCharMonster ? "You" : DobjMonster.EvalPlural(DobjMonster.GetTheName(true, true, false, true, Globals.Buf01), "They"),
						isCharMonster ? "are" : DobjMonster.EvalPlural("is", "are"),
						artifactList.Count == 0 ? "" :
						isCharMonster ? "carrying " : DobjMonster.EvalPlural("carrying ", "carrying among them "));

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

				var shouldShowHealthStatusWhenInventoried = DobjMonster.ShouldShowHealthStatusWhenInventoried();

				if (shouldShowHealthStatusWhenInventoried)
				{
					var isUninjuredGroup = DobjMonster.GroupCount > 1 && DobjMonster.DmgTaken == 0;

					Globals.Buf.SetFormat("{0}{1} {2} ",
						Environment.NewLine,
						isCharMonster ? "You" :
						isUninjuredGroup ? "They" :
						DobjMonster.GetTheName(true, true, false, true, Globals.Buf01),
						isCharMonster || isUninjuredGroup ? "are" : "is");

					DobjMonster.AddHealthStatus(Globals.Buf);

					gOut.Write("{0}", Globals.Buf);
				}

				if (goldArtifact != null)
				{
					goldArtifact.Dispose();

					goldArtifact = null;
				}

				if (!hasWornInventory && !hasCarriedInventory && !shouldShowHealthStatusWhenInventoried)
				{
					PrintCantVerbObj(DobjMonster);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
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
