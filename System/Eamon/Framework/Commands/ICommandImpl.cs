
// ICommandImpl.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Commands
{
	public interface ICommandImpl : ICommandSignatures
	{
		ICommand Command { get; set; }

		bool ShouldPreTurnProcess();

		void Execute();
	}
}
