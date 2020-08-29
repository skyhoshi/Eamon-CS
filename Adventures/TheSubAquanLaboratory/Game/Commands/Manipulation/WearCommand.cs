
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

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
			Debug.Assert(DobjArtifact != null);

			var artifactList = ActorMonster.GetWornList();

			var scubaGearWorn = artifactList.FirstOrDefault(a => a.Uid == 52) != null;

			// SCUBA gear

			if ((DobjArtifact.Uid == 52 && artifactList.Count > 0 && !scubaGearWorn) || (DobjArtifact.Uid != 52 && scubaGearWorn))
			{
				PrintWearingRemoveFirst01(artifactList[0]);

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
