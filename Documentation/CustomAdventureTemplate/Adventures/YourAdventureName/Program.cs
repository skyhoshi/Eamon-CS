
// Program.cs

// Copyright (c) 2014-2017 by YourAuthorName.  All rights reserved

/*

*/

using Eamon.Framework.Portability;

namespace YourAdventureName
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "YourAdventureName";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
