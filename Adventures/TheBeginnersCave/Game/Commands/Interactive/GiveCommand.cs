
// GiveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, EamonRT.Framework.Commands.IGiveCommand
	{
		protected override void PlayerProcessEvents02()
		{
			if (IobjMonster.Uid == 1 || IobjMonster.Uid == 5 || IobjMonster.Uid == 7)
			{
				Globals.Engine.MonsterSmiles(IobjMonster);

				Globals.Out.WriteLine();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents02();
			}
		}
	}
}
