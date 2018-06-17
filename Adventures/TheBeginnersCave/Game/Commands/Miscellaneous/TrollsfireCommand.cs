
// TrollsfireCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	/// <seealso cref="Framework.Commands.ITrollsfireCommand" />
	[ClassMappings]
	public class TrollsfireCommand : EamonRT.Game.Commands.Command, Framework.Commands.ITrollsfireCommand
	{
		public override void PlayerExecute()
		{
			// find Trollsfire in the game database

			var trollsfireArtifact = Globals.ADB[10];

			Debug.Assert(trollsfireArtifact != null);

			// if Trollsfire is in the player character's room

			if (trollsfireArtifact.IsInRoom(ActorRoom))
			{
				Globals.Out.Print("Maybe you should pick it up first.");

				goto Cleanup;
			}

			// if Trollsfire is not being carried by the player character

			if (!trollsfireArtifact.IsCarriedByCharacter())
			{
				Globals.Out.Print("Nothing happens.");

				goto Cleanup;
			}

			// cast to the Beginner's Cave GameState to allow access to the Trollsfire property

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			// if Trollsfire is currently alight then extinguish it

			if (gameState.Trollsfire == 1)
			{
				Globals.Engine.PrintEffectDesc(6);

				gameState.Trollsfire = 0; 

				goto Cleanup;
			}

			// if Trollsfire is extinguished then light it

			Globals.Engine.PrintEffectDesc(4);

			gameState.Trollsfire = 1;

			// if Trollsfire is not wielded it burns the player character

			if (ActorMonster.Weapon != 10)
			{
				Globals.Engine.PrintEffectDesc(5);

				// create a combat system object to inflict the injury; these can be created and discarded at will

				var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
				{
					// the lambda is used if the combat system needs to set this Command's NextState property (eg, player character dies)

					x.SetNextStateFunc = s => NextState = s;

					// in PlayerExecute, ActorRoom is the player character room and ActorMonster is the player character monster

					x.DfMonster = ActorMonster;

					// we want to bypass armor

					x.OmitArmor = true;
				});

				// calculate and apply the damage to player character

				combatSystem.ExecuteCalculateDamage(1, 5);
		
				// extinguish Trollsfire

				gameState.Trollsfire = 0;
			}

		Cleanup:

			// if we're not going anywhere else, go into the monster processing loop

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public TrollsfireCommand()
		{
			SortOrder = 440;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "TrollsfireCommand";

			Verb = "trollsfire";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
