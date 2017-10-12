
// ConfigHelper.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

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
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IConfig>))]
	public class ConfigHelper : Helper<IConfig>
	{
		#region Protected Methods

		#region Interface IHelper

		#region Validate Methods

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateFieldDesc(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.FieldDesc), Record.FieldDesc);
		}

		protected virtual bool ValidateWordWrapMargin(IField field, IValidateArgs args)
		{
			return Record.WordWrapMargin == Constants.RightMargin;
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Uid);
			}
		}

		protected virtual void ListShowDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.NumberFields)
			{
				field.ListNum = args.ListNum++;
			}

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.ShowDesc));
		}

		protected virtual void ListResolveEffects(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.NumberFields)
			{
				field.ListNum = args.ListNum++;
			}

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.ResolveEffects));
		}

		protected virtual void ListGenerateUids(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.NumberFields)
			{
				field.ListNum = args.ListNum++;
			}

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.GenerateUids));
		}

		protected virtual void ListFieldDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.NumberFields)
			{
				field.ListNum = args.ListNum++;
			}

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Globals.Engine.GetFieldDescNames(Record.FieldDesc));
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.Uid);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputShowDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var showDesc = Record.ShowDesc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(showDesc));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ShowDesc = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

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

			var resolveEffects = Record.ResolveEffects;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(resolveEffects));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ResolveEffects = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

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

			var generateUids = Record.GenerateUids;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(generateUids));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.GenerateUids = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

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

			var fieldDesc01 = Record.FieldDesc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)fieldDesc01);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "2"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "2", null, Globals.Engine.IsChar0To2, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.FieldDesc = (Enums.FieldDesc)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
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
						x.Name = "ShowDesc";
						x.PrintDesc = PrintDescShowDesc;
						x.List = ListShowDesc;
						x.Input = InputShowDesc;
						x.GetPrintedName = () => "Show Descs";
						x.GetValue = () => Record.ShowDesc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ResolveEffects";
						x.PrintDesc = PrintDescResolveEffects;
						x.List = ListResolveEffects;
						x.Input = InputResolveEffects;
						x.GetPrintedName = () => "Resolve Effects";
						x.GetValue = () => Record.ResolveEffects;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "GenerateUids";
						x.PrintDesc = PrintDescGenerateUids;
						x.List = ListGenerateUids;
						x.Input = InputGenerateUids;
						x.GetPrintedName = () => "Generate Uids";
						x.GetValue = () => Record.GenerateUids;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "FieldDesc";
						x.Validate = ValidateFieldDesc;
						x.PrintDesc = PrintDescFieldDesc;
						x.List = ListFieldDesc;
						x.Input = InputFieldDesc;
						x.GetPrintedName = () => "Field Descs";
						x.GetValue = () => Record.FieldDesc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "WordWrapMargin";
						x.Validate = ValidateWordWrapMargin;
						x.GetValue = () => Record.WordWrapMargin;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdFilesetFileName";
						x.GetValue = () => Record.DdFilesetFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdCharacterFileName";
						x.GetValue = () => Record.DdCharacterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdModuleFileName";
						x.GetValue = () => Record.DdModuleFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdRoomFileName";
						x.GetValue = () => Record.DdRoomFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdArtifactFileName";
						x.GetValue = () => Record.DdArtifactFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEffectFileName";
						x.GetValue = () => Record.DdEffectFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdMonsterFileName";
						x.GetValue = () => Record.DdMonsterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdHintFileName";
						x.GetValue = () => Record.DdHintFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MhWorkDir";
						x.GetValue = () => Record.MhWorkDir;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MhFilesetFileName";
						x.GetValue = () => Record.MhFilesetFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MhCharacterFileName";
						x.GetValue = () => Record.MhCharacterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "MhEffectFileName";
						x.GetValue = () => Record.MhEffectFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtFilesetFileName";
						x.GetValue = () => Record.RtFilesetFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtCharacterFileName";
						x.GetValue = () => Record.RtCharacterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtModuleFileName";
						x.GetValue = () => Record.RtModuleFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtRoomFileName";
						x.GetValue = () => Record.RtRoomFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtArtifactFileName";
						x.GetValue = () => Record.RtArtifactFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtEffectFileName";
						x.GetValue = () => Record.RtEffectFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtMonsterFileName";
						x.GetValue = () => Record.RtMonsterFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtHintFileName";
						x.GetValue = () => Record.RtHintFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "RtGameStateFileName";
						x.GetValue = () => Record.RtGameStateFileName;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingFilesets";
						x.GetValue = () => Record.DdEditingFilesets;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingCharacters";
						x.GetValue = () => Record.DdEditingCharacters;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingModules";
						x.GetValue = () => Record.DdEditingModules;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingRooms";
						x.GetValue = () => Record.DdEditingRooms;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingArtifacts";
						x.GetValue = () => Record.DdEditingArtifacts;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingEffects";
						x.GetValue = () => Record.DdEditingEffects;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingMonsters";
						x.GetValue = () => Record.DdEditingMonsters;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DdEditingHints";
						x.GetValue = () => Record.DdEditingHints;
					})
				};
			}

			return Fields;
		}

		#endregion

		#region Class ConfigHelper

		protected virtual void SetConfigUidIfInvalid(bool editRec)
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetConfigUid();

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

		#region Class ConfigHelper

		public ConfigHelper()
		{
			SetUidIfInvalid = SetConfigUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
