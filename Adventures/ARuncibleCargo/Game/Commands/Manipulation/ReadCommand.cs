
// ReadCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework;
using ARuncibleCargo.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IReadCommand))]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			switch (DobjArtifact.Uid)
			{
				case 16:

					// Read sign on Sam's door () () ()

					var command = Globals.CreateInstance<EamonRT.Framework.Commands.IExamineCommand>();

					CopyCommandData(command);

					NextState = command;

					break;

				case 115:

					// Student paper

					if (!gameState.PaperRead)
					{
						var spell = Globals.Engine.GetSpells(Enums.Spell.Speed);

						Debug.Assert(spell != null);

						Globals.Character.ModSpellAbilities(Enums.Spell.Speed, 25);

						if (Globals.Character.GetSpellAbilities(Enums.Spell.Speed) > spell.MaxValue)
						{
							Globals.Character.SetSpellAbilities(Enums.Spell.Speed, spell.MaxValue);
						}

						Globals.Engine.PrintEffectDesc(76);

						Globals.Out.Write("{0}Your ability to cast {1} just increased!{0}", Environment.NewLine, spell.Name);

						gameState.PaperRead = true;
					}
					else
					{
						Globals.Out.WriteLine("{0}Nothing happens.", Environment.NewLine);
					}

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();

					break;

				default:

					base.PlayerExecute();

					break;
			}
		}
	}
}
