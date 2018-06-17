
// Program.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

/*

Originally based upon Eamon's Adventure #150 MAIN PGM:

 1  REM 
WALLED CITY OF DARKNESS
    - BY -
TOM ZUCHOWSKI
CLEMMONS, NC

 5  REM MP51
 6  REM 12/6/89
 8  REM 
EAMON ADVENTURER'S GUILD
CLEMMONS, NC

*/

using Eamon.Framework.Portability;

namespace WalledCityOfDarkness
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "WalledCityOfDarkness";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
