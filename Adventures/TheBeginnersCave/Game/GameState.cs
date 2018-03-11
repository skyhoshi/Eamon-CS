
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		protected long _trollsfire = 0;

		public virtual long Trollsfire
		{
			get
			{
				return _trollsfire;
			}

			set
			{
				RetCode rc;

				Debug.Assert(value >= 0 && value <= 1);

				if (_trollsfire != value)
				{
					var artifact = Globals.ADB[10];

					Debug.Assert(artifact != null);

					var ac = artifact.GetArtifactCategory(Enums.ArtifactType.MagicWeapon);

					Debug.Assert(ac != null);

					if (value != 0)
					{
						rc = artifact.AddStateDesc(Constants.AlightDesc);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						ac.Field4 = 10;
					}
					else
					{
						rc = artifact.RemoveStateDesc(Constants.AlightDesc);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						ac.Field4 = 6;
					}

					_trollsfire = value;
				}
			}
		}

		public virtual long BookWarning { get; set; }

		public GameState()
		{

		}
	}
}
