
// CasinoSchmittMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class CasinoSchmittMenu : Menu, ICasinoSchmittMenu
	{
		public override void Execute()
		{
			RetCode rc;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			var gender = Globals.Engine.GetGenders(Globals.Character.Gender);

			Debug.Assert(gender != null);

			Globals.Out.Write("{0}Schmitty, the owner, says, \"Welcome, {1}!  Perhaps a cocktail before you start?\"{0}", Environment.NewLine, Globals.Character.Name);

			Globals.Out.Write("{0}Some time later you make your way to the floor of the casino and stop at the roulette wheel.{0}", Environment.NewLine);

			Globals.Out.Write("{0}\"Place your bets!  Place your bets!  Would you like in {1} One?\"{0}", Environment.NewLine, gender.MightyDesc);

			while (true)
			{
				Globals.In.KeyPress(Buf);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}You have {1} gold pieces.  The house limit is 10,000.{0}", Environment.NewLine, Globals.Character.HeldGold);

				Globals.Out.Write("{0}Your bet? (0 to leave): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				var bet = Convert.ToInt64(Buf.Trim().ToString());

				if (bet > 0 && bet <= Globals.Character.HeldGold && bet <= 10000)
				{
					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					Globals.Out.Write("{0}The wheel is spinning ... ", Environment.NewLine);

					var cursorPosition = Globals.Out.GetCursorPosition();

					for (var i = 0; i < 25; i++)
					{
						Globals.Out.Write("-");

						Globals.Thread.Sleep(30);

						Globals.Out.SetCursorPosition(cursorPosition);

						Globals.Out.Write("\\");

						Globals.Thread.Sleep(30);

						Globals.Out.SetCursorPosition(cursorPosition);

						Globals.Out.Write("|");

						Globals.Thread.Sleep(30);

						Globals.Out.SetCursorPosition(cursorPosition);

						Globals.Out.Write("/");

						Globals.Thread.Sleep(30);

						Globals.Out.SetCursorPosition(cursorPosition);
					}

					Globals.Out.WriteLine(" ");

					var rl = Globals.Engine.RollDice01(1, 100, 0);

					if (rl > 80)
					{
						Globals.Out.Write("{0}You just won double your money!{0}", Environment.NewLine);

						bet *= 2;
					}
					else if (rl > 55)
					{
						Globals.Out.Write("{0}You won {1} gold pieces!{0}", Environment.NewLine, bet);
					}
					else
					{
						Globals.Out.Write("{0}Sorry, {1}, you lost.{0}", Environment.NewLine, Globals.Character.Name);

						bet = -bet;
					}

					Globals.Character.HeldGold += bet;

					Globals.CharactersModified = true;
				}
				else if (bet > 0)
				{
					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					Globals.Out.Write("{0}The dealer doesn't seem amused by your sense of humor.{0}", Environment.NewLine);
				}
				else if (bet == 0)
				{
					break;
				}
			}
		}

		public CasinoSchmittMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
