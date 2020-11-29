
// WeaponArtifactLoopInitializeState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class WeaponArtifactLoopInitializeState : State, IWeaponArtifactLoopInitializeState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			Globals.LoopWeaponArtifactListIndex = -1;

			Globals.LoopWeaponArtifactList = gEngine.BuildLoopWeaponArtifactList(LoopMonster);

			if (Globals.LoopWeaponArtifactList == null || Globals.LoopWeaponArtifactList.Count <= 0)
			{
				NextState = Globals.CreateInstance<IMonsterUsesNaturalWeaponsCheckState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IWeaponArtifactLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public WeaponArtifactLoopInitializeState()
		{
			Uid = 24;

			Name = "WeaponArtifactLoopInitializeState";
		}
	}
}
