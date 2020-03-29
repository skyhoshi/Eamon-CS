
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

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
			Debug.Assert(gActorMonster != null);

			// Monster selects its attack modality

			gActorMonster.SetAttackModality();

			base.MonsterExecute();
		}

		public AttackCommand()
		{
			// Synonyms can be applied to verbs as well

			Synonyms = new string[] { "kill" };
		}
	}
}
