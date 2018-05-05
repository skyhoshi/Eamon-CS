
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Classes = Eamon.Framework.Primitive.Classes;

namespace ARuncibleCargo.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{
		public virtual IList<Classes.IArtifactLinkage> DoubleDoors { get; set; }

		public override void InitSystem()
		{
			base.InitSystem();

			DoubleDoors = new List<Classes.IArtifactLinkage>();
		}
	}
}
