
// SaveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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

			Debug.Assert(filesetsCount <= Globals.Engine.NumSaveSlots);

			Debug.Assert(SaveSlot >= 1 && SaveSlot <= Math.Min(filesetsCount + 1, Globals.Engine.NumSaveSlots));

			Debug.Assert(SaveName != null);

			if (SaveSlot == filesetsCount + 1)
			{
				fileset = Globals.CreateInstance<IFileset>(x =>
				{
					x.Uid = Globals.Database.GetFilesetUid();
					x.Name = "(none)";
				});

				rc = Globals.Database.AddFileset(fileset);

				Debug.Assert(Globals.Engine.IsSuccess(rc));
			}

			var filesets = Globals.Database.FilesetTable.Records.OrderBy(f => f.Uid).ToList();

			fileset = filesets[(int)SaveSlot - 1];

			if (SaveName.Length == 0)
			{
				if (!string.Equals(fileset.Name, "(none)", StringComparison.OrdinalIgnoreCase))
				{
					Globals.Out.Write("{0}Change name of save (Y/N): ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						fileset.Name = "(none)";
					}
				}

				while (string.Equals(fileset.Name, "(none)", StringComparison.OrdinalIgnoreCase))
				{
					Globals.Out.Write("{0}Enter new name: ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.FsNameLen, null, ' ', '\0', false, null, null, null, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Buf.SetFormat("{0}", Regex.Replace(Globals.Buf.ToString(), @"\s+", " ").ToLower().Trim());

					fileset.Name = Globals.Engine.Capitalize(Globals.Buf.ToString());

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

				SaveName = Globals.Engine.Capitalize(SaveName);

				fileset.Name = Globals.CloneInstance(SaveName);

				Globals.Out.Print("[QUICK SAVE {0}: {1}]", SaveSlot, SaveName);
			}

			var config = Globals.CreateInstance<IConfig>();

			var saveSlotStr = SaveSlot.ToString("D3");

			fileset.WorkDir = "NONE";

			fileset.PluginFileName = "NONE";

			var path = "";

			var name = "";

			var ext = "";

			rc = Globals.Engine.SplitPath(Globals.ConfigFileName, ref path, ref name, ref ext);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			var idx = name.IndexOf('_');

			if (idx >= 0)
			{
				name = name.Substring(0, idx);
			}

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.ConfigFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			fileset.FilesetFileName = "NONE";

			rc = Globals.Engine.SplitPath(Globals.Config.RtCharacterFileName, ref path, ref name, ref ext);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.CharacterFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = Globals.Engine.SplitPath(Globals.Config.RtModuleFileName, ref path, ref name, ref ext);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.ModuleFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = Globals.Engine.SplitPath(Globals.Config.RtRoomFileName, ref path, ref name, ref ext);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.RoomFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = Globals.Engine.SplitPath(Globals.Config.RtArtifactFileName, ref path, ref name, ref ext);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.ArtifactFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = Globals.Engine.SplitPath(Globals.Config.RtEffectFileName, ref path, ref name, ref ext);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.EffectFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = Globals.Engine.SplitPath(Globals.Config.RtMonsterFileName, ref path, ref name, ref ext);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.MonsterFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = Globals.Engine.SplitPath(Globals.Config.RtHintFileName, ref path, ref name, ref ext);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", path, name, saveSlotStr, ext);

			fileset.HintFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = Globals.Engine.SplitPath(Globals.Config.RtGameStateFileName, ref path, ref name, ref ext);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

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
					artifact.SetCarriedByMonsterUid(Globals.GameState.Cm);
				}
				else if (artifact.IsWornByCharacter())
				{
					artifact.SetWornByMonsterUid(Globals.GameState.Cm);
				}
			}

			var gameSaved = true;

			rc = config.SaveGameDatabase(false);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveGameDatabase function call failed");

				gameSaved = false;
			}

			foreach (var artifact in artifacts)
			{
				if (artifact.IsCarriedByMonsterUid(Globals.GameState.Cm))
				{
					artifact.SetCarriedByCharacter();
				}
				else if (artifact.IsWornByMonsterUid(Globals.GameState.Cm))
				{
					artifact.SetWornByCharacter();
				}
			}

			rc = Globals.Database.SaveConfigs(fileset.ConfigFileName, false);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveConfigs function call failed");

				gameSaved = false;
			}

			config.Dispose();

			Globals.Out.Print(gameSaved ? "Game saved." : "Game not saved.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override void PlayerFinishParsing()
		{
			RetCode rc;
			long i;

			if (CommandParser.CurrToken < CommandParser.Tokens.Length && long.TryParse(CommandParser.Tokens[CommandParser.CurrToken], out i) && i >= 1 && i <= Globals.Engine.NumSaveSlots)
			{
				SaveSlot = i;

				CommandParser.CurrToken++;
			}

			if (CommandParser.CurrToken < CommandParser.Tokens.Length && SaveSlot >= 1 && SaveSlot <= Globals.Engine.NumSaveSlots)
			{
				SaveName = string.Join(" ", CommandParser.Tokens.Skip((int)CommandParser.CurrToken));

				if (SaveName.Length > Constants.FsNameLen)
				{
					SaveName = SaveName.Substring(0, Constants.FsNameLen);
				}

				CommandParser.CurrToken += (CommandParser.Tokens.Length - CommandParser.CurrToken);
			}
			else
			{
				SaveName = "Quick Saved Game";
			}

			var filesets = Globals.Database.FilesetTable.Records.ToList();

			var filesetsCount = filesets.Count();

			if (SaveSlot < 1 || SaveSlot > Globals.Engine.NumSaveSlots)
			{
				SaveName = "";

				while (true)
				{
					Globals.Out.Print("Saved games:");

					for (i = 0; i < Globals.Engine.NumSaveSlots; i++)
					{
						Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, i + 1, i < filesets.Count ? filesets[(int)i].Name : "(none)");
					}

					Globals.Out.Print("{0,3}. {1}", i + 1, "(Don't save, return to game)");

					Globals.Out.Write("{0}Enter 1-{1} for saved position: ", Environment.NewLine, i + 1);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, 3, null, ' ', '\0', false, null, null, Globals.Engine.IsCharDigit, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					i = Convert.ToInt64(Globals.Buf.Trim().ToString());

					if (i >= 1 && i <= Globals.Engine.NumSaveSlots)
					{
						SaveSlot = i;

						break;
					}
					else if (i == Globals.Engine.NumSaveSlots + 1)
					{
						break;
					}
				}
			}

			if (SaveSlot > filesetsCount + 1 && SaveSlot <= Globals.Engine.NumSaveSlots)
			{
				SaveSlot = filesetsCount + 1;

				Globals.Out.Print("[Using #{0} instead.]", SaveSlot);
			}

			if (SaveSlot < 1 || SaveSlot > filesetsCount + 1)
			{
				CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldStripTrailingPunctuation()
		{
			return false;
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
