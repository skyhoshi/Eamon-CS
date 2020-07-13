
// SaveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SaveCommand : Command, ISaveCommand
	{
		public virtual long SaveSlot { get; set; }

		public virtual string SaveName { get; set; }

		public override void PlayerExecute()
		{
			IFileset fileset;
			RetCode rc;

			var filesetsCount = Globals.Database.GetFilesetsCount();

			Debug.Assert(filesetsCount <= gEngine.NumSaveSlots);

			Debug.Assert(SaveSlot >= 1 && SaveSlot <= Math.Min(filesetsCount + 1, gEngine.NumSaveSlots));

			Debug.Assert(SaveName != null);

			if (SaveSlot == filesetsCount + 1)
			{
				fileset = Globals.CreateInstance<IFileset>(x =>
				{
					x.Uid = Globals.Database.GetFilesetUid();
					x.Name = "(none)";
				});

				rc = Globals.Database.AddFileset(fileset);

				Debug.Assert(gEngine.IsSuccess(rc));
			}

			var filesets = Globals.Database.FilesetTable.Records.OrderBy(f => f.Uid).ToList();

			fileset = filesets[(int)SaveSlot - 1];

			if (SaveName.Length == 0)
			{
				if (!string.Equals(fileset.Name, "(none)", StringComparison.OrdinalIgnoreCase))
				{
					gOut.Write("{0}Change name of save (Y/N): ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						fileset.Name = "(none)";
					}
				}

				while (string.Equals(fileset.Name, "(none)", StringComparison.OrdinalIgnoreCase))
				{
					gOut.Write("{0}Enter new name: ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.FsNameLen, null, ' ', '\0', false, null, null, null, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Buf.SetFormat("{0}", Regex.Replace(Globals.Buf.ToString(), @"\s+", " ").ToLower().Trim());

					fileset.Name = gEngine.Capitalize(Globals.Buf.ToString());

					if (fileset.Name.Length == 0)
					{
						fileset.Name = "(none)";
					}
				}
			}
			else
			{
				if (!string.Equals(fileset.Name, "(none)", StringComparison.OrdinalIgnoreCase) && string.Equals(SaveName, "Quick Saved Game", StringComparison.OrdinalIgnoreCase))
				{
					SaveName = Globals.CloneInstance(fileset.Name);
				}

				SaveName = gEngine.Capitalize(SaveName);

				fileset.Name = Globals.CloneInstance(SaveName);

				gOut.Print("[QUICK SAVE {0}: {1}]", SaveSlot, SaveName);
			}

			var config = Globals.CreateInstance<IConfig>();

			var saveSlotStr = SaveSlot.ToString("D3");

			fileset.WorkDir = "NONE";

			fileset.PluginFileName = "NONE";

			var path = "";

			var name = "";

			var ext = "";

			rc = gEngine.SplitPath(Globals.ConfigFileName, ref path, ref name, ref ext);

			Debug.Assert(gEngine.IsSuccess(rc));

			var idx = name.IndexOf('_');

			if (idx >= 0)
			{
				name = name.Substring(0, idx);
			}

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.ConfigFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			fileset.FilesetFileName = "NONE";

			rc = gEngine.SplitPath(Globals.Config.RtCharacterFileName, ref path, ref name, ref ext);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.CharacterFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtModuleFileName, ref path, ref name, ref ext);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.ModuleFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtRoomFileName, ref path, ref name, ref ext);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.RoomFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtArtifactFileName, ref path, ref name, ref ext);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.ArtifactFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtEffectFileName, ref path, ref name, ref ext);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.EffectFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtMonsterFileName, ref path, ref name, ref ext);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.MonsterFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtHintFileName, ref path, ref name, ref ext);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.HintFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtGameStateFileName, ref path, ref name, ref ext);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.GameStateFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			config.RtFilesetFileName = Globals.CloneInstance(Globals.Config.RtFilesetFileName);

			config.RtCharacterFileName = Globals.CloneInstance(fileset.CharacterFileName);

			config.RtModuleFileName = Globals.CloneInstance(fileset.ModuleFileName);

			config.RtRoomFileName = Globals.CloneInstance(fileset.RoomFileName);

			config.RtArtifactFileName = Globals.CloneInstance(fileset.ArtifactFileName);

			config.RtEffectFileName = Globals.CloneInstance(fileset.EffectFileName);

			config.RtMonsterFileName = Globals.CloneInstance(fileset.MonsterFileName);

			config.RtHintFileName = Globals.CloneInstance(fileset.HintFileName);

			config.RtGameStateFileName = Globals.CloneInstance(fileset.GameStateFileName);
			
			Globals.Config.DdFilesetFileName = config.RtFilesetFileName;

			Globals.Config.DdCharacterFileName = config.RtCharacterFileName;

			Globals.Config.DdModuleFileName = config.RtModuleFileName;

			Globals.Config.DdRoomFileName = config.RtRoomFileName;

			Globals.Config.DdArtifactFileName = config.RtArtifactFileName;

			Globals.Config.DdEffectFileName = config.RtEffectFileName;

			Globals.Config.DdMonsterFileName = config.RtMonsterFileName;

			Globals.Config.DdHintFileName = config.RtHintFileName;

			Globals.Config.DdEditingFilesets = true;

			Globals.Config.DdEditingCharacters = true;

			Globals.Config.DdEditingModules = true;

			Globals.Config.DdEditingRooms = true;

			Globals.Config.DdEditingArtifacts = true;

			Globals.Config.DdEditingEffects = true;

			Globals.Config.DdEditingMonsters = true;

			Globals.Config.DdEditingHints = true;

			var artifacts = Globals.Database.ArtifactTable.Records.ToList();

			foreach (var artifact in artifacts)
			{
				if (artifact.IsCarriedByCharacter())
				{
					artifact.SetCarriedByMonsterUid(gGameState.Cm);
				}
				else if (artifact.IsWornByCharacter())
				{
					artifact.SetWornByMonsterUid(gGameState.Cm);
				}
			}

			var gameSaved = true;

			rc = config.SaveGameDatabase(false);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveGameDatabase function call failed");

				gameSaved = false;
			}

			foreach (var artifact in artifacts)
			{
				if (artifact.IsCarriedByMonsterUid(gGameState.Cm))
				{
					artifact.SetCarriedByCharacter();
				}
				else if (artifact.IsWornByMonsterUid(gGameState.Cm))
				{
					artifact.SetWornByCharacter();
				}
			}

			rc = Globals.Database.SaveConfigs(fileset.ConfigFileName, false);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveConfigs function call failed");

				gameSaved = false;
			}

			config.Dispose();

			gOut.Print(gameSaved ? "Game saved." : "Game not saved.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public SaveCommand()
		{
			SortOrder = 410;

			IsDarkEnabled = true;

			IsMonsterEnabled = false;

			Name = "SaveCommand";

			Verb = "save";

			Type = CommandType.Miscellaneous;

			SaveName = "";
		}
	}
}
