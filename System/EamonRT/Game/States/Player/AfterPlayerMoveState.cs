
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : State, IAfterPlayerMoveState
	{
		/// <summary>
		/// This event fires after the player has moved to a new room and a check has
		/// been made to see if a carried light source needs to be extinguished.
		/// </summary>
		public const long PeAfterExtinguishLightSourceCheck = 1;

		public virtual Enums.Direction Direction { get; set; }

		public virtual IArtifact Artifact { get; set; }

		public override void Execute()
		{
			IArtifact artifact;
			RetCode rc;

			Debug.Assert(Direction == 0 || Enum.IsDefined(typeof(Enums.Direction), Direction));

			Globals.GameState.R3 = Globals.GameState.Ro;

			Globals.GameState.Ro = Globals.GameState.R2;

			Globals.Engine.MoveMonsters();

			Globals.Engine.CheckEnemies();

			var monster = Globals.MDB[Globals.GameState.Cm];

			Debug.Assert(monster != null);

			monster.Location = Globals.GameState.Ro;

			if (Globals.GameState.Ls > 0 && Globals.GameState.Ro != Globals.GameState.R3)
			{
				artifact = Globals.ADB[Globals.GameState.Ls];

				Debug.Assert(artifact != null);

				if (!artifact.IsCarriedByCharacter())
				{
					rc = artifact.RemoveStateDesc(artifact.GetProvidingLightDesc());

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.GameState.Ls = 0;
				}
			}

			var room = Globals.RDB[Globals.GameState.Ro];

			Debug.Assert(room != null);

			Globals.GameState.Lt = (room.LightLvl > 0 || Globals.GameState.Ls > 0 ? 1 : 0);

			if (room.LightLvl > 0 && Globals.GameState.Ls > 0)
			{
				Globals.Engine.CheckToExtinguishLightSource();
			}

			ProcessEvents(PeAfterExtinguishLightSourceCheck);

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

			Globals.NextState = NextState;
		}

		public AfterPlayerMoveState()
		{
			Name = "AfterPlayerMoveState";
		}
	}
}
