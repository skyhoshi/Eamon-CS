
// MarcosCavielliMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using Classes = Eamon.Framework.Primitive.Classes;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class MarcosCavielliMenu : Menu, IMarcosCavielliMenu
	{
		/// <summary></summary>
		protected enum MenuState : long
		{
			/// <summary></summary>
			None = 0,

			/// <summary></summary>
			BuyOrSell,

			/// <summary></summary>
			BuyWeapon,

			/// <summary></summary>
			CheckShield,

			/// <summary></summary>
			SellWeapon,

			/// <summary></summary>
			BuyArmor,

			/// <summary></summary>
			BuyShield,

			/// <summary></summary>
			SetArmorClass,

			/// <summary></summary>
			LeaveShop
		}

		/// <summary></summary>
		protected virtual double? Rtio { get; set; }

		public override void Execute()
		{
			RetCode rc;

			long i = 0;

			long j = 0;

			long a2 = 0;

			long sh = 0;

			long ap = 0;

			long bp = 0;

			long ti = 0;

			var menuState = MenuState.BuyOrSell;

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Print("As you enter the weapon shop, Marcos Cavielli (the owner) comes from out of the back room and says, \"Well, as I live and breathe, if it isn't my old pal, {0}!  So, what do you need?\"", Globals.Character.Name);

			while (true)
			{
				switch(menuState)
				{
					case MenuState.BuyOrSell:

						Globals.Out.Print("{0}", Globals.LineSep);

						Globals.Out.Write("{0}B=Buy weapon, S=Sell weapon, A=Buy better armor, X=Exit: ", Environment.NewLine);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharBOrSOrAOrX, Globals.Engine.IsCharBOrSOrAOrX);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Thread.Sleep(150);

						if (Buf.Length == 0 || Buf[0] == 'X')
						{
								goto Cleanup;
						}

						Globals.Out.Print("{0}", Globals.LineSep);

						/* 
							Full Credit:  Derived wholly from Donald Brown's Classic Eamon

							File: MAIN HALL
							Line: 2020
						*/

						if (Rtio == null)
						{
							var c2 = Globals.Character.GetMerchantAdjustedCharisma();

							Rtio = Globals.Engine.GetMerchantRtio(c2);
						}

						if (Buf[0] == 'B')
						{
							menuState = MenuState.BuyWeapon;
						}
						else if (Buf[0] == 'S')
						{
							menuState = MenuState.SellWeapon;
						}
						else
						{
							Debug.Assert(Buf[0] == 'A');

							menuState = MenuState.BuyArmor;
						}

						break;

					case MenuState.BuyWeapon:

						i = Globals.Character.Weapons.Length - 1;

						if (!Globals.Character.IsWeaponActive(i))
						{
							Globals.Out.Print("Marcos smiles at you and says, \"Good!  I gotta the best.  What kind do you want?\"");

							Globals.Out.Print("{0}", Globals.LineSep);

							Buf.Clear();

							var weaponValues = EnumUtil.GetValues<Enums.Weapon>();

							for (i = 0; i < weaponValues.Count; i++)
							{
								var weapon = Globals.Engine.GetWeapons(weaponValues[(int)i]);

								Debug.Assert(weapon != null);

								Buf.AppendFormat("{0}{1}{2}={3}{4}",
									i == 0 ? Environment.NewLine : "",
									i != 0 ? ", " : "",
									(long)weaponValues[(int)i],
									weapon.MarcosName ?? weapon.Name,
									i == weaponValues.Count - 1 ? ", X=Exit: " : "");
							}

							Globals.Out.Write("{0}", Buf);

							Buf.Clear();

							rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharWpnTypeOrX, Globals.Engine.IsCharWpnTypeOrX);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Thread.Sleep(150);

							Globals.Out.Print("{0}", Globals.LineSep);

							if (Buf.Length > 0 && Buf[0] != 'X')
							{
								i = Convert.ToInt64(Buf.Trim().ToString());

								var weapon = Globals.Engine.GetWeapons((Enums.Weapon)i);

								Debug.Assert(weapon != null);

								var cw = Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
								{
									x.Name = Globals.CloneInstance(weapon.MarcosName ?? weapon.Name).ToLower();
									x.IsPlural = weapon.MarcosIsPlural;
									x.PluralType = weapon.MarcosPluralType;
									x.ArticleType = weapon.MarcosArticleType;
									x.Field2 = i;
									x.Field3 = weapon.MarcosDice;
									x.Field4 = weapon.MarcosSides;
									x.Field5 = weapon.MarcosNumHands;
								});

								Globals.Out.Print("Marcos says, \"Well, I just happen to have three {0}s in, of varying quality.  I've got a very good one for {1} GP, a fair one for {2} GP, and a kinda shabby one for {3} GP.  Which do you want?\"",
									cw.Name,
									Globals.Engine.GetMerchantAskPrice(weapon.MarcosPrice, (double)Rtio),
									Globals.Engine.GetMerchantAskPrice(weapon.MarcosPrice * 0.80, (double)Rtio),
									Globals.Engine.GetMerchantAskPrice(weapon.MarcosPrice * 0.60, (double)Rtio));

								Globals.Out.Print("{0}", Globals.LineSep);

								Globals.Out.Write("{0}G=Good quality, F=Fair quality, P=Poor quality, X=Exit: ", Environment.NewLine);

								Buf.Clear();

								rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharGOrFOrPOrX, Globals.Engine.IsCharGOrFOrPOrX);

								Debug.Assert(Globals.Engine.IsSuccess(rc));

								Globals.Thread.Sleep(150);

								Globals.Out.Print("{0}", Globals.LineSep);

								if (Buf.Length > 0 && Buf[0] != 'X')
								{
									if (Buf[0] == 'G')
									{
										ap = Globals.Engine.GetMerchantAskPrice(weapon.MarcosPrice, (double)Rtio);

										cw.Field1 = 10;
									}
									else if (Buf[0] == 'F')
									{
										ap = Globals.Engine.GetMerchantAskPrice(weapon.MarcosPrice * 0.80, (double)Rtio);
									}
									else
									{
										Debug.Assert(Buf[0] == 'P');

										ap = Globals.Engine.GetMerchantAskPrice(weapon.MarcosPrice * 0.60, (double)Rtio);

										cw.Field1 = -10;
									}

									if (ap > Globals.Character.HeldGold)
									{
										Globals.Out.Print("Marcos shakes a finger at you and says, \"You shouldn't play tricks on an old friend!  Come back when you gotta more gold or you want something you can afford.\"  He then shoos you out the door.");

										Globals.In.KeyPress(Buf);

										goto Cleanup;
									}

									rc = Globals.Character.GetWeaponCount(ref i);

									Debug.Assert(Globals.Engine.IsSuccess(rc));

									Globals.Character.SetWeapons(i, cw);

									Globals.Character.GetWeapons(i).Parent = Globals.Character;

									Globals.Character.StripPoundCharsFromWeaponNames();

									Globals.Character.AddPoundCharsToWeaponNames();

									Globals.Character.HeldGold -= ap;

									Globals.CharactersModified = true;

									Globals.Out.Print("Marcos hands you your weapon and takes the price from you.");

									menuState = MenuState.CheckShield;
								}
								else
								{
									menuState = MenuState.LeaveShop;
								}
							}
							else
							{
								menuState = MenuState.LeaveShop;
							}
						}
						else
						{
							Globals.Out.Print("Marcos smiles at you and says, \"Thatsa good, but first you gotta sell me a weapon.  You know the law:  No more than four weapons per person!\"");

							menuState = MenuState.BuyOrSell;
						}

						break;

					case MenuState.CheckShield:

						ap = Globals.Engine.GetMerchantAskPrice(Constants.ShieldPrice, (double)Rtio);

						if (Globals.Character.HeldGold >= ap)
						{
							Globals.Out.Print("He now asks you, \"Now how about some armor?\"");

							Globals.Out.Print("{0}", Globals.LineSep);

							Globals.Out.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

							Buf.Clear();

							rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Thread.Sleep(150);

							Globals.Out.Print("{0}", Globals.LineSep);

							if (Buf.Length > 0 && Buf[0] == 'Y')
							{
								menuState = MenuState.BuyArmor;
							}
							else
							{
								menuState = MenuState.LeaveShop;
							}
						}
						else
						{
							menuState = MenuState.LeaveShop;
						}

						break;

					case MenuState.SellWeapon:

						if (Globals.Character.IsWeaponActive(0))
						{
							Buf.SetPrint("Marcos says, \"Okay, what've you got?\"");

							rc = Globals.Character.ListWeapons(Buf);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Out.Write("{0}", Buf);

							Globals.Out.WriteLine("{0}{0}{1}", Environment.NewLine, Globals.LineSep);

							Globals.Out.Write("{0}Press the number of the weapon to sell or X to exit: ", Environment.NewLine);

							Buf.Clear();

							rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharWpnNumOrX, Globals.Engine.IsCharWpnNumOrX);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Thread.Sleep(150);

							Globals.Out.Print("{0}", Globals.LineSep);

							if (Buf.Length > 0 && Buf[0] != 'X')
							{
								i = Convert.ToInt64(Buf.Trim().ToString()) - 1;

								var imw = false;

								var weaponPrice = Globals.Engine.GetWeaponPriceOrValue(Globals.Character.GetWeapons(i), true, ref imw);

								ap = Globals.Engine.GetMerchantAskPrice(weaponPrice, (double)Rtio);

								bp = Globals.Engine.GetMerchantBidPrice(weaponPrice, (double)Rtio);

								ti = Math.Min(ap, bp) / 4;

								Globals.Out.Print("Marcos examines your weapon and says, \"{0}Well, {1}I can give you {2} gold piece{3} for it, take it or leave it.\"",
									Globals.Character.GetWeapons(i).Field3 * Globals.Character.GetWeapons(i).Field4 > 25 ? "Very nice, this is a magical weapon.  " :
									Globals.Character.GetWeapons(i).Field3 * Globals.Character.GetWeapons(i).Field4 > 15 || Globals.Character.GetWeapons(i).Field1 > 10 ? "Hey, this is a pretty good weapon!  " : "",
									imw ? "you've banged it up a bit, but " : "",
									ti,
									ti != 1 ? "s" : "");

								Globals.Out.Print("{0}", Globals.LineSep);

								Globals.Out.Write("{0}Press T to take or L to leave: ", Environment.NewLine);

								Buf.Clear();

								rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharTOrL, Globals.Engine.IsCharTOrL);

								Debug.Assert(Globals.Engine.IsSuccess(rc));

								Globals.Thread.Sleep(150);

								Globals.Out.Print("{0}", Globals.LineSep);

								if (Buf.Length > 0 && Buf[0] == 'T')
								{
									Globals.Out.Print("Marcos gives you your money and takes your weapon.");

									Globals.Character.HeldGold += ti;

									while (i + 1 < Globals.Character.Weapons.Length)
									{
										Globals.Character.SetWeapons(i, Globals.Character.GetWeapons(i + 1));

										i++;
									}

									Globals.Character.SetWeapons(i, Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
									{
										x.Parent = Globals.Character;
									}));

									Globals.Character.StripPoundCharsFromWeaponNames();

									Globals.Character.AddPoundCharsToWeaponNames();

									Globals.CharactersModified = true;

									Globals.Out.Print("Marcos asks you, \"How about buying a weapon?\"");

									Globals.Out.Print("{0}", Globals.LineSep);

									Globals.Out.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

									Buf.Clear();

									rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

									Debug.Assert(Globals.Engine.IsSuccess(rc));

									Globals.Thread.Sleep(150);

									Globals.Out.Print("{0}", Globals.LineSep);

									if (Buf.Length > 0 && Buf[0] == 'Y')
									{
										menuState = MenuState.BuyWeapon;
									}
									else
									{
										menuState = MenuState.CheckShield;
									}
								}
								else
								{
									menuState = MenuState.LeaveShop;
								}
							}
							else
							{
								menuState = MenuState.LeaveShop;
							}
						}
						else
						{
							Globals.Out.Print("Marcos grins and says, \"You havea no weapons to sell me, {0}!\"", Globals.Character.Name);

							menuState = MenuState.BuyOrSell;
						}

						break;

					case MenuState.BuyArmor:

						a2 = (long)Globals.Character.ArmorClass / 2;

						sh = (long)Globals.Character.ArmorClass % 2;

						var ima = false;

						var armorPrice = Globals.Engine.GetArmorPriceOrValue(Globals.Character.ArmorClass, true, ref ima);

						ap = Globals.Engine.GetMerchantAskPrice(armorPrice, (double)Rtio);

						bp = Globals.Engine.GetMerchantBidPrice(armorPrice, (double)Rtio);

						ti = Math.Min(ap, bp) / 4;

						var armor = Globals.Engine.GetArmors((Enums.Armor)(a2 * 2));

						Debug.Assert(armor != null);

						Buf.Clear();

						j = Globals.Engine.GetMaxArmorMarcosNum();

						var armorValues = EnumUtil.GetValues<Enums.Armor>();

						for (i = 0; i < armorValues.Count; i++)
						{
							armor = Globals.Engine.GetArmors(armorValues[(int)i]);

							Debug.Assert(armor != null);

							if (armor.MarcosNum > 0)
							{
								Buf.AppendFormat("{0}{1}{2} for the {3}",
									armor.MarcosNum == 1 ? "" : armor.MarcosNum == j ? " and " : ", ",
									Globals.Engine.GetMerchantAskPrice(armor.MarcosPrice, (double)Rtio),
									armor.MarcosNum == 1 ? " gold pieces" : "",
									(armor.MarcosName ?? armor.Name).ToLower());
							}
						}

						var str = Buf.ToString();

						Buf.SetFormat("{0}Marcos takes you to the armor section of his shop and shows you the various suits of armor available for purchase.{0}{0}He says, \"I can put you in any of these very cheaply.  I need {1}.{0}", Environment.NewLine, str);

						if (ti > 0)
						{
							Buf.AppendPrint("Also, I can give you a trade-in on your old armor of {0} gold piece{1}.", ti, ti != 1 ? "s" : "");
						}

						Buf.AppendPrint("Well, what will it be?\"");

						Globals.Out.Write("{0}", Buf);

						Globals.Out.Print("{0}", Globals.LineSep);

						Buf.Clear();

						j = Globals.Engine.GetMaxArmorMarcosNum();

						for (i = 0; i < armorValues.Count; i++)
						{
							armor = Globals.Engine.GetArmors(armorValues[(int)i]);

							Debug.Assert(armor != null);

							if (armor.MarcosNum > 0)
							{
								Buf.AppendFormat("{0}{1}{2}={3}{4}",
									armor.MarcosNum == 1 ? Environment.NewLine : "",
									armor.MarcosNum != 1 ? ", " : "",
									armor.MarcosNum,
									armor.MarcosName ?? armor.Name,
									armor.MarcosNum == j ? ", X=Exit: " : "");
							}
						}

						Globals.Out.Write("{0}", Buf);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharMarcosNumOrX, Globals.Engine.IsCharMarcosNumOrX);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Thread.Sleep(150);

						Globals.Out.Print("{0}", Globals.LineSep);

						if (Buf.Length > 0 && Buf[0] != 'X')
						{
							i = Convert.ToInt64(Buf.Trim().ToString());

							armor = Globals.Engine.GetArmorByMarcosNum(i);

							Debug.Assert(armor != null);

							ap = Globals.Engine.GetMerchantAskPrice(armor.MarcosPrice, (double)Rtio);

							if (Globals.Character.HeldGold + ti >= ap)
							{
								Globals.Out.Print("Marcos takes your gold{0} and helps you into your new armor.", ti > 0 ? " and your old armor" : "");

								Globals.Character.HeldGold += ti;

								Globals.Character.HeldGold -= ap;

								Globals.CharactersModified = true;

								for (i = 0; i < armorValues.Count; i++)
								{
									if (Globals.Engine.GetArmors(armorValues[(int)i]) == armor)
									{
										break;
									}
								}

								Debug.Assert(i < armorValues.Count);

								a2 = (long)armorValues[(int)i] / 2;
							}
							else
							{
								Globals.Out.Print("Marcos frowns when he sees that you do not have enough to pay for your armor and says, \"I don't give credit!\"");
							}
						}

						menuState = MenuState.BuyShield;

						break;

					case MenuState.BuyShield:

						if (sh != 1)
						{
							ap = Globals.Engine.GetMerchantAskPrice(Constants.ShieldPrice, (double)Rtio);

							Globals.Out.Print("Marcos smiles and says, \"Now how about a shield?  I can let you have one for only {0} gold piece{1}!\"", ap, ap != 1 ? "s" : "");

							Globals.Out.Print("{0}", Globals.LineSep);

							Globals.Out.Write("{0}Press Y for yes or N for no: ", Environment.NewLine);

							Buf.Clear();

							rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Thread.Sleep(150);

							Globals.Out.Print("{0}", Globals.LineSep);

							if (Buf.Length > 0 && Buf[0] == 'Y')
							{
								if (Globals.Character.HeldGold >= ap)
								{
									Globals.Out.Print("Marcos takes your gold and gives you a shield.");

									Globals.Character.HeldGold -= ap;

									Globals.CharactersModified = true;

									sh = 1;
								}
								else
								{
									Globals.Out.Print("When he sees that you do not have enough gold to buy the shield, Marcos frowns and says, \"I'm sorry, but I don't give credit!\"");
								}
							}
						}

						menuState = MenuState.SetArmorClass;

						break;

					case MenuState.SetArmorClass:

						if (Globals.Character.ArmorClass != (Enums.Armor)(a2 * 2 + sh))
						{
							Globals.Character.ArmorClass = (Enums.Armor)(a2 * 2 + sh);

							Globals.Character.Armor = Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
							{
								x.Parent = Globals.Character;
							});

							Globals.Character.Shield = Globals.CreateInstance<Classes.ICharacterArtifact>(x =>
							{
								x.Parent = Globals.Character;
							});

							Globals.CharactersModified = true;
						}

						menuState = MenuState.LeaveShop;

						break;

					case MenuState.LeaveShop:

						Globals.Out.Print("Marcos smiles and says, \"Come back again soon!\" as he shoos you out of his shop.");

						Globals.In.KeyPress(Buf);

						goto Cleanup;
				}
			}

		Cleanup:

			;
		}

		public MarcosCavielliMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
