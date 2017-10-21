
// Engine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using EamonDD.Framework;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game
{
	[ClassMappings(typeof(Eamon.Framework.IEngine))]
	public class Engine : Eamon.Game.Engine, IEngine
	{
		public virtual bool IsAdventureFilesetLoaded()
		{
			if (Globals.Config != null)
			{
				return Globals.Config.DdEditingModules && Globals.Config.DdEditingRooms && Globals.Config.DdEditingArtifacts && Globals.Config.DdEditingEffects && Globals.Config.DdEditingMonsters && Globals.Config.DdEditingHints;
			}
			else
			{
				return false;
			}
		}

		public virtual void DdProcessArgv(bool secondPass, ref bool ddfnFlag, ref bool nlFlag)
		{
			long i;

			for (i = 0; i < Globals.Argv.Length; i++)
			{
				if (string.Equals(Globals.Argv[i], "--workingDirectory", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (string.Equals(Globals.Argv[i], "--filePrefix", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (string.Equals(Globals.Argv[i], "--ignoreMutex", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-im", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (string.Equals(Globals.Argv[i], "--runGameEditor", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-rge", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (string.Equals(Globals.Argv[i], "--configFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-cfgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (!secondPass)
						{
							Globals.ConfigFileName = Globals.Argv[i].Trim();
						}
					}
				}
				else if (string.Equals(Globals.Argv[i], "--filesetFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-fsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdFilesetFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingFilesets = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (string.Equals(Globals.Argv[i], "--characterFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-chrfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdCharacterFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingCharacters = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (string.Equals(Globals.Argv[i], "--moduleFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-modfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdModuleFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingModules = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (string.Equals(Globals.Argv[i], "--roomFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-rfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdRoomFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingRooms = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (string.Equals(Globals.Argv[i], "--artifactFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-afn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdArtifactFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingArtifacts = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (string.Equals(Globals.Argv[i], "--effectFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-efn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdEffectFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingEffects = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (string.Equals(Globals.Argv[i], "--monsterFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-monfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdMonsterFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingMonsters = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (string.Equals(Globals.Argv[i], "--hintFileName", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-hfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdHintFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingHints = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (string.Equals(Globals.Argv[i], "--loadAdventure", StringComparison.OrdinalIgnoreCase) || string.Equals(Globals.Argv[i], "-la", StringComparison.OrdinalIgnoreCase))
				{
					if (secondPass)
					{
						Globals.Config.DdEditingModules = true;

						Globals.Config.DdEditingRooms = true;

						Globals.Config.DdEditingArtifacts = true;

						Globals.Config.DdEditingEffects = true;

						Globals.Config.DdEditingMonsters = true;

						Globals.Config.DdEditingHints = true;

						Globals.ConfigsModified = true;
					}
					else
					{
						ddfnFlag = true;
					}
				}
				else if (secondPass)
				{
					if (!nlFlag)
					{
						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
					}

					Globals.Out.Write("{0}Unrecognized command line argument: [{1}]", Environment.NewLine, Globals.Argv[i]);

					nlFlag = true;
				}
			}
		}
	}
}
