
// WearCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		protected override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Can't wear silk robes

			if (DobjArtifact.Uid == 75)
			{
				PrintDontNeedTo();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}

		protected override bool IsAllowedInRoom()
		{
			return Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0;
		}

		public WearCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
