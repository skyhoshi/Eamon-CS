
// IInventoryCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IInventoryCommand : ICommand
	{
		/// <summary></summary>
		bool AllowExtendedContainers { get; set; }

		/// <summary></summary>
		bool OmitHealthStatus { get; set; }
	}
}
