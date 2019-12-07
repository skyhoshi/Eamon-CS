
// ModuleHelper.cs

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
	public class ModuleHelper : Helper<IModule>, IModuleHelper
	{
		#region Protected Methods

		#region Interface IHelper

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameVolLabel()
		{
			return "Volume Label";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameSerialNum()
		{
			return "Serial Number";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameLastMod()
		{
			return "Last Modified";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameIntroStory()
		{
			return "Intro Story";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameNumDirs()
		{
			return "Compass Directions";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameNumRooms()
		{
			return "Number Of Rooms";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameNumArtifacts()
		{
			return "Number Of Artifacts";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameNumEffects()
		{
			return "Number Of Effects";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameNumMonsters()
		{
			return "Number Of Monsters";
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual string GetPrintedNameNumHints()
		{
			return "Number Of Hints";
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

			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= Constants.ModNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateDesc()
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.ModDescLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateAuthor()
		{
			return string.IsNullOrWhiteSpace(Record.Author) == false && Record.Author.Length <= Constants.ModAuthorLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateVolLabel()
		{
			return string.IsNullOrWhiteSpace(Record.VolLabel) == false && Record.VolLabel.Length <= Constants.ModVolLabelLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateSerialNum()
		{
			return string.IsNullOrWhiteSpace(Record.SerialNum) == false && Record.SerialNum.Length <= Constants.ModSerialNumLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateLastMod()
		{
			return Record.LastMod != null && Record.LastMod <= DateTime.Now;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateIntroStory()
		{
			return Record.IntroStory >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateNumDirs()
		{
			return Record.NumDirs == 6 || Record.NumDirs == 10;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateNumRooms()
		{
			return Record.NumRooms >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateNumArtifacts()
		{
			return Record.NumArtifacts >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateNumEffects()
		{
			return Record.NumEffects >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateNumMonsters()
		{
			return Record.NumMonsters >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateNumHints()
		{
			return Record.NumHints >= 0;
		}

		#endregion

		#region ValidateInterdependencies Methods

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateInterdependenciesDesc()
		{
			var result = true;

			long invalidUid = 0;

			var rc = Globals.Engine.ResolveUidMacros(Record.Desc, Buf, false, false, ref invalidUid);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (invalidUid > 0)
			{
				result = false;

				Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Desc"), "effect", invalidUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IEffect);

				NewRecordUid = invalidUid;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateInterdependenciesIntroStory()
		{
			var result = true;

			if (Record.IntroStory > 0)
			{
				var effectUid = Record.IntroStory;

				var effect = Globals.EDB[effectUid];

				if (effect == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("IntroStory"), "effect", effectUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IEffect);

					NewRecordUid = effectUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		protected virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the adventure.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescDesc()
		{
			var fullDesc = "Enter a detailed description of the adventure.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescAuthor()
		{
			var fullDesc = "Enter the name(s) of the adventure's author(s).";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescVolLabel()
		{
			var fullDesc = "Enter the volume label of the adventure, typically the author(s) initials followed by a private serial number.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescSerialNum()
		{
			var fullDesc = "Enter the global serial number of the adventure, typically assigned by an Eamon CS administrator.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		protected virtual void PrintDescIntroStory()
		{
			var fullDesc = "Enter the effect uid of the introduction story for the module." + Environment.NewLine + Environment.NewLine + "You can link multiple effects together to create an extended story segment.";

			var briefDesc = "(GE 0)=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		protected virtual void PrintDescNumDirs()
		{
			var fullDesc = "Enter the number of compass directions to use for connections between rooms in the adventure." + Environment.NewLine + Environment.NewLine + "Typically, six directions are used for simpler indoor adventures while ten directions are used for more complex outdoor adventures, but this is only a rule of thumb not a requirement.";

			var briefDesc = "6=Six compass directions; 10=Ten compass directions";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
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
				Globals.Out.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, Globals.Engine.Capitalize(Record.Name));
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
		protected virtual void ListDesc()
		{
			if (FullDetail && ShowDesc)
			{
				Buf.Clear();

				if (ResolveEffects)
				{
					var rc = Globals.Engine.ResolveUidMacros(Record.Desc, Buf, true, true);

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}
				else
				{
					Buf.Append(Record.Desc);
				}

				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Desc"), null), Buf);
			}
		}

		/// <summary></summary>
		protected virtual void ListAuthor()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Author"), null), Record.Author);
			}
		}

		/// <summary></summary>
		protected virtual void ListVolLabel()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("VolLabel"), null), Record.VolLabel);
			}
		}

		/// <summary></summary>
		protected virtual void ListSerialNum()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("SerialNum"), null), Record.SerialNum);
			}
		}

		/// <summary></summary>
		protected virtual void ListLastMod()
		{
			if (FullDetail && !ExcludeROFields)
			{
				Buf.Clear();

				Buf.Append(Record.LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("LastMod"), null), Buf);
			}
		}

		/// <summary></summary>
		protected virtual void ListIntroStory()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg && Record.IntroStory > 0)
				{
					Buf.Clear();

					var effect = Globals.EDB[Record.IntroStory];

					if (effect != null)
					{
						Buf.Append(effect.Desc);

						if (Buf.Length > 40)
						{
							Buf.Length = 40;
						}

						if (Buf.Length == 40)
						{
							Buf[39] = '.';

							Buf[38] = '.';

							Buf[37] = '.';
						}
					}

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("IntroStory"), null),
						Globals.Engine.BuildValue(51, ' ', 8, Record.IntroStory, null, effect != null ? Buf.ToString() : Globals.Engine.UnknownName));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("IntroStory"), null), Record.IntroStory);
				}
			}
		}

		/// <summary></summary>
		protected virtual void ListNumDirs()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("NumDirs"), null), Record.NumDirs);
			}
		}

		/// <summary></summary>
		protected virtual void ListNumRooms()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("NumRooms"), null), Record.NumRooms);
			}
		}

		/// <summary></summary>
		protected virtual void ListNumArtifacts()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("NumArtifacts"), null), Record.NumArtifacts);
			}
		}

		/// <summary></summary>
		protected virtual void ListNumEffects()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("NumEffects"), null), Record.NumEffects);
			}
		}

		/// <summary></summary>
		protected virtual void ListNumMonsters()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("NumMonsters"), null), Record.NumMonsters);
			}
		}

		/// <summary></summary>
		protected virtual void ListNumHints()
		{
			if (FullDetail && !ExcludeROFields)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("NumHints"), null), Record.NumHints);
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

				var rc = Globals.In.ReadField(Buf, Constants.ModNameLen, null, '_', '\0', false, null, null, null, null);

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
		protected virtual void InputDesc()
		{
			var fieldDesc = FieldDesc;

			var desc = Record.Desc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", desc);

				PrintFieldDesc("Desc", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Desc"), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, Constants.ModDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.Desc = Buf.Trim().ToString();

				if (ValidateField("Desc"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputAuthor()
		{
			var fieldDesc = FieldDesc;

			var author = Record.Author;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", author);

				PrintFieldDesc("Author", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Author"), null));

				var rc = Globals.In.ReadField(Buf, Constants.ModAuthorLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Author = Buf.Trim().ToString();

				if (ValidateField("Author"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputVolLabel()
		{
			var fieldDesc = FieldDesc;

			var volLabel = Record.VolLabel;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", volLabel);

				PrintFieldDesc("VolLabel", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("VolLabel"), null));

				var rc = Globals.In.ReadField(Buf, Constants.ModVolLabelLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.VolLabel = Buf.Trim().ToString();

				if (ValidateField("VolLabel"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputSerialNum()
		{
			var fieldDesc = FieldDesc;

			var serialNum = Record.SerialNum;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", serialNum);

				PrintFieldDesc("SerialNum", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("SerialNum"), "000"));

				var rc = Globals.In.ReadField(Buf, Constants.ModSerialNumLen, null, '_', '\0', true, "000", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.SerialNum = Buf.Trim().ToString();

				if (ValidateField("SerialNum"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputLastMod()
		{
			if (!EditRec)
			{
				Record.LastMod = DateTime.Now;
			}

			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("LastMod"), null), Record.LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputIntroStory()
		{
			var fieldDesc = FieldDesc;

			var introStory = Record.IntroStory;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", introStory);

				PrintFieldDesc("IntroStory", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("IntroStory"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IntroStory = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("IntroStory"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputNumDirs()
		{
			var fieldDesc = FieldDesc;

			var numDirs = Record.NumDirs;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", numDirs);

				PrintFieldDesc("NumDirs", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("NumDirs"), "6"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "6", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.NumDirs = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("NumDirs"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputNumRooms()
		{
			if (!EditRec)
			{
				Record.NumRooms = Globals.Database.GetRoomsCount();
			}

			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("NumRooms"), null), Record.NumRooms);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputNumArtifacts()
		{
			if (!EditRec)
			{
				Record.NumArtifacts = Globals.Database.GetArtifactsCount();
			}

			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("NumArtifacts"), null), Record.NumArtifacts);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputNumEffects()
		{
			if (!EditRec)
			{
				Record.NumEffects = Globals.Database.GetEffectsCount();
			}

			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("NumEffects"), null), Record.NumEffects);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputNumMonsters()
		{
			if (!EditRec)
			{
				Record.NumMonsters = Globals.Database.GetMonstersCount();
			}

			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("NumMonsters"), null), Record.NumMonsters);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		/// <summary></summary>
		protected virtual void InputNumHints()
		{
			if (!EditRec)
			{
				Record.NumHints = Globals.Database.GetHintsCount();
			}

			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("NumHints"), null), Record.NumHints);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class ModuleHelper

		protected override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetModuleUid();

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

		public override void ListErrorField()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(ErrorFieldName));

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Name"), null), Record.Name);

			if (string.Equals(ErrorFieldName, "Desc", StringComparison.OrdinalIgnoreCase) || ShowDesc)
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Desc"), null), Record.Desc);
			}

			if (string.Equals(ErrorFieldName, "IntroStory", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("IntroStory"), null), Record.IntroStory);
			}
		}

		#endregion

		#region Class ModuleHelper

		public ModuleHelper()
		{
			FieldNames = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Name",
				"Desc",
				"Author",
				"VolLabel",
				"SerialNum",
				"LastMod",
				"IntroStory",
				"NumDirs",
				"NumRooms",
				"NumArtifacts",
				"NumEffects",
				"NumMonsters",
				"NumHints",
			};
		}

		#endregion

		#endregion
	}
}
