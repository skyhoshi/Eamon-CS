
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(Eamon.Framework.IGameState))]
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

				// if toggling the Trollsfire effect

				if (_trollsfire != value)
				{
					// find Trollsfire in the game database

					var trollsfireArtifact = Globals.ADB[10];

					Debug.Assert(trollsfireArtifact != null);

					// look up the data relating to artifact type MagicWeapon

					var ac = trollsfireArtifact.GetArtifactCategory(Enums.ArtifactType.MagicWeapon);

					Debug.Assert(ac != null);

					// if activating the Trollsfire effect

					if (value != 0)
					{
						// Trollsfire is now alight so add "(alight)" to the StateDesc property

						rc = trollsfireArtifact.AddStateDesc(Constants.AlightDesc);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						// Trollsfire now does 1d10 damage; change weapon sides to 10

						ac.Field4 = 10;
					}
					else
					{
						// Trollsfire is now extinguished so remove "(alight)" from the StateDesc property

						rc = trollsfireArtifact.RemoveStateDesc(Constants.AlightDesc);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						// Trollsfire now does 1d6 damage; change weapon sides to 6

						ac.Field4 = 6;
					}

					// store change to Trollsfire property in backing field

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
