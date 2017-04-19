
// EatCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class EatCommand : Command, IEatCommand
	{
		protected override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var drinkableAc = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Drinkable);

			var edibleAc = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Edible);

			var ac = edibleAc != null ? edibleAc : drinkableAc;

			if (ac != null)
			{
				if (ac.Type == Enums.ArtifactType.Drinkable)
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

				if (ac.Field6 < 1)
				{
					Globals.Out.WriteLine("{0}There's none left.", Environment.NewLine);

					goto Cleanup;
				}

				if (ac.Field6 != Constants.InfiniteDrinkableEdible)
				{
					ac.Field6--;
				}

				rc = DobjArtifact.SyncArtifactClasses(ac);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				if (ac.Field6 < 1)
				{
					DobjArtifact.Value = 0;

					Globals.RtEngine.RemoveWeight(DobjArtifact);

					DobjArtifact.SetInLimbo();

					PrintVerbItAll(DobjArtifact);
				}
				else if (ac.Field5 == 0)
				{
					Globals.Out.WriteLine("{0}Okay.", Environment.NewLine);
				}

				if (ac.Field5 != 0)
				{
					ActorMonster.DmgTaken -= ac.Field5;

					if (ActorMonster.DmgTaken < 0)
					{
						ActorMonster.DmgTaken = 0;
					}

					Globals.Out.WriteLine("{0}You feel {1}!", Environment.NewLine, ac.Field5 < 0 ? "worse" : "better");

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

		protected override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();
		}

		public EatCommand()
		{
			SortOrder = 140;

			Name = "EatCommand";

			Verb = "eat";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
