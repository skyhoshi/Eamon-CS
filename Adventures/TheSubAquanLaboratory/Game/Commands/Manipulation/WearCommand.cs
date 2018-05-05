
// WearCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, EamonRT.Framework.Commands.IWearCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var artifacts = ActorMonster.GetWornList();

			var scubaGearWorn = artifacts.FirstOrDefault(a => a.Uid == 52) != null;

			// SCUBA gear

			if ((DobjArtifact.Uid == 52 && artifacts.Count > 0 && !scubaGearWorn) || (DobjArtifact.Uid != 52 && scubaGearWorn))
			{
				PrintWearingRemoveFirst01(artifacts[0]);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
