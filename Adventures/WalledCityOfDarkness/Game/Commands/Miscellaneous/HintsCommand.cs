
// HintsCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static WalledCityOfDarkness.Game.Plugin.PluginContext;

namespace WalledCityOfDarkness.Game.Commands
{
	[ClassMappings]
	public class HintsCommand : EamonRT.Game.Commands.HintsCommand, IHintsCommand
	{
		protected override void PrintHintsAnswer(IList<IHint> hints, int i, int j)
		{
			Debug.Assert(hints != null);

			if (i == 2)
			{
				Globals.Out.Print("{0}", Globals.LineSep);
			}

			base.PrintHintsAnswer(hints, i, j);

			if (i == 2)
			{
				Globals.Out.WriteLine();

				Globals.In.KeyPress(Globals.Buf);

				Globals.Out.Print("{0}", Globals.LineSep);
			}
		}
	}
}
