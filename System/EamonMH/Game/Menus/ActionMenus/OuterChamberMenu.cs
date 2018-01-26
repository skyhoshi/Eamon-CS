
// OuterChamberMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class OuterChamberMenu : Menu, IOuterChamberMenu
	{
		public override void Execute()
		{
			RetCode rc;
			IMenu menu;

			Globals.Out.WriteLine("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

			Globals.Engine.PrintTitle("WELCOME TO THE", false);

			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle("EAMON CS COMPUTERIZED FANTASY GAMING SYSTEM", false);

			Buf.SetFormat("{0}{0}You are in the outer chamber of the hall of the Guild of Free Adventurers.  Many men and women are guzzling beer and there is loud singing and laughter.{0}", Environment.NewLine);

			Buf.AppendPrint("On the north side of the chamber is a cubbyhole with a desk.  Over the desk is a sign which says:  REGISTER HERE OR ELSE!");

			Buf.AppendPrint("Do you go over to the desk or join the men drinking beer?");

			Globals.Out.Write("{0}", Buf);

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Write("{0}Press D for desk or M for men: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharDOrM, Globals.Engine.IsCharDOrM);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length > 0 && Buf[0] == 'D')
			{
				menu = Globals.CreateInstance<IRegistrationDeskMenu>();
			}
			else
			{
				menu = Globals.CreateInstance<IDrinkBeerMenu>();
			}

			menu.Execute();
		}

		public OuterChamberMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
