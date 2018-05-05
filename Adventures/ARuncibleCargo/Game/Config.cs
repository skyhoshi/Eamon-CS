
// Config.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using Eamon;
using Eamon.Game.Attributes;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game
{
	[ClassMappings]
	public class Config : Eamon.Game.Config, Eamon.Framework.IConfig
	{
		public override RetCode DeleteGameState(string configFileName, bool startOver)
		{
			RetCode rc;

			try
			{
				Globals.File.Delete(Globals.GetPrefixedFileName(Constants.SnapshotFileName));
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}

			rc = base.DeleteGameState(configFileName, startOver);

			return rc;
		}
	}
}
