
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class ReadyCommand : EamonRT.Game.Commands.ReadyCommand, IReadyCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			base.PlayerExecute();

			// Player readies Trollsfire

			if (gActorMonster.Weapon == gDobjArtifact.Uid && string.Equals(gDobjArtifact.Name, "Trollsfire", StringComparison.OrdinalIgnoreCase) && gDobjArtifact.Field4 == 10)
			{
				gEngine.PrintEffectDesc(6);
			}
		}
	}
}
