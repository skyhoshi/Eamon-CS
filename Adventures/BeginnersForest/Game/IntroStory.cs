
// IntroStory.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework;
using RTEnums = EamonRT.Framework.Primitive.Enums;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game
{
	[ClassMappings]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		protected override void PrintOutputBeginnersPrelude()
		{
			Globals.Engine.PrintEffectDesc(9);

			Globals.Engine.PrintEffectDesc(11);
		}

		protected override void PrintOutputBeginnersTooManyWeapons()
		{
			Globals.Engine.PrintEffectDesc(16);
		}

		protected override void PrintOutputBeginnersNoWeapons()
		{
			Globals.Engine.PrintEffectDesc(12);
		}

		protected override void PrintOutputBeginnersNotABeginner()
		{
			Globals.Engine.PrintEffectDesc(13);
		}

		protected override void PrintOutputBeginnersMayNowProceed()
		{
			Globals.Engine.PrintEffectDesc(15);
		}

		public IntroStory()
		{
			StoryType = RTEnums.IntroStoryType.Beginners;
		}
	}
}
