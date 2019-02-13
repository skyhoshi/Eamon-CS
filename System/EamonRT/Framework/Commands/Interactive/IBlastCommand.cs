
// IBlastCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IBlastCommand : ICommand
	{
		/// <summary></summary>
		bool CastSpell { get; set; }

		/// <summary></summary>
		bool CheckAttack { get; set; }
	}
}
