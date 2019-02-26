
// BeforePlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforePlayerMoveState : State, IBeforePlayerMoveState
	{
		public const long PeAfterDestinationRoomSet = 1;

		public virtual Direction Direction { get; set; }

		public virtual IArtifact Artifact { get; set; }

		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterDestinationRoomSet)
			{
				if (Globals.GameState.GetNBTL(Friendliness.Enemy) > 0 && Globals.GameState.Lt > 0)
				{
					PrintEnemiesNearby();

					NextState = Globals.CreateInstance<IStartState>();
				}
			}
		}

		public override void Execute()
		{
			Debug.Assert(Enum.IsDefined(typeof(Direction), Direction) || Artifact != null);

			var room = Globals.RDB[Globals.GameState.Ro];

			Debug.Assert(room != null);

			Globals.GameState.R2 = Artifact != null ? 0 : room.GetDirs(Direction);

			ProcessEvents(PeAfterDestinationRoomSet);

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
				{
					x.Direction = Direction;

					x.Artifact = Artifact;
				});
			}

			Globals.NextState = NextState;
		}

		public BeforePlayerMoveState()
		{
			Name = "BeforePlayerMoveState";
		}
	}
}
