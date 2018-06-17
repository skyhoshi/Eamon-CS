
// GetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void PrintCantVerbThat(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var ac = artifact.GetCategories(0);

			Debug.Assert(ac != null);

			Globals.Buf.Clear();

			switch (ac.Field4)
			{
				case -1:

					Globals.Buf.SetPrint("{0} {1} affixed to the wall.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf01), artifact.EvalPlural("is", "are"));

					break;

				case -2:

					Globals.Buf.SetPrint("{0} {1} carved into the wall.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf01), artifact.EvalPlural("is", "are"));

					break;

				case -3:

					Globals.Buf.SetPrint("{0} {1} bolted down, and can't be removed.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf01), artifact.EvalPlural("is", "are"));

					break;

				case -4:

					Globals.Buf.SetPrint("You can't get near enough to {0} to grab {1}.", artifact.GetDecoratedName03(false, true, false, false, Globals.Buf01), artifact.EvalPlural("it", "them"));

					break;
			}

			if (Globals.Buf.Length > 0)
			{
				Globals.Out.Write("{0}", Globals.Buf);
			}
			else
			{
				base.PrintCantVerbThat(artifact);
			}
		}
	}
}
