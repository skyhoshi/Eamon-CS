
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework.Primitive.Classes;

namespace ARuncibleCargo.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{
		public virtual IList<IArtifactLinkage> DoubleDoors { get; set; }

		public override void InitSystem()
		{
			base.InitSystem();

			DoubleDoors = new List<IArtifactLinkage>();
		}
	}
}
