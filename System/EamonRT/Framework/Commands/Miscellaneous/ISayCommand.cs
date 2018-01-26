
// ISayCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Commands;

namespace EamonRT.Framework.Commands
{
	public interface ISayCommand : ICommand
	{
		string OriginalPhrase { get; set; }

		string PrintedPhrase { get; set; }

		string ProcessedPhrase { get; set; }
	}
}
