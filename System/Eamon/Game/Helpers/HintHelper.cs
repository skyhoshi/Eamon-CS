
// HintHelper.cs

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
	[ClassMappings(typeof(IHelper<IHint>))]
	public class HintHelper : Helper<IHint>
	{
		#region Protected Methods

		#region Interface IHelper

		#region Validate Methods

		protected virtual bool ValidateUid(IField field, IValidateArgs args)
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateQuestion(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.Question) == false && Record.Question.Length <= Constants.HntQuestionLen;
		}

		protected virtual bool ValidateNumAnswers(IField field, IValidateArgs args)
		{
			return Record.NumAnswers >= 1 && Record.NumAnswers <= Record.Answers.Length;
		}

		protected virtual bool ValidateAnswers(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(i >= 0 && i < Record.Answers.Length);

			return i < Record.NumAnswers ? string.IsNullOrWhiteSpace(Record.GetAnswers(i)) == false && Record.GetAnswers(i).Length <= Constants.HntAnswerLen : Record.GetAnswers(i) == "";
		}

		protected virtual bool ValidateInterdependenciesQuestion(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			long invalidUid = 0;

			var rc = Globals.Engine.ResolveUidMacros(Record.Question, args.Buf, false, false, ref invalidUid);

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

		protected virtual bool ValidateInterdependenciesAnswers(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var result = true;

			long invalidUid = 0;

			var i = Convert.ToInt64(field.UserData);

			Debug.Assert(i >= 0 && i < Record.Answers.Length);

			if (i < Record.NumAnswers)
			{
				var rc = Globals.Engine.ResolveUidMacros(Record.GetAnswers(i), args.Buf, false, false, ref invalidUid);

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
			}

		Cleanup:

			return result;
		}

		#endregion

		#region PrintFieldDesc Methods

		protected virtual void PrintDescActive(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the active status of the hint." + Environment.NewLine + Environment.NewLine + "An active hint is immediately available to the player, while inactive hints must be activated by special (user programmed) events.";

			var briefDesc = "0=Inactive; 1=Active";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescQuestion(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the name, topic or question of the hint.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescNumAnswers(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the number of answers for the hint.";

			var briefDesc = string.Format("1-{0}=Valid value", Record.Answers.Length);

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescAnswers(IField field, IPrintDescArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			var fullDesc = string.Format("Enter the hint's answer #{0}.", i + 1);

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		#endregion

		#region List Methods

		protected virtual void ListUid(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null && args.Buf01 != null);

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
				args.Buf.SetFormat("{0,3}. {1}", Record.Uid, Record.Question);

				Globals.Engine.WordWrap(args.Buf.ToString(), args.Buf01);

				var k = args.Buf01.IndexOf(Environment.NewLine);

				if (k >= 0)
				{
					args.Buf01.Length = k--;

					if (k >= 0)
					{
						args.Buf01[k--] = '.';
					}

					if (k >= 0)
					{
						args.Buf01[k--] = '.';
					}

					if (k >= 0)
					{
						args.Buf01[k--] = '.';
					}
				}

				Globals.Out.Write("{0}{1}", Environment.NewLine, args.Buf01);
			}
		}

		protected virtual void ListActive(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.Active));
			}
		}

		protected virtual void ListQuestion(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail)
			{
				args.Buf.Clear();

				if (args.ResolveEffects)
				{
					var rc = Globals.Engine.ResolveUidMacros(Record.Question, args.Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					args.Buf.Append(Record.Question);
				}

				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
			}
		}

		protected virtual void ListNumAnswers(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.NumAnswers);
			}
		}

		protected virtual void ListAnswers(IField field, IListArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (args.FullDetail && i < Record.NumAnswers)
			{
				args.Buf.Clear();

				if (args.ResolveEffects)
				{
					var rc = Globals.Engine.ResolveUidMacros(Record.GetAnswers(i), args.Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					args.Buf.Append(Record.GetAnswers(i));
				}

				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
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

		protected virtual void InputActive(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var active = Record.Active;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(active));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Active = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputQuestion(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var question = Record.Question;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", question);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.HntQuestionLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.Question = args.Buf.Trim().ToString();

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNumAnswers(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var numAnswers = Record.NumAnswers;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", numAnswers);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.NumAnswers = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			var i = Math.Min(Record.NumAnswers, numAnswers);

			var j = Math.Max(Record.NumAnswers, numAnswers);

			while (i < j)
			{
				Record.SetAnswers(i, Record.NumAnswers > numAnswers ? "NONE" : "");

				i++;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputAnswers(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && field.UserData != null && args != null && args.Buf != null);

			var i = Convert.ToInt64(field.UserData);

			if (i < Record.NumAnswers)
			{
				var fieldDesc = args.FieldDesc;

				var answer = Record.GetAnswers(i);

				while (true)
				{
					args.Buf.SetFormat(args.EditRec ? "{0}" : "", answer);

					PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

					Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

					Globals.Out.WordWrap = false;

					var rc = Globals.In.ReadField(args.Buf, Constants.HntAnswerLen, null, '_', '\0', false, null, null, null, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Out.WordWrap = true;

					Record.SetAnswers(i, args.Buf.Trim().ToString());

					if (ValidateField(field, args.Vargs))
					{
						break;
					}

					fieldDesc = Enums.FieldDesc.Brief;
				}

				Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
			}
			else
			{
				Record.SetAnswers(i, "");
			}
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
						x.Name = "Active";
						x.PrintDesc = PrintDescActive;
						x.List = ListActive;
						x.Input = InputActive;
						x.GetPrintedName = () => "Active";
						x.GetValue = () => Record.Active;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Question";
						x.Validate = ValidateQuestion;
						x.ValidateInterdependencies = ValidateInterdependenciesQuestion;
						x.PrintDesc = PrintDescQuestion;
						x.List = ListQuestion;
						x.Input = InputQuestion;
						x.GetPrintedName = () => "Question";
						x.GetValue = () => Record.Question;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NumAnswers";
						x.Validate = ValidateNumAnswers;
						x.PrintDesc = PrintDescNumAnswers;
						x.List = ListNumAnswers;
						x.Input = InputNumAnswers;
						x.GetPrintedName = () => "Number Of Answers";
						x.GetValue = () => Record.NumAnswers;
					})
				};

				for (var i = 0; i < Record.Answers.Length; i++)
				{
					var j = i;

					Fields.Add
					(
						Globals.CreateInstance<IField>(x =>
						{
							x.Name = string.Format("Answers[{0}]", j);
							x.UserData = j;
							x.Validate = ValidateAnswers;
							x.ValidateInterdependencies = ValidateInterdependenciesAnswers;
							x.PrintDesc = PrintDescAnswers;
							x.List = ListAnswers;
							x.Input = InputAnswers;
							x.GetPrintedName = () => string.Format("Answer #{0}", j + 1);
							x.GetValue = () => Record.GetAnswers(j);
						})
					);
				}
			}

			return Fields;
		}

		#endregion

		#region Class HintHelper

		protected virtual void SetHintUidIfInvalid(bool editRec)
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetHintUid();

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

			if (string.Equals(args.ErrorField.Name, "Question", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), Record.Question);
			}
			else if (args.ErrorField.Name.StartsWith("Answers[", StringComparison.OrdinalIgnoreCase))
			{
				Debug.Assert(args.ErrorField.UserData != null);

				var i = Convert.ToInt64(args.ErrorField.UserData);

				args.Buf.SetFormat("{0}{1}", Globals.Engine.BuildPrompt(27, '.', 0, GetField("Question").GetPrintedName(), null), Record.Question);

				Globals.Engine.WordWrap(args.Buf.ToString(), args.Buf);

				var k = args.Buf.IndexOf(Environment.NewLine);

				if (k >= 0)
				{
					args.Buf.Length = k--;

					if (k >= 0)
					{
						args.Buf[k--] = '.';
					}

					if (k >= 0)
					{
						args.Buf[k--] = '.';
					}

					if (k >= 0)
					{
						args.Buf[k--] = '.';
					}
				}

				Globals.Out.Write("{0}{1}", Environment.NewLine, args.Buf);

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), Record.GetAnswers(i));
			}
		}

		#endregion

		#region Class HintHelper

		public HintHelper()
		{
			SetUidIfInvalid = SetHintUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
