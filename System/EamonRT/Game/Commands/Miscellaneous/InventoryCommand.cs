
// InventoryCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class InventoryCommand : Command, IInventoryCommand
	{
		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (Globals.GameState.Lt == 0)
			{
				Debug.Assert(DobjMonster != null && DobjMonster.IsCharacterMonster());
			}

			if (DobjArtifact != null)
			{
				var ac = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Container);

				if (ac != null)
				{
					if (DobjArtifact.IsEmbeddedInRoom(ActorRoom))
					{
						DobjArtifact.SetInRoom(ActorRoom);
					}

					if (!ac.IsOpen())
					{
						PrintMustFirstOpen(DobjArtifact);

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}

					var artifactList = DobjArtifact.GetContainedList();

					var showCharOwned = !DobjArtifact.IsCarriedByCharacter() && !DobjArtifact.IsWornByCharacter();

					Globals.Buf.SetFormat("{0}{1} {2} ",
						Environment.NewLine,
						DobjArtifact.GetDecoratedName03(true, showCharOwned, false, false, Globals.Buf01),
						artifactList.Count > 0 ? DobjArtifact.EvalPlural("contains", "contain") : DobjArtifact.EvalPlural("is", "are"));

					if (artifactList.Count > 0)
					{
						rc = Globals.Engine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), Enums.ArticleType.A, showCharOwned, true, false, Globals.Buf);

						Debug.Assert(Globals.Engine.IsSuccess(rc));
					}
					else
					{
						Globals.Buf.Append("empty");
					}

					Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

					Globals.Out.Write("{0}", Globals.Buf);
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

				if (!isCharMonster && DobjMonster.Friendliness < Enums.Friendliness.Friend)
				{
					Globals.Engine.MonsterSmiles(DobjMonster);

					Globals.Out.WriteLine();

					goto Cleanup;
				}

				if (DobjMonster.HasWornInventory())
				{
					var artifactList = DobjMonster.GetWornList();

					if (artifactList.Count > 0)
					{
						Globals.Buf.SetFormat("{0}{1} {2} {3}",
							Environment.NewLine,
							isCharMonster ? "You" : DobjMonster.EvalPlural(DobjMonster.GetDecoratedName03(true, true, false, true, Globals.Buf01), "They"),
							isCharMonster ? "are" : DobjMonster.EvalPlural("is", "are"),
							isCharMonster ? "wearing " : DobjMonster.EvalPlural("wearing ", "wearing among them "));

						rc = Globals.Engine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), Enums.ArticleType.A, isCharMonster ? false : true, true, false, Globals.Buf);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

						Globals.Out.Write("{0}", Globals.Buf);
					}
				}

				if (DobjMonster.HasCarriedInventory())
				{
					var artifactList = DobjMonster.GetCarriedList();

					if (isCharMonster && Globals.Character.HeldGold != 0)
					{
						goldArtifact = Globals.CreateInstance<IArtifact>(x =>
						{
							x.Name = string.Format("{0}{1} gold piece{2}",
											Globals.Character.HeldGold < 0 ? "a debt of " : "",
											Globals.Engine.GetStringFromNumber(Math.Abs(Globals.Character.HeldGold), false, Globals.Buf),
											Math.Abs(Globals.Character.HeldGold) != 1 ? "s" : "");
						});

						artifactList.Add(goldArtifact);
					}

					Globals.Buf.SetFormat("{0}{1} {2} {3}",
						Environment.NewLine,
						isCharMonster ? "You" : DobjMonster.EvalPlural(DobjMonster.GetDecoratedName03(true, true, false, true, Globals.Buf01), "They"),
						isCharMonster ? "are" : DobjMonster.EvalPlural("is", "are"),
						artifactList.Count == 0 ? "" :
						isCharMonster ? "carrying " : DobjMonster.EvalPlural("carrying ", "carrying among them "));

					if (artifactList.Count > 0)
					{
						rc = Globals.Engine.GetRecordNameList(artifactList.Cast<IGameBase>().ToList(), Enums.ArticleType.A, isCharMonster ? false : true, true, false, Globals.Buf);

						Debug.Assert(Globals.Engine.IsSuccess(rc));
					}
					else
					{
						Globals.Buf.Append("empty handed");
					}

					Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

					Globals.Out.Write("{0}", Globals.Buf);
				}

				var isUninjuredGroup = DobjMonster.GroupCount > 1 && DobjMonster.DmgTaken == 0;

				Globals.Buf.SetFormat("{0}{1} {2} ",
					Environment.NewLine,
					isCharMonster ? "You" : 
					isUninjuredGroup ? "They" : 
					DobjMonster.GetDecoratedName03(true, true, false, true, Globals.Buf01),
					isCharMonster || isUninjuredGroup ? "are" : "is");

				DobjMonster.AddHealthStatus(Globals.Buf);

				Globals.Out.Write("{0}", Globals.Buf);

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
			if (CommandParser.CurrToken < CommandParser.Tokens.Length)
			{
				if (Globals.GameState.Lt != 0)
				{
					if (Globals.IsRulesetVersion(5))
					{
						PlayerResolveArtifact();
					}
					else
					{
						CommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch03;

						PlayerResolveMonster();
					}
				}
				else
				{
					CommandParser.NextState.Discarded = true;

					CommandParser.NextState = Globals.CreateInstance<IStartState>();
				}
			}
			else
			{
				Dobj = Globals.MDB[Globals.GameState.Cm];
			}
		}

		public InventoryCommand()
		{
			SortOrder = 320;

			IsDarkEnabled = true;

			Name = "InventoryCommand";

			Verb = "inventory";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
