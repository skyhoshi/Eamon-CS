
// Program.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

/*

*/

using Eamon.Framework.Portability;

namespace TheTempleOfNgurct
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "TheTempleOfNgurct";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
