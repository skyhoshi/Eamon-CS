
// ListArgs.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Text;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Args
{
	[ClassMappings]
	public class ListArgs : IListArgs
	{
		public virtual bool FullDetail { get; set; }

		public virtual bool ShowDesc { get; set; }

		public virtual bool ResolveEffects { get; set; }

		public virtual bool LookupMsg { get; set; }

		public virtual bool NumberFields { get; set; }

		public virtual bool ExcludeROFields { get; set; }

		public virtual StringBuilder Buf { get; set; }

		public virtual StringBuilder Buf01 { get; set; }

		public virtual long ListNum { get; set; }

		public ListArgs()
		{
			Buf = new StringBuilder(Constants.BufSize);

			Buf01 = new StringBuilder(Constants.BufSize);

			ListNum = 1;
		}
	}
}
