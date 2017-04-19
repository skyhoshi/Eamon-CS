
// Config.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game
{
	[ClassMappings(typeof(Eamon.Framework.IConfig))]
	public class Config : Eamon.Game.Config, IConfig
	{
		public override RetCode DeleteGameState(string configFileName, bool startOver)
		{
			RetCode rc;

			try
			{
				Globals.File.Delete(Constants.SnapshotFileName);
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
