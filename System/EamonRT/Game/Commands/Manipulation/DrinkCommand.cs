
// DrinkCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class DrinkCommand : Command, IDrinkCommand
	{
		/// <summary>
		/// This event fires before a check is made to see if an artifact has been fully drunk.
		/// </summary>
		public const long PpeBeforeArtifactNowEmptyCheck = 1;

		/// <summary>
		/// This event fires after the player drinks an artifact.
		/// </summary>
		public const long PpeAfterArtifactDrink = 2;

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var drinkableAc = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Drinkable);

			var edibleAc = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Edible);

			var ac = drinkableAc != null ? drinkableAc : edibleAc;

			if (ac != null)
			{
				if (ac.Type == Enums.ArtifactType.Edible)
				{
					NextState = Globals.CreateInstance<IEatCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				if (!ac.IsOpen())
				{
					PrintMustFirstOpen(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (ac.Field2 < 1)
				{
					PrintNoneLeft(DobjArtifact);

					goto Cleanup;
				}

				if (ac.Field2 != Constants.InfiniteDrinkableEdible)
				{
					ac.Field2--;
				}

				rc = DobjArtifact.SyncArtifactCategories(ac);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				PlayerProcessEvents(PpeBeforeArtifactNowEmptyCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (ac.Field2 < 1)
				{
					DobjArtifact.Value = 0;

					DobjArtifact.AddStateDesc(DobjArtifact.GetEmptyDesc());

					PrintVerbItAll(DobjArtifact);
				}
				else if (ac.Field1 == 0)
				{
					PrintOkay(DobjArtifact);
				}

				if (ac.Field1 != 0)
				{
					if (Globals.IsRulesetVersion(5))
					{
						Globals.GameState.ModDTTL(ActorMonster.Friendliness, -(ac.Field1 >= 0 ? Math.Min(ActorMonster.DmgTaken, ac.Field1) : ac.Field1));
					}

					ActorMonster.DmgTaken -= ac.Field1;

					if (ActorMonster.DmgTaken < 0)
					{
						ActorMonster.DmgTaken = 0;
					}

					if (ac.Field1 > 0)
					{
						PrintFeelBetter(DobjArtifact);
					}
					else
					{
						PrintFeelWorse(DobjArtifact);
					}

					Globals.Buf.SetFormat("{0}You are ", Environment.NewLine);

					ActorMonster.AddHealthStatus(Globals.Buf);

					Globals.Out.Write("{0}", Globals.Buf);

					if (ActorMonster.IsDead())
					{
						Globals.GameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						goto Cleanup;
					}
				}

				PlayerProcessEvents(PpeAfterArtifactDrink);

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
			PlayerResolveArtifact();
		}

		public DrinkCommand()
		{
			SortOrder = 120;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "DrinkCommand";

			Verb = "drink";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
