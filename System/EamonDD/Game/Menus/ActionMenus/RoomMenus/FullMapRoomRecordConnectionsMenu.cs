
// FullMapRoomRecordConnectionsMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class FullMapRoomRecordConnectionsMenu : Menu, IFullMapRoomRecordConnectionsMenu
	{
		public override void Execute()
		{
			RetCode rc;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("FULL MAP ROOM RECORD CONNECTIONS", true);

			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.Print("A map of: {0}",
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);

				Globals.Out.Print("{0}", Globals.LineSep);
			}

			var numDirs = Globals.Module != null ? Globals.Module.NumDirs : 6;

			var directionValues = EnumUtil.GetValues<Direction>();

			foreach (var room in Globals.Database.RoomTable.Records)
			{
				Buf.SetFormat("{0}\tRoom {1}: {2}", Environment.NewLine, room.Uid, room.Name);

				Buf.AppendFormat("{0}{0}{1}{0}", Environment.NewLine, room.Desc);

				for (var i = 0; i < numDirs; i++)
				{
					var direction = Globals.Engine.GetDirections(directionValues[i]);

					Debug.Assert(direction != null);

					Buf.AppendFormat("{0}{1,-2}: {2,-6}", (directionValues[i] == Direction.North || directionValues[i] == Direction.Up || directionValues[i] == Direction.Southeast) ? Environment.NewLine : "\t", direction.Abbr, room.GetDirs(directionValues[i]));
				}

				Globals.Out.WriteLine("{0}", Buf);

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.Print("{0}", Globals.LineSep);

				if (Buf.Length > 0 && Buf[0] == 'X')
				{
					break;
				}
			}

			Globals.Out.Print("Done fully mapping room record connections.");
		}

		public FullMapRoomRecordConnectionsMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
