
// BriefMapRoomRecordConnectionsMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonDD.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class BriefMapRoomRecordConnectionsMenu : Menu, IBriefMapRoomRecordConnectionsMenu
	{
		public override void Execute()
		{
			RetCode rc;

			var showHeader = true;

			var nlFlag = false;

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("BRIEF MAP ROOM RECORD CONNECTIONS", true);

			if (Globals.Engine.IsAdventureFilesetLoaded())
			{
				Globals.Out.WriteLine("{0}A map of: {1}",
					Environment.NewLine,
					Globals.Module != null ? Globals.Module.Name : Globals.Engine.UnknownName);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}

			var numDirs = Globals.Module != null ? Globals.Module.NumDirs : 6;

			var directionValues = EnumUtil.GetValues<Enums.Direction>();

			var k = Globals.Database.GetRoomsCount();

			var i = 0;

			foreach (var room in Globals.Database.RoomTable.Records)
			{
				if (showHeader)
				{
					if (numDirs == 10)
					{
						Buf.SetFormat("{0}{1,-28}", Environment.NewLine, "Room name:");

						for (var j = 0; j < numDirs; j++)
						{
							var direction = Globals.Engine.GetDirections(directionValues[j]);

							Debug.Assert(direction != null);

							Buf.AppendFormat("{0}:{1}", direction.Abbr, direction.Abbr.Length == 2 ? "  " : "   ");
						}
					}
					else
					{
						Buf.SetFormat("{0}{1,-48}", Environment.NewLine, "Room name:");

						for (var j = 0; j < numDirs; j++)
						{
							var direction = Globals.Engine.GetDirections(directionValues[j]);

							Debug.Assert(direction != null);

							Buf.AppendFormat("{0}:   ", direction.Abbr);
						}
					}

					Globals.Out.Write("{0}", Buf);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.LineSep);

					showHeader = false;
				}

				Buf.SetFormat("{0}{1,3}. {2}", Environment.NewLine, room.Uid, room.Name);

				if (numDirs == 10)
				{
					while (Buf.Length < 29)
					{
						Buf.Append('.');
					}

					Buf.Length = 29;
				}
				else
				{
					while (Buf.Length < 49)
					{
						Buf.Append('.');
					}

					Buf.Length = 49;
				}

				if (Environment.NewLine.Length == 1)
				{
					Buf.Length--;
				}

				Buf.Append('.');

				for (var j = 0; j < numDirs; j++)
				{
					Buf.AppendFormat("{0,-4} ", room.GetDirs(directionValues[j]));
				}

				Globals.Out.Write("{0}", Buf);

				nlFlag = true;

				if ((i != 0 && (i % (Constants.NumRows - 10)) == 0) || i == k - 1)
				{
					nlFlag = false;

					Globals.Out.WriteLine();

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					if (Buf.Length > 0 && Buf[0] == 'X')
					{
						break;
					}

					showHeader = true;
				}

				i++;
			}

			if (nlFlag)
			{
				Globals.Out.WriteLine();
			}

			Globals.Out.WriteLine("{0}Done briefly mapping room record connections.", Environment.NewLine);
		}

		public BriefMapRoomRecordConnectionsMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
