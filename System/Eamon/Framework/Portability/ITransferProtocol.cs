
// ITransferProtocol.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Portability
{
	public interface ITransferProtocol
	{
		void SendCharacterToMainHall(string filesetFileName, string characterFileName, string effectFileName, string characterName);

		void SendCharacterOnAdventure(string workDir, string pluginFileName);

		void RecallCharacterFromAdventure(string workDir, string pluginFileName);
	}
}
