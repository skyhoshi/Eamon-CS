﻿
// DrinkCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class DrinkCommand : Command, IDrinkCommand
	{
		/// <summary></summary>
		public virtual IArtifactCategory DrinkableAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory EdibleAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			DrinkableAc = DobjArtifact.Drinkable;

			EdibleAc = DobjArtifact.Edible;

			DobjArtAc = DrinkableAc != null ? DrinkableAc : EdibleAc;

			if (DobjArtAc != null)
			{
				if (DobjArtAc.Type == ArtifactType.Edible)
				{
					NextState = Globals.CreateInstance<IEatCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				if (!DobjArtAc.IsOpen())
				{
					PrintMustFirstOpen(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (DobjArtAc.Field2 < 1)
				{
					PrintNoneLeft(DobjArtifact);

					goto Cleanup;
				}

				if (DobjArtAc.Field2 != Constants.InfiniteDrinkableEdible)
				{
					DobjArtAc.Field2--;
				}

				PlayerProcessEvents(EventType.BeforeArtifactNowEmptyCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (DobjArtAc.Field2 < 1)
				{
					DobjArtifact.Value = 0;

					DobjArtifact.AddStateDesc(DobjArtifact.GetEmptyDesc());

					PrintVerbItAll(DobjArtifact);
				}
				else if (DobjArtAc.Field1 == 0)
				{
					PrintOkay(DobjArtifact);
				}

				if (DobjArtAc.Field1 != 0)
				{
					ActorMonster.DmgTaken -= DobjArtAc.Field1;

					if (ActorMonster.DmgTaken < 0)
					{
						ActorMonster.DmgTaken = 0;
					}

					if (DobjArtAc.Field1 > 0)
					{
						PrintFeelBetter(DobjArtifact);
					}
					else
					{
						PrintFeelWorse(DobjArtifact);
					}

					Globals.Buf.SetFormat("{0}You are ", Environment.NewLine);

					ActorMonster.AddHealthStatus(Globals.Buf);

					gOut.Write("{0}", Globals.Buf);

					if (ActorMonster.IsDead())
					{
						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						goto Cleanup;
					}
				}

				PlayerProcessEvents(EventType.AfterArtifactDrink);

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

			Type = CommandType.Manipulation;
		}
	}
}
