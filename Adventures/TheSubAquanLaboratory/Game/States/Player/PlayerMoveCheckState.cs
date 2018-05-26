
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected override void PrintRideOffIntoSunset()
		{
			Globals.Out.Print("You successfully teleport back to the Main Hall.");
		}

		protected override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterBlockingArtifactCheck)
			{
				if (Globals.GameState.R2 == -17)
				{
					Globals.Out.Print("You wouldn't make it 10 meters out into that lake!");
				}
				else if (Globals.GameState.R2 == -18)
				{
					Globals.Out.Print("A fake-looking back wall blocks northward movement.");
				}
				else if (Globals.GameState.R2 == -19)
				{
					var dirCommand = Globals.LastCommand;

					var pushCommand = Globals.CreateInstance<Framework.Commands.IPushCommand>();

					dirCommand.CopyCommandData(pushCommand, false);

					pushCommand.Dobj = Globals.ADB[dirCommand is IDownCommand ? 4 : 3];

					Debug.Assert(pushCommand.DobjArtifact != null);

					NextState = pushCommand;
				}
				else if (Globals.GameState.R2 == -20)
				{
					Globals.Out.Print("You find that all the doors are sealed shut!");

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
