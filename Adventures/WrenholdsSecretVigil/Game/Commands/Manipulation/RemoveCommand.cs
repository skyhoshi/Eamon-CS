
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IRemoveCommand))]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Remove orb from metal pedestal

			if (DobjArtifact.Uid == 4 && IobjArtifact != null && IobjArtifact.Uid == 43)
			{
				Globals.Out.Print("{0} {1} stuck to {2} and won't budge.", DobjArtifact.GetDecoratedName03(true, true, false, false, Globals.Buf), DobjArtifact.EvalPlural("is", "are"), IobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf01));

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
