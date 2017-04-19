
// IntroStory.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IIntroStory))]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		protected virtual bool IsCharWpnNum(char ch)
		{
			var i = 0L;

			var rc = Globals.Character.GetWeaponCount(ref i);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			return ch >= '1' && ch <= ('1' + (i - 1));
		}

		public override void PrintOutput()
		{
			RetCode rc;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			Globals.Engine.PrintEffectDesc(8);

			var room = Globals.RDB[Globals.RtEngine.StartRoom];

			Debug.Assert(room != null);

			Globals.Out.Write("{0}{1}{0}", Environment.NewLine, room.Desc);

			Globals.Engine.PrintEffectDesc(10);

			var i = 0L;			// weird disambiguation hack

			if (!Globals.Character.GetWeapons(i).IsActive())
			{
				Globals.Engine.PrintEffectDesc(9);

				Globals.MainLoop.ShouldExecute = false;

				Globals.ExitType = Enums.ExitType.GoToMainHall;
			}
			else if (Globals.Character.ArmorExpertise != 0 || Globals.Character.GetWeaponAbilities(Enums.Weapon.Axe) != 5 || Globals.Character.GetWeaponAbilities(Enums.Weapon.Club) != 20 || Globals.Character.GetWeaponAbilities(Enums.Weapon.Sword) != 0)
			{
				Globals.Engine.PrintEffectDesc(11);

				Globals.MainLoop.ShouldExecute = false;

				Globals.ExitType = Enums.ExitType.GoToMainHall;
			}
			else
			{
				if (Globals.Character.GetWeapons(1).IsActive())
				{
					Globals.Engine.PrintEffectDesc(13);

					Buf.Clear();

					Globals.Character.ListWeapons(Buf);

					Globals.Out.WriteLine("{0}", Buf);

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					Globals.Out.Write("{0}Press the number of the weapon to select: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, IsCharWpnNum, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					var gameState = Globals.GameState as IGameState;

					Debug.Assert(gameState != null);

					gameState.UsedWpnIdx = Convert.ToInt64(Buf.Trim().ToString());

					gameState.UsedWpnIdx--;
				}

				Globals.Engine.PrintEffectDesc(12);
			}
		}
	}
}
