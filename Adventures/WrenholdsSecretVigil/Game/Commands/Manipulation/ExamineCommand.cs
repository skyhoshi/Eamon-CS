
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, EamonRT.Framework.Commands.IExamineCommand
	{
		protected override void PlayerExecute()
		{
			if (DobjArtifact != null)
			{
				var diaryArtifact = Globals.ADB[3];

				Debug.Assert(diaryArtifact != null);

				var leverArtifact = Globals.ADB[48];

				Debug.Assert(leverArtifact != null);

				// Find dead zombies are in disguise

				if (DobjArtifact.Uid >= 58 && DobjArtifact.Uid <= 63)
				{
					Globals.Engine.PrintEffectDesc(15);
				}

				// Find diary on dead adventurer

				else if (DobjArtifact.Uid == 2 && diaryArtifact.IsInLimbo())
				{
					Globals.Engine.PrintEffectDesc(16);

					diaryArtifact.SetInRoom(ActorRoom);
				}

				// Examine slime

				else if (DobjArtifact.Uid == 24 || DobjArtifact.Uid == 25)
				{
					Globals.Engine.PrintEffectDesc(17);
				}

				// Examine green device, find lever

				else if (DobjArtifact.Uid == 44 && DobjArtifact.IsInRoom(ActorRoom))
				{
					base.PlayerExecute();

					if (leverArtifact.IsInLimbo())
					{
						leverArtifact.SetInRoom(ActorRoom);
					}
				}
				else if (DobjArtifact.IsCharOwned)
				{
					Globals.Out.Print("You see nothing unusual about {0}.", DobjArtifact.GetDecoratedName02(false, true, false, false, Globals.Buf));
				}

				// If not special dead body, send msg

				else if (DobjArtifact.Uid >= 51)
				{
					Globals.Out.Print("You find nothing special about {0}.", DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
				}
				else
				{
					base.PlayerExecute();
				}

				if (NextState == null)
				{
					NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
