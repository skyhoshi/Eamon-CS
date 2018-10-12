
// IAttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Commands
{
	public interface IAttackCommand : ICommand
	{
		bool BlastSpell { get; set; }

		bool CheckAttack { get; set; }

		long MemberNumber { get; set; }

		long AttackNumber { get; set; }
	}
}
