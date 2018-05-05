
// Hint.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings]
	public class Hint : Eamon.Game.Hint, Eamon.Framework.IHint
	{
		public override bool Active
		{
			get
			{
				Eamon.Framework.IArtifact artifact;
				Eamon.Framework.IRoom room;

				var gameState = Globals.GameState as Framework.IGameState;

				if (gameState != null)        // null in EamonDD; non-null in EamonRT
				{
					switch (Uid)
					{
						case 3:

							room = Globals.RDB[2];

							Debug.Assert(room != null);

							return room.Seen;

						case 4:

							artifact = Globals.ADB[16];

							Debug.Assert(artifact != null);

							return artifact.Seen;

						case 5:

							artifact = Globals.ADB[9];

							Debug.Assert(artifact != null);

							return artifact.Seen;

						case 6:

							room = Globals.RDB[18];

							Debug.Assert(room != null);

							return room.Seen;

						case 7:

							artifact = Globals.ADB[57];

							Debug.Assert(artifact != null);

							return artifact.Seen;

						default:

							return base.Active;
					}
				}
				else
				{
					return base.Active;
				}			
			}

			set
			{
				base.Active = value;
			}
		}
	}
}
