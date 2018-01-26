
// CasinoSchmittMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("Schmitty, the owner, says, \"Welcome, {0}!  Perhaps a cocktail before you start?\"", Globals.Character.Name);

			Globals.Out.Print("Some time later you make your way to the floor of the casino and stop at the roulette wheel.");

			Globals.Out.Print("\"Place your bets!  Place your bets!  Would you like in {0} One?\"", Globals.Character.EvalGender("Mighty", "Fair", "Androgynous"));

			while (true)
			{
				Globals.In.KeyPress(Buf);

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("You have {0} gold piece{1}.  The house limit is 10,000.", Globals.Character.HeldGold, Globals.Character.HeldGold != 1 ? "s" : "");

				Globals.Out.Write("{0}Your bet? (0 to leave): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				var bet = Convert.ToInt64(Buf.Trim().ToString());

				if (bet > 0 && bet <= Globals.Character.HeldGold && bet <= 10000)
				{
					Globals.Out.Print("{0}", Globals.LineSep);

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
						Globals.Out.Print("You just won double your money!");

						bet *= 2;
					}
					else if (rl > 55)
					{
						Globals.Out.Print("You won {0} gold piece{1}!", bet, bet != 1 ? "s" : "");
					}
					else
					{
						Globals.Out.Print("Sorry, {0}, you lost.", Globals.Character.Name);

						bet = -bet;
					}

					Globals.Character.HeldGold += bet;

					Globals.CharactersModified = true;
				}
				else if (bet > 0)
				{
					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Out.Print("The dealer doesn't seem amused by your sense of humor.");
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
