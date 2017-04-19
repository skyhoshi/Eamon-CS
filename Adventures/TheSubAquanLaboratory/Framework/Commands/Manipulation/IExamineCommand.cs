
// IExamineCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace TheSubAquanLaboratory.Framework.Commands
{
	public interface IExamineCommand : EamonRT.Framework.Commands.IExamineCommand
	{
		bool ExamineConsole { get; set; }
	}
}
