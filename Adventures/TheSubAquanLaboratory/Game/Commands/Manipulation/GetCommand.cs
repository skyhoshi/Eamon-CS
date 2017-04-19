
// GetCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using TheSubAquanLaboratory.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IGetCommand))]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		protected override void PrintCantVerbThat(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var ac = artifact.GetClasses(0);

			Debug.Assert(ac != null);

			Globals.Buf.Clear();

			switch (ac.Field8)
			{
				case -1:

					Globals.Buf.SetFormat("{0}{1} {2} affixed to the wall.{0}", Environment.NewLine, artifact.GetDecoratedName03(true, true, false, false, Globals.Buf01), artifact.EvalPlural("is", "are"));

					break;

				case -2:

					Globals.Buf.SetFormat("{0}{1} {2} carved into the wall.{0}", Environment.NewLine, artifact.GetDecoratedName03(true, true, false, false, Globals.Buf01), artifact.EvalPlural("is", "are"));

					break;

				case -3:

					Globals.Buf.SetFormat("{0}{1} {2} bolted down, and can't be removed.{0}", Environment.NewLine, artifact.GetDecoratedName03(true, true, false, false, Globals.Buf01), artifact.EvalPlural("is", "are"));

					break;

				case -4:

					Globals.Buf.SetFormat("{0}You can't get near enough to {1} to grab {2}.{0}", Environment.NewLine, artifact.GetDecoratedName03(false, true, false, false, Globals.Buf01), artifact.EvalPlural("it", "them"));

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
