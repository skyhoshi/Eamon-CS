
// DrinkCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class DrinkCommand : EamonRT.Game.Commands.DrinkCommand, IDrinkCommand
	{
		public virtual long DmgTaken { get; set; }

		public virtual bool DrankItAll { get; set; }

		public override void PrintVerbItAll(IArtifact artifact)
		{
			DrankItAll = true;
		}

		public override void PrintFeelBetter(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (DmgTaken > 0)
			{
				gOut.Print("Some of your wounds seem to clear up.");
			}
		}

		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			DmgTaken = gActorMonster.DmgTaken;

			var ac = gDobjArtifact.Drinkable;

			// Sulphuric acid

			if (gDobjArtifact.Uid == 53 && ac.IsOpen())
			{
				gEngine.PrintEffectDesc(29);

				gGameState.Die = 1;

				NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				});
			}

			// Human blood

			else if (gDobjArtifact.Uid == 52 && ac.IsOpen())
			{
				gEngine.PrintEffectDesc(30);

				DrankItAll = true;

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}

			// Wine

			else if (gDobjArtifact.Uid == 69 && ac.IsOpen())
			{
				var stat = gEngine.GetStats(Stat.Agility);

				Debug.Assert(stat != null);

				gEngine.PrintEffectDesc(31);

				gActorMonster.Agility *= 2;

				gActorMonster.Agility = (long)Math.Round((double)gActorMonster.Agility / 3);

				if (gActorMonster.Agility < stat.MinValue)
				{
					gActorMonster.Agility = stat.MinValue;
				}

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}

			if (DrankItAll)
			{
				gDobjArtifact.Value = 0;

				gDobjArtifact.SetInLimbo();
			}
		}

		public override bool IsAllowedInRoom()
		{
			return gGameState.GetNBTL(Friendliness.Enemy) <= 0;
		}

		public DrinkCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
