
// SayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			// Summon Alkanda

			if (eventType == PpeBeforePlayerSayTextPrint && string.Equals(ProcessedPhrase, "annal natthrac", StringComparison.OrdinalIgnoreCase))
			{
				var medallionArtifact = gADB[77];

				Debug.Assert(medallionArtifact != null);

				if (medallionArtifact.IsCarriedByCharacter() || medallionArtifact.IsInRoom(gActorRoom))
				{
					var alkandaMonster = gMDB[56];

					Debug.Assert(alkandaMonster != null);

					if (!alkandaMonster.IsInRoom(gActorRoom) && !gGameState.AlkandaKilled)
					{
						alkandaMonster.SetInRoom(gActorRoom);

						NextState = Globals.CreateInstance<IStartState>();
					}
				}
				else
				{
					gOut.Print("You don't have the medallion of Ngurct!");

					NextState = Globals.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}
			}

			base.PlayerProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
