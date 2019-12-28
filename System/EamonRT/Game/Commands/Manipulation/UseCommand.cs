
// UseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class UseCommand : Command, IUseCommand
	{
		/// <summary></summary>
		public const long PpeBeforeArtifactUse = 1;

		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			PlayerProcessEvents(PpeBeforeArtifactUse);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			var ac = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (ac != null)
			{
				if (ac.IsWeapon01())
				{
					NextState = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				if (ac.Type == ArtifactType.DisguisedMonster)
				{
					if (!DobjArtifact.IsUnmovable() && !DobjArtifact.IsCarriedByCharacter())
					{
						if (DobjArtifact.IsCarriedByContainer())
						{
							PrintRemovingFirst(DobjArtifact);
						}
						else
						{
							PrintTakingFirst(DobjArtifact);
						}
					}

					Globals.Engine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

					NextState = Globals.CreateInstance<IMonsterStartState>();

					goto Cleanup;
				}

				if (ac.Type == ArtifactType.Drinkable)
				{
					NextState = Globals.CreateInstance<IDrinkCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				if (ac.Type == ArtifactType.Edible)
				{
					NextState = Globals.CreateInstance<IEatCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				Debug.Assert(ac.Type == ArtifactType.Wearable);

				NextState = Globals.CreateInstance<IWearCommand>();

				CopyCommandData(NextState as ICommand);

				goto Cleanup;
			}
			else
			{
				PrintTryDifferentCommand(DobjArtifact);
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();

			if (DobjArtifact != null && IsIobjEnabled && CommandParser.CurrToken < CommandParser.Tokens.Length)
			{
				CommandParser.ObjData = CommandParser.IobjData;

				CommandParser.ObjData.QueryDesc = string.Format("{0}Use {1} on who or what? ", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"));

				CommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

				CommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch02;

				CommandParser.ObjData.MonsterNotFoundFunc = PrintDontHaveItNotHere;

				PlayerResolveArtifact();
			}
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var ac = artifact.GetArtifactCategory(ArtTypes, false);

			if (ac != null)
			{
				if (ac.IsWeapon01())
				{
					return !artifact.IsReadyableByCharacter() || artifact.IsCarriedByCharacter();
				}
				else if (ac.Type == ArtifactType.Wearable)
				{
					return artifact.IsCarriedByCharacter();
				}
				else
				{
					return true;
				}
			}
			else
			{
				return true;
			}
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			var prepNames = new string[] { TODO };

			return prepNames.FirstOrDefault(pn => string.Equals(prep.Name, pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public UseCommand()
		{
			SortOrder = 230;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "UseCommand";

			Verb = "use";

			Type = CommandType.Manipulation;

			ArtTypes = new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon, ArtifactType.DisguisedMonster, ArtifactType.Drinkable, ArtifactType.Edible, ArtifactType.Wearable };
		}
	}
}
