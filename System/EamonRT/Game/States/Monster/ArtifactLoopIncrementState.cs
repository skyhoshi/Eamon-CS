
// ArtifactLoopIncrementState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ArtifactLoopIncrementState : State, IArtifactLoopIncrementState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			Debug.Assert(Globals.LoopArtifactList != null);

			Debug.Assert(Globals.LoopArtifactListIndex >= -1 && Globals.LoopArtifactListIndex < Globals.LoopArtifactList.Count);

			Globals.LoopArtifactListIndex++;

			if (monster.Weapon > 0 || Globals.LoopArtifactListIndex >= Globals.LoopArtifactList.Count)
			{
				NextState = Globals.CreateInstance<IMonsterUsesNaturalWeaponsCheckState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IBeforeMonsterPicksUpWeaponState>();
			}

			Globals.NextState = NextState;
		}

		public ArtifactLoopIncrementState()
		{
			Name = "ArtifactLoopIncrementState";
		}
	}
}
