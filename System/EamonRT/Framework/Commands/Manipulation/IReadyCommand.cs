﻿
// IReadyCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IReadyCommand : ICommand
	{
		ArtifactType[] ArtTypes { get; set; }
	}
}
