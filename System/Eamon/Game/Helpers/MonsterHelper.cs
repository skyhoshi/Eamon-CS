
// MonsterHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
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

		#region GetPrintedName Methods

		protected virtual string GetPrintedNameStateDesc()
		{
			return "State Description";
		}

		protected virtual string GetPrintedNameIsListed()
		{
			return "Is Listed";
		}

		protected virtual string GetPrintedNamePluralType()
		{
			return "Plural Type";
		}

		protected virtual string GetPrintedNameGroupCount()
		{
			return "Group Count";
		}

		protected virtual string GetPrintedNameAttackCount()
		{
			return "Attack Count";
		}

		protected virtual string GetPrintedNameCombatCode()
		{
			return "Combat Code";
		}

		protected virtual string GetPrintedNameWeapon()
		{
			return "Weapon Uid";
		}

		protected virtual string GetPrintedNameNwDice()
		{
			return "Natural Wpn Dice";
		}

		protected virtual string GetPrintedNameNwSides()
		{
			return "Natural Wpn Sides";
		}

		protected virtual string GetPrintedNameDeadBody()
		{
			return "Dead Body Uid";
		}

		protected virtual string GetPrintedNameField1()
		{
			return "Field #1";
		}

		protected virtual string GetPrintedNameField2()
		{
			return "Field #2";
		}

		#endregion

		#region GetName Methods

		// do nothing

		#endregion

		#region GetValue Methods

		// do nothing

		#endregion

		#region Validate Methods

		protected virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		protected virtual bool ValidateName()
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

		protected virtual bool ValidateStateDesc()
		{
			return Record.StateDesc != null && Record.StateDesc.Length <= Constants.MonStateDescLen;
		}

		protected virtual bool ValidateDesc()
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= Constants.MonDescLen;
		}

		protected virtual bool ValidatePluralType()
		{
			return Globals.Engine.IsValidPluralType(Record.PluralType);
		}

		protected virtual bool ValidateArticleType()
		{
			return Enum.IsDefined(typeof(Enums.ArticleType), Record.ArticleType);
		}

		protected virtual bool ValidateHardiness()
		{
			return Record.Hardiness >= 0;           // 0=Must be calculated
		}

		protected virtual bool ValidateAgility()
		{
			return Record.Agility >= 0;          // 0=Must be calculated
		}

		protected virtual bool ValidateGroupCount()
		{
			return Record.GroupCount >= 0;          // 0=Must be calculated
		}

		protected virtual bool ValidateAttackCount()
		{
			if (Record.AttackCount == 0)
			{
				Record.AttackCount = 1;
			}

			return Record.AttackCount > 0 || Record.AttackCount < -1;
		}

		protected virtual bool ValidateCourage()
		{
			return Globals.Engine.IsValidMonsterCourage(Record.Courage);
		}

		protected virtual bool ValidateCombatCode()
		{
			return Enum.IsDefined(typeof(Enums.CombatCode), Record.CombatCode);
		}

		protected virtual bool ValidateArmor()
		{
			return Globals.Engine.IsValidMonsterArmor(Record.Armor);
		}

		protected virtual bool ValidateNwDice()
		{
			return Record.NwDice >= 0;
		}

		protected virtual bool ValidateNwSides()
		{
			return Record.NwSides >= 0;
		}

		protected virtual bool ValidateFriendliness()
		{
			return Globals.Engine.IsValidMonsterFriendliness(Record.Friendliness);
		}

		protected virtual bool ValidateGender()
		{
			return Enum.IsDefined(typeof(Enums.Gender), Record.Gender);
		}

		protected virtual bool ValidateOrigGroupCount()
		{
			return Record.OrigGroupCount >= Record.GroupCount;
		}

		protected virtual bool ValidateOrigFriendliness()
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

		protected virtual bool ValidateDmgTaken()
		{
			return Record.DmgTaken >= 0;
		}

		#endregion

		#region ValidateInterdependencies Methods

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

		protected virtual bool ValidateInterdependenciesPluralType()
		{
			var result = true;

			var effectUid = Globals.Engine.GetPluralTypeEffectUid(Record.PluralType);

			if (effectUid > 0)
			{
				var effect = Globals.EDB[effectUid];

				if (effect == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("PluralType"), "effect", effectUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IEffect);

					NewRecordUid = effectUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesLocation()
		{
			var result = true;

			var roomUid = Record.GetInRoomUid();

			if (roomUid > 0)
			{
				var room = Globals.RDB[roomUid];

				if (room == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Location"), "room", roomUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IRoom);

					NewRecordUid = roomUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesWeapon()
		{
			var result = true;

			var artUid = Record.GetCarryingWeaponUid();

			if (artUid > 0)
			{
				var artifact = Globals.ADB[artUid];

				if (artifact == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Weapon"), "artifact", artUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					NewRecordUid = artUid;

					goto Cleanup;
				}
				else if (!artifact.IsReadyableByMonster(Record))
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Weapon"), "artifact", artUid, "which should be a readyable weapon, but isn't");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					EditRecord = artifact;

					goto Cleanup;
				}
				else if (!artifact.IsCarriedByMonster(Record) && !artifact.IsWornByMonster(Record))
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("Weapon"), "artifact", artUid, "which should be carried/worn by this monster, but isn't");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					EditRecord = artifact;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		protected virtual bool ValidateInterdependenciesDeadBody()
		{
			var result = true;

			var artUid = Record.GetDeadBodyUid();

			if (artUid > 0)
			{
				var artifact = Globals.ADB[artUid];

				if (artifact == null)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("DeadBody"), "artifact", artUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					NewRecordUid = artUid;

					goto Cleanup;
				}
				else if (artifact.Location > 0 && Record.Location > 0)
				{
					result = false;

					Buf.SetFormat(Constants.RecIdepErrorFmtStr, GetPrintedName("DeadBody"), "artifact", artUid, "which should have a location compatible with this monster's, but doesn't");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					EditRecord = artifact;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		#endregion

		#region PrintDesc Methods

		protected virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the monster." + Environment.NewLine + Environment.NewLine + "Monster names should always be in singular form and capitalized when appropriate.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		protected virtual void PrintDescStateDesc()
		{
			var fullDesc = "Enter the state description of the monster (will typically be empty).";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		protected virtual void PrintDescDesc()
		{
			var fullDesc = "Enter a detailed description of the monster.";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		protected virtual void PrintDescSeen()
		{
			var fullDesc = "Enter the Seen status of the monster.";

			var briefDesc = "0=Not seen; 1=Seen";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescIsListed()
		{
			var fullDesc = "Enter the Is Listed status of the monster." + Environment.NewLine + Environment.NewLine + "If true, the monster will be included in any listing (room, inventory, etc); if false, it will not.";

			var briefDesc = "0=Not listed; 1=Listed";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescPluralType()
		{
			var fullDesc = "Enter the plural type of the monster.";

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'; (1000 + N)=Use effect uid N as plural name";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescArticleType()
		{
			var fullDesc = "Enter the article type of the monster.";

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescHardiness()
		{
			var fullDesc = "Enter the Hardiness of the monster.";

			var briefDesc = "2-8=Weak monster; 9-15=Medium monster; 16-30=Tough monster; 31-60=Exceptional monster";

			if (FieldDesc == Enums.FieldDesc.Full)
			{
				Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}", Environment.NewLine, Constants.ToughDesc, fullDesc, briefDesc);
			}
			else if (FieldDesc == Enums.FieldDesc.Brief)
			{
				Buf01.AppendPrint("{0}", briefDesc);
			}
		}

		protected virtual void PrintDescAgility()
		{
			var fullDesc = "Enter the Agility of the monster.";

			var briefDesc = "5-9=Weak monster; 10-16=Medium monster; 17-24=Tough monster; 25-30=Exceptional monster";

			if (FieldDesc == Enums.FieldDesc.Full)
			{
				if (EditRec && EditField)
				{
					Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}", Environment.NewLine, Constants.ToughDesc, fullDesc, briefDesc);
				}
				else
				{
					Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}", Environment.NewLine, fullDesc, briefDesc);
				}
			}
			else if (FieldDesc == Enums.FieldDesc.Brief)
			{
				Buf01.AppendPrint("{0}", briefDesc);
			}
		}

		protected virtual void PrintDescGroupCount()
		{
			var fullDesc = "Enter the number of members in the monster's group." + (Globals.IsRulesetVersion(5) ? Environment.NewLine + Environment.NewLine + "For classic Eamon games this value should always be 1." : "");

			var briefDesc = "1=Single monster; (GT 1)=Multiple monsters";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescAttackCount()
		{
			var fullDesc = "Enter the number of attacks per round for the monster." + Environment.NewLine + Environment.NewLine + (Globals.IsRulesetVersion(5) ? "For classic Eamon games this value should always be 1." : "The monster can attack this many times per round.  For group monsters, each member has this many attacks per round.  For AttackCounts < -1, use ABS(AttackCount) as attacks per round.");

			var briefDesc = "1=Single attack, single target; (GT 1)=Multiple attacks, single target;      (LT -1)=Multiple attacks, multiple targets";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescCourage()
		{
			var fullDesc = "Enter the courage of the monster." + (Globals.IsRulesetVersion(5) ? Environment.NewLine + Environment.NewLine + "For classic Eamon games this value should always be between 1 and 100, inclusive." : "");

			var briefDesc = "80-90=Weak monster; 95-100=Medium monster; 200=Tough/Exceptional monster";

			if (FieldDesc == Enums.FieldDesc.Full)
			{
				if (EditRec && EditField)
				{
					Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}{0}{4}{0}", Environment.NewLine, Constants.ToughDesc, Constants.CourageDesc, fullDesc, briefDesc);
				}
				else
				{
					Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}", Environment.NewLine, Constants.CourageDesc, fullDesc, briefDesc);
				}
			}
			else if (FieldDesc == Enums.FieldDesc.Brief)
			{
				Buf01.AppendPrint("{0}", briefDesc);
			}
		}

		protected virtual void PrintDescLocation()
		{
			var fullDesc = "Enter the location of the monster.";

			var briefDesc = "(LE 0)=Limbo; (GT 0)=Room uid";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescCombatCode()
		{
			var fullDesc = "Enter the combat code of the monster.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var combatCodeValues = EnumUtil.GetValues<Enums.CombatCode>();

			for (var j = 0; j < combatCodeValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)combatCodeValues[j], Globals.Engine.GetCombatCodeDescs(combatCodeValues[j]));
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescArmor()
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

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc01.ToString());
		}

		protected virtual void PrintDescWeapon()
		{
			var fullDesc = "Enter the weapon of the monster.";

			var briefDesc = "(LT 0)=Weaponless; 0=Natural weapons; (GT 0)=Weapon artifact uid";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescNwDice()
		{
			var fullDesc = "Enter the monster's natural weapon hit dice.";

			var briefDesc = "(GE 0)=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescNwSides()
		{
			var fullDesc = "Enter the monster's natural weapon hit dice sides.";

			var briefDesc = "(GE 0)=Valid value";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescDeadBody()
		{
			var fullDesc = "Enter the dead body of the monster.";

			var briefDesc = "0=No dead body used; (GT 0)=Dead body artifact uid";

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		protected virtual void PrintDescFriendliness()
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

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		protected virtual void PrintDescGender()
		{
			var fullDesc = "Enter the gender of the monster.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var genderValues = EnumUtil.GetValues<Enums.Gender>();

			for (var j = 0; j < genderValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)genderValues[j], Globals.Engine.EvalGender(genderValues[j], "Male", "Female", "Neutral"));
			}

			Globals.Engine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		#endregion

		#region List Methods

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

		protected virtual void ListName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Name"), null), Record.Name);
			}
		}

		protected virtual void ListStateDesc()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (!string.IsNullOrWhiteSpace(Record.StateDesc))
				{
					Buf.Clear();

					Buf.Append(Record.StateDesc);

					Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("StateDesc"), null), Buf);
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("StateDesc"), null), Record.StateDesc);
				}
			}
		}

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

		protected virtual void ListSeen()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Seen"), null), Convert.ToInt64(Record.Seen));
			}
		}

		protected virtual void ListIsListed()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("IsListed"), null), Convert.ToInt64(Record.IsListed));
			}
		}

		protected virtual void ListPluralType()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Buf.Clear();

					Buf01.Clear();

					var effectUid = Globals.Engine.GetPluralTypeEffectUid(Record.PluralType);

					var effect = Globals.EDB[effectUid];

					if (effect != null)
					{
						Buf01.Append(effect.Desc.Length > Constants.MonNameLen - 6 ? effect.Desc.Substring(0, Constants.MonNameLen - 9) + "..." : effect.Desc);

						Buf.AppendFormat("Use '{0}'", Buf01.ToString());
					}
					else
					{
						Buf.AppendFormat("Use effect uid {0}", effectUid);
					}

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("PluralType"), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.PluralType, null,
						Record.PluralType == Enums.PluralType.None ? "No change" :
						Record.PluralType == Enums.PluralType.S ? "Use 's'" :
						Record.PluralType == Enums.PluralType.Es ? "Use 'es'" :
						Record.PluralType == Enums.PluralType.YIes ? "Use 'y' to 'ies'" :
						Buf.ToString()));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("PluralType"), null), (long)Record.PluralType);
				}
			}
		}

		protected virtual void ListArticleType()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ArticleType"), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.ArticleType, null,
						Record.ArticleType == Enums.ArticleType.None ? "No article" :
						Record.ArticleType == Enums.ArticleType.A ? "Use 'a'" :
						Record.ArticleType == Enums.ArticleType.An ? "Use 'an'" :
						Record.ArticleType == Enums.ArticleType.Some ? "Use 'some'" :
						"Use 'the'"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("ArticleType"), null), (long)Record.ArticleType);
				}
			}
		}

		protected virtual void ListHardiness()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Hardiness"), null), Record.Hardiness);
			}
		}

		protected virtual void ListAgility()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Agility"), null), Record.Agility);
			}
		}

		protected virtual void ListGroupCount()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("GroupCount"), null), Record.GroupCount);
			}
		}

		protected virtual void ListAttackCount()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("AttackCount"), null), Record.AttackCount);
			}
		}

		protected virtual void ListCourage()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Courage"), null), Record.Courage);
			}
		}

		protected virtual void ListLocation()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg && Record.Location > 0)
				{
					var room = Globals.RDB[Record.Location];

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Location"), null),
						Globals.Engine.BuildValue(51, ' ', 8, Record.Location, null, room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Location"), null), Record.Location);
				}
			}
		}

		protected virtual void ListCombatCode()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CombatCode"), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.CombatCode, null, Globals.Engine.GetCombatCodeDescs(Record.CombatCode)));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("CombatCode"), null), (long)Record.CombatCode);
				}
			}
		}

		protected virtual void ListArmor()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					var armor = Globals.Engine.GetArmors((Enums.Armor)(Record.Armor * 2));

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Armor"), null),
						Globals.Engine.BuildValue(51, ' ', 8, Record.Armor, null, armor != null ? armor.Name : "Magic/Exotic"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Armor"), null), Record.Armor);
				}
			}
		}

		protected virtual void ListWeapon()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					if (Record.Weapon > 0)
					{
						var artifact = Globals.ADB[Record.Weapon];

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Weapon"), null),
							Globals.Engine.BuildValue(51, ' ', 8, Record.Weapon, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Weapon"), null),
							Globals.Engine.BuildValue(51, ' ', 8, Record.Weapon, null, Record.Weapon == 0 ? "Natural weapons" : "Weaponless"));
					}
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Weapon"), null), Record.Weapon);
				}
			}
		}

		protected virtual void ListNwDice()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("NwDice"), null), Record.NwDice);
			}
		}

		protected virtual void ListNwSides()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("NwSides"), null), Record.NwSides);
			}
		}

		protected virtual void ListDeadBody()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					if (Record.DeadBody > 0)
					{
						var artifact = Globals.ADB[Record.DeadBody];

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("DeadBody"), null),
							Globals.Engine.BuildValue(51, ' ', 8, Record.DeadBody, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("DeadBody"), null),
							Globals.Engine.BuildValue(51, ' ', 8, Record.DeadBody, null, "No dead body"));
					}
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("DeadBody"), null), Record.DeadBody);
				}
			}
		}

		protected virtual void ListFriendliness()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					if (Globals.Engine.IsValidMonsterFriendlinessPct(Record.Friendliness))
					{
						Buf.SetFormat("{0}% Chance", Globals.Engine.GetMonsterFriendlinessPct(Record.Friendliness));

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Friendliness"), null),
							Globals.Engine.BuildValue(51, ' ', 8, (long)Record.Friendliness, null, Buf.ToString()));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Friendliness"), null),
							Globals.Engine.BuildValue(51, ' ', 8, (long)Record.Friendliness, null, Record.EvalFriendliness("Enemy", "Neutral", "Friend")));
					}
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Friendliness"), null), (long)Record.Friendliness);
				}
			}
		}

		protected virtual void ListGender()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Gender"), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Record.Gender, null, Record.EvalGender("Male", "Female", "Neutral")));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Gender"), null), (long)Record.Gender);
				}
			}
		}

		protected virtual void ListField1()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Field1"), null), Record.Field1);
			}
		}

		protected virtual void ListField2()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', listNum, GetPrintedName("Field2"), null), Record.Field2);
			}
		}

		#endregion

		#region Input Methods

		protected virtual void InputUid()
		{
			Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputName()
		{
			var fieldDesc = FieldDesc;

			var name = Record.Name;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", name);

				PrintFieldDesc("Name", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Name"), null));

				var rc = Globals.In.ReadField(Buf, Constants.MonNameLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Name = Buf.Trim().ToString();

				if (ValidateField("Name"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputStateDesc()
		{
			var fieldDesc = FieldDesc;

			var stateDesc = Record.StateDesc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", stateDesc);

				PrintFieldDesc("StateDesc", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("StateDesc"), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(Buf, Constants.MonStateDescLen, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.StateDesc = Buf.Trim().ToString();

				if (ValidateField("StateDesc"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

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

				var rc = Globals.In.ReadField(Buf, Constants.MonDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Record.Desc = Buf.Trim().ToString();

				if (ValidateField("Desc"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputSeen()
		{
			var fieldDesc = FieldDesc;

			var seen = Record.Seen;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(seen));

				PrintFieldDesc("Seen", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Seen"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Seen = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("Seen"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputIsListed()
		{
			var fieldDesc = FieldDesc;

			var isListed = Record.IsListed;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isListed));

				PrintFieldDesc("IsListed", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("IsListed"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.IsListed = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsListed"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputPluralType()
		{
			var fieldDesc = FieldDesc;

			var pluralType = Record.PluralType;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)pluralType);

				PrintFieldDesc("PluralType", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("PluralType"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.PluralType = (Enums.PluralType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("PluralType"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputArticleType()
		{
			var fieldDesc = FieldDesc;

			var articleType = Record.ArticleType;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)articleType);

				PrintFieldDesc("ArticleType", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("ArticleType"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.ArticleType = (Enums.ArticleType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArticleType"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputHardiness()
		{
			var fieldDesc = FieldDesc;

			var hardiness = Record.Hardiness;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", hardiness);

				PrintFieldDesc("Hardiness", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Hardiness"), "16"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "16", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Hardiness = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Hardiness"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Record.DmgTaken = 0;

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputAgility()
		{
			var fieldDesc = FieldDesc;

			var agility = Record.Agility;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", agility);

				PrintFieldDesc("Agility", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Agility"), "15"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "15", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Agility = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Agility"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputGroupCount()
		{
			var fieldDesc = FieldDesc;

			var groupCount = Record.GroupCount;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", groupCount);

				PrintFieldDesc("GroupCount", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("GroupCount"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.GroupCount = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("GroupCount"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Record.OrigGroupCount = Record.GroupCount;

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputAttackCount()
		{
			var fieldDesc = FieldDesc;

			var attackCount = Record.AttackCount;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", attackCount);

				PrintFieldDesc("AttackCount", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("AttackCount"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.AttackCount = Convert.ToInt64(Buf.Trim().ToString());

					if (Record.AttackCount == 0)
					{
						error = true;
					}
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("AttackCount"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputCourage()
		{
			var fieldDesc = FieldDesc;

			var courage = Record.Courage;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", courage);

				PrintFieldDesc("Courage", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Courage"), "100"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "100", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Courage = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Courage"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputLocation()
		{
			var fieldDesc = FieldDesc;

			var location = Record.Location;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", location);

				PrintFieldDesc("Location", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Location"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Location = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Location"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputCombatCode()
		{
			var fieldDesc = FieldDesc;

			var combatCode = Record.CombatCode;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)combatCode);

				PrintFieldDesc("CombatCode", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("CombatCode"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.CombatCode = (Enums.CombatCode)Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("CombatCode"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputArmor()
		{
			var fieldDesc = FieldDesc;

			var armor = Record.Armor;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", armor);

				PrintFieldDesc("Armor", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Armor"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Armor = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Armor"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputWeapon()
		{
			var fieldDesc = FieldDesc;

			var weapon = Record.Weapon;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", weapon);

				PrintFieldDesc("Weapon", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Weapon"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Weapon = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Weapon"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputNwDice()
		{
			var fieldDesc = FieldDesc;

			var nwDice = Record.NwDice;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", nwDice);

				PrintFieldDesc("NwDice", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("NwDice"), "1"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.NwDice = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("NwDice"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputNwSides()
		{
			var fieldDesc = FieldDesc;

			var nwSides = Record.NwSides;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", nwSides);

				PrintFieldDesc("NwSides", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("NwSides"), "4"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "4", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.NwSides = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("NwSides"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputDeadBody()
		{
			var fieldDesc = FieldDesc;

			var deadBody = Record.DeadBody;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", deadBody);

				PrintFieldDesc("DeadBody", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("DeadBody"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.DeadBody = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("DeadBody"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputFriendliness()
		{
			var fieldDesc = FieldDesc;

			var friendliness = Record.Friendliness;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)friendliness);

				PrintFieldDesc("Friendliness", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Friendliness"), "3"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "3", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Friendliness = (Enums.Friendliness)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Friendliness"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Record.OrigFriendliness = (Enums.Friendliness)((long)Record.Friendliness >= 100 && (long)Record.Friendliness <= 200 ? (long)Record.Friendliness :
				Record.Friendliness == Enums.Friendliness.Friend ? 200 :
				Record.Friendliness == Enums.Friendliness.Neutral ? 150 :
				100);

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputGender()
		{
			var fieldDesc = FieldDesc;

			var gender = Record.Gender;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)gender);

				PrintFieldDesc("Gender", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Gender"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0To2, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Record.Gender = (Enums.Gender)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Gender"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputField1()
		{
			var fieldDesc = FieldDesc;

			var field1 = Record.Field1;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", field1);

				PrintFieldDesc("Field1", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Field1"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Field1 = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Field1"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		protected virtual void InputField2()
		{
			var fieldDesc = FieldDesc;

			var field2 = Record.Field2;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", field2);

				PrintFieldDesc("Field2", EditRec, EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, GetPrintedName("Field2"), "0"));

				var rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharPlusMinusDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Field2 = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Field2"))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class MonsterHelper

		protected override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = Globals.Database.GetMonsterUid();

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

		public override void ListErrorField()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(ErrorFieldName));

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Uid"), null), Record.Uid);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Name"), null), Record.Name);

			if (string.Equals(ErrorFieldName, "Desc", StringComparison.OrdinalIgnoreCase) || ShowDesc)
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Desc"), null), Record.Desc);
			}

			if (string.Equals(ErrorFieldName, "PluralType", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("PluralType"), null), (long)Record.PluralType);
			}
			else if (string.Equals(ErrorFieldName, "Location", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Location"), null), Record.Location);
			}
			else if (string.Equals(ErrorFieldName, "Weapon", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("Weapon"), null), Record.Weapon);
			}
			else if (string.Equals(ErrorFieldName, "DeadBody", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.Print("{0}{1}", Globals.Engine.BuildPrompt(27, '.', 0, GetPrintedName("DeadBody"), null), Record.DeadBody);
			}
		}

		#endregion

		#region Class MonsterHelper

		public MonsterHelper()
		{
			FieldNames = new List<string>()
			{
				"Uid",
				"IsUidRecycled",
				"Name",
				"StateDesc",
				"Desc",
				"Seen",
				"IsListed",
				"PluralType",
				"ArticleType",
				"Hardiness",
				"Agility",
				"GroupCount",
				"AttackCount",
				"Courage",
				"Location",
				"CombatCode",
				"Armor",
				"Weapon",
				"NwDice",
				"NwSides",
				"DeadBody",
				"Friendliness",
				"Gender",
				"InitGroupCount",
				"OrigGroupCount",
				"OrigFriendliness",
				"DmgTaken",
				"Field1",
				"Field2",
			};
		}

		#endregion

		#endregion
	}
}
