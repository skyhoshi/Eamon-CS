
// GiveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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

						Globals.Engine.CheckEnemies();

						PrintGiveObjToActor(DobjArtifact, IobjMonster);

						Globals.Engine.PrintEffectDesc(13);

						if (IobjMonster.Friendliness == Friendliness.Friend)
						{
							Globals.Out.Print("{0} barks once and wags its tail!", IobjMonster.GetDecoratedName03(true, true, false, false, Globals.Buf));
						}
					}
					else
					{
						Globals.Engine.MonsterSmiles(IobjMonster);

						Globals.Out.WriteLine();
					}

					GotoCleanup = true;
				}

				// Further disable bribing

				else if (IobjMonster.CastTo<Framework.IMonster>().ShouldRefuseToAcceptGift01(DobjArtifact))
				{
					Globals.Engine.MonsterSmiles(IobjMonster);

					Globals.Out.WriteLine();

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
					Globals.Engine.MonsterSmiles(IobjMonster);

					Globals.Out.WriteLine();

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
