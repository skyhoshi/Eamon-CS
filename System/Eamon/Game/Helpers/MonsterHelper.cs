
// MonsterHelper.cs

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
	[ClassMappings(typeof(IHelper<IMonster>))]
	public class MonsterHelper : Helper<IMonster>
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
			var result = !string.IsNullOrWhiteSpace(Record.Name);

			if (result && Record.Name.Length > Constants.MonNameLen)
			{
				for (var i = Constants.MonNameLen; i < Record.Name.Length; i++)
				{
					if (Record.Name[i] != '#')
					{
						result = false;

						break;
					}
				}
			}

			return result;
		}

		protected virtual bool ValidateStateDesc(IField field, IValidateArgs args)
		{
			return Record.StateDesc != null && Record.StateDesc.Length <= Constants.MonStateDescLen;
		}

		protected virtual bool ValidateDesc(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.MonDescLen;
		}

		protected virtual bool ValidatePluralType(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidPluralType(Record.PluralType);
		}

		protected virtual bool ValidateArticleType(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.ArticleType), Record.ArticleType);
		}

		protected virtual bool ValidateHardiness(IField field, IValidateArgs args)
		{
			return Record.Hardiness >= 0;           // 0=Must be calculated
		}

		protected virtual bool ValidateAgility(IField field, IValidateArgs args)
		{
			return Record.Agility >= 0;          // 0=Must be calculated
		}

		protected virtual bool ValidateGroupCount(IField field, IValidateArgs args)
		{
			return Record.GroupCount >= 0;          // 0=Must be calculated
		}

		protected virtual bool ValidateCourage(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidMonsterCourage(Record.Courage);
		}

		protected virtual bool ValidateCombatCode(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.CombatCode), Record.CombatCode);
		}

		protected virtual bool ValidateArmor(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidMonsterArmor(Record.Armor);
		}

		protected virtual bool ValidateNwDice(IField field, IValidateArgs args)
		{
			return Record.NwDice >= 0;
		}

		protected virtual bool ValidateNwSides(IField field, IValidateArgs args)
		{
			return Record.NwSides >= 0;
		}

		protected virtual bool ValidateFriendliness(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidMonsterFriendliness(Record.Friendliness);
		}

		protected virtual bool ValidateGender(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.Gender), Record.Gender);
		}

		protected virtual bool ValidateOrigGroupCount(IField field, IValidateArgs args)
		{
			return Record.OrigGroupCount >= Record.GroupCount;
		}

		protected virtual bool ValidateOrigFriendliness(IField field, IValidateArgs args)
		{
			if (Record.OrigFriendliness == 0)
			{
				Record.OrigFriendliness = (Enums.Friendliness)((long)Record.Friendliness >= 100 && (long)Record.Friendliness <= 200 ? (long)Record.Friendliness :
					Record.Friendliness == Enums.Friendliness.Friend ? 200 :
					Record.Friendliness == Enums.Friendliness.Neutral ? 150 :
					100);
			}

			return Globals.Engine.IsValidMonsterFriendliness(Record.OrigFriendliness);
		}

		protected virtual bool ValidateDmgTaken(IField field, IValidateArgs args)
		{
			return Record.DmgTaken >= 0;
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

		protected virtual bool ValidateInterdependenciesPluralType(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			var effectUid = Globals.Engine.GetPluralTypeEffectUid(Record.PluralType);

			if (effectUid > 0)
			{
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

		protected virtual bool ValidateInterdependenciesLocation(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			var roomUid = Record.GetInRoomUid();

			if (roomUid > 0)
			{
				var room = Globals.RDB[roomUid];

				if (room == null)
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "room", roomUid, "which doesn't exist");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IRoom);

					args.NewRecordUid = roomUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesWeapon(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			var artUid = Record.GetCarryingWeaponUid();

			if (artUid > 0)
			{
				var artifact = Globals.ADB[artUid];

				if (artifact == null)
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which doesn't exist");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IArtifact);

					args.NewRecordUid = artUid;

					goto Cleanup;
				}
				else if (!artifact.IsReadyableByMonster(Record))
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which should be a readyable weapon, but isn't");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IArtifact);

					args.EditRecord = artifact;

					goto Cleanup;
				}
				else if (!artifact.IsCarriedByMonster(Record) && !artifact.IsWornByMonster(Record))
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which should be carried/worn by this monster, but isn't");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IArtifact);

					args.EditRecord = artifact;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesDeadBody(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			var artUid = Record.GetDeadBodyUid();

			if (artUid > 0)
			{
				var artifact = Globals.ADB[artUid];

				if (artifact == null)
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which doesn't exist");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IArtifact);

					args.NewRecordUid = artUid;

					goto Cleanup;
				}
				else if (artifact.Location > 0 && Record.Location > 0)
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which should have a location compatible with this monster's, but doesn't");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IArtifact);

					args.EditRecord = artifact;

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
			var fullDesc = "Enter the name of the monster." + Environment.NewLine + Environment.NewLine + "Monster names should always be in singular form and capitalized when appropriate.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescStateDesc(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the state description of the monster (will typically be empty).";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescDesc(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter a detailed description of the monster.";

			Globals.Engine.AppendFieldDesc(args, fullDesc, null);
		}

		protected virtual void PrintDescSeen(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the Seen status of the monster.";

			var briefDesc = "0=Not seen; 1=Seen";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescIsListed(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the Is Listed status of the monster." + Environment.NewLine + Environment.NewLine + "If true, the monster will be included in any listing (room, inventory, etc); if false, it will not.";

			var briefDesc = "0=Not listed; 1=Listed";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescPluralType(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the plural type of the monster.";

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'; (1000 + N)=Use effect uid N as plural name";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescArticleType(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the article type of the monster.";

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescHardiness(IField field, IPrintDescArgs args)
		{
			Debug.Assert(args != null && args.Buf != null);

			var fullDesc = "Enter the Hardiness of the monster.";

			var briefDesc = "2-8=Weak monster; 9-15=Medium monster; 16-30=Tough monster; 31-60=Exceptional monster";

			if (args.FieldDesc == Enums.FieldDesc.Full)
			{
				args.Buf.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}", Environment.NewLine, Constants.ToughDesc, fullDesc, briefDesc);
			}
			else if (args.FieldDesc == Enums.FieldDesc.Brief)
			{
				args.Buf.AppendFormat("{0}{1}{0}", Environment.NewLine, briefDesc);
			}
		}

		protected virtual void PrintDescAgility(IField field, IPrintDescArgs args)
		{
			Debug.Assert(args != null && args.Buf != null);

			var fullDesc = "Enter the Agility of the monster.";

			var briefDesc = "5-9=Weak monster; 10-16=Medium monster; 17-24=Tough monster; 25-30=Exceptional monster";

			if (args.FieldDesc == Enums.FieldDesc.Full)
			{
				if (args.EditRec && args.EditField)
				{
					args.Buf.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}", Environment.NewLine, Constants.ToughDesc, fullDesc, briefDesc);
				}
				else
				{
					args.Buf.AppendFormat("{0}{1}{0}{0}{2}{0}", Environment.NewLine, fullDesc, briefDesc);
				}
			}
			else if (args.FieldDesc == Enums.FieldDesc.Brief)
			{
				args.Buf.AppendFormat("{0}{1}{0}", Environment.NewLine, briefDesc);
			}
		}

		protected virtual void PrintDescGroupCount(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the number of members in the monster's group." + (Globals.IsRulesetVersion(5) ? Environment.NewLine + Environment.NewLine + "For classic Eamon games this value should always be 1." : "");

			var briefDesc = "1=Single monster; (GT 1)=Multiple monsters";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescCourage(IField field, IPrintDescArgs args)
		{
			Debug.Assert(args != null && args.Buf != null);

			var fullDesc = "Enter the courage of the monster." + (Globals.IsRulesetVersion(5) ? Environment.NewLine + Environment.NewLine + "For classic Eamon games this value should always be between 1 and 100, inclusive." : "");

			var briefDesc = "80-90=Weak monster; 95-100=Medium monster; 200=Tough/Exceptional monster";

			if (args.FieldDesc == Enums.FieldDesc.Full)
			{
				if (args.EditRec && args.EditField)
				{
					args.Buf.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}{0}{4}{0}", Environment.NewLine, Constants.ToughDesc, Constants.CourageDesc, fullDesc, briefDesc);
				}
				else
				{
					args.Buf.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}", Environment.NewLine, Constants.CourageDesc, fullDesc, briefDesc);
				}
			}
			else if (args.FieldDesc == Enums.FieldDesc.Brief)
			{
				args.Buf.AppendFormat("{0}{1}{0}", Environment.NewLine, briefDesc);
			}
		}

		protected virtual void PrintDescLocation(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the location of the monster.";

			var briefDesc = "(LE 0)=Limbo; (GT 0)=Room uid";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescCombatCode(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the combat code of the monster.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var combatCodeValues = EnumUtil.GetValues<Enums.CombatCode>();

			for (var j = 0; j < combatCodeValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)combatCodeValues[j], Globals.Engine.GetCombatCodeDescs(combatCodeValues[j]));
			}

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescArmor(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the armor of the monster.";

			var briefDesc = "0=Weak monster; 1=Medium monster; 2-3=Tough monster; 3-7+=Exceptional monster";

			var briefDesc01 = new StringBuilder(Constants.BufSize);

			var armorValues = EnumUtil.GetValues<Enums.Armor>(a => ((long)a) % 2 == 0);

			for (var j = 0; j < armorValues.Count; j++)
			{
				var armor = Globals.Engine.GetArmors(armorValues[j]);

				Debug.Assert(armor != null);

				briefDesc01.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", ((long)armorValues[j]) / 2, armor.Name);
			}

			briefDesc01.AppendFormat("{0}{0}{1}", Environment.NewLine, briefDesc);

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc01.ToString());
		}

		protected virtual void PrintDescWeapon(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the weapon of the monster.";

			var briefDesc = "(LT 0)=Weaponless; 0=Natural weapons; (GT 0)=Weapon artifact uid";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescNwDice(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the monster's natural weapon hit dice.";

			var briefDesc = "(GE 0)=Valid value";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescNwSides(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the monster's natural weapon hit dice sides.";

			var briefDesc = "(GE 0)=Valid value";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescDeadBody(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the dead body of the monster.";

			var briefDesc = "0=No dead body used; (GT 0)=Dead body artifact uid";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescFriendliness(IField field, IPrintDescArgs args)
		{
			int j;

			var fullDesc = "Enter the friendliness of the monster." + (Globals.IsRulesetVersion(5) ? Environment.NewLine + Environment.NewLine + "For classic Eamon games this value should always be between 100 and 200, inclusive." : "");

			var briefDesc = new StringBuilder(Constants.BufSize);

			var friendlinessValues = EnumUtil.GetValues<Enums.Friendliness>();

			for (j = 0; j < friendlinessValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)friendlinessValues[j], Globals.Engine.EvalFriendliness(friendlinessValues[j], "Enemy", "Neutral", "Friend"));
			}

			briefDesc.AppendFormat("{0}(100 + N)=N% chance of being friend", j != 0 ? "; " : "");

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescGender(IField field, IPrintDescArgs args)
		{
			var fullDesc = "Enter the gender of the monster.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var genderValues = EnumUtil.GetValues<Enums.Gender>();

			for (var j = 0; j < genderValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)genderValues[j], Globals.Engine.EvalGender(genderValues[j], "Male", "Female", "Neutral"));
			}

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc.ToString());
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

		protected virtual void ListStateDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (!string.IsNullOrWhiteSpace(Record.StateDesc))
				{
					args.Buf.Clear();

					args.Buf.Append(Record.StateDesc);

					Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.StateDesc);
				}
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

		protected virtual void ListSeen(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.Seen));
			}
		}

		protected virtual void ListIsListed(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Record.IsListed));
			}
		}

		protected virtual void ListPluralType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null && args.Buf01 != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					args.Buf.Clear();

					args.Buf01.Clear();

					var effectUid = Globals.Engine.GetPluralTypeEffectUid(Record.PluralType);

					var effect = Globals.EDB[effectUid];

					if (effect != null)
					{
						args.Buf01.Append(effect.Desc.Length > Constants.MonNameLen - 6 ? effect.Desc.Substring(0, Constants.MonNameLen - 9) + "..." : effect.Desc);

						args.Buf.AppendFormat("Use '{0}'", args.Buf01.ToString());
					}
					else
					{
						args.Buf.AppendFormat("Use effect uid {0}", effectUid);
					}

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.PluralType, null,
						Record.PluralType == Enums.PluralType.None ? "No change" :
						Record.PluralType == Enums.PluralType.S ? "Use 's'" :
						Record.PluralType == Enums.PluralType.Es ? "Use 'es'" :
						Record.PluralType == Enums.PluralType.YIes ? "Use 'y' to 'ies'" :
						args.Buf.ToString()));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.PluralType);
				}
			}
		}

		protected virtual void ListArticleType(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.ArticleType, null,
						Record.ArticleType == Enums.ArticleType.None ? "No article" :
						Record.ArticleType == Enums.ArticleType.A ? "Use 'a'" :
						Record.ArticleType == Enums.ArticleType.An ? "Use 'an'" :
						Record.ArticleType == Enums.ArticleType.Some ? "Use 'some'" :
						"Use 'the'"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.ArticleType);
				}
			}
		}

		protected virtual void ListHardiness(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Hardiness);
			}
		}

		protected virtual void ListAgility(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Agility);
			}
		}

		protected virtual void ListGroupCount(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.GroupCount);
			}
		}

		protected virtual void ListCourage(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Courage);
			}
		}

		protected virtual void ListLocation(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg && Record.Location > 0)
				{
					var room = Globals.RDB[Record.Location];

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, Record.Location, null, room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Location);
				}
			}
		}

		protected virtual void ListCombatCode(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.CombatCode, null, Globals.Engine.GetCombatCodeDescs(Record.CombatCode)));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.CombatCode);
				}
			}
		}

		protected virtual void ListArmor(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					var armor = Globals.Engine.GetArmors((Enums.Armor)(Record.Armor * 2));

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, Record.Armor, null, armor != null ? armor.Name : "Magic/Exotic"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Armor);
				}
			}
		}

		protected virtual void ListWeapon(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					if (Record.Weapon > 0)
					{
						var artifact = Globals.ADB[Record.Weapon];

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, Record.Weapon, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, Record.Weapon, null, Record.Weapon == 0 ? "Natural weapons" : "Weaponless"));
					}
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Weapon);
				}
			}
		}

		protected virtual void ListNwDice(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.NwDice);
			}
		}

		protected virtual void ListNwSides(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.NwSides);
			}
		}

		protected virtual void ListDeadBody(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					if (Record.DeadBody > 0)
					{
						var artifact = Globals.ADB[Record.DeadBody];

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, Record.DeadBody, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, Record.DeadBody, null, "No dead body"));
					}
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.DeadBody);
				}
			}
		}

		protected virtual void ListFriendliness(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					if (Globals.Engine.IsValidMonsterFriendlinessPct(Record.Friendliness))
					{
						args.Buf.SetFormat("{0}% Chance", Globals.Engine.GetMonsterFriendlinessPct(Record.Friendliness));

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, (long)Record.Friendliness, null, args.Buf.ToString()));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, (long)Record.Friendliness, null, Record.EvalFriendliness("Enemy", "Neutral", "Friend")));
					}
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.Friendliness);
				}
			}
		}

		protected virtual void ListGender(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (args.LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.Gender, null, Record.EvalGender("Male", "Female", "Neutral")));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Record.Gender);
				}
			}
		}

		protected virtual void ListField1(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Field1);
			}
		}

		protected virtual void ListField2(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Record.Field2);
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

				var rc = Globals.In.ReadField(args.Buf, Constants.MonNameLen, null, '_', '\0', false, null, null, null, null);

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

		protected virtual void InputStateDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var stateDesc = Record.StateDesc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", stateDesc);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.MonStateDescLen, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.StateDesc = args.Buf.Trim().ToString();

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

				var rc = Globals.In.ReadField(args.Buf, Constants.MonDescLen, null, '_', '\0', false, null, null, null, null);

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

		protected virtual void InputSeen(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var seen = Record.Seen;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(seen));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Seen = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputIsListed(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var isListed = Record.IsListed;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isListed));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IsListed = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputPluralType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var pluralType = Record.PluralType;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)pluralType);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.PluralType = (Enums.PluralType)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputArticleType(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var articleType = Record.ArticleType;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)articleType);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArticleType = (Enums.ArticleType)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputHardiness(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var hardiness = Record.Hardiness;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", hardiness);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "16"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "16", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Hardiness = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Record.DmgTaken = 0;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputAgility(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var agility = Record.Agility;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", agility);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "15"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "15", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Agility = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputGroupCount(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var groupCount = Record.GroupCount;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", groupCount);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.GroupCount = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Record.OrigGroupCount = Record.GroupCount;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputCourage(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var courage = Record.Courage;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", courage);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "100"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "100", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Courage = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputLocation(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var location = Record.Location;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", location);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Location = Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputCombatCode(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var combatCode = Record.CombatCode;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)combatCode);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.CombatCode = (Enums.CombatCode)Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputArmor(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var armor = Record.Armor;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", armor);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Armor = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputWeapon(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var weapon = Record.Weapon;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", weapon);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Weapon = Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNwDice(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var nwDice = Record.NwDice;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", nwDice);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.NwDice = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputNwSides(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var nwSides = Record.NwSides;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", nwSides);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "4"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "4", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.NwSides = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputDeadBody(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var deadBody = Record.DeadBody;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", deadBody);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.DeadBody = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputFriendliness(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var friendliness = Record.Friendliness;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)friendliness);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "3"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "3", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Friendliness = (Enums.Friendliness)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Record.OrigFriendliness = (Enums.Friendliness)((long)Record.Friendliness >= 100 && (long)Record.Friendliness <= 200 ? (long)Record.Friendliness :
				Record.Friendliness == Enums.Friendliness.Friend ? 200 :
				Record.Friendliness == Enums.Friendliness.Neutral ? 150 :
				100);

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputGender(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var gender = Record.Gender;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)gender);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0To2, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Gender = (Enums.Gender)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputField1(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var field1 = Record.Field1;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", field1);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Field1 = Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputField2(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var field2 = Record.Field2;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", field2);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Field2 = Convert.ToInt64(args.Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField(field, args.Vargs))
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
					GetNameField(),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "StateDesc";
						x.Validate = ValidateStateDesc;
						x.PrintDesc = PrintDescStateDesc;
						x.List = ListStateDesc;
						x.Input = InputStateDesc;
						x.GetPrintedName = () => "State Description";
						x.GetValue = () => Record.StateDesc;
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
						x.Name = "Seen";
						x.PrintDesc = PrintDescSeen;
						x.List = ListSeen;
						x.Input = InputSeen;
						x.GetPrintedName = () => "Seen";
						x.GetValue = () => Record.Seen;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsListed";
						x.PrintDesc = PrintDescIsListed;
						x.List = ListIsListed;
						x.Input = InputIsListed;
						x.GetPrintedName = () => "Is Listed";
						x.GetValue = () => Record.IsListed;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "PluralType";
						x.Validate = ValidatePluralType;
						x.ValidateInterdependencies = ValidateInterdependenciesPluralType;
						x.PrintDesc = PrintDescPluralType;
						x.List = ListPluralType;
						x.Input = InputPluralType;
						x.GetPrintedName = () => "Plural Type";
						x.GetValue = () => Record.PluralType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArticleType";
						x.Validate = ValidateArticleType;
						x.PrintDesc = PrintDescArticleType;
						x.List = ListArticleType;
						x.Input = InputArticleType;
						x.GetPrintedName = () => "Article Type";
						x.GetValue = () => Record.ArticleType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Hardiness";
						x.Validate = ValidateHardiness;
						x.PrintDesc = PrintDescHardiness;
						x.List = ListHardiness;
						x.Input = InputHardiness;
						x.GetPrintedName = () => "Hardiness";
						x.GetValue = () => Record.Hardiness;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Agility";
						x.Validate = ValidateAgility;
						x.PrintDesc = PrintDescAgility;
						x.List = ListAgility;
						x.Input = InputAgility;
						x.GetPrintedName = () => "Agility";
						x.GetValue = () => Record.Agility;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "GroupCount";
						x.Validate = ValidateGroupCount;
						x.PrintDesc = PrintDescGroupCount;
						x.List = ListGroupCount;
						x.Input = InputGroupCount;
						x.GetPrintedName = () => "Group Count";
						x.GetValue = () => Record.GroupCount;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Courage";
						x.Validate = ValidateCourage;
						x.PrintDesc = PrintDescCourage;
						x.List = ListCourage;
						x.Input = InputCourage;
						x.GetPrintedName = () => "Courage";
						x.GetValue = () => Record.Courage;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Location";
						x.ValidateInterdependencies = ValidateInterdependenciesLocation;
						x.PrintDesc = PrintDescLocation;
						x.List = ListLocation;
						x.Input = InputLocation;
						x.GetPrintedName = () => "Location";
						x.GetValue = () => Record.Location;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CombatCode";
						x.Validate = ValidateCombatCode;
						x.PrintDesc = PrintDescCombatCode;
						x.List = ListCombatCode;
						x.Input = InputCombatCode;
						x.GetPrintedName = () => "Combat Code";
						x.GetValue = () => Record.CombatCode;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Armor";
						x.Validate = ValidateArmor;
						x.PrintDesc = PrintDescArmor;
						x.List = ListArmor;
						x.Input = InputArmor;
						x.GetPrintedName = () => "Armor";
						x.GetValue = () => Record.Armor;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Weapon";
						x.ValidateInterdependencies = ValidateInterdependenciesWeapon;
						x.PrintDesc = PrintDescWeapon;
						x.List = ListWeapon;
						x.Input = InputWeapon;
						x.GetPrintedName = () => "Weapon Uid";
						x.GetValue = () => Record.Weapon;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NwDice";
						x.Validate = ValidateNwDice;
						x.PrintDesc = PrintDescNwDice;
						x.List = ListNwDice;
						x.Input = InputNwDice;
						x.GetPrintedName = () => "Natural Wpn Dice";
						x.GetValue = () => Record.NwDice;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NwSides";
						x.Validate = ValidateNwSides;
						x.PrintDesc = PrintDescNwSides;
						x.List = ListNwSides;
						x.Input = InputNwSides;
						x.GetPrintedName = () => "Natural Wpn Sides";
						x.GetValue = () => Record.NwSides;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DeadBody";
						x.ValidateInterdependencies = ValidateInterdependenciesDeadBody;
						x.PrintDesc = PrintDescDeadBody;
						x.List = ListDeadBody;
						x.Input = InputDeadBody;
						x.GetPrintedName = () => "Dead Body Uid";
						x.GetValue = () => Record.DeadBody;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Friendliness";
						x.Validate = ValidateFriendliness;
						x.PrintDesc = PrintDescFriendliness;
						x.List = ListFriendliness;
						x.Input = InputFriendliness;
						x.GetPrintedName = () => "Friendliness";
						x.GetValue = () => Record.Friendliness;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Gender";
						x.Validate = ValidateGender;
						x.PrintDesc = PrintDescGender;
						x.List = ListGender;
						x.Input = InputGender;
						x.GetPrintedName = () => "Gender";
						x.GetValue = () => Record.Gender;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "InitGroupCount";
						x.GetValue = () => Record.InitGroupCount;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "OrigGroupCount";
						x.Validate = ValidateOrigGroupCount;
						x.GetValue = () => Record.OrigGroupCount;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "OrigFriendliness";
						x.Validate = ValidateOrigFriendliness;
						x.GetValue = () => Record.OrigFriendliness;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DmgTaken";
						x.Validate = ValidateDmgTaken;
						x.GetValue = () => Record.DmgTaken;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Field1";
						x.List = ListField1;
						x.Input = InputField1;
						x.GetPrintedName = () => "Field #1";
						x.GetValue = () => Record.Field1;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Field2";
						x.List = ListField2;
						x.Input = InputField2;
						x.GetPrintedName = () => "Field #2";
						x.GetValue = () => Record.Field2;
					})
				};
			}

			return Fields;
		}

		protected override IField GetNameField()
		{
			if (NameField == null)
			{
				NameField = Globals.CreateInstance<IField>(x =>
				{
					x.Name = "Name";
					x.GetPrintedName = () => "Name";
					x.Validate = ValidateName;
					x.PrintDesc = PrintDescName;
					x.List = ListName;
					x.Input = InputName;
					x.BuildValue = null;
					x.GetValue = () => Record.Name;
				});
			}

			return NameField;
		}

		#endregion

		#region Class MonsterHelper

		protected virtual void SetMonsterUidIfInvalid(bool editRec)
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetMonsterUid();

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

			if (string.Equals(args.ErrorField.Name, "PluralType", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), (long)Record.PluralType);
			}
			else if (string.Equals(args.ErrorField.Name, "Location", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), Record.Location);
			}
			else if (string.Equals(args.ErrorField.Name, "Weapon", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), Record.Weapon);
			}
			else if (string.Equals(args.ErrorField.Name, "DeadBody", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), Record.DeadBody);
			}
		}

		#endregion

		#region Class MonsterHelper

		public MonsterHelper()
		{
			SetUidIfInvalid = SetMonsterUidIfInvalid;
		}

		#endregion

		#endregion
	}
}
