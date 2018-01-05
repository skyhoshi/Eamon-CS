
// RemoveCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework;
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
				Globals.Out.Write("{0}{1} {2} stuck to {3} and won't budge.{0}", Environment.NewLine, DobjArtifact.GetDecoratedName03(true, true, false, false, Globals.Buf), DobjArtifact.EvalPlural("is", "are"), IobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf01));

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
