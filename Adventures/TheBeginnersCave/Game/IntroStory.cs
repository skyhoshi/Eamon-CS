
// IntroStory.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework;
using RTEnums = EamonRT.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IIntroStory))]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		protected override void PrintOutputBeginnersPrelude()
		{
			Globals.Engine.PrintEffectDesc(8);

			var room = Globals.RDB[Globals.Engine.StartRoom];

			Debug.Assert(room != null);

			Globals.Out.Print("{0}", room.Desc);

			Globals.Engine.PrintEffectDesc(10);
		}

		protected override void PrintOutputBeginnersTooManyWeapons()
		{
			Globals.Engine.PrintEffectDesc(13);
		}

		protected override void PrintOutputBeginnersNoWeapons()
		{
			Globals.Engine.PrintEffectDesc(9);
		}

		protected override void PrintOutputBeginnersNotABeginner()
		{
			Globals.Engine.PrintEffectDesc(11);
		}

		protected override void PrintOutputBeginnersMayNowProceed()
		{
			Globals.Engine.PrintEffectDesc(12);
		}

		public IntroStory()
		{
			StoryType = RTEnums.IntroStoryType.Beginners;
		}
	}
}
