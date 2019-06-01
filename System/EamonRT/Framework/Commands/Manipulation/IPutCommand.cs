﻿
// IPutCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IPutCommand : ICommand
	{
		/// <summary></summary>
		bool GetCommandCalled { get; set; }

		/// <summary></summary>
		ContainerType ContainerType { get; set; }
	}
}
