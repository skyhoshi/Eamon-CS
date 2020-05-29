
// GiveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterEnforceMonsterWeightLimitsCheck)
			{
				if (gIobjMonster.Uid == 1)
				{
					// Give death dog the dead rabbit

					if (gDobjArtifact.Uid == 15)
					{
						gDobjArtifact.SetInLimbo();

						gIobjMonster.Friendliness = (Friendliness)150;

						gIobjMonster.OrigFriendliness = (Friendliness)150;

						gIobjMonster.ResolveFriendlinessPct(gCharacter);

						PrintGiveObjToActor(gDobjArtifact, gIobjMonster);

						gEngine.PrintEffectDesc(13);

						if (gIobjMonster.Friendliness == Friendliness.Friend)
						{
							gOut.Print("{0} barks once and wags its tail!", gIobjMonster.GetTheName(true));
						}
					}
					else
					{
						gEngine.MonsterEmotes(gIobjMonster);

						gOut.WriteLine();
					}

					GotoCleanup = true;
				}

				// Further disable bribing

				else if (gIobjMonster.ShouldRefuseToAcceptGift01(gDobjArtifact))
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
			else if (eventType == PpeBeforeMonsterTakesGold)
			{
				// Disable bribing

				if (gIobjMonster.Uid == 1 || gIobjMonster.Friendliness < Friendliness.Friend)
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
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}
	}
}
