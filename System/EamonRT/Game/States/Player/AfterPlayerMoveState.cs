
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : State, IAfterPlayerMoveState
	{
		public const long PeAfterExtinguishLightSourceCheck = 1;

		public const long PeAfterMoveMonsters = 2;

		public virtual IRoom Room { get; set; }

		public virtual IMonster Monster { get; set; }

		public virtual Direction Direction { get; set; }

		public virtual IArtifact Artifact { get; set; }

		public virtual bool MoveMonsters { get; set; }

		public override void Execute()
		{
			IArtifact artifact;
			RetCode rc;

			Debug.Assert(Direction == 0 || Enum.IsDefined(typeof(Direction), Direction));

			gGameState.R3 = gGameState.Ro;

			gGameState.Ro = gGameState.R2;

			if (MoveMonsters)
			{
				gEngine.MoveMonsters();
			}

			ProcessEvents(PeAfterMoveMonsters);

			Monster = gMDB[gGameState.Cm];

			Debug.Assert(Monster != null);

			Monster.Location = gGameState.Ro;

			if (gGameState.Ls > 0 && gGameState.Ro != gGameState.R3)
			{
				artifact = gADB[gGameState.Ls];

				Debug.Assert(artifact != null);

				if (!artifact.IsCarriedByCharacter())
				{
					rc = artifact.RemoveStateDesc(artifact.GetProvidingLightDesc());

					Debug.Assert(gEngine.IsSuccess(rc));

					gGameState.Ls = 0;
				}
			}

			Room = gRDB[gGameState.Ro];

			Debug.Assert(Room != null);

			if (Room.LightLvl > 0 && gGameState.Ls > 0)
			{
				gEngine.CheckToExtinguishLightSource();
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

			MoveMonsters = true;
		}
	}
}
