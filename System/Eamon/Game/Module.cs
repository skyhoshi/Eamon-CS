
// Module.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Module : GameBase, IModule
	{
		#region Public Properties

		#region Interface IModule

		public virtual string Author { get; set; }

		public virtual string VolLabel { get; set; }

		public virtual string SerialNum { get; set; }

		public virtual DateTime LastMod { get; set; }

		public virtual long IntroStory { get; set; }

		public virtual long NumDirs { get; set; }

		public virtual long NumRooms { get; set; }

		public virtual long NumArtifacts { get; set; }

		public virtual long NumEffects { get; set; }

		public virtual long NumMonsters { get; set; }

		public virtual long NumHints { get; set; }

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
				Globals.Database.FreeModuleUid(Uid);

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

		protected virtual bool ValidateName(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Name) == false && Name.Length <= Constants.ModNameLen;
		}

		protected virtual bool ValidateDesc(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Desc) == false && Desc.Length <= Constants.ModDescLen;
		}

		protected virtual bool ValidateAuthor(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Author) == false && Author.Length <= Constants.ModAuthorLen;
		}

		protected virtual bool ValidateVolLabel(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(VolLabel) == false && VolLabel.Length <= Constants.ModVolLabelLen;
		}

		protected virtual bool ValidateSerialNum(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(SerialNum) == false && SerialNum.Length <= Constants.ModSerialNumLen;
		}

		protected virtual bool ValidateLastMod(IField field, IValidateArgs args)
		{
			return LastMod != null && LastMod <= DateTime.Now;
		}

		protected virtual bool ValidateIntroStory(IField field, IValidateArgs args)
		{
			return IntroStory >= 0;
		}

		protected virtual bool ValidateNumDirs(IField field, IValidateArgs args)
		{
			return NumDirs == 6 || NumDirs == 10;
		}

		protected virtual bool ValidateNumRooms(IField field, IValidateArgs args)
		{
			return NumRooms >= 0;
		}

		protected virtual bool ValidateNumArtifacts(IField field, IValidateArgs args)
		{
			return NumArtifacts >= 0;
		}

		protected virtual bool ValidateNumEffects(IField field, IValidateArgs args)
		{
			return NumEffects >= 0;
		}

		protected virtual bool ValidateNumMonsters(IField field, IValidateArgs args)
		{
			return NumMonsters >= 0;
		}

		protected virtual bool ValidateNumHints(IField field, IValidateArgs args)
		{
			return NumHints >= 0;
		}

		protected virtual bool ValidateInterdependenciesDesc(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			long invalidUid = 0;

			var rc = Globals.Engine.ResolveUidMacros(Desc, args.Buf, false, false, ref invalidUid);

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

			if (IntroStory > 0)
			{
				var effectUid = IntroStory;

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

					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Uid);
				}
			}
			else
			{
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Uid, Globals.Engine.Capitalize(Name));
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

		protected virtual void ListDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail && args.ShowDesc)
			{
				args.Buf.Clear();

				if (args.ResolveEffects)
				{
					var rc = Globals.Engine.ResolveUidMacros(Desc, args.Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					args.Buf.Append(Desc);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Author);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), VolLabel);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), SerialNum);
			}
		}

		protected virtual void ListLastMod(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail && !args.ExcludeROFields)
			{
				args.Buf.Clear();

				args.Buf.Append(LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

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

				if (args.LookupMsg && IntroStory > 0)
				{
					args.Buf.Clear();

					var effect = Globals.EDB[IntroStory];

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
						Globals.Engine.BuildValue(51, ' ', 8, IntroStory, null, effect != null ? args.Buf.ToString() : Globals.Engine.UnknownName));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), IntroStory);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), NumDirs);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), NumRooms);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), NumArtifacts);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), NumEffects);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), NumMonsters);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), NumHints);
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

				var rc = Globals.In.ReadField(args.Buf, Constants.ModNameLen, null, '_', '\0', false, null, null, null, null);

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

		protected virtual void InputDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var desc = Desc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", desc);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.ModDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Desc = args.Buf.Trim().ToString();

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

			var author = Author;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", author);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.ModAuthorLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Author = args.Buf.Trim().ToString();

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

			var volLabel = VolLabel;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", volLabel);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				var rc = Globals.In.ReadField(args.Buf, Constants.ModVolLabelLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				VolLabel = args.Buf.Trim().ToString();

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

			var serialNum = SerialNum;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", serialNum);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "000"));

				var rc = Globals.In.ReadField(args.Buf, Constants.ModSerialNumLen, null, '_', '\0', true, "000", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				SerialNum = args.Buf.Trim().ToString();

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
				LastMod = DateTime.Now;
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputIntroStory(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var introStory = IntroStory;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", introStory);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				IntroStory = Convert.ToInt64(args.Buf.Trim().ToString());

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

			var numDirs = NumDirs;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", numDirs);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "6"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "6", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				NumDirs = Convert.ToInt64(args.Buf.Trim().ToString());

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
				NumRooms = Globals.Database.GetRoomsCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), NumRooms);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumArtifacts(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				NumArtifacts = Globals.Database.GetArtifactsCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), NumArtifacts);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumEffects(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				NumEffects = Globals.Database.GetEffectsCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), NumEffects);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumMonsters(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				NumMonsters = Globals.Database.GetMonstersCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), NumMonsters);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumHints(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (!args.EditRec)
			{
				NumHints = Globals.Database.GetHintsCount();
			}

			Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null), NumHints);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		#endregion

		#endregion

		#region Class Module

		protected virtual void SetModuleUidIfInvalid(bool editRec)
		{
			if (Uid <= 0)
			{
				Uid = Globals.Database.GetModuleUid();

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
						x.Name = "Desc";
						x.Validate = ValidateDesc;
						x.ValidateInterdependencies = ValidateInterdependenciesDesc;
						x.PrintDesc = PrintDescDesc;
						x.List = ListDesc;
						x.Input = InputDesc;
						x.GetPrintedName = () => "Description";
						x.GetValue = () => Desc;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Author";
						x.Validate = ValidateAuthor;
						x.PrintDesc = PrintDescAuthor;
						x.List = ListAuthor;
						x.Input = InputAuthor;
						x.GetPrintedName = () => "Author";
						x.GetValue = () => Author;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "VolLabel";
						x.Validate = ValidateVolLabel;
						x.PrintDesc = PrintDescVolLabel;
						x.List = ListVolLabel;
						x.Input = InputVolLabel;
						x.GetPrintedName = () => "Volume Label";
						x.GetValue = () => VolLabel;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "SerialNum";
						x.Validate = ValidateSerialNum;
						x.PrintDesc = PrintDescSerialNum;
						x.List = ListSerialNum;
						x.Input = InputSerialNum;
						x.GetPrintedName = () => "Serial Number";
						x.GetValue = () => SerialNum;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "LastMod";
						x.Validate = ValidateLastMod;
						x.List = ListLastMod;
						x.Input = InputLastMod;
						x.GetPrintedName = () => "Last Modified";
						x.GetValue = () => LastMod;
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
						x.GetValue = () => IntroStory;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumDirs";
						x.Validate = ValidateNumDirs;
						x.PrintDesc = PrintDescNumDirs;
						x.List = ListNumDirs;
						x.Input = InputNumDirs;
						x.GetPrintedName = () => "Compass Directions";
						x.GetValue = () => NumDirs;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumRooms";
						x.Validate = ValidateNumRooms;
						x.List = ListNumRooms;
						x.Input = InputNumRooms;
						x.GetPrintedName = () => "Number Of Rooms";
						x.GetValue = () => NumRooms;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumArtifacts";
						x.Validate = ValidateNumArtifacts;
						x.List = ListNumArtifacts;
						x.Input = InputNumArtifacts;
						x.GetPrintedName = () => "Number Of Artifacts";
						x.GetValue = () => NumArtifacts;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumEffects";
						x.Validate = ValidateNumEffects;
						x.List = ListNumEffects;
						x.Input = InputNumEffects;
						x.GetPrintedName = () => "Number Of Effects";
						x.GetValue = () => NumEffects;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumMonsters";
						x.Validate = ValidateNumMonsters;
						x.List = ListNumMonsters;
						x.Input = InputNumMonsters;
						x.GetPrintedName = () => "Number Of Monsters";
						x.GetValue = () => NumMonsters;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumHints";
						x.Validate = ValidateNumHints;
						x.List = ListNumHints;
						x.Input = InputNumHints;
						x.GetPrintedName = () => "Number Of Hints";
						x.GetValue = () => NumHints;
					})
				};
			}

			return Fields;
		}

		public override void ListErrorField(IValidateArgs args)
		{
			Debug.Assert(args != null && args.ErrorField != null && args.Buf != null);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Uid").GetPrintedName(), null), Uid);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Name").GetPrintedName(), null), Name);

			if (string.Equals(args.ErrorField.Name, "Desc", StringComparison.OrdinalIgnoreCase) || args.ShowDesc)
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Desc").GetPrintedName(), null), Desc);
			}

			if (string.Equals(args.ErrorField.Name, "IntroStory", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), IntroStory);
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IModule module)
		{
			return this.Uid.CompareTo(module.Uid);
		}

		#endregion

		#region Interface IModule

		public virtual void PrintInfo()
		{
			var buf = new StringBuilder(Constants.BufSize);

			buf.AppendFormat("{0}This is {1}, by {2}.{0}",
				Environment.NewLine,
				Name,
				Author);

			buf.AppendFormat("{0}Serial Number:  {1}{0}Volume  Label:  {2}{0}Last Modified:  {3}{0}{0}",
				Environment.NewLine,
				SerialNum,
				VolLabel,
				LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

			buf.AppendFormat("{0}{1}", Desc, Environment.NewLine);

			Globals.Out.Write("{0}", buf);
		}

		#endregion

		#region Class Module

		public Module()
		{
			SetUidIfInvalid = SetModuleUidIfInvalid;

			Author = "";

			VolLabel = "";

			SerialNum = "";

			LastMod = DateTime.Now;
		}

		#endregion

		#endregion
	}
}
