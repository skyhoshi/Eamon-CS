
// GiveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeBeforeMonsterTakesGold && (gIobjMonster.Uid == 1 || gIobjMonster.Uid == 5 || gIobjMonster.Uid == 7))
			{
				gEngine.MonsterEmotes(gIobjMonster);

				gOut.WriteLine();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
