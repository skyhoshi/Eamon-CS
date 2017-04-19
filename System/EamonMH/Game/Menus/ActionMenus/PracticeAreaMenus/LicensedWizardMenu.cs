
// LicensedWizardMenu.cs

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
	public class LicensedWizardMenu : Menu, ILicensedWizardMenu
	{
		protected virtual double? Rtio { get; set; }

		public override void Execute()
		{
			Classes.ISpell spell;
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

			var gender = Globals.Engine.GetGenders(Globals.Character.Gender);

			Debug.Assert(gender != null);

			Globals.Out.Write("{0}\"Ah, so.  Welcome to my shop, oh {1} Adventurer.  May the blessings of the gods be yours.\"{0}", Environment.NewLine, gender.MightyDesc);

			Globals.Out.Write("{0}\"And what mystical prowess do you wish this humble one to impart to one of your magnificence?\"{0}", Environment.NewLine);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Buf.Clear();

			var spellValues = EnumUtil.GetValues<Enums.Spell>();

			for (i = 0; i < spellValues.Count; i++)
			{
				spell = Globals.Engine.GetSpells(spellValues[(int)i]);

				Debug.Assert(spell != null);

				Buf.AppendFormat("{0}{1}{2}={3}{4}",
					i == 0 ? Environment.NewLine : "",
					i != 0 ? ", " : "",
					(long)spellValues[(int)i],
					spell.HokasName ?? spell.Name,
					i == spellValues.Count - 1 ? ": " : "");
			}

			Globals.Out.Write("{0}", Buf);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, null, Globals.MhEngine.IsCharSpellType, Globals.MhEngine.IsCharSpellType);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Debug.Assert(Buf.Length > 0);

			i = Convert.ToInt64(Buf.Trim().ToString());

			spell = Globals.Engine.GetSpells((Enums.Spell)i);

			Debug.Assert(spell != null);

			if (Globals.Character.GetSpellAbilities(i) == 0)
			{
				Globals.Out.Write("{0}\"You will have to first buy that spell from the mage in the Main Hall!\"{0}", Environment.NewLine);

				goto Cleanup;
			}

			ap = Globals.Engine.GetMerchantAskPrice(Constants.SpellTrainingPrice, (double)Rtio);

			Globals.Out.Write("{0}\"So you wish to learn how to use your spell more effectively!  My fee is {1} gold pieces for every try.\"{0}", Environment.NewLine, ap);

			Globals.Out.Write("{0}The mystic teaches you to concentrate while casting spells and says, \"Now cast your spell while I offer suggestions from nearby.\"{0}", Environment.NewLine);

			while (true)
			{
				Globals.In.KeyPress(Buf);

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

				Globals.Out.Write("{0}Ability: {1}        Gold: {2}{0}", Environment.NewLine, Globals.Character.GetSpellAbilities(i), Globals.Character.HeldGold);

				if (Globals.Character.HeldGold >= ap)
				{
					Globals.Out.Write("{0}1=Cast, 2=Rest, X=Exit: ", Environment.NewLine);

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
							Globals.Out.Write("{0}\"Perfect cast!  Incredible!\"{0}", Environment.NewLine);

							Globals.Character.ModSpellAbilities(i, 4);
						}
						else if (rl > 50)
						{
							Globals.Out.Write("{0}\"Success!  A good cast!\"{0}", Environment.NewLine);

							Globals.Character.ModSpellAbilities(i, 2);
						}
						else
						{
							Globals.Out.Write("{0}\"Nothing?  Concentrate!\"{0}", Environment.NewLine);
						}

						if (Globals.Character.GetSpellAbilities(i) > spell.MaxValue)
						{
							Globals.Character.SetSpellAbilities(i, spell.MaxValue);
						}

						Globals.Character.HeldGold -= ap;

						Globals.CharactersModified = true;
					}
					else
					{
						Globals.Out.Write("{0}GASP ... MUMBLE ... GASP.  \"Breathe deeply!\"{0}", Environment.NewLine);
					}
				}
				else
				{
					Globals.Out.Write("{0}\"Sorry, but my fee exceeds your assets!\"{0}", Environment.NewLine);

					break;
				}
			}

		Cleanup:

			Globals.Out.Write("{0}\"My ancient ancestors thank you for your excellent patronage!  May your magicks be always successful!\"{0}", Environment.NewLine);

			Globals.In.KeyPress(Buf);
		}

		public LicensedWizardMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
