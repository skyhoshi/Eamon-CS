
// WearCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class WearCommand : Command, IWearCommand
	{
		/// <summary>
		/// An event that fires after the player wears an <see cref="IArtifact">Artifact</see>.
		/// </summary>
		public const long PpeAfterArtifactWear = 1;

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var dobjAc = DobjArtifact.Wearable;

			if (dobjAc != null)
			{
				if (DobjArtifact.IsWornByCharacter())
				{
					gOut.Print("You're already wearing {0}!", DobjArtifact.EvalPlural("it", "them"));

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByCharacter())
				{
					if (!GetCommandCalled)
					{
						RedirectToGetCommand<IWearCommand>(DobjArtifact);
					}
					else if (DobjArtifact.DisguisedMonster == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (dobjAc.Field1 > 0)
				{
					var arArtifact = gADB[gGameState.Ar];

					var shArtifact = gADB[gGameState.Sh];

					var arAc = arArtifact != null ? arArtifact.Wearable : null;

					var shAc = shArtifact != null ? shArtifact.Wearable : null;

					if (dobjAc.Field1 > 1)
					{
						if (dobjAc.Field1 > 14)
						{
							dobjAc.Field1 = 14;
						}

						if (arAc != null)
						{
							gOut.Print("You're already wearing armor!");

							NextState = Globals.CreateInstance<IStartState>();

							goto Cleanup;
						}

						gGameState.Ar = DobjArtifact.Uid;

						ActorMonster.Armor = (dobjAc.Field1 / 2) + ((dobjAc.Field1 / 2) >= 3 ? 2 : 0) + (shAc != null ? shAc.Field1 : 0);
					}
					else
					{
						if (shAc != null)
						{
							gOut.Print("You're already wearing a shield!");

							NextState = Globals.CreateInstance<IStartState>();

							goto Cleanup;
						}

						// can't wear shield while using two-handed weapon

						var weapon = ActorMonster.Weapon > 0 ? gADB[ActorMonster.Weapon] : null;

						var weaponAc = weapon != null ? weapon.GeneralWeapon : null;

						if (weaponAc != null && weaponAc.Field5 > 1)
						{
							PrintCantWearShieldWithWeapon(DobjArtifact, weapon);

							NextState = Globals.CreateInstance<IStartState>();

							goto Cleanup;
						}

						gGameState.Sh = DobjArtifact.Uid;

						ActorMonster.Armor = (arAc != null ? (arAc.Field1 / 2) + ((arAc.Field1 / 2) >= 3 ? 2 : 0) : 0) + dobjAc.Field1;
					}
				}

				DobjArtifact.SetWornByCharacter();

				PrintWorn(DobjArtifact);

				PlayerProcessEvents(PpeAfterArtifactWear);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{ 
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public WearCommand()
		{
			SortOrder = 240;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "WearCommand";

			Verb = "wear";

			Type = CommandType.Manipulation;
		}
	}
}
