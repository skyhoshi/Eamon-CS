
// TransferProtocol.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework.Portability;
using Eamon.Mobile;

namespace EamonPM.Game.Portability
{
	public class TransferProtocol : ITransferProtocol
	{
		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		protected virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(System.IO.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', System.IO.Path.DirectorySeparatorChar) : null;
		}

		public virtual void SendCharacterToMainHall(string filePrefix, string filesetFileName, string characterFileName, string effectFileName, string characterName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(filesetFileName));

			Debug.Assert(!string.IsNullOrWhiteSpace(characterFileName));

			Debug.Assert(!string.IsNullOrWhiteSpace(effectFileName));

			Debug.Assert(!string.IsNullOrWhiteSpace(characterName));

			if (!string.IsNullOrWhiteSpace(filePrefix))
			{
				App.NextArgs = new string[] { "-pfn", "EamonMH.dll", "-wd", ".", "-fp", filePrefix, "-fsfn", filesetFileName, "-chrfn", characterFileName, "-efn", effectFileName, "-chrnm", characterName };
			}
			else
			{
				App.NextArgs = new string[] { "-pfn", "EamonMH.dll", "-wd", ".", "-fsfn", filesetFileName, "-chrfn", characterFileName, "-efn", effectFileName, "-chrnm", characterName };
			}
		}

		public virtual void SendCharacterOnAdventure(string workDir, string filePrefix, string pluginFileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(workDir));

			Debug.Assert(!string.IsNullOrWhiteSpace(pluginFileName));

			if (!string.IsNullOrWhiteSpace(filePrefix))
			{
				App.NextArgs = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-fp", filePrefix, "-cfgfn", "EAMONCFG.XML" };
			}
			else
			{
				App.NextArgs = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-cfgfn", "EAMONCFG.XML" };
			}
		}

		public virtual void RecallCharacterFromAdventure(string workDir, string filePrefix, string pluginFileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(workDir));

			Debug.Assert(!string.IsNullOrWhiteSpace(pluginFileName));

			var dir = System.IO.Directory.GetCurrentDirectory();

			string[] args = null;

			if (!string.IsNullOrWhiteSpace(filePrefix))
			{
				args = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-fp", filePrefix, "-dgs", "-im" };
			}
			else
			{
				args = new string[] { "-pfn", pluginFileName, "-wd", NormalizePath(workDir), "-dgs", "-im" };
			}

			App.ExecutePlugin(args, false);

			System.IO.Directory.SetCurrentDirectory(dir);
		}
	}
}
