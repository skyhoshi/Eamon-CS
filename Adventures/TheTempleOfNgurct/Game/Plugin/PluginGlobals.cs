
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace TheTempleOfNgurct.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
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

		public virtual bool FireDamage { get; set; }
	}
}
