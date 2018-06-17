
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
			Debug.Assert(DobjArtifact != null);

			base.PlayerExecute();

			// Player readies Trollsfire

			if (ActorMonster.Weapon == DobjArtifact.Uid && string.Equals(DobjArtifact.Name, "Trollsfire", StringComparison.OrdinalIgnoreCase) && DobjArtifact.Field4 == 10)
			{
				Globals.Engine.PrintEffectDesc(6);
			}
		}
	}
}
