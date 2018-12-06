
// Hint.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings]
	public class Hint : Eamon.Game.Hint, IHint
	{
		public override bool Active
		{
			get
			{
				var gameState = Globals.GameState as Framework.IGameState;

				if (Globals.EnableCalculatedProperties && gameState != null)        // null in EamonDD; non-null in EamonRT
				{
					switch (Uid)
					{
						case 3:

							var room2 = Globals.RDB[2];

							Debug.Assert(room2 != null);

							return room2.Seen;

						case 4:

							var ovalDoorArtifact = Globals.ADB[16];

							Debug.Assert(ovalDoorArtifact != null);

							return ovalDoorArtifact.Seen;

						case 5:

							var plaqueArtifact = Globals.ADB[9];

							Debug.Assert(plaqueArtifact != null);

							return plaqueArtifact.Seen;

						case 6:

							var room18 = Globals.RDB[18];

							Debug.Assert(room18 != null);

							return room18.Seen;

						case 7:

							var aquatronArtifact = Globals.ADB[57];

							Debug.Assert(aquatronArtifact != null);

							return aquatronArtifact.Seen;

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
