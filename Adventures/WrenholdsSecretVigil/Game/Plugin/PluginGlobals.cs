
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using WrenholdsSecretVigil.Framework.Plugin;

namespace WrenholdsSecretVigil.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, IPluginGlobals
	{
		public virtual new Framework.IEngine Engine
		{
			get
			{
				return (Framework.IEngine)base.Engine;
			}

			set
			{
				if (base.Engine != value)
				{
					base.Engine = value;
				}
			}
		}

		public virtual bool MonsterCurses { get; set; }

		public virtual bool DeviceOpened { get; set; }
	}
}
