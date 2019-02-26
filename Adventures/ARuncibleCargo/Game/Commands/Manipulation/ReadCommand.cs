
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			switch (DobjArtifact.Uid)
			{
				case 16:

					// Read sign on Sam's door () () ()

					var command = Globals.CreateInstance<IExamineCommand>();

					CopyCommandData(command);

					NextState = command;

					break;

				case 115:

					// Student paper

					if (!gameState.PaperRead)
					{
						var spell = Globals.Engine.GetSpells(Spell.Speed);

						Debug.Assert(spell != null);

						Globals.Character.ModSpellAbilities(Spell.Speed, 25);

						if (Globals.Character.GetSpellAbilities(Spell.Speed) > spell.MaxValue)
						{
							Globals.Character.SetSpellAbilities(Spell.Speed, spell.MaxValue);
						}

						Globals.Engine.PrintEffectDesc(76);

						Globals.Out.Print("Your ability to cast {0} just increased!", spell.Name);

						gameState.PaperRead = true;
					}
					else
					{
						Globals.Out.Print("Nothing happens.");
					}

					NextState = Globals.CreateInstance<IMonsterStartState>();

					break;

				default:

					base.PlayerExecute();

					break;
			}
		}
	}
}
