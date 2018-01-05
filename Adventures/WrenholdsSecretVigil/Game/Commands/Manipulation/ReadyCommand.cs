
// ReadyCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IReadyCommand))]
	public class ReadyCommand : EamonRT.Game.Commands.ReadyCommand, IReadyCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			base.PlayerExecute();

			// Player readies Trollsfire

			if (ActorMonster.Weapon == DobjArtifact.Uid && string.Equals(DobjArtifact.Name, "Trollsfire", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Engine.PrintEffectDesc(6);
			}
		}
	}
}
