
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			// Plain scroll increases BLAST ability

			if (eventType == PpeAfterArtifactRead && DobjArtifact.Uid == 29)
			{
				var spell = Globals.Engine.GetSpells(Enums.Spell.Blast);

				Debug.Assert(spell != null);

				Globals.Engine.RemoveWeight(DobjArtifact);

				DobjArtifact.SetInLimbo();

				Globals.Character.ModSpellAbilities(Enums.Spell.Blast, 10);

				if (Globals.Character.GetSpellAbilities(Enums.Spell.Blast) > spell.MaxValue)
				{
					Globals.Character.SetSpellAbilities(Enums.Spell.Blast, spell.MaxValue);
				}

				Globals.GameState.ModSa(Enums.Spell.Blast, 250);

				if (Globals.GameState.GetSa(Enums.Spell.Blast) > spell.MaxValue)
				{
					Globals.GameState.SetSa(Enums.Spell.Blast, spell.MaxValue);
				}
			}

			base.PlayerProcessEvents(eventType);
		}
	}
}
