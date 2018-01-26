
// FilesetHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IFileset>))]
	public class FilesetHelper : Helper<IFileset>
	{
		#region Protected Methods

		#region Interface IHelper

		#region Validate Methods

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.FsNameLen;
		}

		protected virtual bool ValidateWorkDir(IField field, IValidateArgs args)
		{
			return Record.WorkDir.Length > 0 && Record.WorkDir[Record.WorkDir.Length - 1] != Globals.Path.DirectorySeparatorChar;
		}

		protected virtual bool ValidatePluginFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.PluginFileName) == false && Record.PluginFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.PluginFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateConfigFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.ConfigFileName) == false && Record.ConfigFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.ConfigFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateFilesetFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.FilesetFileName) == false && Record.FilesetFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.FilesetFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateCharacterFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.CharacterFileName) == false && Record.CharacterFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.CharacterFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateModuleFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.ModuleFileName) == false && Record.ModuleFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.ModuleFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateRoomFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.RoomFileName) == false && Record.RoomFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.RoomFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateArtifactFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.ArtifactFileName) == false && Record.ArtifactFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.ArtifactFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateEffectFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.EffectFileName) == false && Record.EffectFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.EffectFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateMonsterFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.MonsterFileName) == false && Record.MonsterFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.MonsterFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateHintFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.HintFileName) == false && Record.HintFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.HintFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateGameStateFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.GameStateFileName) == false && Record.GameStateFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && Record.GameStateFileName.Length <= Constants.FsFileNameLen;
		}

		#endregion

		#region PrintFieldDesc Methods

		protected virtual void PrintDescName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the name of the fileset." + Environment.NewLine + Environment.NewLine + "If the fileset represents an adventure, use the adventure name; if it represents an author catalog use the catalog name.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescWorkDir(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the working directory of the fileset." + Environment.NewLine + Environment.NewLine + "This is where the files are found.  It can be an absolute or relative path, and should not end with a path separator.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescPluginFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the plugin filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescConfigFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the config filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescFilesetFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the fileset filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescCharacterFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the character filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescModuleFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the module filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescRoomFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the room filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescArtifactFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the artifact filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescEffectFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the effect filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescMonsterFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the monster filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescHintFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the hint filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescGameStateFileName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the game state filename of the fileset.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		#endregion

		#region List Methods

		protected virtual void ListUid(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (!args.ExcludeROFields)
				{
					if (args.NumberFields)
					{
						field.ListNum = args.ListNum++;
					}

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, Record.Name);
			}
		}

		protected virtual void ListName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Name);
			}
		}

		protected virtual void ListWorkDir(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.WorkDir);
			}
		}

		protected virtual void ListPluginFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.PluginFileName);
			}
		}

		protected virtual void ListConfigFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.ConfigFileName);
			}
		}

		protected virtual void ListFilesetFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.FilesetFileName);
			}
		}

		protected virtual void ListCharacterFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.CharacterFileName);
			}
		}

		protected virtual void ListModuleFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.ModuleFileName);
			}
		}

		protected virtual void ListRoomFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.RoomFileName);
			}
		}

		protected virtual void ListArtifactFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.ArtifactFileName);
			}
		}

		protected virtual void ListEffectFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.EffectFileName);
			}
		}

		protected virtual void ListMonsterFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.MonsterFileName);
			}
		}

		protected virtual void ListHintFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.HintFileName);
			}
		}

		protected virtual void ListGameStateFileName(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{0}{0}{2}",
					Environment.NewLine,
					Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
					Record.GameStateFileName);
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var name = Record.Name;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", name);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Name = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputWorkDir(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var workDir = Record.WorkDir;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", workDir);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.MaxPathLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.WorkDir = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputPluginFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var pluginFileName = Record.PluginFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", pluginFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.PluginFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputConfigFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var configFileName = Record.ConfigFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", configFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ConfigFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputFilesetFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var filesetFileName = Record.FilesetFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", filesetFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.FilesetFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputCharacterFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var characterFileName = Record.CharacterFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", characterFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.CharacterFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputModuleFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var moduleFileName = Record.ModuleFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", moduleFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ModuleFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputRoomFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var roomFileName = Record.RoomFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", roomFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.RoomFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputArtifactFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var artifactFileName = Record.ArtifactFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", artifactFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArtifactFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputEffectFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var effectFileName = Record.EffectFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", effectFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.EffectFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputMonsterFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var monsterFileName = Record.MonsterFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", monsterFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.MonsterFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputHintFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var hintFileName = Record.HintFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", hintFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.HintFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputGameStateFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var gameStateFileName = Record.GameStateFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", gameStateFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.GameStateFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		#endregion

		protected override IList<IField> GetFields()
		{
			if (Fields == null)
			{
				Fields = new List<IField>()
				{
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Uid";
						x.Validate = ValidateUid;
						x.List = ListUid;
						x.Input = InputUid;
						x.GetPrintedName = () => "Uid";
						x.GetValue = () => Record.Uid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsUidRecycled";
						x.GetPrintedName = () => "Is Uid Recycled";
						x.GetValue = () => Record.IsUidRecycled;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Name";
						x.Validate = ValidateName;
						x.PrintDesc = PrintDescName;
						x.List = ListName;
						x.Input = InputName;
						x.GetPrintedName = () => "Name";
						x.GetValue = () => Record.Name;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "WorkDir";
						x.Validate = ValidateWorkDir;
						x.PrintDesc = PrintDescWorkDir;
						x.List = ListWorkDir;
						x.Input = InputWorkDir;
						x.GetPrintedName = () => "Working Directory";
						x.GetValue = () => Record.WorkDir;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "PluginFileName";
						x.Validate = ValidatePluginFileName;
						x.PrintDesc = PrintDescPluginFileName;
						x.List = ListPluginFileName;
						x.Input = InputPluginFileName;
						x.GetPrintedName = () => "Plugin Filename";
						x.GetValue = () => Record.PluginFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ConfigFileName";
						x.Validate = ValidateConfigFileName;
						x.PrintDesc = PrintDescConfigFileName;
						x.List = ListConfigFileName;
						x.Input = InputConfigFileName;
						x.GetPrintedName = () => "Config Filename";
						x.GetValue = () => Record.ConfigFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "FilesetFileName";
						x.Validate = ValidateFilesetFileName;
						x.PrintDesc = PrintDescFilesetFileName;
						x.List = ListFilesetFileName;
						x.Input = InputFilesetFileName;
						x.GetPrintedName = () => "Fileset Filename";
						x.GetValue = () => Record.FilesetFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CharacterFileName";
						x.Validate = ValidateCharacterFileName;
						x.PrintDesc = PrintDescCharacterFileName;
						x.List = ListCharacterFileName;
						x.Input = InputCharacterFileName;
						x.GetPrintedName = () => "Character Filename";
						x.GetValue = () => Record.CharacterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ModuleFileName";
						x.Validate = ValidateModuleFileName;
						x.PrintDesc = PrintDescModuleFileName;
						x.List = ListModuleFileName;
						x.Input = InputModuleFileName;
						x.GetPrintedName = () => "Module Filename";
						x.GetValue = () => Record.ModuleFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RoomFileName";
						x.Validate = ValidateRoomFileName;
						x.PrintDesc = PrintDescRoomFileName;
						x.List = ListRoomFileName;
						x.Input = InputRoomFileName;
						x.GetPrintedName = () => "Room Filename";
						x.GetValue = () => Record.RoomFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArtifactFileName";
						x.Validate = ValidateArtifactFileName;
						x.PrintDesc = PrintDescArtifactFileName;
						x.List = ListArtifactFileName;
						x.Input = InputArtifactFileName;
						x.GetPrintedName = () => "Artifact Filename";
						x.GetValue = () => Record.ArtifactFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "EffectFileName";
						x.Validate = ValidateEffectFileName;
						x.PrintDesc = PrintDescEffectFileName;
						x.List = ListEffectFileName;
						x.Input = InputEffectFileName;
						x.GetPrintedName = () => "Effect Filename";
						x.GetValue = () => Record.EffectFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MonsterFileName";
						x.Validate = ValidateMonsterFileName;
						x.PrintDesc = PrintDescMonsterFileName;
						x.List = ListMonsterFileName;
						x.Input = InputMonsterFileName;
						x.GetPrintedName = () => "Monster Filename";
						x.GetValue = () => Record.MonsterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "HintFileName";
						x.Validate = ValidateHintFileName;
						x.PrintDesc = PrintDescHintFileName;
						x.List = ListHintFileName;
						x.Input = InputHintFileName;
						x.GetPrintedName = () => "Hint Filename";
						x.GetValue = () => Record.HintFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "GameStateFileName";
						x.Validate = ValidateGameStateFileName;
						x.PrintDesc = PrintDescGameStateFileName;
						x.List = ListGameStateFileName;
						x.Input = InputGameStateFileName;
						x.GetPrintedName = () => "Game State Filename";
						x.GetValue = () => Record.GameStateFileName;
					})
				};
			}

			return Fields;
		}

		#endregion

		#region Class FilesetHelper

		protected virtual void SetFilesetUidIfInvalid(bool editRec)
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetFilesetUid();

				Record.IsUidRecycled = true;
			}
			else if (!editRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Class FilesetHelper

		public FilesetHelper()
		{
			SetUidIfInvalid = SetFilesetUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
