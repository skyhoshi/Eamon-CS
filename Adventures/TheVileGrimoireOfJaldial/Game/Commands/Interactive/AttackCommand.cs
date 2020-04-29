
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void MonsterExecute()
		{
			Debug.Assert(gActorMonster != null && gDobjMonster != null);

			while (true)
			{
				// Monster selects its attack modality

				gActorMonster.SetAttackModality();

				// Beholder's clumsiness spells only work on non-group monsters

				if (gActorMonster.Uid == 36 && string.Equals(gActorMonster.AttackDesc, "cast{0} a clumsiness spell on", StringComparison.OrdinalIgnoreCase) && gDobjMonster.OrigGroupCount > 1)
				{
					gGameState.ClumsySpells--;
				}
				else
				{
					break;
				}
			}

			base.MonsterExecute();
		}

		public AttackCommand()
		{
			// Synonyms can be applied to verbs as well

			Synonyms = new string[] { "kill" };
		}
	}
}
