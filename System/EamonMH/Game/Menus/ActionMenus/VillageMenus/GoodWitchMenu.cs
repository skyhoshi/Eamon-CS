
// GoodWitchMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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

			Globals.Out.Print("{0}", Globals.LineSep);

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

			Globals.Out.Print("A lovely young woman says, \"Good day, {0}.  Ah, you're surprised that I know your name?  I also know the extent of your intellect, hardiness, agility, and charisma.  If you wish, I can magically raise your attributes.  Which one would you like me to focus on?\"", Globals.Character.Name);

			Globals.Out.Print("{0}", Globals.LineSep);

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

			Globals.Out.Print("{0}", Globals.LineSep);

			Debug.Assert(Buf.Length > 0);

			i = Convert.ToInt64(Buf.Trim().ToString());

			stat = Globals.Engine.GetStats((Enums.Stat)i);

			Debug.Assert(stat != null);

			ap = Globals.Engine.GetMerchantAskPrice(Constants.StatGainPrice, (double)Rtio);

			Globals.Out.Print("\"My standard price is {0} gold piece{1} per attribute point.\"", ap, ap != 1 ? "s" : "");

			while (true)
			{
				ap = Globals.Engine.GetMerchantAskPrice(Constants.StatGainPrice, (double)Rtio);

				Globals.In.KeyPress(Buf);

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("Attribute: {0}        Gold: {1}        Cost: {2}", Globals.Character.GetStats(i), Globals.Character.HeldGold, ap);

				if (Globals.Character.HeldGold >= ap)
				{
					Globals.Out.Write("{0}1=Raise, X=Exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsChar1OrX, Globals.Engine.IsChar1OrX);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					Globals.Out.Print("{0}", Globals.LineSep);

					if (Buf.Length == 0 || Buf[0] == 'X')
					{
						break;
					}
					else
					{
						Globals.Out.Print("The witch begins an incantation and you are enveloped by a hazy white cloud.");

						var rl = Globals.Engine.RollDice01(1, 24, 0);

						if (rl >= Globals.Character.GetStats(Enums.Stat.Charisma))
						{
							Globals.Out.Print("\"It is done!\" she exclaims.");

							Globals.Character.ModStats(i, 1);
						}
						else
						{
							Globals.Out.Print("\"Because of your powerful adventurer's aura, my spells will sometimes fail.  Unfortunately, this was one of those times.\"");
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
					Globals.Out.Print("\"Ah, but I see you can't afford my modest fee.\"");

					break;
				}
			}

			Globals.Out.Print("\"Good faring, {0}!\"", Globals.Character.Name);

			Globals.In.KeyPress(Buf);
		}

		public GoodWitchMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
