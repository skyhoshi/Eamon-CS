
// Room.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IRoom))]
	public class Room : Eamon.Game.Room, Framework.IRoom
	{
		public override long GetDirs(long index)
		{
			if (Globals.EnableGameOverrides)
			{
				if (Uid == 116)
				{
					var largeFountainArtifact = gADB[24];

					return largeFountainArtifact?.DoorGate != null && index == 6 ? 117 : base.GetDirs(index);
				}
				else
				{
					return base.GetDirs(index);
				}
			}
			else
			{
				return base.GetDirs(index);
			}
		}

		public override RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true)
		{
			// Use exit direction names or abbreviations based on configuration setting

			return base.GetExitList(buf, modFunc, gGameState.ExitDirNames);
		}

		public virtual bool IsGroundsRoom()
		{
			return (Uid > 0 && Uid < 54) || (Uid > 117 && Uid < 122);
		}

		public virtual bool IsFenceRoom()
		{
			var roomUids = new long[] { 1, 2, 3, 4, 5, 6, 7, 11, 12, 16, 17, 21, 22, 24, 29, 30, 35, 36, 40 };
			
			return roomUids.Contains(Uid);
		}

		public virtual bool IsBodyChamberRoom()
		{
			var roomUids = new long[] { 59, 60, 61, 80, 85, 86 };

			return roomUids.Contains(Uid);
		}
	}
}
