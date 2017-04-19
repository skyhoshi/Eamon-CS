
// DonDiegoMenu.cs

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
	public class DonDiegoMenu : Menu, IDonDiegoMenu
	{
		protected virtual double? Rtio { get; set; }

		public override void Execute()
		{
			Classes.IWeapon weapon;
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

			Globals.Out.Write("{0}The man behind the counter says, \"I'm Don Leif Thor Robin Hercules Diego, at your service!  I teach weapons.  Select your weapon of interest.\"{0}", Environment.NewLine);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Buf.Clear();

			var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

			for (i = 0; i < weaponValues.Count; i++)
			{
				weapon = Globals.Engine.GetWeapons(weaponValues[(int)i]);

				Debug.Assert(weapon != null);

				Buf.AppendFormat("{0}{1}{2}={3}{4}",
					i == 0 ? Environment.NewLine : "",
					i != 0 ? ", " : "",
					(long)weaponValues[(int)i],
					weapon.MarcosName ?? weapon.Name,
					i == weaponValues.Count - 1 ? ": " : "");
			}

			Globals.Out.Write("{0}", Buf);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, null, Globals.MhEngine.IsCharWpnType, Globals.MhEngine.IsCharWpnType);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Debug.Assert(Buf.Length > 0);

			i = Convert.ToInt64(Buf.Trim().ToString());

			weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

			Debug.Assert(weapon != null);

			ap = Globals.Engine.GetMerchantAskPrice(Constants.WeaponTrainingPrice, (double)Rtio);

			Globals.Out.Write("{0}\"My fee is {1} gold pieces per try.  Your current ability is {2}%.\"{0}", Environment.NewLine, ap, Globals.Character.GetWeaponAbilities(i));

			Globals.Out.Write("{0}Don asks you to enter his shop.  \"{1}, see that {2} over there?  It's all in the wrist...  ATTACK!\"{0}", Environment.NewLine, Globals.Character.Name, i == (long)Enums.Weapon.Bow || i == (long)Enums.Weapon.Spear ? "target" : "dummy");

			while (true)
			{
				Globals.In.KeyPress(Buf);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Ability: {1}        Gold: {2}{0}", Environment.NewLine, Globals.Character.GetWeaponAbilities(i), Globals.Character.HeldGold);

				if (Globals.Character.HeldGold >= ap)
				{
					Globals.Out.Write("{0}1=Attack, 2=Rest, X=Exit: ", Environment.NewLine);

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
							Globals.Out.Write("{0}\"A critical hit!  Very good!  Now, continue.\"{0}", Environment.NewLine);

							Globals.Character.ModWeaponAbilities(i, 2);
						}
						else if (rl > 50)
						{
							Globals.Out.Write("{0}\"A hit!  Good!  Now, continue.\"{0}", Environment.NewLine);

							Globals.Character.ModWeaponAbilities(i, 1);
						}
						else
						{
							Globals.Out.Write("{0}\"A miss!  Try harder!  Now, continue.\"{0}", Environment.NewLine);
						}

						if (Globals.Character.GetWeaponAbilities(i) > weapon.MaxValue)
						{
							Globals.Character.SetWeaponAbilities(i, weapon.MaxValue);
						}

						Globals.Character.HeldGold -= ap;

						Globals.CharactersModified = true;
					}
					else
					{
						Globals.Out.Write("{0}PUFF  PUFF  PUFF.  \"Stamina is important!\"{0}", Environment.NewLine);
					}
				}
				else
				{
					Globals.Out.Write("{0}\"Sorry, but you have too little gold!\"{0}", Environment.NewLine);

					break;
				}
			}

			Globals.Out.Write("{0}\"Goodbye and good luck!\"{0}", Environment.NewLine);

			Globals.In.KeyPress(Buf);
		}

		public DonDiegoMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
