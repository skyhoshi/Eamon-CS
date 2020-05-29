
// PowerCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		protected virtual IList<long> FriendDeadBodyUids { get; set; }

		public PowerCommand()
		{
			FriendDeadBodyUids = gEngine.GetMonsterList(m => m.Friendliness == Friendliness.Friend && !m.IsCharacterMonster()).Select(m => m.DeadBody).ToList();

			// Can't resurrect dead friends

			ResurrectWhereClauseFuncs = new Func<IArtifact, bool>[]
			{
				a => (a.IsCarriedByCharacter() || a.IsInRoomUid(gGameState.Ro)) && a.DeadBody != null && !FriendDeadBodyUids.Contains(a.Uid)
			};

			// Can't make dead friends vanish

			VanishWhereClauseFuncs = new Func<IArtifact, bool>[]
			{
				a => a.IsInRoomUid(gGameState.Ro) && !a.IsUnmovable() && !FriendDeadBodyUids.Contains(a.Uid)
			};
		}
	}
}
