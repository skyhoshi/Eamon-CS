
// Fileset.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.DataEntry;
using Eamon.Game.Extensions;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Fileset : Editable, IFileset
	{
		#region Public Properties

		#region Interface IHaveUid

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		#endregion

		#region Interface IFileset

		public virtual string Name { get; set; }

		public virtual string WorkDir { get; set; }

		public virtual string PluginFileName { get; set; }

		public virtual string ConfigFileName { get; set; }

		public virtual string FilesetFileName { get; set; }

		public virtual string CharacterFileName { get; set; }

		public virtual string ModuleFileName { get; set; }

		public virtual string RoomFileName { get; set; }

		public virtual string ArtifactFileName { get; set; }

		public virtual string EffectFileName { get; set; }

		public virtual string MonsterFileName { get; set; }

		public virtual string HintFileName { get; set; }

		public virtual string GameStateFileName { get; set; }

		#endregion

		#endregion

		#region Protected Methods

		#region Interface IDisposable

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeFilesetUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IValidator

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Uid > 0;
		}

		protected virtual bool ValidateName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Name) == false && Name.Length <= Constants.FsNameLen;
		}

		protected virtual bool ValidateWorkDir(IField field, IValidateArgs args)
		{
			return WorkDir.Length > 0 && WorkDir[WorkDir.Length - 1] != Globals.Path.DirectorySeparatorChar;
		}

		protected virtual bool ValidatePluginFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(PluginFileName) == false && PluginFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && PluginFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateConfigFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(ConfigFileName) == false && ConfigFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && ConfigFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateFilesetFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(FilesetFileName) == false && FilesetFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && FilesetFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateCharacterFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(CharacterFileName) == false && CharacterFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && CharacterFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateModuleFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(ModuleFileName) == false && ModuleFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && ModuleFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateRoomFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(RoomFileName) == false && RoomFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && RoomFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateArtifactFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(ArtifactFileName) == false && ArtifactFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && ArtifactFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateEffectFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(EffectFileName) == false && EffectFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && EffectFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateMonsterFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(MonsterFileName) == false && MonsterFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && MonsterFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateHintFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(HintFileName) == false && HintFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && HintFileName.Length <= Constants.FsFileNameLen;
		}

		protected virtual bool ValidateGameStateFileName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(GameStateFileName) == false && GameStateFileName.IndexOf(Globals.Path.DirectorySeparatorChar) == -1 && GameStateFileName.Length <= Constants.FsFileNameLen;
		}

		#endregion

		#region Interface IEditable

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

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Uid, Name);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Name);
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

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), WorkDir);
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
					PluginFileName);
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
					ConfigFileName);
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
					FilesetFileName);
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
					CharacterFileName);
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
					ModuleFileName);
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
					RoomFileName);
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
					ArtifactFileName);
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
					EffectFileName);
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
					MonsterFileName);
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
					HintFileName);
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
					GameStateFileName);
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Uid);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var name = Name;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", name);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Name = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputWorkDir(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var workDir = WorkDir;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", workDir);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.MaxPathLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				WorkDir = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputPluginFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var pluginFileName = PluginFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", pluginFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				PluginFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputConfigFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var configFileName = ConfigFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", configFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ConfigFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputFilesetFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var filesetFileName = FilesetFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", filesetFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				FilesetFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputCharacterFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var characterFileName = CharacterFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", characterFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				CharacterFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputModuleFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var moduleFileName = ModuleFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", moduleFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ModuleFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputRoomFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var roomFileName = RoomFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", roomFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				RoomFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputArtifactFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var artifactFileName = ArtifactFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", artifactFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ArtifactFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputEffectFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var effectFileName = EffectFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", effectFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				EffectFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputMonsterFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var monsterFileName = MonsterFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", monsterFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				MonsterFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputHintFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var hintFileName = HintFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", hintFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				HintFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputGameStateFileName(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var gameStateFileName = GameStateFileName;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", gameStateFileName);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "NONE"));

				var rc = Globals.In.ReadField(args.Buf, Constants.FsFileNameLen, null, '_', '\0', true, "NONE", null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				GameStateFileName = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		#endregion

		#endregion

		#region Class Fileset

		protected virtual void SetFilesetUidIfInvalid(bool editRec)
		{
			if (Uid <= 0)
			{
				Uid = Globals.Database.GetFilesetUid();

				IsUidRecycled = true;
			}
			else if (!editRec)
			{
				IsUidRecycled = false;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHaveFields

		public override IList<IField> GetFields()
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
						x.GetValue = () => Uid;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsUidRecycled";
						x.GetPrintedName = () => "Is Uid Recycled";
						x.GetValue = () => IsUidRecycled;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Name";
						x.Validate = ValidateName;
						x.PrintDesc = PrintDescName;
						x.List = ListName;
						x.Input = InputName;
						x.GetPrintedName = () => "Name";
						x.GetValue = () => Name;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "WorkDir";
						x.Validate = ValidateWorkDir;
						x.PrintDesc = PrintDescWorkDir;
						x.List = ListWorkDir;
						x.Input = InputWorkDir;
						x.GetPrintedName = () => "Working Directory";
						x.GetValue = () => WorkDir;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "PluginFileName";
						x.Validate = ValidatePluginFileName;
						x.PrintDesc = PrintDescPluginFileName;
						x.List = ListPluginFileName;
						x.Input = InputPluginFileName;
						x.GetPrintedName = () => "Plugin Filename";
						x.GetValue = () => PluginFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ConfigFileName";
						x.Validate = ValidateConfigFileName;
						x.PrintDesc = PrintDescConfigFileName;
						x.List = ListConfigFileName;
						x.Input = InputConfigFileName;
						x.GetPrintedName = () => "Config Filename";
						x.GetValue = () => ConfigFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "FilesetFileName";
						x.Validate = ValidateFilesetFileName;
						x.PrintDesc = PrintDescFilesetFileName;
						x.List = ListFilesetFileName;
						x.Input = InputFilesetFileName;
						x.GetPrintedName = () => "Fileset Filename";
						x.GetValue = () => FilesetFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CharacterFileName";
						x.Validate = ValidateCharacterFileName;
						x.PrintDesc = PrintDescCharacterFileName;
						x.List = ListCharacterFileName;
						x.Input = InputCharacterFileName;
						x.GetPrintedName = () => "Character Filename";
						x.GetValue = () => CharacterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ModuleFileName";
						x.Validate = ValidateModuleFileName;
						x.PrintDesc = PrintDescModuleFileName;
						x.List = ListModuleFileName;
						x.Input = InputModuleFileName;
						x.GetPrintedName = () => "Module Filename";
						x.GetValue = () => ModuleFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RoomFileName";
						x.Validate = ValidateRoomFileName;
						x.PrintDesc = PrintDescRoomFileName;
						x.List = ListRoomFileName;
						x.Input = InputRoomFileName;
						x.GetPrintedName = () => "Room Filename";
						x.GetValue = () => RoomFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArtifactFileName";
						x.Validate = ValidateArtifactFileName;
						x.PrintDesc = PrintDescArtifactFileName;
						x.List = ListArtifactFileName;
						x.Input = InputArtifactFileName;
						x.GetPrintedName = () => "Artifact Filename";
						x.GetValue = () => ArtifactFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "EffectFileName";
						x.Validate = ValidateEffectFileName;
						x.PrintDesc = PrintDescEffectFileName;
						x.List = ListEffectFileName;
						x.Input = InputEffectFileName;
						x.GetPrintedName = () => "Effect Filename";
						x.GetValue = () => EffectFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MonsterFileName";
						x.Validate = ValidateMonsterFileName;
						x.PrintDesc = PrintDescMonsterFileName;
						x.List = ListMonsterFileName;
						x.Input = InputMonsterFileName;
						x.GetPrintedName = () => "Monster Filename";
						x.GetValue = () => MonsterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "HintFileName";
						x.Validate = ValidateHintFileName;
						x.PrintDesc = PrintDescHintFileName;
						x.List = ListHintFileName;
						x.Input = InputHintFileName;
						x.GetPrintedName = () => "Hint Filename";
						x.GetValue = () => HintFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "GameStateFileName";
						x.Validate = ValidateGameStateFileName;
						x.PrintDesc = PrintDescGameStateFileName;
						x.List = ListGameStateFileName;
						x.Input = InputGameStateFileName;
						x.GetPrintedName = () => "Game State Filename";
						x.GetValue = () => GameStateFileName;
					})
				};
			}

			return Fields;
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IFileset fileset)
		{
			return this.Uid.CompareTo(fileset.Uid);
		}

		#endregion

		#region Interface IFileset

		public virtual RetCode DeleteFiles(IField field, bool useFilePrefix)
		{
			StringBuilder buf;
			string fileName;
			RetCode rc;

			rc = RetCode.Success;

			buf = new StringBuilder(Constants.BufSize);

			var fields = new List<IField>();

			if (field == null)
			{
				fields.AddRange(GetFields().Where(f => f.Name.EndsWith("FileName")));
			}
			else
			{
				fields.Add(field);
			}

			foreach (var f in fields)
			{
				buf.Clear();

				fileName = f.GetValue() as string;

				if (!string.IsNullOrWhiteSpace(fileName) && !string.Equals(fileName, "NONE", StringComparison.OrdinalIgnoreCase))
				{
					if (!string.IsNullOrWhiteSpace(WorkDir) && !string.Equals(WorkDir, "NONE", StringComparison.OrdinalIgnoreCase))
					{
						buf.Append(Globals.Path.Combine(WorkDir, fileName));
					}
					else
					{
						buf.Append(fileName);
					}

					try
					{
						Globals.File.Delete(useFilePrefix ? Globals.GetPrefixedFileName(buf.ToString()) : buf.ToString());
					}
					catch (Exception ex)
					{
						if (ex != null)
						{
							// do nothing
						}
					}
				}
			}

			return rc;
		}

		#endregion

		#region Class Fileset

		public Fileset()
		{
			SetUidIfInvalid = SetFilesetUidIfInvalid;

			IsUidRecycled = true;

			Name = "";

			WorkDir = "";

			PluginFileName = "";

			ConfigFileName = "";

			FilesetFileName = "";

			CharacterFileName = "";

			ModuleFileName = "";

			RoomFileName = "";

			ArtifactFileName = "";

			EffectFileName = "";

			MonsterFileName = "";

			HintFileName = "";

			GameStateFileName = "";
		}

		#endregion

		#endregion
	}
}
