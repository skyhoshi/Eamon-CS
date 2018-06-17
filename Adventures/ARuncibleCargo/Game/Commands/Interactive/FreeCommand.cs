
// FreeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, IFreeCommand
	{
		public override void PrintMonsterFreed()
		{
			// Swarmy

			if (Monster.Uid == 31)
			{
				Globals.Buf.Clear();

				var rc = Monster.BuildPrintedFullDesc(Globals.Buf, false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.Write("{0}", Globals.Buf);

				Monster.Seen = true;

				Globals.Engine.PrintEffectDesc(64);
			}
			else
			{
				base.PrintMonsterFreed();
			}
		}
	}
}
