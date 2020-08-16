
// SayCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Extensions;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void PlayerProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforePlayerSayTextPrint)
			{
				var Lisa = gMDB[3];

				// Assume custom text output, skip default behavior

				GotoCleanup = true;

				if (gLMKKP1.SaidHello == 1 && gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "hi", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(61);
				}
				else if (gLMKKP1.SaidHello == 1 && gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "hello", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(61);
				}
				else if (gLMKKP1.SaidHello == 0 && gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "hello", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(16);
					gEngine.PrintEffectDesc(17);
					gEngine.PrintEffectDesc(18);
					gLMKKP1.SaidHello = 1;
				}
				else if (gLMKKP1.SaidHello == 0 && gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "hi", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(19);
					gEngine.PrintEffectDesc(17);
					gEngine.PrintEffectDesc(18);
					gLMKKP1.SaidHello = 1;
				}
				else if (gLMKKP1.NecklaceTaken == 2 && ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "hi", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(62);
				}
				else if (gLMKKP1.NecklaceTaken == 2 && ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "hello", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(62);
				}
				else if (gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "damian", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(63);
				}
				else if (gLMKKP1.NecklaceTaken == 2 && ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "damian", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(64);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "necklace", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(65);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "bats", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(66);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "bat", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(66);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "cave", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(67);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "lighthouse", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(68);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "opening", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(69);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "window", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(69);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "squid", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(70);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "squids", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(70);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "warrior", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(71);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "tree", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "oak", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "oak tree", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "large tree", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "large oak", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "lisa", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(73);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "swamp", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(74);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "monster", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(75);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "monsters", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(75);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "werewolves", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(76);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "werewolf", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(76);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "ogres", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(77);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "ogre", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(77);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "king", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(78);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "mountain king", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(78);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "creatures", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(79);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "enemies", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(79);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "servants", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(79);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "land", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(81);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "reward", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(82);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "mountains", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(83);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "forest", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(84);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral && string.Equals(ProcessedPhrase, "hair", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(85);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Friendliness == Friendliness.Neutral)
				{
					gEngine.PrintEffectDesc(80);
				}
				else
				{
					// No custom text output, use default behavior

					GotoCleanup = false;
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
