
// WearCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			var artifacts = gActorMonster.GetWornList();

			var scubaGearWorn = artifacts.FirstOrDefault(a => a.Uid == 52) != null;

			// SCUBA gear

			if ((gDobjArtifact.Uid == 52 && artifacts.Count > 0 && !scubaGearWorn) || (gDobjArtifact.Uid != 52 && scubaGearWorn))
			{
				PrintWearingRemoveFirst01(artifacts[0]);

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
