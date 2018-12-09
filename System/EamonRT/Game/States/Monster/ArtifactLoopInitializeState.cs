
// ArtifactLoopInitializeState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ArtifactLoopInitializeState : State, IArtifactLoopInitializeState
	{
		public virtual void BuildLoopArtifactList(IMonster monster)
		{
			Debug.Assert(monster != null);

			Globals.LoopArtifactList = null;

			if (monster.CombatCode == Enums.CombatCode.NaturalWeapons && monster.Weapon <= 0)
			{
				Globals.LoopArtifactList = Globals.Engine.GetReadyableWeaponList(monster);

				if (Globals.LoopArtifactList != null && Globals.LoopArtifactList.Count > 0)
				{
					var wpnArtifact = Globals.LoopArtifactList[0];

					Debug.Assert(wpnArtifact != null);

					var ac = wpnArtifact.GeneralWeapon;

					Debug.Assert(ac != null);

					if (monster.Weapon != -wpnArtifact.Uid - 1 && monster.NwDice * monster.NwSides > ac.Field3 * ac.Field4)
					{
						Globals.LoopArtifactList = null;
					}
				}
			}
			else if ((monster.CombatCode == Enums.CombatCode.Weapons || monster.CombatCode == Enums.CombatCode.Attacks) && monster.Weapon < 0)
			{
				Globals.LoopArtifactList = Globals.Engine.GetReadyableWeaponList(monster);
			}
		}

		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			Globals.LoopArtifactListIndex = -1;

			BuildLoopArtifactList(monster);

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
