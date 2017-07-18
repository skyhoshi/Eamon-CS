
// IntroStory.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using TheTrainingGround.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IIntroStory))]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		public override void PrintOutput()
		{
			base.PrintOutput();

			Globals.In.KeyPress(Buf);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			// Inspect the player

			Globals.Engine.PrintEffectDesc(17);

			if (Globals.Character.ArmorExpertise > 25)
			{
				Globals.Engine.PrintEffectDesc(18);

				Globals.MainLoop.ShouldExecute = false;

				Globals.ExitType = Enums.ExitType.GoToMainHall;
			}
			else
			{
				if (Globals.Character.ArmorExpertise == 0)
				{
					Globals.Engine.PrintEffectDesc(19);
				}

				Globals.Out.Write("{0}\"OK, let's be careful in there, {1}!\" he says, as he walks away.{0}", Environment.NewLine, Globals.Character.EvalGender("son", "miss", ""));
			}
		}
	}
}
