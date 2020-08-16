
// BeforeMonsterPicksUpWeaponState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforeMonsterPicksUpWeaponState : State, IBeforeMonsterPicksUpWeaponState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual IArtifact WeaponArtifact { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		/// <summary></summary>
		public virtual ContainerType WeaponContainerType { get; set; }

		/// <summary></summary>
		public virtual string ContainerPrepName { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			WeaponArtifact = Globals.LoopWeaponArtifactList[(int)Globals.LoopWeaponArtifactListIndex];

			Debug.Assert(WeaponArtifact != null);

			if (!WeaponArtifact.IsCarriedByMonster(LoopMonster))
			{
				WeaponContainerType = WeaponArtifact.GetCarriedByContainerContainerType();

				RedirectCommand = Enum.IsDefined(typeof(ContainerType), WeaponContainerType) ? (ICommand)Globals.CreateInstance<IRemoveCommand>() : (ICommand)Globals.CreateInstance<IGetCommand>();

				RedirectCommand.ActorMonster = LoopMonster;

				RedirectCommand.ActorRoom = LoopMonsterRoom;

				RedirectCommand.Dobj = WeaponArtifact;

				if (Enum.IsDefined(typeof(ContainerType), WeaponContainerType))
				{
					ContainerPrepName = gEngine.EvalContainerType(WeaponContainerType, "in", "on", "under", "behind");

					RedirectCommand.Iobj = WeaponArtifact.GetCarriedByContainer();

					RedirectCommand.Prep = gEngine.Preps.FirstOrDefault(prep => string.Equals(prep.Name, ContainerPrepName, StringComparison.OrdinalIgnoreCase));
				}

				RedirectCommand.NextState = Globals.CreateInstance<IBeforeMonsterReadiesWeaponState>();

				NextState = RedirectCommand;

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
