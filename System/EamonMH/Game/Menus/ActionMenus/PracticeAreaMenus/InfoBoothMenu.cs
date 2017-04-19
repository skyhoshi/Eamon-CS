
// InfoBoothMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class InfoBoothMenu : Menu, IInfoBoothMenu
	{
		protected virtual double? Rtio { get; set; }

		protected virtual bool GotInfo { get; set; }

		public override void Execute()
		{
			RetCode rc;

			long ap = 0;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			/* 
				Full Credit:  Derived wholly from Donald Brown's Classic Eamon

				File: MAIN HALL
				Line: 3010
			*/

			if (Rtio == null)
			{
				var c2 = Globals.Character.GetMerchantAdjustedCharisma();

				Rtio = Globals.Engine.GetMerchantRtio(c2);
			}

			ap = Globals.Engine.GetMerchantAskPrice(Constants.InfoBoothPrice, (double)Rtio);

			Globals.Out.Write("{0}The man leans over the counter and says, \"I give general directions for free and for a small charge of {1} gold pieces, I give interesting and useful information.\"{0}{0}\"So what'll it be?\"{0}", Environment.NewLine, ap);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Out.Write("{0}D=Get directions, I=Buy info, X=Exit: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.MhEngine.IsCharDOrIOrX, Globals.MhEngine.IsCharDOrIOrX);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			if (Buf.Length == 0 || Buf[0] == 'X')
			{
				goto Cleanup;
			}

			if (Buf[0] == 'D')
			{
				Globals.Out.Write("{0}\"The proprietor of the shop to the northwest coaches spell casting.  It's expensive, though.\"{0}", Environment.NewLine);

				Globals.Out.Write("{0}\"The weapons training school to the southwest is quite popular.  The owner is an excellent instructor.\"{0}", Environment.NewLine);

				Globals.Out.Write("{0}\"The eastern shop belongs to an unusual man.  But he's quite adept at teaching armor skills.\"{0}", Environment.NewLine);

				goto Cleanup;
			}
			else
			{
				if (Globals.Character.HeldGold >= ap)
				{
					if (!GotInfo)
					{
						Globals.Out.Write("{0}\"If you go to the southeast corner of this area and look to the south, you may discover wealth!\"{0}", Environment.NewLine);

						var rl = Globals.Engine.RollDice01(1, 100, 0);

						if (rl <= 50)
						{
							Globals.Out.Write("{0}You search around and discover a loose block with an old sack hidden behind it.  Inside the sack you find 100 gold coins!{0}", Environment.NewLine);

							Globals.Character.HeldGold += 100;
						}
						else
						{
							Globals.Out.Write("{0}You search around but find nothing there.  Obviously a bad tip.{0}", Environment.NewLine);
						}

						GotInfo = true;
					}
					else
					{
						Globals.Out.Write("{0}\"Next time you're in the village, you may wish to look at the statue above the fountain.\"{0}", Environment.NewLine);
					}

					Globals.Character.HeldGold -= ap;

					Globals.CharactersModified = true;

					goto Cleanup;
				}
				else
				{
					Globals.Out.Write("{0}\"You don't have enough gold for my tip!\"{0}", Environment.NewLine);

					goto Cleanup;
				}
			}

		Cleanup:

			Globals.Out.Write("{0}The man at the Info Booth smiles at you as he turns to help someone else.{0}", Environment.NewLine);

			Globals.In.KeyPress(Buf);
		}

		public InfoBoothMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
