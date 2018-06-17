
// IntroStory.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using RTEnums = EamonRT.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game
{
	[ClassMappings]
	public class IntroStory : IIntroStory
	{
		public virtual StringBuilder Buf { get; set; }

		public virtual RTEnums.IntroStoryType StoryType { get; set; }

		public virtual bool ShouldPrintOutput
		{
			get
			{
				var result = true;

				if (Globals.Database.GetFilesetsCount() > 0)
				{
					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Out.Print("Welcome back to {0}!", Globals.Module.Name);

					Globals.Out.Write("{0}Would you like to see the introduction story again (Y/N) [N]: ", Environment.NewLine);

					Buf.Clear();

					var rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, "N", Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					if (Buf.Length > 0 && Buf[0] != 'Y')
					{
						result = false;
					}
				}

				return result;
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		protected virtual bool IsCharWpnNum(char ch)
		{
			var i = 0L;

			var rc = Globals.Character.GetWeaponCount(ref i);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			return ch >= '1' && ch <= ('1' + (i - 1));
		}

		protected virtual void PrintOutputBeginnersPrelude() { }

		protected virtual void PrintOutputBeginnersTooManyWeapons() { }

		protected virtual void PrintOutputBeginnersNoWeapons() { }

		protected virtual void PrintOutputBeginnersNotABeginner() { }

		protected virtual void PrintOutputBeginnersMayNowProceed() { }

		protected virtual void PrintOutputBeginners()
		{
			RetCode rc;

			Globals.Out.Print("{0}", Globals.LineSep);

			PrintOutputBeginnersPrelude();

			var i = 0L;       // weird disambiguation hack

			if (!Globals.Character.GetWeapons(i).IsActive())
			{
				PrintOutputBeginnersNoWeapons();

				Globals.MainLoop.ShouldExecute = false;

				Globals.ExitType = Enums.ExitType.GoToMainHall;
			}
			else if (Globals.Character.ArmorExpertise != 0 || Globals.Character.GetWeaponAbilities(Enums.Weapon.Axe) != 5 || Globals.Character.GetWeaponAbilities(Enums.Weapon.Club) != 20 || Globals.Character.GetWeaponAbilities(Enums.Weapon.Sword) != 0)
			{
				PrintOutputBeginnersNotABeginner();

				Globals.MainLoop.ShouldExecute = false;

				Globals.ExitType = Enums.ExitType.GoToMainHall;
			}
			else
			{
				if (Globals.Character.GetWeapons(1).IsActive())
				{
					PrintOutputBeginnersTooManyWeapons();

					Buf.Clear();

					Globals.Character.ListWeapons(Buf);

					Globals.Out.WriteLine("{0}", Buf);

					Globals.Out.Print("{0}", Globals.LineSep);

					Globals.Out.Write("{0}Press the number of the weapon to select: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, IsCharWpnNum, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					Globals.Out.Print("{0}", Globals.LineSep);

					Debug.Assert(Globals.GameState != null);

					Globals.GameState.UsedWpnIdx = Convert.ToInt64(Buf.Trim().ToString());

					Globals.GameState.UsedWpnIdx--;
				}

				PrintOutputBeginnersMayNowProceed();
			}
		}

		protected virtual void PrintOutputDefault()
		{
			Globals.Out.Print("{0}", Globals.LineSep);

			var effect = Globals.EDB[Globals.Module.IntroStory];

			if (effect != null)
			{
				Globals.Engine.PrintMacroReplacedPagedString(effect.Desc, Buf);
			}
			else
			{
				Globals.Out.Print("There is no introduction story for this adventure.");
			}
		}

		public virtual void PrintOutput()
		{
			switch(StoryType)
			{
				case RTEnums.IntroStoryType.Beginners:

					PrintOutputBeginners();

					break;

				default:

					PrintOutputDefault();

					break;
			}
		}

		public IntroStory()
		{
			Buf = Globals.Buf;

			StoryType = RTEnums.IntroStoryType.Default;
		}
	}
}
