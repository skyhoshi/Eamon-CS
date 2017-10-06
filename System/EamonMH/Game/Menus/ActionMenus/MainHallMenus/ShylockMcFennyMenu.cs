
// ShylockMcFennyMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ShylockMcFennyMenu : Menu, IShylockMcFennyMenu
	{
		public override void Execute()
		{
			RetCode rc;
			long i;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}You have no trouble spotting Shylock McFenny, the local banker, due to his large belly.  You attract his attention, and he comes over to you.{0}{0}\"Well, {1} my dear {2} what a pleasure to see you!  Do you want to make a deposit or a withdrawal?\"{0}",
				Environment.NewLine,
				Globals.Character.Name,
				Globals.Character.EvalGender("boy", "girl", "thing"));

			Globals.Out.Write("{0}You have {1} GP in hand, {2} GP in the bank.{0}",
				Environment.NewLine,
				Globals.Character.HeldGold,
				Globals.Character.BankGold);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}D=Deposit gold, W=Withdraw gold, X=Exit: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.MhEngine.IsCharDOrWOrX, Globals.MhEngine.IsCharDOrWOrX);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length == 0 || Buf[0] == 'X')
			{
				goto Cleanup;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			if (Buf[0] == 'D')
			{
				Globals.Out.Write("{0}Shylock gets a wide grin on his face and says, \"Good for you!  How much do you want to deposit?\"{0}", Environment.NewLine);

				Globals.Out.Write("{0}You have {1} GP in hand, {2} GP in the bank.{0}",
					Environment.NewLine,
					Globals.Character.HeldGold,
					Globals.Character.BankGold);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Enter the amount to deposit: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				i = Convert.ToInt64(Buf.Trim().ToString());

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				if (i > 0)
				{
					if (i <= Globals.Character.HeldGold)
					{
						if (Globals.Character.BankGold + i > Constants.MaxGoldValue)
						{
							i = Constants.MaxGoldValue - Globals.Character.BankGold;
						}

						Globals.Character.HeldGold -= i;

						Globals.Character.BankGold += i;

						Globals.CharactersModified = true;

						Buf.SetFormat("{0}Shylock takes your money, puts it in his bag, listens to it jingle, then thanks you and walks away.{0}", Environment.NewLine);
					}
					else
					{
						Buf.SetFormat("{0}Shylock was very pleased when you told him the sum, but when he discovered that you didn't have that much on you, he walked away shouting about fools who try to play tricks on a kindly banker.{0}", Environment.NewLine);
					}
				}
				else
				{
					Buf.SetFormat("{0}The banker says, \"Well, if you change your mind and need my services, just let me know!\"{0}", Environment.NewLine);
				}

				Globals.Out.Write("{0}", Buf);
			}
			else
			{
				Debug.Assert(Buf[0] == 'W');

				Globals.Out.Write("{0}Shylock says, \"Well, you have {1} gold pieces stored with me.  How many do you want to take back?\"{0}",
					Environment.NewLine,
					Globals.Character.BankGold);

				Globals.Out.Write("{0}You have {1} GP in hand, {2} GP in the bank.{0}",
					Environment.NewLine,
					Globals.Character.HeldGold,
					Globals.Character.BankGold);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Enter the amount to withdraw: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				i = Convert.ToInt64(Buf.Trim().ToString());

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				if (i > 0)
				{
					if (i <= Globals.Character.BankGold)
					{
						if (Globals.Character.HeldGold + i > Constants.MaxGoldValue)
						{
							i = Constants.MaxGoldValue - Globals.Character.HeldGold;
						}

						Globals.Character.BankGold -= i;

						Globals.Character.HeldGold += i;

						Globals.CharactersModified = true;

						Buf.SetFormat("{0}The banker hands you your gold and says, \"That leaves you with {1} gold pieces in my care.\"{0}{0}He shakes your hand and walks away.{0}", 
							Environment.NewLine,
							Globals.Character.BankGold);
					}
					else
					{
						Buf.SetFormat("{0}The banker throws you a terrible glance and says, \"That's more than you've got!  You know I don't make loans to your kind!\"  With that, he loses himself in the crowd.{0}", Environment.NewLine);
					}
				}
				else
				{
					Buf.SetFormat("{0}The banker says, \"Well, if you change your mind and need my services, just let me know!\"{0}", Environment.NewLine);
				}

				Globals.Out.Write("{0}", Buf);
			}

			Globals.In.KeyPress(Buf);

		Cleanup:

			;
		}

		public ShylockMcFennyMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
