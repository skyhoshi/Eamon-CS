
// DrinkCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IDrinkCommand))]
	public class DrinkCommand : EamonRT.Game.Commands.DrinkCommand, IDrinkCommand
	{
		protected virtual long DmgTaken { get; set; }

		protected virtual bool DrankItAll { get; set; }

		protected override void PrintVerbItAll(Eamon.Framework.IArtifact artifact)
		{
			DrankItAll = true;
		}

		protected override void PrintFeelBetter(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (DmgTaken > 0)
			{
				Globals.Out.Write("{0}Some of your wounds seem to clear up.{0}", Environment.NewLine);
			}
		}

		protected override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			DmgTaken = ActorMonster.DmgTaken;

			var ac = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Drinkable);

			// Sulphuric acid

			if (DobjArtifact.Uid == 53 && ac.IsOpen())
			{
				Globals.Engine.PrintEffectDesc(29);

				Globals.GameState.Die = 1;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				});
			}

			// Human blood

			else if (DobjArtifact.Uid == 52 && ac.IsOpen())
			{
				Globals.Engine.PrintEffectDesc(30);

				DrankItAll = true;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}

			// Wine

			else if (DobjArtifact.Uid == 69 && ac.IsOpen())
			{
				var stat = Globals.Engine.GetStats(Enums.Stat.Agility);

				Debug.Assert(stat != null);

				Globals.Engine.PrintEffectDesc(31);

				ActorMonster.Agility *= 2;

				ActorMonster.Agility = (long)Math.Round((double)ActorMonster.Agility / 3);

				if (ActorMonster.Agility < stat.MinValue)
				{
					ActorMonster.Agility = stat.MinValue;
				}

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}

			if (DrankItAll)
			{
				DobjArtifact.Value = 0;

				Globals.Engine.RemoveWeight(DobjArtifact);

				DobjArtifact.SetInLimbo();
			}
		}

		protected override bool IsAllowedInRoom()
		{
			return Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0;
		}

		public DrinkCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
