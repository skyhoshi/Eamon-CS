
// WearCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			// Steel gauntlets boost weapon skills

			if (eventType == PpeAfterArtifactWear && gDobjArtifact.Uid == 16)
			{
				var weaponValues = EnumUtil.GetValues<Weapon>();

				foreach (var wv in weaponValues)
				{
					var weapon = gEngine.GetWeapons(wv);

					Debug.Assert(weapon != null);

					gCharacter.ModWeaponAbilities(wv, 5);

					if (gCharacter.GetWeaponAbilities(wv) > weapon.MaxValue)
					{
						gCharacter.SetWeaponAbilities(wv, weapon.MaxValue);
					}
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

			// Crimson cloak boosts armor class

			if (gDobjArtifact.Uid == 19 && gDobjArtifact.IsCarriedByCharacter())
			{
				var armorArtifact = gGameState.Ar > 0 ? gADB[gGameState.Ar] : null;

				if (armorArtifact != null)
				{
					armorArtifact.Wearable.Field1 += 2;
				}
				else
				{
					gDobjArtifact.Wearable.Field1 += 2;
				}
			}

			base.PlayerExecute();
		}
	}
}
