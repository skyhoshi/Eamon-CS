
// BeforeMonsterPicksUpWeaponState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
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
				var containerType = wpnArtifact.GetCarriedByContainerContainerType();

				var command = Enum.IsDefined(typeof(ContainerType), containerType) ? (ICommand)Globals.CreateInstance<IRemoveCommand>() : (ICommand)Globals.CreateInstance<IGetCommand>();

				command.ActorMonster = monster;

				command.ActorRoom = room;

				command.Dobj = wpnArtifact;

				if (Enum.IsDefined(typeof(ContainerType), containerType))
				{
					var prepName = Globals.Engine.EvalContainerType(containerType, "in", "on", "under", "behind");

					command.Iobj = wpnArtifact.GetCarriedByContainer();

					command.Prep = Globals.Engine.Preps.FirstOrDefault(prep => string.Equals(prep.Name, prepName, StringComparison.OrdinalIgnoreCase));
				}

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
