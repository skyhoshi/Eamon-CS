
// WeaponArtifactLoopIncrementState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class WeaponArtifactLoopIncrementState : State, IWeaponArtifactLoopIncrementState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			Debug.Assert(Globals.LoopWeaponArtifactList != null);

			Debug.Assert(Globals.LoopWeaponArtifactListIndex >= -1 && Globals.LoopWeaponArtifactListIndex < Globals.LoopWeaponArtifactList.Count);

			Globals.LoopWeaponArtifactListIndex++;

			if (LoopMonster.Weapon > 0 || Globals.LoopWeaponArtifactListIndex >= Globals.LoopWeaponArtifactList.Count)
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

		public WeaponArtifactLoopIncrementState()
		{
			Name = "WeaponArtifactLoopIncrementState";
		}
	}
}
