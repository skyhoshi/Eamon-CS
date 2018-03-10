
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
		public virtual Enums.Direction Direction { get; set; }

		public virtual IArtifact Artifact { get; set; }

		protected virtual void ProcessEvents()
		{

		}

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
				artifact = Globals.ADB[Globals.GameState.Ls];

				Debug.Assert(artifact != null);

				var ac = artifact.GetArtifactCategory(Enums.ArtifactType.LightSource);

				Debug.Assert(ac != null);

				if (ac.Field1 != -1)
				{
					Globals.Out.Write("{0}It's not dark here.  Extinguish {1} (Y/N): ", Environment.NewLine, artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						rc = artifact.RemoveStateDesc(artifact.GetProvidingLightDesc());

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.GameState.Ls = 0;
					}
				}
			}

			ProcessEvents();

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
