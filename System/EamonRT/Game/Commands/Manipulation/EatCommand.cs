
// EatCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class EatCommand : Command, IEatCommand
	{
		/// <summary></summary>
		public const long PpeBeforeArtifactNowEmptyCheck = 1;

		/// <summary></summary>
		public const long PpeAfterArtifactEat = 2;

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(gDobjArtifact != null);

			var drinkableAc = gDobjArtifact.Drinkable;

			var edibleAc = gDobjArtifact.Edible;

			var ac = edibleAc != null ? edibleAc : drinkableAc;

			if (ac != null)
			{
				if (ac.Type == ArtifactType.Drinkable)
				{
					NextState = Globals.CreateInstance<IDrinkCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				if (!ac.IsOpen())
				{
					PrintMustFirstOpen(gDobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (ac.Field2 < 1)
				{
					PrintNoneLeft(gDobjArtifact);

					goto Cleanup;
				}

				if (ac.Field2 != Constants.InfiniteDrinkableEdible)
				{
					ac.Field2--;
				}

				rc = gDobjArtifact.SyncArtifactCategories(ac);

				Debug.Assert(gEngine.IsSuccess(rc));

				PlayerProcessEvents(PpeBeforeArtifactNowEmptyCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (ac.Field2 < 1)
				{
					gDobjArtifact.Value = 0;

					gDobjArtifact.SetInLimbo();

					PrintVerbItAll(gDobjArtifact);
				}
				else if (ac.Field1 == 0)
				{
					PrintOkay(gDobjArtifact);
				}

				if (ac.Field1 != 0)
				{
					gActorMonster.DmgTaken -= ac.Field1;

					if (gActorMonster.DmgTaken < 0)
					{
						gActorMonster.DmgTaken = 0;
					}

					if (ac.Field1 > 0)
					{
						PrintFeelBetter(gDobjArtifact);
					}
					else
					{
						PrintFeelWorse(gDobjArtifact);
					}

					Globals.Buf.SetFormat("{0}You are ", Environment.NewLine);

					gActorMonster.AddHealthStatus(Globals.Buf);

					gOut.Write("{0}", Globals.Buf);

					if (gActorMonster.IsDead())
					{
						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						goto Cleanup;
					}
				}

				PlayerProcessEvents(PpeAfterArtifactEat);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{
				PrintCantVerbObj(gDobjArtifact);

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

		public EatCommand()
		{
			SortOrder = 140;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "EatCommand";

			Verb = "eat";

			Type = CommandType.Manipulation;
		}
	}
}
