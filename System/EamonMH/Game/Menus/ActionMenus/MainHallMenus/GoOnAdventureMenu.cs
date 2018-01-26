
// GoOnAdventureMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class GoOnAdventureMenu : Menu, IGoOnAdventureMenu
	{
		protected virtual void SelectAdventure(long index)
		{
			RetCode rc;

			if (index < 0)
			{
				// PrintError

				goto Cleanup;
			}

			var nlFlag = false;

			var j = Globals.Database.GetFilesetsCount();

			if (index == 0)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Print("When you inquire with the burly Irishman about the adventures available to you he says, \"Ye cannat wait to put yerself to the test, eh?  {0}\"",
					j > 0 ? "Well, maybe one of these will suit yer fancy." : "Well, I just don't know where ye can venture right now.");
			}

			if (j == 0)
			{
				if (index == 0)
				{
					Globals.In.KeyPress(Buf);
				}

				goto Cleanup;
			}

			while (true)
			{
				Globals.Out.Print("{0}", Globals.LineSep);

				var i = 0;

				var helper = Globals.CreateInstance<IHelper<IFileset>>();

				var filesets = Globals.Database.FilesetTable.Records;

				foreach (var fileset01 in filesets)
				{
					helper.Record = fileset01;

					helper.ListRecord(false, false, false, false, false, false);

					nlFlag = true;

					if (i != 0 && (i % (Constants.NumRows - 8)) == 0)
					{
						nlFlag = false;

						Globals.Out.WriteLine("{0}{0}{1}", Environment.NewLine, Globals.LineSep);

						Globals.Out.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

						Buf.Clear();

						rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNullOrX, null, Globals.Engine.IsCharAny);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Thread.Sleep(150);

						if (Buf.Length > 0 && Buf[0] == 'X')
						{
							break;
						}

						Globals.Out.Print("{0}", Globals.LineSep);
					}

					i++;
				}

				if (nlFlag)
				{
					Globals.Out.WriteLine();
				}

				Globals.Out.Print("{0}", Globals.LineSep);

				Globals.Out.Write("{0}Enter the selection or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharDigitOrX, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'X')
				{
					goto Cleanup;
				}

				IFileset fileset = null;

				try
				{
					var filesetUid = Convert.ToInt64(Buf.Trim().ToString());

					fileset = Globals.FSDB[filesetUid];
				}
				catch (Exception)
				{
					// do nothing
				}

				if (fileset != null)
				{
					if (!Globals.Directory.Exists(fileset.WorkDir))
					{
						var errorMessage = string.Format("Attempted to access a path [{0}] that is not on the disk.", fileset.WorkDir);

						throw new Exception(errorMessage);
					}

					rc = Globals.PushDatabase();

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (!string.IsNullOrWhiteSpace(fileset.FilesetFileName) && !string.Equals(fileset.FilesetFileName, "NONE", StringComparison.OrdinalIgnoreCase))
					{
						var fsfn = Globals.Path.Combine(fileset.WorkDir, fileset.FilesetFileName);

						rc = Globals.Database.LoadFilesets(fsfn, printOutput: false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						SelectAdventure(index + 1);
					}
					else
					{
						Globals.Out.Print("{0}", Globals.LineSep);

						var chrfn = Globals.Path.Combine(fileset.WorkDir, "FRESHMEAT.XML");

						rc = Globals.Database.LoadCharacters(Globals.GetPrefixedFileName(chrfn), printOutput: false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						var character = Globals.Database.CharacterTable.Records.FirstOrDefault();

						if (character != null && character.Uid > 0 && !string.IsNullOrWhiteSpace(character.Name) && !string.Equals(character.Name, "NONE", StringComparison.OrdinalIgnoreCase))
						{
							Globals.Out.Print("{0} is already adventuring there!", character.Name);

							goto Cleanup01;
						}

						Globals.Database.FreeCharacters();

						Globals.Character.Status = Enums.Status.Adventuring;

						Globals.CharactersModified = true;

						character = Globals.CloneInstance(Globals.Character);

						rc = Globals.Database.AddCharacter(character);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						rc = Globals.Database.SaveCharacters(Globals.GetPrefixedFileName(chrfn), false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						var fsfn = Globals.Path.Combine(fileset.WorkDir, "SAVEGAME.XML");

						rc = Globals.Database.LoadFilesets(Globals.GetPrefixedFileName(fsfn), printOutput: false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						filesets = Globals.Database.FilesetTable.Records;

						foreach (var fileset01 in filesets)
						{
							fileset01.DeleteFiles(null, true);
						}

						Globals.Database.FreeFilesets();

						rc = Globals.Database.SaveFilesets(Globals.GetPrefixedFileName(fsfn), false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						var cfgfn = Globals.Path.Combine(fileset.WorkDir, "EAMONCFG.XML");

						rc = Globals.Database.LoadConfigs(Globals.GetPrefixedFileName(cfgfn), printOutput: false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Database.FreeConfigs();

						var config = Globals.CloneInstance(Globals.Config);

						config.Uid = Globals.Database.GetConfigUid();

						config.IsUidRecycled = true;

						config.MhWorkDir = @"..\..\System\Bin";         // config.MhWorkDir = Globals.CloneInstance(Globals.WorkDir);

						config.RtFilesetFileName = "SAVEGAME.XML";

						config.DdFilesetFileName = Globals.CloneInstance(config.RtFilesetFileName);

						config.RtCharacterFileName = "FRESHMEAT.XML";

						config.DdCharacterFileName = Globals.CloneInstance(config.RtCharacterFileName);

						config.RtModuleFileName = Globals.CloneInstance(fileset.ModuleFileName);

						config.DdModuleFileName = Globals.CloneInstance(config.RtModuleFileName);

						config.RtRoomFileName = Globals.CloneInstance(fileset.RoomFileName);

						config.DdRoomFileName = Globals.CloneInstance(config.RtRoomFileName);

						config.RtArtifactFileName = Globals.CloneInstance(fileset.ArtifactFileName);

						config.DdArtifactFileName = Globals.CloneInstance(config.RtArtifactFileName);

						config.RtEffectFileName = Globals.CloneInstance(fileset.EffectFileName);

						config.DdEffectFileName = Globals.CloneInstance(config.RtEffectFileName);

						config.RtMonsterFileName = Globals.CloneInstance(fileset.MonsterFileName);

						config.DdMonsterFileName = Globals.CloneInstance(config.RtMonsterFileName);

						config.RtHintFileName = Globals.CloneInstance(fileset.HintFileName);

						config.DdHintFileName = Globals.CloneInstance(config.RtHintFileName);

						config.RtGameStateFileName = "GAMESTATE.XML";

						config.DdEditingFilesets = true;

						config.DdEditingCharacters = true;

						rc = Globals.Database.AddConfig(config);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						rc = Globals.Database.SaveConfigs(Globals.GetPrefixedFileName(cfgfn), false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Out.Print("You are about to adventure in {0}!", fileset.Name);

						IDatabase database = null;

						rc = Globals.GetDatabase(0, ref database);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						rc = Globals.PushDatabase(database);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						// silently sync characters file with newly created files above

						rc = Globals.Database.SaveCharacters(Globals.GetPrefixedFileName(Globals.Config.MhCharacterFileName), false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						rc = Globals.PopDatabase(false);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						Globals.Fileset = Globals.CloneInstance(fileset);

						Globals.GoOnAdventure = true;

						Globals.In.KeyPress(Buf);

					Cleanup01:

						;
					}

					rc = Globals.PopDatabase();

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Globals.Fileset != null)
					{
						goto Cleanup;
					}
				}
				else
				{
					goto Cleanup;
				}
			}

		Cleanup:

			;
		}

		public override void Execute()
		{
			SelectAdventure(0);
		}

		public GoOnAdventureMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
