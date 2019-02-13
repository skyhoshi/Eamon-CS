
// IIntroStory.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Text;
using RTEnums = EamonRT.Framework.Primitive.Enums;

namespace EamonRT.Framework
{
	/// <summary></summary>
	public interface IIntroStory
	{
		/// <summary></summary>
		StringBuilder Buf { get; set; }

		/// <summary></summary>
		RTEnums.IntroStoryType StoryType { get; set; }

		/// <summary></summary>
		bool ShouldPrintOutput { get; set; }

		/// <summary></summary>
		void PrintOutput();
	}
}
