
// ArtifactLoopInitializeState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ArtifactLoopInitializeState : State, IArtifactLoopInitializeState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			Globals.LoopArtifactListIndex = -1;

			Globals.LoopArtifactList = Globals.Engine.BuildLoopArtifactList(monster);

			if (Globals.LoopArtifactList == null || Globals.LoopArtifactList.Count <= 0)
			{
				NextState = Globals.CreateInstance<IMonsterUsesNaturalWeaponsCheckState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IArtifactLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public ArtifactLoopInitializeState()
		{
			Name = "ArtifactLoopInitializeState";
		}
	}
}
