
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

			if (eventType == PpeAfterArtifactOpen && gDobjArtifact.Uid == 17 && !gGameState.AmoebaAppeared)
			{
				var amoebaMonster = gMDB[25];

				Debug.Assert(amoebaMonster != null);

				amoebaMonster.SetInRoom(gActorRoom);

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
			Debug.Assert(gDobjArtifact != null);

			// Book of runes can't be opened

			if (gDobjArtifact.Uid == 27)
			{
				gEngine.PrintEffectDesc(90);

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}

			// DoorGates and InContainers sometimes need to be pryed open

			else if (((gDobjArtifact.Uid == 3 || gDobjArtifact.Uid == 4 || gDobjArtifact.Uid == 5) && !gDobjArtifact.DoorGate.IsOpen()) || ((gDobjArtifact.Uid == 13 || gDobjArtifact.Uid == 35) && !gDobjArtifact.InContainer.IsOpen() && gDobjArtifact.InContainer.GetKeyUid() == -1))
			{
				// Reset the Key Uid to the available Artifact with the best leverage (if any)

				var keyList = gADB.Records.Cast<Framework.IArtifact>().Where(a => (a.IsInRoom(gActorRoom) || a.IsCarriedByCharacter()) && a.GetLeverageBonus() > 0).OrderByDescending(a01 => a01.GetLeverageBonus()).ToList();

				var key = keyList.Count > 0 ? keyList[0] : null;

				if (key != null)
				{
					if (gDobjArtifact.DoorGate != null)
					{
						gDobjArtifact.DoorGate.SetKeyUid(key.Uid);
					}
					else
					{
						gDobjArtifact.InContainer.SetKeyUid(key.Uid);
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
