
// DefaultMonsterDecisionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class DefaultMonsterDecisionState : EamonRT.Game.States.DefaultMonsterDecisionState, IDefaultMonsterDecisionState
	{
		public override void Execute()
		{
			var monster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(monster != null);

			// Try to open running device, all flee

			if (monster.CanMoveToRoom(true) && Globals.DeviceOpened)
			{
				NextState = Globals.CreateInstance<IBeforeMonsterFleesRoomState>();

				Globals.NextState = NextState;
			}
			else
			{
				base.Execute();
			}
		}
	}
}

