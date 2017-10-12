
// ModuleHelper.cs

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
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IModule>))]
	public class ModuleHelper : Helper<IModule>
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
			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.ModNameLen;
		}

		protected virtual bool ValidateDesc(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.ModDescLen;
		}

		protected virtual bool ValidateAuthor(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.Author) == false && Record.Author.Length <= Constants.ModAuthorLen;
		}

		protected virtual bool ValidateVolLabel(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.VolLabel) == false && Record.VolLabel.Length <= Constants.ModVolLabelLen;
		}

		protected virtual bool ValidateSerialNum(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.SerialNum) == false && Record.SerialNum.Length <= Constants.ModSerialNumLen;
		}

		protected virtual bool ValidateLastMod(IField field, IValidateArgs args)
		{
			return Record.LastMod != null && Record.LastMod <= DateTime.Now;
		}

		protected virtual bool ValidateIntroStory(IField field, IValidateArgs args)
		{
			return Record.IntroStory >= 0;
		}

		protected virtual bool ValidateNumDirs(IField field, IValidateArgs args)
		{
			return Record.NumDirs == 6 || Record.NumDirs == 10;
		}

		protected virtual bool ValidateNumRooms(IField field, IValidateArgs args)
		{
			return Record.NumRooms >= 0;
		}

		protected virtual bool ValidateNumArtifacts(IField field, IValidateArgs args)
		{
			return Record.NumArtifacts >= 0;
		}

		protected virtual bool ValidateNumEffects(IField field, IValidateArgs args)
		{
			return Record.NumEffects >= 0;
		}

		protected virtual bool ValidateNumMonsters(IField field, IValidateArgs args)
		{
			return Record.NumMonsters >= 0;
		}

		protected virtual bool ValidateNumHints(IField field, IValidateArgs args)
		{
			return Record.NumHints >= 0;
		}

		protected virtual bool ValidateInterdependenciesDesc(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			long invalidUid = 0;

			var rc = Globals.Engine.ResolveUidMacros(Record.Desc, args.Buf, false, false, ref invalidUid);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (invalidUid > 0)
			{
				result = false;

				args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "effect", invalidUid, "which doesn't exist");

				args.ErrorMessage = args.Buf.ToString();

				args.RecordType = typeof(IEffect);

				args.NewRecordUid = invalidUid;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesIntroStory(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			if (Record.IntroStory > 0)
			{
				var effectUid = Record.IntroStory;

				var effect = Globals.EDB[effectUid];

				if (effect == null)
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "effect", effectUid, "which doesn't exist");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IEffect);

					args.NewRecordUid = effectUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		#endregion

		#region PrintFieldDesc Methods

		protected virtual void PrintDescName(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the name of the adventure.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescDesc(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter a detailed description of the adventure.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescAuthor(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the name(s) of the adventure's author(s).";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescVolLabel(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the volume label of the adventure, typically the author(s) initials followed by a private serial number.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescSerialNum(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the global serial number of the adventure, typically assigned by The Powers That Be.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescIntroStory(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the effect uid of the introduction story for the module." + Environment.NewLine + Environment.NewLine + "You can link multiple effects together to create an extended story segment.";

			var briefDesc = "(GE 0)=Valid value";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescNumDirs(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the number of compass directions to use for connections between rooms in the adventure." + Environment.NewLine + Environment.NewLine + "Typically, six directions are used for simpler indoor adventures while ten directions are used for more complex outdoor adventures, but this is only a rule of thumb not a requirement.";

			var briefDesc = "6=Six compass directions; 10=Ten compass directions";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
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
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, Globals.Engine.Capitalize(Record.Name));
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

		protected virtual void ListDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail && args.ShowDesc)
			{
				args.Buf.Clear();

				if (args.ResolveEffects)
				{
					var rc = Globals.Engine.ResolveUidMacros(Record.Desc, args.Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					args.Buf.Append(Record.Desc);
				}

				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
			}
		}

		protected virtual void ListAuthor(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Author);
			}
		}

		protected virtual void ListVolLabel(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.VolLabel);
			}
		}

		protected virtual void ListSerialNum(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.SerialNum);
			}
		}

		protected virtual void ListLastMod(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail && !args.ExcludeROFields)
			{
				args.Buf.Clear();

				args.Buf.Append(Record.LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
			}
		}

		protected virtual void ListIntroStory(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg && Record.IntroStory > 0)
				{
					args.Buf.Clear();

					var effect = Globals.EDB[Record.IntroStory];

					if (effect != null)
					{
						args.Buf.Append(effect.Desc);

						if (args.Buf.Length > 40)
						{
							args.Buf.Length = 40;
						}

						if (args.Buf.Length == 40)
						{
							args.Buf[39] = '.';

							args.Buf[38] = '.';

							args.Buf[37] = '.';
						}
					}

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, Record.IntroStory, null, effect != null ? args.Buf.ToString() : Globals.Engine.UnknownName));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.IntroStory);
				}
			}
		}

		protected virtual void ListNumDirs(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.NumDirs);
			}
		}

		protected virtual void ListNumRooms(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail && !args.ExcludeROFields)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.NumRooms);
			}
		}

		protected virtual void ListNumArtifacts(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail && !args.ExcludeROFields)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.NumArtifacts);
			}
		}

		protected virtual void ListNumEffects(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail && !args.ExcludeROFields)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.NumEffects);
			}
		}

		protected virtual void ListNumMonsters(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail && !args.ExcludeROFields)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.NumMonsters);
			}
		}

		protected virtual void ListNumHints(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail && !args.ExcludeROFields)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.NumHints);
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.Uid);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
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

				var rc = Globals.In.ReadField(args.Buf, Constants.ModNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Name = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var desc = Record.Desc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", desc);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.ModDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.Desc = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputAuthor(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var author = Record.Author;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", author);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.ModAuthorLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Author = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputVolLabel(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var volLabel = Record.VolLabel;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", volLabel);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.ModVolLabelLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.VolLabel = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputSerialNum(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var serialNum = Record.SerialNum;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", serialNum);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "000"));

				var rc = Globals.In.ReadField(args.Buf, Constants.ModSerialNumLen, null, '_', '\0', true, "000", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.SerialNum = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputLastMod(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				Record.LastMod = DateTime.Now;
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputIntroStory(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var introStory = Record.IntroStory;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", introStory);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IntroStory = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumDirs(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var numDirs = Record.NumDirs;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", numDirs);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "6"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "6", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.NumDirs = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumRooms(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				Record.NumRooms = Globals.Database.GetRoomsCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.NumRooms);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumArtifacts(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				Record.NumArtifacts = Globals.Database.GetArtifactsCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.NumArtifacts);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumEffects(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				Record.NumEffects = Globals.Database.GetEffectsCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.NumEffects);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumMonsters(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				Record.NumMonsters = Globals.Database.GetMonstersCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.NumMonsters);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumHints(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				Record.NumHints = Globals.Database.GetHintsCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), Record.NumHints);

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
						x.Name = "Desc";
						x.Validate = ValidateDesc;
						x.ValidateInterdependencies = ValidateInterdependenciesDesc;
						x.PrintDesc = PrintDescDesc;
						x.List = ListDesc;
						x.Input = InputDesc;
						x.GetPrintedName = () => "Description";
						x.GetValue = () => Record.Desc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Author";
						x.Validate = ValidateAuthor;
						x.PrintDesc = PrintDescAuthor;
						x.List = ListAuthor;
						x.Input = InputAuthor;
						x.GetPrintedName = () => "Author";
						x.GetValue = () => Record.Author;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "VolLabel";
						x.Validate = ValidateVolLabel;
						x.PrintDesc = PrintDescVolLabel;
						x.List = ListVolLabel;
						x.Input = InputVolLabel;
						x.GetPrintedName = () => "Volume Label";
						x.GetValue = () => Record.VolLabel;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "SerialNum";
						x.Validate = ValidateSerialNum;
						x.PrintDesc = PrintDescSerialNum;
						x.List = ListSerialNum;
						x.Input = InputSerialNum;
						x.GetPrintedName = () => "Serial Number";
						x.GetValue = () => Record.SerialNum;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "LastMod";
						x.Validate = ValidateLastMod;
						x.List = ListLastMod;
						x.Input = InputLastMod;
						x.GetPrintedName = () => "Last Modified";
						x.GetValue = () => Record.LastMod;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IntroStory";
						x.Validate = ValidateIntroStory;
						x.ValidateInterdependencies = ValidateInterdependenciesIntroStory;
						x.PrintDesc = PrintDescIntroStory;
						x.List = ListIntroStory;
						x.Input = InputIntroStory;
						x.GetPrintedName = () => "Intro Story";
						x.GetValue = () => Record.IntroStory;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumDirs";
						x.Validate = ValidateNumDirs;
						x.PrintDesc = PrintDescNumDirs;
						x.List = ListNumDirs;
						x.Input = InputNumDirs;
						x.GetPrintedName = () => "Compass Directions";
						x.GetValue = () => Record.NumDirs;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumRooms";
						x.Validate = ValidateNumRooms;
						x.List = ListNumRooms;
						x.Input = InputNumRooms;
						x.GetPrintedName = () => "Number Of Rooms";
						x.GetValue = () => Record.NumRooms;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumArtifacts";
						x.Validate = ValidateNumArtifacts;
						x.List = ListNumArtifacts;
						x.Input = InputNumArtifacts;
						x.GetPrintedName = () => "Number Of Artifacts";
						x.GetValue = () => Record.NumArtifacts;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumEffects";
						x.Validate = ValidateNumEffects;
						x.List = ListNumEffects;
						x.Input = InputNumEffects;
						x.GetPrintedName = () => "Number Of Effects";
						x.GetValue = () => Record.NumEffects;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumMonsters";
						x.Validate = ValidateNumMonsters;
						x.List = ListNumMonsters;
						x.Input = InputNumMonsters;
						x.GetPrintedName = () => "Number Of Monsters";
						x.GetValue = () => Record.NumMonsters;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumHints";
						x.Validate = ValidateNumHints;
						x.List = ListNumHints;
						x.Input = InputNumHints;
						x.GetPrintedName = () => "Number Of Hints";
						x.GetValue = () => Record.NumHints;
					})
				};
			}

			return Fields;
		}

		#endregion

		#region Class ModuleHelper

		protected virtual void SetModuleUidIfInvalid(bool editRec)
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetModuleUid();

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

		#region Interface IHelper

		public override void ListErrorField(IValidateArgs args)
		{
			Debug.Assert(args != null && args.ErrorField != null && args.Buf != null);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Uid").GetPrintedName(), null), Record.Uid);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Name").GetPrintedName(), null), Record.Name);

			if (string.Equals(args.ErrorField.Name, "Desc", StringComparison.OrdinalIgnoreCase) || args.ShowDesc)
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Desc").GetPrintedName(), null), Record.Desc);
			}

			if (string.Equals(args.ErrorField.Name, "IntroStory", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), Record.IntroStory);
			}
		}

		#endregion

		#region Class ModuleHelper

		public ModuleHelper()
		{
			SetUidIfInvalid = SetModuleUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
