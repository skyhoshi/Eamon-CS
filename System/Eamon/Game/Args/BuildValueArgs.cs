
// BuildValueArgs.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Text;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Args
{
	[ClassMappings]
	public class BuildValueArgs : IBuildValueArgs
	{
		public virtual long BufSize { get; set; }

		public virtual char FillChar { get; set; }

		public virtual long Offset { get; set; }

		public virtual StringBuilder Buf { get; set; }

		public BuildValueArgs()
		{
			Buf = new StringBuilder(Constants.BufSize);
		}
	}
}
