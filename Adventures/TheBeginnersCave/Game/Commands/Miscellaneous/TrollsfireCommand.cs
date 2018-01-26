
// TrollsfireCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework;
using TheBeginnersCave.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class TrollsfireCommand : EamonRT.Game.Commands.Command, ITrollsfireCommand
	{
		protected override void PlayerExecute()
		{
			var artifact = Globals.ADB[10];

			Debug.Assert(artifact != null);

			if (artifact.IsInRoom(ActorRoom))
			{
				Globals.Out.Print("Maybe you should pick it up first.");

				goto Cleanup;
			}

			if (!artifact.IsCarriedByCharacter())
			{
				Globals.Out.Print("Nothing happens.");

				goto Cleanup;
			}

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			if (gameState.Trollsfire == 1)
			{
				Globals.Engine.PrintEffectDesc(6);

				gameState.Trollsfire = 0; 

				goto Cleanup;
			}

			Globals.Engine.PrintEffectDesc(4);

			gameState.Trollsfire = 1;

			if (ActorMonster.Weapon != 10)
			{
				Globals.Engine.PrintEffectDesc(5);

				var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.DfMonster = ActorMonster;

					x.OmitArmor = true;
				});

				combatSystem.ExecuteCalculateDamage(1, 5);
		
				gameState.Trollsfire = 0;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
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
