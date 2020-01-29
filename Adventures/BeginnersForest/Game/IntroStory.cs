
// IntroStory.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework;
using EamonRT.Framework.Primitive.Enums;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game
{
	[ClassMappings]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		protected override void PrintOutputBeginnersPrelude()
		{
			gEngine.PrintEffectDesc(9);

			gEngine.PrintEffectDesc(11);
		}

		protected override void PrintOutputBeginnersTooManyWeapons()
		{
			gEngine.PrintEffectDesc(16);
		}

		protected override void PrintOutputBeginnersNoWeapons()
		{
			gEngine.PrintEffectDesc(12);
		}

		protected override void PrintOutputBeginnersNotABeginner()
		{
			gEngine.PrintEffectDesc(13);
		}

		protected override void PrintOutputBeginnersMayNowProceed()
		{
			gEngine.PrintEffectDesc(15);
		}

		public IntroStory()
		{
			StoryType = IntroStoryType.Beginners;
		}
	}
}
