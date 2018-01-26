
// IIntroStory.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Text;
using RTEnums = EamonRT.Framework.Primitive.Enums;

namespace EamonRT.Framework
{
	public interface IIntroStory
	{
		StringBuilder Buf { get; set; }

		RTEnums.IntroStoryType StoryType { get; set; }

		bool ShouldPrintOutput { get; set; }

		void PrintOutput();
	}
}
