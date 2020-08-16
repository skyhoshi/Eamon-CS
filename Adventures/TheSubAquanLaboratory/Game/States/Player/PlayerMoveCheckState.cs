
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void PrintRideOffIntoSunset()
		{
			gOut.Print("You successfully teleport back to the Main Hall.");
		}

		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				if (gGameState.R2 == -17)
				{
					gOut.Print("You wouldn't make it 10 meters out into that lake!");
				}
				else if (gGameState.R2 == -18)
				{
					gOut.Print("A fake-looking back wall blocks northward movement.");
				}
				else if (gGameState.R2 == -19)
				{
					var dirCommand = Globals.LastCommand;

					var pushCommand = Globals.CreateInstance<Framework.Commands.IPushCommand>();

					dirCommand.CopyCommandData(pushCommand, false);

					pushCommand.Dobj = gADB[dirCommand is IDownCommand ? 4 : 3];

					Debug.Assert(pushCommand.DobjArtifact != null);

					NextState = pushCommand;
				}
				else if (gGameState.R2 == -20)
				{
					gOut.Print("You find that all the doors are sealed shut!");

					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
				else
				{
					base.ProcessEvents(eventType);
				}
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
