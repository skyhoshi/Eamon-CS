
// IArtifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework
{
	public interface IArtifact : Eamon.Framework.IArtifact
	{
		long GetLeverageBonus();
	}
}
