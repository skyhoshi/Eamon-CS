
// TransferProtocol.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class TransferProtocol : ITransferProtocol
	{
		protected virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(System.IO.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', System.IO.Path.DirectorySeparatorChar) : null;
		}

		public virtual void SendCharacterToMainHall(string filesetFileName, string characterFileName, string effectFileName, string characterName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(filesetFileName));

			Debug.Assert(!string.IsNullOrWhiteSpace(characterFileName));

			Debug.Assert(!string.IsNullOrWhiteSpace(effectFileName));

			Debug.Assert(!string.IsNullOrWhiteSpace(characterName));

			Program.NextArgs = new string[] { "-pfn", "EamonMH.dll", "-wd", ".", "-fsfn", filesetFileName, "-chrfn", characterFileName, "-efn", effectFileName, "-chrnm", characterName };
		}

		public virtual void SendCharacterOnAdventure(string workDir, string pluginFileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(workDir));

			Debug.Assert(!string.IsNullOrWhiteSpace(pluginFileName));

			Program.NextArgs = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-cfgfn", "EAMONCFG.XML" };
		}

		public virtual void RecallCharacterFromAdventure(string workDir, string pluginFileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(workDir));

			Debug.Assert(!string.IsNullOrWhiteSpace(pluginFileName));

			var dir = System.IO.Directory.GetCurrentDirectory();

			var args = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-dgs", "-im" };

			Program.ExecutePlugin(args, false);

			System.IO.Directory.SetCurrentDirectory(dir);
		}
	}
}
