
// PluginClassMappings.cs

// Copyright (c) 2014-2017 by YourAuthorName.  All rights reserved

using System.Reflection;
using Eamon;
using YourAdventureName.Framework.Plugin;

namespace YourAdventureName.Game.Plugin
{
	public class PluginClassMappings : EamonRT.Game.Plugin.PluginClassMappings, IPluginClassMappings
	{
		public override RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = base.LoadPluginClassMappings();

			if (rc != RetCode.Success)
			{
				goto Cleanup;
			}

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

		Cleanup:

			return rc;
		}
	}
}
