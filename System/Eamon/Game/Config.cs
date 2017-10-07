
// Config.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Config : GameBase, IConfig
	{
		#region Public Properties

		#region Interface IConfig

		public virtual bool ShowDesc { get; set; }

		public virtual bool ResolveEffects { get; set; }

		public virtual bool GenerateUids { get; set; }

		public virtual Enums.FieldDesc FieldDesc { get; set; }

		public virtual long WordWrapMargin { get; set; }

		public virtual string DdFilesetFileName { get; set; }

		public virtual string DdCharacterFileName { get; set; }

		public virtual string DdModuleFileName { get; set; }

		public virtual string DdRoomFileName { get; set; }

		public virtual string DdArtifactFileName { get; set; }

		public virtual string DdEffectFileName { get; set; }

		public virtual string DdMonsterFileName { get; set; }

		public virtual string DdHintFileName { get; set; }

		public virtual string MhWorkDir { get; set; }

		public virtual string MhFilesetFileName { get; set; }

		public virtual string MhCharacterFileName { get; set; }

		public virtual string MhEffectFileName { get; set; }

		public virtual string RtFilesetFileName { get; set; }

		public virtual string RtCharacterFileName { get; set; }

		public virtual string RtModuleFileName { get; set; }

		public virtual string RtRoomFileName { get; set; }

		public virtual string RtArtifactFileName { get; set; }

		public virtual string RtEffectFileName { get; set; }

		public virtual string RtMonsterFileName { get; set; }

		public virtual string RtHintFileName { get; set; }

		public virtual string RtGameStateFileName { get; set; }

		public virtual bool DdEditingFilesets { get; set; }

		public virtual bool DdEditingCharacters { get; set; }

		public virtual bool DdEditingModules { get; set; }

		public virtual bool DdEditingRooms { get; set; }

		public virtual bool DdEditingArtifacts { get; set; }

		public virtual bool DdEditingEffects { get; set; }

		public virtual bool DdEditingMonsters { get; set; }

		public virtual bool DdEditingHints { get; set; }

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
				Globals.Database.FreeConfigUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IGameBase

		#region Validate Methods

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Uid > 0;
		}

		protected virtual bool ValidateFieldDesc(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.FieldDesc), FieldDesc);
		}

		protected virtual bool ValidateWordWrapMargin(IField field, IValidateArgs args)
		{
			return WordWrapMargin == Constants.RightMargin;
		}

		#endregion

		#region PrintFieldDesc Methods

		protected virtual void PrintDescShowDesc(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter whether to omit or show descriptions during record detail listing.";

			var briefDesc = string.Format("{0}=Omit descriptions; {1}=Show descriptions", Convert.ToInt64(false), Convert.ToInt64(true));

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescResolveEffects(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter whether to show or resolve effect uids in descriptions during record detail listing.";

			var briefDesc = string.Format("{0}=Show effect uids; {1}=Resolve effect uids", Convert.ToInt64(false), Convert.ToInt64(true));

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescGenerateUids(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter whether to allow user input of uids or use system generated uids when adding new records.";

			var briefDesc = string.Format("{0}=Allow user input of uids; {1}=Use system generated uids", Convert.ToInt64(false), Convert.ToInt64(true));

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescFieldDesc(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the verbosity of the field descriptions shown during record input.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var fieldDescValues = EnumUtil.GetValues<Enums.FieldDesc>();

			for (var j = 0; j < fieldDescValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)fieldDescValues[j], Globals.Engine.GetFieldDescNames(fieldDescValues[j]));
			}

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
		}

		#endregion

		#region List Methods

		protected virtual void ListUid(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.ExcludeROFields)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Uid);
			}
		}

		protected virtual void ListShowDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.NumberFields)
			{
				field.ListNum = args.ListNum++;
			}

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(ShowDesc));
		}

		protected virtual void ListResolveEffects(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.NumberFields)
			{
				field.ListNum = args.ListNum++;
			}

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(ResolveEffects));
		}

		protected virtual void ListGenerateUids(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.NumberFields)
			{
				field.ListNum = args.ListNum++;
			}

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(GenerateUids));
		}

		protected virtual void ListFieldDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.NumberFields)
			{
				field.ListNum = args.ListNum++;
			}

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Globals.Engine.GetFieldDescNames(FieldDesc));
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Uid);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputShowDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var showDesc = ShowDesc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(showDesc));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ShowDesc = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputResolveEffects(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var resolveEffects = ResolveEffects;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(resolveEffects));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ResolveEffects = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputGenerateUids(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var generateUids = GenerateUids;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(generateUids));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				GenerateUids = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputFieldDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var fieldDesc01 = FieldDesc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)fieldDesc01);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "2"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "2", null, Globals.Engine.IsChar0To2, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				FieldDesc = (Enums.FieldDesc)Convert.ToInt64(args.Buf.Trim().ToString());

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

		#region Class Config

		protected virtual void SetConfigUidIfInvalid(bool editRec)
		{
			if (Uid <= 0)
			{
				Uid = Globals.Database.GetConfigUid();

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

		#region Interface IGameBase

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
						x.Name = "ShowDesc";
						x.PrintDesc = PrintDescShowDesc;
						x.List = ListShowDesc;
						x.Input = InputShowDesc;
						x.GetPrintedName = () => "Show Descs";
						x.GetValue = () => ShowDesc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ResolveEffects";
						x.PrintDesc = PrintDescResolveEffects;
						x.List = ListResolveEffects;
						x.Input = InputResolveEffects;
						x.GetPrintedName = () => "Resolve Effects";
						x.GetValue = () => ResolveEffects;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "GenerateUids";
						x.PrintDesc = PrintDescGenerateUids;
						x.List = ListGenerateUids;
						x.Input = InputGenerateUids;
						x.GetPrintedName = () => "Generate Uids";
						x.GetValue = () => GenerateUids;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "FieldDesc";
						x.Validate = ValidateFieldDesc;
						x.PrintDesc = PrintDescFieldDesc;
						x.List = ListFieldDesc;
						x.Input = InputFieldDesc;
						x.GetPrintedName = () => "Field Descs";
						x.GetValue = () => FieldDesc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "WordWrapMargin";
						x.Validate = ValidateWordWrapMargin;
						x.GetValue = () => WordWrapMargin;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdFilesetFileName";
						x.GetValue = () => DdFilesetFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdCharacterFileName";
						x.GetValue = () => DdCharacterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdModuleFileName";
						x.GetValue = () => DdModuleFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdRoomFileName";
						x.GetValue = () => DdRoomFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdArtifactFileName";
						x.GetValue = () => DdArtifactFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEffectFileName";
						x.GetValue = () => DdEffectFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdMonsterFileName";
						x.GetValue = () => DdMonsterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdHintFileName";
						x.GetValue = () => DdHintFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MhWorkDir";
						x.GetValue = () => MhWorkDir;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MhFilesetFileName";
						x.GetValue = () => MhFilesetFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MhCharacterFileName";
						x.GetValue = () => MhCharacterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MhEffectFileName";
						x.GetValue = () => MhEffectFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtFilesetFileName";
						x.GetValue = () => RtFilesetFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtCharacterFileName";
						x.GetValue = () => RtCharacterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtModuleFileName";
						x.GetValue = () => RtModuleFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtRoomFileName";
						x.GetValue = () => RtRoomFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtArtifactFileName";
						x.GetValue = () => RtArtifactFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtEffectFileName";
						x.GetValue = () => RtEffectFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtMonsterFileName";
						x.GetValue = () => RtMonsterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtHintFileName";
						x.GetValue = () => RtHintFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtGameStateFileName";
						x.GetValue = () => RtGameStateFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingFilesets";
						x.GetValue = () => DdEditingFilesets;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingCharacters";
						x.GetValue = () => DdEditingCharacters;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingModules";
						x.GetValue = () => DdEditingModules;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingRooms";
						x.GetValue = () => DdEditingRooms;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingArtifacts";
						x.GetValue = () => DdEditingArtifacts;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingEffects";
						x.GetValue = () => DdEditingEffects;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingMonsters";
						x.GetValue = () => DdEditingMonsters;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingHints";
						x.GetValue = () => DdEditingHints;
					})
				};
			}

			return Fields;
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IConfig config)
		{
			return this.Uid.CompareTo(config.Uid);
		}

		#endregion

		#region Interface IConfig

		public virtual RetCode LoadGameDatabase(bool useFilePrefix, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			rc = Globals.Database.LoadFilesets(Globals.GetPrefixedFileName(RtFilesetFileName), validate, printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadFilesets function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadCharacters(Globals.GetPrefixedFileName(RtCharacterFileName), validate, printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadCharacters function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadModules(useFilePrefix ? Globals.GetPrefixedFileName(RtModuleFileName) : RtModuleFileName, validate, printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadModules function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadRooms(useFilePrefix ? Globals.GetPrefixedFileName(RtRoomFileName) : RtRoomFileName, validate, printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRooms function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadArtifacts(useFilePrefix ? Globals.GetPrefixedFileName(RtArtifactFileName) : RtArtifactFileName, validate, printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadArtifacts function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadEffects(useFilePrefix ? Globals.GetPrefixedFileName(RtEffectFileName) : RtEffectFileName, validate, printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadEffects function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadMonsters(useFilePrefix ? Globals.GetPrefixedFileName(RtMonsterFileName) : RtMonsterFileName, validate, printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadMonsters function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadHints(useFilePrefix ? Globals.GetPrefixedFileName(RtHintFileName) : RtHintFileName, validate, printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadHints function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadGameStates(Globals.GetPrefixedFileName(RtGameStateFileName), validate, printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadGameStates function call failed");

				goto Cleanup;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode SaveGameDatabase(bool printOutput = true)
		{
			RetCode rc;

			rc = Globals.Database.SaveGameStates(Globals.GetPrefixedFileName(RtGameStateFileName), printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveGameStates function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveHints(Globals.GetPrefixedFileName(RtHintFileName), printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveHints function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveMonsters(Globals.GetPrefixedFileName(RtMonsterFileName), printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveMonsters function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveEffects(Globals.GetPrefixedFileName(RtEffectFileName), printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveEffects function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveArtifacts(Globals.GetPrefixedFileName(RtArtifactFileName), printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveArtifacts function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveRooms(Globals.GetPrefixedFileName(RtRoomFileName), printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRooms function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveModules(Globals.GetPrefixedFileName(RtModuleFileName), printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveModules function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveCharacters(Globals.GetPrefixedFileName(RtCharacterFileName), printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveCharacters function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveFilesets(Globals.GetPrefixedFileName(RtFilesetFileName), printOutput);

			if (Globals.Engine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveFilesets function call failed");

				goto Cleanup;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode DeleteGameState(string configFileName, bool startOver)
		{
			RetCode rc;

			if (configFileName == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			foreach (var fs in Globals.Database.FilesetTable.Records)
			{
				rc = fs.DeleteFiles(null, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));
			}

			if (startOver)
			{
				rc = Globals.Database.FreeFilesets();

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				rc = Globals.Database.SaveFilesets(Globals.GetPrefixedFileName(RtFilesetFileName), false);

				Debug.Assert(Globals.Engine.IsSuccess(rc));
			}
			else
			{
				try
				{
					Globals.File.Delete(Globals.GetPrefixedFileName(configFileName));
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// do nothing
					}
				}

				try
				{
					Globals.File.Delete(Globals.GetPrefixedFileName(RtCharacterFileName));
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// do nothing
					}
				}

				try
				{
					Globals.File.Delete(Globals.GetPrefixedFileName(RtFilesetFileName));
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// do nothing
					}
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode CopyProperties(IConfig config)
		{
			RetCode rc;

			if (config == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			Uid = config.Uid;

			IsUidRecycled = config.IsUidRecycled;

			ShowDesc = config.ShowDesc;

			ResolveEffects = config.ResolveEffects;

			GenerateUids = config.GenerateUids;

			FieldDesc = config.FieldDesc;

			WordWrapMargin = config.WordWrapMargin;

			DdFilesetFileName = Globals.CloneInstance(config.DdFilesetFileName);

			DdCharacterFileName = Globals.CloneInstance(config.DdCharacterFileName);

			DdModuleFileName = Globals.CloneInstance(config.DdModuleFileName);

			DdRoomFileName = Globals.CloneInstance(config.DdRoomFileName);

			DdArtifactFileName = Globals.CloneInstance(config.DdArtifactFileName);

			DdEffectFileName = Globals.CloneInstance(config.DdEffectFileName);

			DdMonsterFileName = Globals.CloneInstance(config.DdMonsterFileName);

			DdHintFileName = Globals.CloneInstance(config.DdHintFileName);

			MhWorkDir = Globals.CloneInstance(config.MhWorkDir);

			MhFilesetFileName = Globals.CloneInstance(config.MhFilesetFileName);

			MhCharacterFileName = Globals.CloneInstance(config.MhCharacterFileName);

			MhEffectFileName = Globals.CloneInstance(config.MhEffectFileName);

			RtFilesetFileName = Globals.CloneInstance(config.RtFilesetFileName);

			RtCharacterFileName = Globals.CloneInstance(config.RtCharacterFileName);

			RtModuleFileName = Globals.CloneInstance(config.RtModuleFileName);

			RtRoomFileName = Globals.CloneInstance(config.RtRoomFileName);

			RtArtifactFileName = Globals.CloneInstance(config.RtArtifactFileName);

			RtEffectFileName = Globals.CloneInstance(config.RtEffectFileName);

			RtMonsterFileName = Globals.CloneInstance(config.RtMonsterFileName);

			RtHintFileName = Globals.CloneInstance(config.RtHintFileName);

			RtGameStateFileName = Globals.CloneInstance(config.RtGameStateFileName);

			DdEditingFilesets = config.DdEditingFilesets;

			DdEditingCharacters = config.DdEditingCharacters;

			DdEditingModules = config.DdEditingModules;

			DdEditingRooms = config.DdEditingRooms;

			DdEditingArtifacts = config.DdEditingArtifacts;

			DdEditingEffects = config.DdEditingEffects;

			DdEditingMonsters = config.DdEditingMonsters;

			DdEditingHints = config.DdEditingHints;

		Cleanup:

			return rc;
		}

		#endregion

		#region Class Config

		public Config()
		{
			SetUidIfInvalid = SetConfigUidIfInvalid;

			DdFilesetFileName = "";

			DdCharacterFileName = "";

			DdModuleFileName = "";

			DdRoomFileName = "";

			DdArtifactFileName = "";

			DdEffectFileName = "";

			DdMonsterFileName = "";

			DdHintFileName = "";

			MhWorkDir = "";

			MhFilesetFileName = "";

			MhCharacterFileName = "";

			MhEffectFileName = "";

			RtFilesetFileName = "";

			RtCharacterFileName = "";

			RtModuleFileName = "";

			RtRoomFileName = "";

			RtArtifactFileName = "";

			RtEffectFileName = "";

			RtMonsterFileName = "";

			RtHintFileName = "";

			RtGameStateFileName = "";
		}

		#endregion

		#endregion
	}
}
