
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Classes = Eamon.Framework.Primitive.Classes;

namespace ARuncibleCargo.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		IList<Classes.IArtifactLinkage> DoubleDoors { get; set; }
	}
}
