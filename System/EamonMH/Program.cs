
// EamonMH.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Portability;
using EamonMH.Framework.Menus.ActionMenus;
using EamonMH.Framework.Menus.HierarchicalMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonMH.Game.Plugin.PluginContext;
using static EamonMH.Game.Plugin.PluginContextStack;

namespace EamonMH
{
	public class Program : IProgram
	{
		protected virtual string ProgramName { get; set; } = "EamonMH";

		protected virtual Type ConstantsType { get; set; } = typeof(Game.Plugin.PluginConstants);

		protected virtual Type ClassMappingsType { get; set; } = typeof(Game.Plugin.PluginClassMappings);

		protected virtual Type GlobalsType { get; set; } = typeof(Game.Plugin.PluginGlobals);

		public virtual bool EnableStdio { get; set; }

		public virtual bool LineWrapUserInput { get; set; }

		public virtual Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		public virtual void Main(string[] args)
		{
			IConfig config;
			bool nlFlag;
			RetCode rc;
			long i;

			try
			{
				nlFlag = true;

				rc = RetCode.Success;

				PushConstants(ConstantsType);

				PushClassMappings(ClassMappingsType);

				ClassMappings.EnableStdio = EnableStdio;

				ClassMappings.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

				// resolve portability class mappings

				ClassMappings.ResolvePortabilityClassMappings();

				// process command line args

				ClassMappings.ProcessArgv(args);

				// load plugin class mappings

				rc = ClassMappings.LoadPluginClassMappings();

				if (rc != RetCode.Success)
				{
					nlFlag = false;

					goto Cleanup;
				}

				try
				{
					PushGlobals(GlobalsType);

					// initialize system

					Globals.InitSystem();

					Globals.LineWrapUserInput = LineWrapUserInput;

					// disable resolution of uid macros

					Globals.Out.ResolveUidMacros = false;

					// make the cursor disappear

					Globals.Out.CursorVisible = false;

					// initialize Config record

					Globals.Config.Uid = 1;

					Globals.Config.ShowDesc = true;

					Globals.Config.GenerateUids = true;

					Globals.Config.FieldDesc = Enums.FieldDesc.Full;

					Globals.Config.WordWrapMargin = Constants.RightMargin;

					// change window title bar and size

					Globals.Out.SetWindowTitle(ProgramName);

					try
					{
						Globals.Out.SetWindowSize(Math.Min(Constants.WindowWidth, Globals.Out.GetLargestWindowWidth()),
															Math.Min(Math.Max(Constants.WindowHeight, Globals.Out.GetWindowHeight()), (long)(Globals.Out.GetLargestWindowHeight() * 0.95)));

						Globals.Out.SetBufferSize(Constants.BufferWidth, Constants.BufferHeight);
					}
					catch (Exception)
					{
						// do nothing
					}

					// make announcements

					Globals.Out.Write("{0}Eamon CS Main Hall ({1}) {2}", Environment.NewLine, ProgramName, Constants.ProgVersion);

					Globals.Out.Write("{0}Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved.", Environment.NewLine);

					Globals.Out.Write("{0}As defined by the GPL license, this is free software.{0}", Environment.NewLine);

					// copy and store command line args

					Globals.Argv = new string[args.Length];

					for (i = 0; i < args.Length; i++)
					{
						Globals.Argv[i] = Globals.CloneInstance(args[i]);
					}

					// process command line args

					Globals.MhEngine.ProcessArgv(false, ref nlFlag);

					// assign default work directory, if necessary

					if (Globals.WorkDir.Length == 0)
					{
						Globals.WorkDir = Constants.DefaultWorkDir;
					}

					// initialize Config record

					Globals.Config.MhFilesetFileName = "ADVENTURES.XML";

					Globals.Config.MhCharacterFileName = "CHARACTERS.XML";

					Globals.Config.MhEffectFileName = "SNAPPY.XML";

					if (Globals.WorkDir.Length > 0)
					{
						// if working directory does not exist

						if (!Globals.Directory.Exists(Globals.WorkDir))
						{
							Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

							Globals.Out.Write("{0}The working directory [{1}] does not exist.{0}", Environment.NewLine, Globals.WorkDir);

							Globals.Out.Write("{0}Would you like to create it (Y/N) [N]: ", Environment.NewLine);

							Globals.Buf.Clear();

							rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', true, "N", Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, Globals.Engine.IsCharYOrN);

							Debug.Assert(Globals.Engine.IsSuccess(rc));

							Globals.Thread.Sleep(150);

							if (Globals.Buf[0] != 'Y')
							{
								nlFlag = false;

								goto Cleanup;
							}

							// create working directory

							Globals.Directory.CreateDirectory(Globals.WorkDir);
						}

						// change to working directory

						Globals.Directory.SetCurrentDirectory(Globals.WorkDir);
					}

					// load the config textfile

					if (Globals.ConfigFileName.Length > 0)
					{
						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

						rc = Globals.Database.LoadConfigs(Globals.GetPrefixedFileName(Globals.ConfigFileName));

						if (Globals.Engine.IsFailure(rc))
						{
							Globals.Error.Write("Error: LoadConfigs function call failed");

							goto Cleanup;
						}

						config = Globals.Engine.GetConfig();

						if (config != null)
						{
							if (config.MhFilesetFileName.Length == 0)
							{
								config.MhFilesetFileName = Globals.Config.MhFilesetFileName;

								Globals.ConfigsModified = true;
							}

							if (config.MhCharacterFileName.Length == 0)
							{
								config.MhCharacterFileName = Globals.Config.MhCharacterFileName;

								Globals.ConfigsModified = true;
							}

							if (config.MhEffectFileName.Length == 0)
							{
								config.MhEffectFileName = Globals.Config.MhEffectFileName;

								Globals.ConfigsModified = true;
							}
						}
						else
						{
							Globals.Config.Uid = Globals.Database.GetConfigUid();

							Globals.Config.IsUidRecycled = true;

							rc = Globals.Database.AddConfig(Globals.Config);

							if (Globals.Engine.IsFailure(rc))
							{
								// PrintError

								goto Cleanup;
							}

							Globals.ConfigsModified = true;

							config = Globals.Config;
						}

						Globals.Config = config;

						Globals.Out.WriteLine();
					}

					nlFlag = false;

					// process command line args

					Globals.MhEngine.ProcessArgv(true, ref nlFlag);

					if (nlFlag)
					{
						Globals.Out.WriteLine();
					}

					nlFlag = true;

					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					rc = Globals.Database.LoadFilesets(Globals.Config.MhFilesetFileName);

					if (Globals.Engine.IsFailure(rc))
					{
						Globals.Error.Write("Error: LoadFilesets function call failed");

						goto Cleanup;
					}

					rc = Globals.Database.LoadCharacters(Globals.GetPrefixedFileName(Globals.Config.MhCharacterFileName));

					if (Globals.Engine.IsFailure(rc))
					{
						Globals.Error.Write("Error: LoadCharacters function call failed");

						goto Cleanup;
					}

					rc = Globals.Database.LoadEffects(Globals.Config.MhEffectFileName);

					if (Globals.Engine.IsFailure(rc))
					{
						Globals.Error.Write("Error: LoadEffects function call failed");

						goto Cleanup;
					}

					Globals.Database.Compact();

					Globals.Out.WriteLine();

					// auto load character if necessary

					if (Globals.CharacterName.Length > 0 && !string.Equals(Globals.CharacterName, "NONE", StringComparison.OrdinalIgnoreCase))
					{
						Globals.Character = Globals.Database.CharacterTable.Records.FirstOrDefault(c => string.Equals(c.Name, Globals.CharacterName, StringComparison.OrdinalIgnoreCase));

						if (Globals.Character == null || Globals.Character.Uid <= 0 || Globals.Character.Status != Enums.Status.Alive)
						{
							Globals.CharacterName = "";

							Globals.Character = null;
						}
					}

					// create appropriate menu

					if (Globals.Character != null)
					{
						Globals.Menu = Globals.CreateInstance<IMainHallMenu>();
					}
					else
					{
						Globals.Menu = Globals.CreateInstance<IOuterChamberMenu>();
					}

					// call appropriate menu

					Globals.Menu.Execute();

					// save textfiles, if any modifications were made

					if ((Globals.ConfigFileName.Length > 0 && Globals.ConfigsModified) || Globals.FilesetsModified || Globals.CharactersModified || Globals.EffectsModified)
					{
						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

						// save the textfiles

						if (Globals.EffectsModified)
						{
							rc = Globals.Database.SaveEffects(Globals.Config.MhEffectFileName);

							if (Globals.Engine.IsFailure(rc))
							{
								Globals.Error.Write("Error: SaveEffects function call failed");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (Globals.CharactersModified)
						{
							rc = Globals.Database.SaveCharacters(Globals.GetPrefixedFileName(Globals.Config.MhCharacterFileName));

							if (Globals.Engine.IsFailure(rc))
							{
								Globals.Error.Write("Error: SaveCharacters function call failed");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (Globals.FilesetsModified)
						{
							rc = Globals.Database.SaveFilesets(Globals.Config.MhFilesetFileName);

							if (Globals.Engine.IsFailure(rc))
							{
								Globals.Error.Write("Error: SaveFilesets function call failed");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (Globals.ConfigFileName.Length > 0 && Globals.ConfigsModified)
						{
							rc = Globals.Database.SaveConfigs(Globals.GetPrefixedFileName(Globals.ConfigFileName));

							if (Globals.Engine.IsFailure(rc))
							{
								Globals.Error.Write("Error: SaveConfigs function call failed");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						Globals.Out.WriteLine();
					}

					// send character on adventure if necessary

					if (Globals.GoOnAdventure)
					{
						Debug.Assert(Globals.Character != null);

						Debug.Assert(Globals.Fileset != null);

						Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

						Globals.TransferProtocol.SendCharacterOnAdventure(Globals.Fileset.WorkDir, Globals.FilePrefix, Globals.Fileset.PluginFileName);
					}

					nlFlag = false;
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					// de-initialize system

					Globals.DeinitSystem();

					PopGlobals();
				}

			Cleanup:

				if (nlFlag)
				{
					if (rc == RetCode.Success)
					{
						ClassMappings.Out.WriteLine();
					}
					else
					{
						ClassMappings.Error.WriteLine();
					}

					nlFlag = false;
				}

				if (rc != RetCode.Success)
				{
					ClassMappings.Error.WriteLine("{0}{1}", Environment.NewLine, new string('-', (int)Constants.RightMargin));

					ClassMappings.Error.Write("{0}Press any key to continue: ", Environment.NewLine);

					ClassMappings.In.ReadKey(true);

					ClassMappings.Error.WriteLine();

					ClassMappings.Thread.Sleep(150);
				}

				return;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				PopClassMappings();

				PopConstants();
			}
		}
	}
}
