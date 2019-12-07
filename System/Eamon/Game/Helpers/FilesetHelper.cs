﻿
// FilesetHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class FilesetHelper : Helper<IFileset>, IFilesetHelper
	{
		#region Protected Properties

		/// <summary></summary>
		protected virtual Regex WorkDirRegex { get; set; }

		#endregion

		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameWorkDir()
		{
			return "Working Directory";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNamePluginFileName()
		{
			return "Plugin Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameConfigFileName()
		{
			return "Config Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameFilesetFileName()
		{
			return "Fileset Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameCharacterFileName()
		{
			return "Character Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameModuleFileName()
		{
			return "Module Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameRoomFileName()
		{
			return "Room Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameArtifactFileName()
		{
			return "Artifact Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameEffectFileName()
		{
			return "Effect Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameMonsterFileName()
		{
			return "Monster Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameHintFileName()
		{
			return "Hint Filename";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameGameStateFileName()
		{
			return "Game State Filename";
		}

		#endregion

		#region GetName Methods

		// do nothing

		#endregion

		#region GetValue Methods

		// do nothing

		#endregion

		#region Validate Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateName()
		{
			if (Record.Name != null)
			{
				Record.Name = Regex.Replace(Record.Name, @"\s+", " ").Trim();
			}

			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.FsNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWorkDir()
		{
			return WorkDirRegex.IsMatch(Record.WorkDir);
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidatePluginFileName()
		{
			return string.IsNullOrWhiteSpace(Record.PluginFileName) == false && Record.PluginFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.PluginFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateConfigFileName()
		{
			return string.IsNullOrWhiteSpace(Record.ConfigFileName) == false && Record.ConfigFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.ConfigFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateFilesetFileName()
		{
			return string.IsNullOrWhiteSpace(Record.FilesetFileName) == false && Record.FilesetFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.FilesetFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateCharacterFileName()
		{
			return string.IsNullOrWhiteSpace(Record.CharacterFileName) == false && Record.CharacterFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.CharacterFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateModuleFileName()
		{
			return string.IsNullOrWhiteSpace(Record.ModuleFileName) == false && Record.ModuleFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.ModuleFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateRoomFileName()
		{
			return string.IsNullOrWhiteSpace(Record.RoomFileName) == false && Record.RoomFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.RoomFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateArtifactFileName()
		{
			return string.IsNullOrWhiteSpace(Record.ArtifactFileName) == false && Record.ArtifactFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.ArtifactFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateEffectFileName()
		{
			return string.IsNullOrWhiteSpace(Record.EffectFileName) == false && Record.EffectFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.EffectFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateMonsterFileName()
		{
			return string.IsNullOrWhiteSpace(Record.MonsterFileName) == false && Record.MonsterFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.MonsterFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateHintFileName()
		{
			return string.IsNullOrWhiteSpace(Record.HintFileName) == false && Record.HintFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.HintFileName.Length <= Constants.FsFileNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateGameStateFileName()
		{
			return string.IsNullOrWhiteSpace(Record.GameStateFileName) == false && Record.GameStateFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.GameStateFileName.Length <= Constants.FsFileNameLen;
		}

		#endregion

		#region ValidateInterdependencies Methods

		// do nothing

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		protected virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the fileset." + Environment.NewLine + Environment.NewLine + "If the fileset represents an adventure, use the adventure name; if it represents an author catalog use the catalog name.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescWorkDir()
		{
			var fullDesc = "Enter the working directory of the fileset." + Environment.NewLine + Environment.NewLine + "This is where the files are found.  It can be an absolute or relative path, and should not end with a path separator.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescPluginFileName()
		{
			var fullDesc = "Enter the plugin filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescConfigFileName()
		{
			var fullDesc = "Enter the config filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescFilesetFileName()
		{
			var fullDesc = "Enter the fileset filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescCharacterFileName()
		{
			var fullDesc = "Enter the character filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescModuleFileName()
		{
			var fullDesc = "Enter the module filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescRoomFileName()
		{
			var fullDesc = "Enter the room filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescArtifactFileName()
		{
			var fullDesc = "Enter the artifact filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescEffectFileName()
		{
			var fullDesc = "Enter the effect filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescMonsterFileName()
		{
			var fullDesc = "Enter the monster filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescHintFileName()
		{
			var fullDesc = "Enter the hint filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescGameStateFileName()
		{
			var fullDesc = "Enter the game state filename of the fileset.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		#endregion

		#region List Methods

		/// <summary></summary>
		protected virtual void ListUid()
		{
			if (FullDetail)
			{
				if (!ExcludeROFields)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Uid"), null), Record.Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, Record.Name);
			}
		}

		/// <summary></summary>
		protected virtual void ListName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Name"), null), Record.Name);
			}
		}

		/// <summary></summary>
		protected virtual void ListWorkDir()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("WorkDir"), null), Record.WorkDir);
			}
		}

		/// <summary></summary>
		protected virtual void ListPluginFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("PluginFileName"), null),
					Record.PluginFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListConfigFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ConfigFileName"), null),
					Record.ConfigFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListFilesetFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("FilesetFileName"), null),
					Record.FilesetFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListCharacterFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CharacterFileName"), null),
					Record.CharacterFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListModuleFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ModuleFileName"), null),
					Record.ModuleFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListRoomFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("RoomFileName"), null),
					Record.RoomFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListArtifactFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ArtifactFileName"), null),
					Record.ArtifactFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListEffectFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("EffectFileName"), null),
					Record.EffectFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListMonsterFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("MonsterFileName"), null),
					Record.MonsterFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListHintFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("HintFileName"), null),
					Record.HintFileName);
			}
		}

		/// <summary></summary>
		protected virtual void ListGameStateFileName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("GameStateFileName"), null),
					Record.GameStateFileName);
			}
		}

		#endregion

		#region Input Methods

		/// <summary></summary>
		protected virtual void InputUid()
		{
			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputName()
		{
			var fieldDesc = FieldDesc;

			var name = Record.Name;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", name);

				PrintFieldDesc("Name", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Name"), null));

				var rc = Globals.In.ReadField(Buf, Constants.FsNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Name = Buf.ToString();

				if (ValidateField("Name"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputWorkDir()
		{
			var fieldDesc = FieldDesc;

			var workDir = Record.WorkDir;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", workDir);

				PrintFieldDesc("WorkDir", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("WorkDir"), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, Constants.MaxPathLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.WorkDir = Buf.Trim().ToString();

				if (ValidateField("WorkDir"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputPluginFileName()
		{
			var fieldDesc = FieldDesc;

			var pluginFileName = Record.PluginFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", pluginFileName);

				PrintFieldDesc("PluginFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("PluginFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.PluginFileName = Buf.Trim().ToString();

				if (ValidateField("PluginFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputConfigFileName()
		{
			var fieldDesc = FieldDesc;

			var configFileName = Record.ConfigFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", configFileName);

				PrintFieldDesc("ConfigFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("ConfigFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ConfigFileName = Buf.Trim().ToString();

				if (ValidateField("ConfigFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputFilesetFileName()
		{
			var fieldDesc = FieldDesc;

			var filesetFileName = Record.FilesetFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", filesetFileName);

				PrintFieldDesc("FilesetFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("FilesetFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.FilesetFileName = Buf.Trim().ToString();

				if (ValidateField("FilesetFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputCharacterFileName()
		{
			var fieldDesc = FieldDesc;

			var characterFileName = Record.CharacterFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", characterFileName);

				PrintFieldDesc("CharacterFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("CharacterFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.CharacterFileName = Buf.Trim().ToString();

				if (ValidateField("CharacterFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputModuleFileName()
		{
			var fieldDesc = FieldDesc;

			var moduleFileName = Record.ModuleFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", moduleFileName);

				PrintFieldDesc("ModuleFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("ModuleFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ModuleFileName = Buf.Trim().ToString();

				if (ValidateField("ModuleFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputRoomFileName()
		{
			var fieldDesc = FieldDesc;

			var roomFileName = Record.RoomFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", roomFileName);

				PrintFieldDesc("RoomFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("RoomFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.RoomFileName = Buf.Trim().ToString();

				if (ValidateField("RoomFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputArtifactFileName()
		{
			var fieldDesc = FieldDesc;

			var artifactFileName = Record.ArtifactFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", artifactFileName);

				PrintFieldDesc("ArtifactFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("ArtifactFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArtifactFileName = Buf.Trim().ToString();

				if (ValidateField("ArtifactFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputEffectFileName()
		{
			var fieldDesc = FieldDesc;

			var effectFileName = Record.EffectFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", effectFileName);

				PrintFieldDesc("EffectFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("EffectFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.EffectFileName = Buf.Trim().ToString();

				if (ValidateField("EffectFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputMonsterFileName()
		{
			var fieldDesc = FieldDesc;

			var monsterFileName = Record.MonsterFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", monsterFileName);

				PrintFieldDesc("MonsterFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("MonsterFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.MonsterFileName = Buf.Trim().ToString();

				if (ValidateField("MonsterFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputHintFileName()
		{
			var fieldDesc = FieldDesc;

			var hintFileName = Record.HintFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", hintFileName);

				PrintFieldDesc("HintFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("HintFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.HintFileName = Buf.Trim().ToString();

				if (ValidateField("HintFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputGameStateFileName()
		{
			var fieldDesc = FieldDesc;

			var gameStateFileName = Record.GameStateFileName;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", gameStateFileName);

				PrintFieldDesc("GameStateFileName", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("GameStateFileName"), "NONE"));

				var rc = Globals.In.ReadField(Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.GameStateFileName = Buf.Trim().ToString();

				if (ValidateField("GameStateFileName"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class FilesetHelper

		protected override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetFilesetUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		#endregion

		#region Class FilesetHelper

		public FilesetHelper()
		{
			FieldNames = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Name",
				"WorkDir",
				"PluginFileName",
				"ConfigFileName",
				"FilesetFileName",
				"CharacterFileName",
				"ModuleFileName",
				"RoomFileName",
				"ArtifactFileName",
				"EffectFileName",
				"MonsterFileName",
				"HintFileName",
				"GameStateFileName",
			};

			WorkDirRegex = new Regex(Constants.ValidWorkDirRegexPattern);
		}

		#endregion

		#endregion
	}
}
