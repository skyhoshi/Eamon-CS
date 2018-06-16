
// WearCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class WearCommand : Command, IWearCommand
	{
		/// <summary>
		/// This event fires after the player wears an artifact.
		/// </summary>
		public const long PpeAfterArtifactWear = 1;

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var dobjAc = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Wearable);

			if (dobjAc != null)
			{
				if (DobjArtifact.IsWornByCharacter())
				{
					Globals.Out.Print("You're already wearing {0}!", DobjArtifact.EvalPlural("it", "them"));

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByCharacter())
				{
					PrintTakingFirst(DobjArtifact);

					NextState = Globals.CreateInstance<IGetCommand>();

					CopyCommandData(NextState as ICommand);

					NextState.NextState = Globals.CreateInstance<IWearCommand>();

					CopyCommandData(NextState.NextState as ICommand);

					goto Cleanup;
				}

				if (dobjAc.Field1 > 0)
				{
					var arArtifact = Globals.ADB[Globals.GameState.Ar];

					var shArtifact = Globals.ADB[Globals.GameState.Sh];

					var arAc = arArtifact != null ? arArtifact.GetArtifactCategory(Enums.ArtifactType.Wearable) : null;

					var shAc = shArtifact != null ? shArtifact.GetArtifactCategory(Enums.ArtifactType.Wearable) : null;

					if (dobjAc.Field1 > 1)
					{
						if (dobjAc.Field1 > 14)
						{
							dobjAc.Field1 = 14;
						}

						if (arAc != null)
						{
							Globals.Out.Print("You're already wearing armor!");

							NextState = Globals.CreateInstance<IStartState>();

							goto Cleanup;
						}

						Globals.GameState.Ar = DobjArtifact.Uid;

						ActorMonster.Armor = (dobjAc.Field1 / 2) + (shAc != null ? shAc.Field1 : 0);
					}
					else
					{
						if (shAc != null)
						{
							Globals.Out.Print("You're already wearing a shield!");

							NextState = Globals.CreateInstance<IStartState>();

							goto Cleanup;
						}

						Globals.GameState.Sh = DobjArtifact.Uid;

						ActorMonster.Armor = (arAc != null ? arAc.Field1 / 2 : 0) + dobjAc.Field1;
					}
				}

				DobjArtifact.SetWornByCharacter();

				Globals.Out.Print("{0} worn.", DobjArtifact.GetDecoratedName01(true, false, false, false, Globals.Buf));

				PlayerProcessEvents(PpeAfterArtifactWear);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{ 
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom),
				a => a.IsWornByCharacter()
			};

			PlayerResolveArtifact();
		}

		public WearCommand()
		{
			SortOrder = 240;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "WearCommand";

			Verb = "wear";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
