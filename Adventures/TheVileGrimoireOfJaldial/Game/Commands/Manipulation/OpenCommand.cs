
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			// If wine cask opened reveal crimson amoeba

			if (eventType == PpeAfterArtifactOpen && DobjArtifact.Uid == 17 && !gGameState.AmoebaAppeared)
			{
				var amoebaMonster = gMDB[25];

				Debug.Assert(amoebaMonster != null);

				amoebaMonster.SetInRoom(ActorRoom);

				gEngine.PrintEffectDesc(102);

				gGameState.AmoebaAppeared = true;

				NextState = Globals.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Book of runes can't be opened

			if (DobjArtifact.Uid == 27)
			{
				gEngine.PrintEffectDesc(90);

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}

			// DoorGates and InContainers sometimes need to be pryed open

			else if (((DobjArtifact.Uid == 3 || DobjArtifact.Uid == 4 || DobjArtifact.Uid == 5) && !DobjArtifact.DoorGate.IsOpen()) || ((DobjArtifact.Uid == 13 || DobjArtifact.Uid == 35) && !DobjArtifact.InContainer.IsOpen() && DobjArtifact.InContainer.GetKeyUid() == -1))
			{
				// Reset the Key Uid to the available Artifact with the best leverage (if any)

				var keyList = gADB.Records.Cast<Framework.IArtifact>().Where(a => (a.IsInRoom(ActorRoom) || a.IsCarriedByCharacter()) && a.GetLeverageBonus() > 0).OrderByDescending(a01 => a01.GetLeverageBonus()).ToList();

				var key = keyList.Count > 0 ? keyList[0] : null;

				if (key != null)
				{
					if (DobjArtifact.DoorGate != null)
					{
						DobjArtifact.DoorGate.SetKeyUid(key.Uid);
					}
					else
					{
						DobjArtifact.InContainer.SetKeyUid(key.Uid);
					}

					gOut.Print("[Using {0} for leverage.]", key.GetTheName());

					// Failed save throw versus Hardiness means door/container still stuck (should try again)

					if (!gEngine.SaveThrow(Stat.Hardiness, key.GetLeverageBonus()))
					{
						var keyLocation = key.Location;

						key.SetInLimbo();

						base.PlayerExecute();

						key.Location = keyLocation;
					}
					else
					{
						base.PlayerExecute();
					}
				}
				else
				{
					base.PlayerExecute();
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
