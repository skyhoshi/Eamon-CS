
// BeforeMonsterPicksUpWeaponState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforeMonsterPicksUpWeaponState : State, IBeforeMonsterPicksUpWeaponState
	{
		public override void Execute()
		{
			var monster = Globals.MDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var wpnArtifact = Globals.LoopArtifactList[(int)Globals.LoopArtifactListIndex];

			Debug.Assert(wpnArtifact != null);

			if (!wpnArtifact.IsCarriedByMonster(monster))
			{
				var command = Globals.CreateInstance<IGetCommand>();

				command.ActorMonster = monster;

				command.ActorRoom = room;

				command.Dobj = wpnArtifact;

				command.NextState = Globals.CreateInstance<IBeforeMonsterReadiesWeaponState>();

				NextState = command;

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IBeforeMonsterReadiesWeaponState>();
			}

			Globals.NextState = NextState;
		}

		public BeforeMonsterPicksUpWeaponState()
		{
			Name = "BeforeMonsterPicksUpWeaponState";
		}
	}
}
