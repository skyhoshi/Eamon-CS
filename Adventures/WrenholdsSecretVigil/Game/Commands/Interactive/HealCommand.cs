
// HealCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IHealCommand))]
	public class HealCommand : EamonRT.Game.Commands.HealCommand, IHealCommand
	{
		protected override void PlayerExecute()
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			var medallionArtifact = Globals.ADB[10];

			Debug.Assert(medallionArtifact != null);

			if (medallionArtifact.IsCarriedByCharacter() && gameState.MedallionCharges > 0)
			{
				Globals.Out.Write("{0}{1} feel{2} warm in your hand!{0}", Environment.NewLine, medallionArtifact.GetDecoratedName03(true, true, false, false, Globals.Buf), medallionArtifact.EvalPlural("s", ""));

				gameState.MedallionCharges--;

				CastSpell = false;
			}

			base.PlayerExecute();
		}
	}
}
