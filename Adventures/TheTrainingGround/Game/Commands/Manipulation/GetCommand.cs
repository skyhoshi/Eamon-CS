
// GetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using TheTrainingGround.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IGetCommand))]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		protected virtual bool RevealSecretPassage { get; set; }

		protected override void PrintTaken(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			base.PrintTaken(artifact);

			// Taking Purple book reveals secret passage

			if (artifact.Uid == 27 && ActorMonster.Uid == gameState.Cm && ActorRoom.Uid == 24 && !gameState.LibrarySecretPassageFound)
			{
				RevealSecretPassage = true;

				gameState.LibrarySecretPassageFound = true;
			}
		}

		protected override void PlayerExecute()
		{
			base.PlayerExecute();

			if (RevealSecretPassage)
			{
				Globals.Engine.PrintEffectDesc(12);

				ActorRoom.SetDirs(Enums.Direction.East, 25);
			}
		}
	}
}
