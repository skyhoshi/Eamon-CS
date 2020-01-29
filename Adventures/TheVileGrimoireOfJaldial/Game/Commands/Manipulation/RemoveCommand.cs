
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			// Steel gauntlets boost weapon skills

			if (eventType == PpeAfterWornArtifactRemove && gDobjArtifact.Uid == 16)
			{
				var weaponValues = EnumUtil.GetValues<Weapon>();

				foreach (var wv in weaponValues)
				{
					gCharacter.ModWeaponAbilities(wv, -5);
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			if (gIobjArtifact == null)
			{
				var armorArtifact = gGameState.Ar > 0 ? gADB[gGameState.Ar] : null;

				var cloakArtifact = gADB[19];

				Debug.Assert(cloakArtifact != null);

				// Crimson cloak boosts armor class

				if (gDobjArtifact.Uid == 19 && armorArtifact != null)
				{
					armorArtifact.Wearable.Field1 -= 2;
				}

				// Remove crimson cloak if removing armor

				if (gDobjArtifact.Uid != 19 && gDobjArtifact.Uid == gGameState.Ar && cloakArtifact.IsWornByCharacter())
				{
					gOut.Print("[Removing {0} first.]", cloakArtifact.GetTheName());

					cloakArtifact.SetCarriedByCharacter();

					armorArtifact.Wearable.Field1 -= 2;
				}

				base.PlayerExecute();
			}

			// Large fountain

			else if (gIobjArtifact.Uid == 24)
			{
				var bucketArtifact = gADB[6];

				Debug.Assert(bucketArtifact != null);

				// Bail out water

				if (gDobjArtifact.Uid == 40)
				{
					// Use the wooden bucket 

					if (bucketArtifact.IsCarriedByCharacter() || bucketArtifact.IsInRoom(gActorRoom))
					{
						gOut.Print("[Using {0}.]", bucketArtifact.GetTheName());

						gOut.Print("You remove all the water from the fountain.");

						gDobjArtifact.SetInLimbo();

						gIobjArtifact.Desc = gIobjArtifact.Desc.Replace("squirts", "squirted");

						NextState = Globals.CreateInstance<IMonsterStartState>();
					}
					else
					{
						gOut.Print("There's no obvious way to do that.");

						NextState = Globals.CreateInstance<IStartState>();
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
