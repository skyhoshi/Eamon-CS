﻿
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void PlayerExecute()
		{
			if (DobjArtifact != null && !Enum.IsDefined(typeof(ContainerType), ContainerType))
			{
				var diaryArtifact = gADB[3];

				Debug.Assert(diaryArtifact != null);

				var leverArtifact = gADB[48];

				Debug.Assert(leverArtifact != null);

				// Find dead zombies are in disguise

				if (DobjArtifact.Uid >= 58 && DobjArtifact.Uid <= 63)
				{
					gEngine.PrintEffectDesc(15);
				}

				// Find diary on dead adventurer

				else if (DobjArtifact.Uid == 2 && diaryArtifact.IsInLimbo())
				{
					gEngine.PrintEffectDesc(16);

					diaryArtifact.SetInRoom(ActorRoom);
				}

				// Examine slime

				else if (DobjArtifact.Uid == 24 || DobjArtifact.Uid == 25)
				{
					gEngine.PrintEffectDesc(17);
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
					gOut.Print("You see nothing unusual about {0}.", DobjArtifact.GetArticleName());
				}

				// If not special dead body, send msg

				else if (DobjArtifact.Uid >= 51)
				{
					gOut.Print("You find nothing special about {0}.", DobjArtifact.GetTheName());
				}
				else
				{
					base.PlayerExecute();
				}

				if (NextState == null)
				{
					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
