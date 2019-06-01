
// EatCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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

			Debug.Assert(DobjArtifact != null);

			var drinkableAc = DobjArtifact.Drinkable;

			var edibleAc = DobjArtifact.Edible;

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

					DobjArtifact.SetInLimbo();

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

				PlayerProcessEvents(PpeAfterArtifactEat);

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
