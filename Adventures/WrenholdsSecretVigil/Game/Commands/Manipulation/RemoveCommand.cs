
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			// Remove orb from metal pedestal

			if (gDobjArtifact.Uid == 4 && gIobjArtifact != null && gIobjArtifact.Uid == 43)
			{
				gOut.Print("{0} {1} stuck to {2} and won't budge.", gDobjArtifact.GetTheName(true), gDobjArtifact.EvalPlural("is", "are"), gIobjArtifact.GetTheName(buf: Globals.Buf01));

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
