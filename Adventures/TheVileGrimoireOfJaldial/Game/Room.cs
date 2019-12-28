
// Room.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			// Use exit direction names or abbreviations based on configuration setting

			return base.GetExitList(buf, modFunc, gameState.ExitDirNames);
		}
	}
}
