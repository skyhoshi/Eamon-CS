
// PutCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class PutCommand : EamonRT.Game.Commands.PutCommand, IPutCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null && IobjArtifact != null);

			if (DobjArtifact.Uid == 82)
			{
				if (IobjArtifact.Uid == 1)
				{
					Globals.Engine.PrintEffectDesc(38);

					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
				else if (IobjArtifact.Uid == 26)
				{
					var gameState = Globals.GameState as Framework.IGameState;

					Debug.Assert(gameState != null);

					var artifact = Globals.ADB[16];

					Debug.Assert(artifact != null);

					if (gameState.Sterilize && !artifact.IsInLimbo())
					{
						Globals.Engine.PrintEffectDesc(40);

						artifact.SetInLimbo();
					}
					else
					{
						Globals.Engine.PrintEffectDesc(39);
					}

					NextState = Globals.CreateInstance<IMonsterStartState>();
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
