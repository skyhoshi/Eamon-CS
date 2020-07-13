
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterArtifactOpen)
			{
				// Large cabinet

				if (DobjArtifact.Uid == 11 && !gGameState.CabinetOpen)
				{
					gEngine.PrintEffectDesc(34);

					gGameState.CabinetOpen = true;
				}

				// Locker

				if (DobjArtifact.Uid == 51 && !gGameState.LockerOpen)
				{
					gEngine.PrintEffectDesc(36);

					gGameState.LockerOpen = true;
				}
			}

			base.PlayerProcessEvents(eventType);
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Humming cabinet

			if (DobjArtifact.Uid == 49)
			{
				gEngine.PrintEffectDesc(35);

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
