
// GoodWitchMenu.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using Classes = Eamon.Framework.Primitive.Classes;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class GoodWitchMenu : Menu, IGoodWitchMenu
	{
		protected virtual double? Rtio { get; set; }

		public override void Execute()
		{
			Classes.IStat stat;
			RetCode rc;
			long ap = 0;
			long i;

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

			Globals.Out.Write("{0}A lovely young woman says, \"Good day, {1}.  Ah, you're surprised that I know your name?  I also know the extent of your intellect, hardiness, agility, and charisma.  If you wish, I can magically raise your attributes.  Which one would you like me to focus on?\"{0}", Environment.NewLine, Globals.Character.Name);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Buf.Clear();

			var statValues = EnumUtil.GetValues<Enums.Stat>();

			for (i = 0; i < statValues.Count; i++)
			{
				stat = Globals.Engine.GetStats(statValues[(int)i]);

				Debug.Assert(stat != null);

				Buf.AppendFormat("{0}{1}{2}={3}{4}",
					i == 0 ? Environment.NewLine : "",
					i != 0 ? ", " : "",
					(long)statValues[(int)i],
					stat.Name,
					i == statValues.Count - 1 ? ": " : "");
			}

			Globals.Out.Write("{0}", Buf);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, null, Globals.Engine.IsCharStat, Globals.Engine.IsCharStat);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Debug.Assert(Buf.Length > 0);

			i = Convert.ToInt64(Buf.Trim().ToString());

			stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			ap = Globals.Engine.GetMerchantAskPrice(Constants.StatGainPrice, (double)Rtio);

			Globals.Out.Write("{0}\"My standard price is {1} gold pieces per attribute point.\"{0}", Environment.NewLine, ap);

			while (true)
			{
				ap = Globals.Engine.GetMerchantAskPrice(Constants.StatGainPrice, (double)Rtio);

				Globals.In.KeyPress(Buf);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Attribute: {1}        Gold: {2}        Cost: {3}{0}", Environment.NewLine, Globals.Character.GetStats(i), Globals.Character.HeldGold, ap);

				if (Globals.Character.HeldGold >= ap)
				{
					Globals.Out.Write("{0}1=Raise, X=Exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsChar1OrX, Globals.Engine.IsChar1OrX);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					if (Buf.Length == 0 || Buf[0] == 'X')
					{
						break;
					}
					else
					{
						Globals.Out.Write("{0}The witch begins an incantation and you are enveloped by a hazy white cloud.{0}", Environment.NewLine);

						var rl = Globals.Engine.RollDice01(1, 24, 0);

						if (rl >= Globals.Character.GetStats(Enums.Stat.Charisma))
						{
							Globals.Out.Write("{0}\"It is done!\" she exclaims.{0}", Environment.NewLine);

							Globals.Character.ModStats(i, 1);
						}
						else
						{
							Globals.Out.Write("{0}\"Because of your powerful adventurer's aura, my spells will sometimes fail.  Unfortunately, this was one of those times.\"{0}", Environment.NewLine);
						}

						if (Globals.Character.GetStats(i) > stat.MaxValue)
						{
							Globals.Character.SetStats(i, stat.MaxValue);
						}

						Globals.Character.HeldGold -= ap;

						Globals.CharactersModified = true;
					}
				}
				else
				{
					Globals.Out.Write("{0}\"Ah, but I see you can't afford my modest fee.\"{0}", Environment.NewLine);

					break;
				}
			}

			Globals.Out.Write("{0}\"Good faring, {1}!\"{0}", Environment.NewLine, Globals.Character.Name);

			Globals.In.KeyPress(Buf);
		}

		public GoodWitchMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
