
// SaveCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game.Commands
{
	[ClassMappings]
	public class SaveCommand : EamonRT.Game.Commands.SaveCommand, ISaveCommand
	{
		public override void PlayerFinishParsing()
		{
			if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				gEngine.PrintEffectDesc(30); //Cannot save with enemies nearby
				gCommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.PlayerFinishParsing();
			}
		}
	}
}
