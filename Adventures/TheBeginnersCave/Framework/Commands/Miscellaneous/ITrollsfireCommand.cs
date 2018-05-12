
// ITrollsfireCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Commands;

namespace TheBeginnersCave.Framework.Commands
{
	/// <summary>
	/// This is the command used by the player to activate/deactivate the Trollsfire sword.
	/// </summary>
	/// <remarks>
	/// The TrollsfireCommand concrete implements this interface, which is used for dependency injection.  You will
	/// notice that nowhere in The Beginner's Cave is this command explicitly added to a list of available commands;
	/// rather, this is accomplished in <see cref="EamonRT.Framework.IEngine.CreateCommands"/>, which reflectively
	/// probes for all classes derived from <see cref="ICommand"/> and adds any found.
	/// </remarks>
	/// <seealso cref="Game.Commands.TrollsfireCommand" />
	public interface ITrollsfireCommand : ICommand
	{

	}
}
