
// PluginGlobals.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using TheTempleOfNgurct.Framework.Plugin;

namespace TheTempleOfNgurct.Game.Plugin
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

		public virtual bool FireDamage { get; set; }
	}
}
