
// PlayerMoveCheckState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework.Commands;
using TheSubAquanLaboratory.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings(typeof(EamonRT.Framework.States.IPlayerMoveCheckState))]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		protected override void PrintRideOffIntoSunset()
		{
			Globals.Out.WriteLine("{0}You successfully teleport back to the Main Hall.", Environment.NewLine);
		}

		protected override void ProcessEvents01()
		{
			if (Globals.GameState.R2 == -17)
			{
				Globals.Out.WriteLine("{0}You wouldn't make it 10 meters out into that lake!", Environment.NewLine);
			}
			else if (Globals.GameState.R2 == -18)
			{
				Globals.Out.WriteLine("{0}A fake-looking back wall blocks northward movement.", Environment.NewLine);
			}
			else if (Globals.GameState.R2 == -19)
			{
				var dirCommand = Globals.LastCommand;

				var pushCommand = Globals.CreateInstance<IPushCommand>();

				dirCommand.CopyCommandData(pushCommand, false);

				pushCommand.DobjArtifact = Globals.ADB[dirCommand is EamonRT.Framework.Commands.IDownCommand ? 4 : 3];

				Debug.Assert(pushCommand.DobjArtifact != null);

				NextState = pushCommand;
			}
			else if (Globals.GameState.R2 == -20)
			{
				Globals.Out.WriteLine("{0}You find that all the doors are sealed shut!", Environment.NewLine);

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.ProcessEvents01();
			}
		}
	}
}
