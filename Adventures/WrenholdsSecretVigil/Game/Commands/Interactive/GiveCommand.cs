
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
				if (IobjMonster.Uid == 1)
				{
					// Give death dog the dead rabbit

					if (DobjArtifact.Uid == 15)
					{
						DobjArtifact.SetInLimbo();

						IobjMonster.Friendliness = (Friendliness)150;

						IobjMonster.OrigFriendliness = (Friendliness)150;

						IobjMonster.ResolveFriendlinessPct(gCharacter);

						PrintGiveObjToActor(DobjArtifact, IobjMonster);

						gEngine.PrintEffectDesc(13);

						if (IobjMonster.Friendliness == Friendliness.Friend)
						{
							gOut.Print("{0} barks once and wags its tail!", IobjMonster.GetTheName(true));
						}
					}
					else
					{
						gEngine.MonsterEmotes(IobjMonster);

						gOut.WriteLine();
					}

					GotoCleanup = true;
				}

				// Further disable bribing

				else if (gIobjMonster(this).ShouldRefuseToAcceptGift01(DobjArtifact))
				{
					gEngine.MonsterEmotes(IobjMonster);

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

				if (IobjMonster.Uid == 1 || IobjMonster.Friendliness < Friendliness.Friend)
				{
					gEngine.MonsterEmotes(IobjMonster);

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
