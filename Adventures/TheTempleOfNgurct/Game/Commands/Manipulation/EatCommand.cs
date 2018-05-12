
// EatCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class EatCommand : EamonRT.Game.Commands.EatCommand, IEatCommand
	{
		protected virtual long DmgTaken { get; set; }

		protected override void PrintVerbItAll(IArtifact artifact)
		{
			// Carcass

			if (artifact.Uid == 67)
			{
				PrintOkay(artifact);
			}
		}

		protected override void PrintFeelBetter(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (DmgTaken > 0)
			{
				Globals.Out.Print("Some of your wounds seem to clear up.");
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

			base.PlayerExecute();
		}

		protected override bool IsAllowedInRoom()
		{
			return Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0;
		}

		public EatCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
