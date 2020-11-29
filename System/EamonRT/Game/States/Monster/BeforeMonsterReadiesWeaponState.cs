
// BeforeMonsterReadiesWeaponState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforeMonsterReadiesWeaponState : State, IBeforeMonsterReadiesWeaponState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual IArtifact WeaponArtifact { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			WeaponArtifact = Globals.LoopWeaponArtifactList[(int)Globals.LoopWeaponArtifactListIndex];

			Debug.Assert(WeaponArtifact != null);

			if (WeaponArtifact.IsCarriedByMonster(LoopMonster))
			{
				RedirectCommand = Globals.CreateInstance<IReadyCommand>();

				RedirectCommand.ActorMonster = LoopMonster;

				RedirectCommand.ActorRoom = LoopMonsterRoom;

				RedirectCommand.Dobj = WeaponArtifact;

				RedirectCommand.NextState = Globals.CreateInstance<IWeaponArtifactLoopIncrementState>();

				NextState = RedirectCommand;

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IWeaponArtifactLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public BeforeMonsterReadiesWeaponState()
		{
			Uid = 14;

			Name = "BeforeMonsterReadiesWeaponState";
		}
	}
}
