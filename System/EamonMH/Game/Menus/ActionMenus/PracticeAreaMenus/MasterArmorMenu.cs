
// MasterArmorMenu.cs

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
	public class MasterArmorMenu : Menu, IMasterArmorMenu
	{
		protected virtual double? Rtio { get; set; }

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

			ap = Globals.Engine.GetMerchantAskPrice(Constants.ArmorTrainingPrice, (double)Rtio);

			Globals.Out.Write("{0}A giant steps forward and says, \"Me good teacher.  You need help with armor skills?  Good, I teach for {1} gold pieces per strike.  Come with me!\"{0}", Environment.NewLine, ap);

			Globals.Out.Write("{0}He takes you to an open area in the shop and says, \"Try and make me miss you!\"{0}", Environment.NewLine);

			while (true)
			{
				Globals.In.KeyPress(Buf);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Ability: {1}        Gold: {2}{0}", Environment.NewLine, Globals.Character.ArmorExpertise, Globals.Character.HeldGold);

				if (Globals.Character.HeldGold >= ap)
				{
					Globals.Out.Write("{0}1=Hit, 2=Rest, X=Exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.MhEngine.IsChar1Or2OrX, Globals.MhEngine.IsChar1Or2OrX);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					if (Buf.Length == 0 || Buf[0] == 'X')
					{
						break;
					}
					else if (Buf[0] == '1')
					{
						var rl = Globals.Engine.RollDice01(1, 100, 0);

						if (rl > 90)
						{
							Globals.Out.Write("{0}\"Dat was real good!\"{0}", Environment.NewLine);

							Globals.Character.ArmorExpertise += 2;
						}
						else if (rl > 50)
						{
							Globals.Out.Write("{0}\"Good!  You getting it!\"{0}", Environment.NewLine);

							Globals.Character.ArmorExpertise++;
						}
						else
						{
							Globals.Out.Write("{0}\"You need move much faster!  Now get up!\"{0}", Environment.NewLine);
						}

						if (Globals.Character.ArmorExpertise > 79)
						{
							Globals.Character.ArmorExpertise = 79;
						} 

						Globals.Character.HeldGold -= ap;

						Globals.CharactersModified = true;
					}
					else
					{
						Globals.Out.Write("{0}CLANK ... PUFF ... CLANK.  \"You okay?\"{0}", Environment.NewLine);
					}
				}
				else
				{
					Globals.Out.Write("{0}\"You not carry enough gold!\"{0}", Environment.NewLine);

					break;
				}
			}

			Globals.Out.Write("{0}\"Me sorry see you go.  Bye now!\"{0}", Environment.NewLine);

			Globals.In.KeyPress(Buf);
		}

		public MasterArmorMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
