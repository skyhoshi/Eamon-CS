
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework
{
	public interface IEngine : EamonRT.Framework.IEngine
	{
		bool SaveThrow(Stat stat, long bonus = 0);		// Note:  maybe move this into EamonRT's Engine class?
	}
}
