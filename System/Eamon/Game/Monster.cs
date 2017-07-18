
// Monster.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using Eamon.Game.DataEntry;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Monster : Editable, IMonster
	{
		#region Protected Properties

		[ExcludeFromSerialization]
		protected virtual IField NameField { get; set; }

		#endregion

		#region Public Properties

		#region Interface IHaveUid

		public virtual long Uid { get; set; }

		public virtual bool IsUidRecycled { get; set; }

		#endregion

		#region Interface IHaveListedName

		public virtual string Name { get; set; }

		public virtual string[] Synonyms { get; set; }

		public virtual bool Seen { get; set; }

		public virtual Enums.ArticleType ArticleType { get; set; }

		#endregion

		#region Interface IMonster

		public virtual string StateDesc { get; set; }

		public virtual string Desc { get; set; }

		public virtual bool IsListed { get; set; }

		public virtual Enums.PluralType PluralType { get; set; }

		public virtual long Hardiness { get; set; }

		public virtual long Agility { get; set; }

		public virtual long GroupCount { get; set; }

		public virtual long Courage { get; set; }

		public virtual long Location { get; set; }

		public virtual Enums.CombatCode CombatCode { get; set; }

		public virtual long Armor { get; set; }

		public virtual long Weapon { get; set; }

		public virtual long NwDice { get; set; }

		public virtual long NwSides { get; set; }

		public virtual long DeadBody { get; set; }

		public virtual Enums.Friendliness Friendliness { get; set; }

		public virtual Enums.Gender Gender { get; set; }

		public virtual long InitGroupCount { get; set; }

		public virtual long OrigGroupCount { get; set; }

		public virtual long DmgTaken { get; set; }

		public virtual long Field1 { get; set; }

		public virtual long Field2 { get; set; }

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
				Globals.Database.FreeMonsterUid(Uid);

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
			var result = !string.IsNullOrWhiteSpace(Name);

			if (result && Name.Length > Constants.MonNameLen)
			{
				for (var i = Constants.MonNameLen; i < Name.Length; i++)
				{
					if (Name[i] != '#')
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
			return StateDesc != null && StateDesc.Length <= Constants.MonStateDescLen;
		}

		protected virtual bool ValidateDesc(IField field, IValidateArgs args)
		{
			return string.IsNullOrWhiteSpace(Desc) == false && Desc.Length <= Constants.MonDescLen;
		}

		protected virtual bool ValidatePluralType(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidPluralType(PluralType);
		}

		protected virtual bool ValidateArticleType(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.ArticleType), ArticleType);
		}

		protected virtual bool ValidateHardiness(IField field, IValidateArgs args)
		{
			return Hardiness >= 0;				// 0=Must be calculated
		}

		protected virtual bool ValidateAgility(IField field, IValidateArgs args)
		{
			return Agility >= 0;          // 0=Must be calculated
		}

		protected virtual bool ValidateGroupCount(IField field, IValidateArgs args)
		{
			return GroupCount >= 0;          // 0=Must be calculated
		}

		protected virtual bool ValidateCourage(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidMonsterCourage(Courage);
		}

		protected virtual bool ValidateCombatCode(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.CombatCode), CombatCode);
		}

		protected virtual bool ValidateArmor(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidMonsterArmor(Armor);
		}

		protected virtual bool ValidateNwDice(IField field, IValidateArgs args)
		{
			return NwDice >= 0;
		}

		protected virtual bool ValidateNwSides(IField field, IValidateArgs args)
		{
			return NwSides >= 0;
		}

		protected virtual bool ValidateFriendliness(IField field, IValidateArgs args)
		{
			return Globals.Engine.IsValidMonsterFriendliness(Friendliness);
		}

		protected virtual bool ValidateGender(IField field, IValidateArgs args)
		{
			return Enum.IsDefined(typeof(Enums.Gender), Gender);
		}

		protected virtual bool ValidateOrigGroupCount(IField field, IValidateArgs args)
		{
			return OrigGroupCount >= GroupCount;
		}

		protected virtual bool ValidateDmgTaken(IField field, IValidateArgs args)
		{
			return DmgTaken >= 0;
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

		protected virtual bool ValidateInterdependenciesPluralType(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var result = true;

			var effectUid = Globals.Engine.GetPluralTypeEffectUid(PluralType);

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

			var roomUid = GetInRoomUid();

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

			var artUid = GetCarryingWeaponUid();

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
				else if (!artifact.IsWeapon01())
				{
					result = false;

					args.Buf.SetFormat(Constants.RecIdepErrorFmtStr, field.GetPrintedName(), "artifact", artUid, "which should be a weapon, but isn't");

					args.ErrorMessage = args.Buf.ToString();

					args.RecordType = typeof(IArtifact);

					args.EditRecord = artifact;

					goto Cleanup;
				}
				else if (!artifact.IsCarriedByMonster(this) && !artifact.IsWornByMonster(this))
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

			var artUid = GetDeadBodyUid();

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
				else if (artifact.Location > 0 && Location > 0)
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

		#region Interface IEditable

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
			var fullDesc = "Enter the number of members in the monster's group.";

			var briefDesc = "1=Single monster; (GT 1)=Multiple monsters";

			Globals.Engine.AppendFieldDesc(args, fullDesc, briefDesc);
		}

		protected virtual void PrintDescCourage(IField field, IPrintDescArgs args)
		{
			Debug.Assert(args != null && args.Buf != null);

			var fullDesc = "Enter the courage of the monster.";

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

			var fullDesc = "Enter the friendliness of the monster.";

			var briefDesc = new StringBuilder(Constants.BufSize);

			var friendlinessValues = EnumUtil.GetValues<Enums.Friendliness>();

			for (j = 0; j < friendlinessValues.Count; j++)
			{
				var friendliness = Globals.Engine.GetFriendlinesses(friendlinessValues[j]);

				Debug.Assert(friendliness != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)friendlinessValues[j], friendliness.Name);
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
				var gender = Globals.Engine.GetGenders(genderValues[j]);

				Debug.Assert(gender != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)genderValues[j], gender.Name);
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

		protected virtual void ListStateDesc(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				if (!string.IsNullOrWhiteSpace(StateDesc))
				{
					args.Buf.Clear();

					args.Buf.Append(StateDesc);

					Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), args.Buf);
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), StateDesc);
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

		protected virtual void ListSeen(IField field, IListArgs args)
		{
			Debug.Assert(field != null && args != null);

			if (args.FullDetail)
			{
				if (args.NumberFields)
				{
					field.ListNum = args.ListNum++;
				}

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(Seen));
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Convert.ToInt64(IsListed));
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

					var effectUid = Globals.Engine.GetPluralTypeEffectUid(PluralType);

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
						Globals.Engine.BuildValue(51, ' ', 8, (long)PluralType, null,
						PluralType == Enums.PluralType.None ? "No change" :
						PluralType == Enums.PluralType.S ? "Use 's'" :
						PluralType == Enums.PluralType.Es ? "Use 'es'" :
						PluralType == Enums.PluralType.YIes ? "Use 'y' to 'ies'" :
						args.Buf.ToString()));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)PluralType);
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
						Globals.Engine.BuildValue(51, ' ', 8, (long)ArticleType, null,
						ArticleType == Enums.ArticleType.None ? "No article" :
						ArticleType == Enums.ArticleType.A ? "Use 'a'" :
						ArticleType == Enums.ArticleType.An ? "Use 'an'" :
						ArticleType == Enums.ArticleType.Some ? "Use 'some'" :
						"Use 'the'"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)ArticleType);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Hardiness);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Agility);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), GroupCount);
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

				Globals.Out.Write("{0}{1}{2}%", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Courage);
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

				if (args.LookupMsg && Location > 0)
				{
					var room = Globals.RDB[Location];

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, Location, null, room != null ? Globals.Engine.Capitalize(room.Name) : Globals.Engine.UnknownName));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Location);
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
						Globals.Engine.BuildValue(51, ' ', 8, (long)CombatCode, null, Globals.Engine.GetCombatCodeDescs(CombatCode)));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)CombatCode);
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
					var armor = Globals.Engine.GetArmors((Enums.Armor)(Armor * 2));

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, Armor, null, armor != null ? armor.Name : "Magic/Exotic"));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Armor);
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
					if (Weapon > 0)
					{
						var artifact = Globals.ADB[Weapon];

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, Weapon, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, Weapon, null, Weapon == 0 ? "Natural weapons" : "Weaponless"));
					}
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Weapon);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), NwDice);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), NwSides);
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
					if (DeadBody > 0)
					{
						var artifact = Globals.ADB[DeadBody];

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, DeadBody, null, artifact != null ? Globals.Engine.Capitalize(artifact.Name) : Globals.Engine.UnknownName));
					}
					else
					{
						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, DeadBody, null, "No dead body"));
					}
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), DeadBody);
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
					if (Globals.Engine.IsValidMonsterFriendlinessPct(Friendliness))
					{
						args.Buf.SetFormat("{0}% Chance", Globals.Engine.GetMonsterFriendlinessPct(Friendliness));

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, (long)Friendliness, null, args.Buf.ToString()));
					}
					else
					{
						var friendliness = Globals.Engine.GetFriendlinesses(Friendliness);

						Debug.Assert(friendliness != null);

						Globals.Out.Write("{0}{1}{2}",
							Environment.NewLine,
							Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
							Globals.Engine.BuildValue(51, ' ', 8, (long)Friendliness, null, friendliness.Name));
					}
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Friendliness);
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
					var gender = Globals.Engine.GetGenders(Gender);

					Debug.Assert(gender != null);

					Globals.Out.Write("{0}{1}{2}",
						Environment.NewLine,
						Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null),
						Globals.Engine.BuildValue(51, ' ', 8, (long)Gender, null, gender.Name));
				}
				else
				{
					Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), (long)Gender);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Field1);
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

				Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', field.ListNum, field.GetPrintedName(), null), Field2);
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

				var rc = Globals.In.ReadField(args.Buf, Constants.MonNameLen, null, '_', '\0', false, null, null, null, null);

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

		protected virtual void InputStateDesc(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var stateDesc = StateDesc;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", stateDesc);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), null));

				Globals.Out.WordWrap = false;

				var rc = Globals.In.ReadField(args.Buf, Constants.MonStateDescLen, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				StateDesc = args.Buf.Trim().ToString();

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

				var rc = Globals.In.ReadField(args.Buf, Constants.MonDescLen, null, '_', '\0', false, null, null, null, null);

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

		protected virtual void InputSeen(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var seen = Seen;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(seen));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Seen = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

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

			var isListed = IsListed;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", Convert.ToInt64(isListed));

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsChar0Or1, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				IsListed = Convert.ToInt64(args.Buf.Trim().ToString()) != 0 ? true : false;

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

			var pluralType = PluralType;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)pluralType);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				PluralType = (Enums.PluralType)Convert.ToInt64(args.Buf.Trim().ToString());

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

			var articleType = ArticleType;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)articleType);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ArticleType = (Enums.ArticleType)Convert.ToInt64(args.Buf.Trim().ToString());

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

			var hardiness = Hardiness;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", hardiness);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "16"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "16", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Hardiness = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			DmgTaken = 0;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputAgility(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var agility = Agility;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", agility);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "15"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "15", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Agility = Convert.ToInt64(args.Buf.Trim().ToString());

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

			var groupCount = GroupCount;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", groupCount);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				GroupCount = Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			OrigGroupCount = GroupCount;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputCourage(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var courage = Courage;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", courage);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "100"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "100", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Courage = Convert.ToInt64(args.Buf.Trim().ToString());

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

			var location = Location;

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
					Location = Convert.ToInt64(args.Buf.Trim().ToString());
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

			var combatCode = CombatCode;

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
					CombatCode = (Enums.CombatCode)Convert.ToInt64(args.Buf.Trim().ToString());
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

			var armor = Armor;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", armor);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Armor = Convert.ToInt64(args.Buf.Trim().ToString());

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

			var weapon = Weapon;

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
					Weapon = Convert.ToInt64(args.Buf.Trim().ToString());
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

			var nwDice = NwDice;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", nwDice);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "1"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				NwDice = Convert.ToInt64(args.Buf.Trim().ToString());

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

			var nwSides = NwSides;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", nwSides);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "4"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "4", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				NwSides = Convert.ToInt64(args.Buf.Trim().ToString());

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

			var deadBody = DeadBody;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", deadBody);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				DeadBody = Convert.ToInt64(args.Buf.Trim().ToString());

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

			var friendliness = Friendliness;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)friendliness);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "3"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "3", null, Globals.Engine.IsCharDigit, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Friendliness = (Enums.Friendliness)Convert.ToInt64(args.Buf.Trim().ToString());

				if (ValidateField(field, args.Vargs))
				{
					break;
				}

				fieldDesc = Enums.FieldDesc.Brief;
			}

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);
		}

		protected virtual void InputGender(IField field, IInputArgs args)
		{
			Debug.Assert(field != null && args != null && args.Buf != null);

			var fieldDesc = args.FieldDesc;

			var gender = Gender;

			while (true)
			{
				args.Buf.SetFormat(args.EditRec ? "{0}" : "", (long)gender);

				PrintFieldDesc(field, args.EditRec, args.EditField, fieldDesc);

				Globals.Out.Write("{0}{1}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '\0', 0, field.GetPrintedName(), "0"));

				var rc = Globals.In.ReadField(args.Buf, Constants.BufSize01, null, '_', '\0', true, "0", null, Globals.Engine.IsChar0To2, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Gender = (Enums.Gender)Convert.ToInt64(args.Buf.Trim().ToString());

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

			var field1 = Field1;

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
					Field1 = Convert.ToInt64(args.Buf.Trim().ToString());
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

			var field2 = Field2;

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
					Field2 = Convert.ToInt64(args.Buf.Trim().ToString());
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

		#endregion

		#region Class Monster

		protected virtual void SetMonsterUidIfInvalid(bool editRec)
		{
			if (Uid <= 0)
			{
				Uid = Globals.Database.GetMonsterUid();

				IsUidRecycled = true;
			}
			else if (!editRec)
			{
				IsUidRecycled = false;
			}
		}

		protected virtual bool HasHumanNaturalAttackDescs()
		{
			return false;
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IHaveFields

		public override void FreeFields()
		{
			base.FreeFields();

			NameField = null;
		}

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
					GetNameField(),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "StateDesc";
						x.Validate = ValidateStateDesc;
						x.PrintDesc = PrintDescStateDesc;
						x.List = ListStateDesc;
						x.Input = InputStateDesc;
						x.GetPrintedName = () => "State Description";
						x.GetValue = () => StateDesc;
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
						x.Name = "Seen";
						x.PrintDesc = PrintDescSeen;
						x.List = ListSeen;
						x.Input = InputSeen;
						x.GetPrintedName = () => "Seen";
						x.GetValue = () => Seen;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "IsListed";
						x.PrintDesc = PrintDescIsListed;
						x.List = ListIsListed;
						x.Input = InputIsListed;
						x.GetPrintedName = () => "Is Listed";
						x.GetValue = () => IsListed;
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
						x.GetValue = () => PluralType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "ArticleType";
						x.Validate = ValidateArticleType;
						x.PrintDesc = PrintDescArticleType;
						x.List = ListArticleType;
						x.Input = InputArticleType;
						x.GetPrintedName = () => "Article Type";
						x.GetValue = () => ArticleType;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Hardiness";
						x.Validate = ValidateHardiness;
						x.PrintDesc = PrintDescHardiness;
						x.List = ListHardiness;
						x.Input = InputHardiness;
						x.GetPrintedName = () => "Hardiness";
						x.GetValue = () => Hardiness;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Agility";
						x.Validate = ValidateAgility;
						x.PrintDesc = PrintDescAgility;
						x.List = ListAgility;
						x.Input = InputAgility;
						x.GetPrintedName = () => "Agility";
						x.GetValue = () => Agility;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "GroupCount";
						x.Validate = ValidateGroupCount;
						x.PrintDesc = PrintDescGroupCount;
						x.List = ListGroupCount;
						x.Input = InputGroupCount;
						x.GetPrintedName = () => "Group Count";
						x.GetValue = () => GroupCount;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Courage";
						x.Validate = ValidateCourage;
						x.PrintDesc = PrintDescCourage;
						x.List = ListCourage;
						x.Input = InputCourage;
						x.GetPrintedName = () => "Courage";
						x.GetValue = () => Courage;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Location";
						x.ValidateInterdependencies = ValidateInterdependenciesLocation;
						x.PrintDesc = PrintDescLocation;
						x.List = ListLocation;
						x.Input = InputLocation;
						x.GetPrintedName = () => "Location";
						x.GetValue = () => Location;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "CombatCode";
						x.Validate = ValidateCombatCode;
						x.PrintDesc = PrintDescCombatCode;
						x.List = ListCombatCode;
						x.Input = InputCombatCode;
						x.GetPrintedName = () => "Combat Code";
						x.GetValue = () => CombatCode;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Armor";
						x.Validate = ValidateArmor;
						x.PrintDesc = PrintDescArmor;
						x.List = ListArmor;
						x.Input = InputArmor;
						x.GetPrintedName = () => "Armor";
						x.GetValue = () => Armor;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Weapon";
						x.ValidateInterdependencies = ValidateInterdependenciesWeapon;
						x.PrintDesc = PrintDescWeapon;
						x.List = ListWeapon;
						x.Input = InputWeapon;
						x.GetPrintedName = () => "Weapon Uid";
						x.GetValue = () => Weapon;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NwDice";
						x.Validate = ValidateNwDice;
						x.PrintDesc = PrintDescNwDice;
						x.List = ListNwDice;
						x.Input = InputNwDice;
						x.GetPrintedName = () => "Natural Wpn Dice";
						x.GetValue = () => NwDice;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "NwSides";
						x.Validate = ValidateNwSides;
						x.PrintDesc = PrintDescNwSides;
						x.List = ListNwSides;
						x.Input = InputNwSides;
						x.GetPrintedName = () => "Natural Wpn Sides";
						x.GetValue = () => NwSides;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DeadBody";
						x.ValidateInterdependencies = ValidateInterdependenciesDeadBody;
						x.PrintDesc = PrintDescDeadBody;
						x.List = ListDeadBody;
						x.Input = InputDeadBody;
						x.GetPrintedName = () => "Dead Body Uid";
						x.GetValue = () => DeadBody;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Friendliness";
						x.Validate = ValidateFriendliness;
						x.PrintDesc = PrintDescFriendliness;
						x.List = ListFriendliness;
						x.Input = InputFriendliness;
						x.GetPrintedName = () => "Friendliness";
						x.GetValue = () => Friendliness;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Gender";
						x.Validate = ValidateGender;
						x.PrintDesc = PrintDescGender;
						x.List = ListGender;
						x.Input = InputGender;
						x.GetPrintedName = () => "Gender";
						x.GetValue = () => Gender;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "InitGroupCount";
						x.GetValue = () => InitGroupCount;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "OrigGroupCount";
						x.Validate = ValidateOrigGroupCount;
						x.GetValue = () => OrigGroupCount;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "DmgTaken";
						x.Validate = ValidateDmgTaken;
						x.GetValue = () => DmgTaken;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Field1";
						x.List = ListField1;
						x.Input = InputField1;
						x.GetPrintedName = () => "Field #1";
						x.GetValue = () => Field1;
					}),
					Globals.CreateInstance<IField>(x =>
					{
						x.Name = "Field2";
						x.List = ListField2;
						x.Input = InputField2;
						x.GetPrintedName = () => "Field #2";
						x.GetValue = () => Field2;
					})
				};
			}

			return Fields;
		}

		#endregion

		#region Interface IHaveListedName

		public virtual string GetPluralName(IField field, StringBuilder buf)
		{
			IEffect effect;
			long effectUid;
			string result;

			if (field == null || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(field.Name == "Name");

			buf.Clear();

			effectUid = Globals.Engine.GetPluralTypeEffectUid(PluralType);

			effect = Globals.EDB[effectUid];

			if (effect != null)
			{
				buf.Append(effect.Desc.Substring(0, Math.Min(Constants.MonNameLen, effect.Desc.Length)).Trim());
			}
			else
			{
				buf.Append(Name);

				if (buf.Length > 0 && PluralType == Enums.PluralType.YIes)
				{
					buf.Length--;
				}

				buf.Append(PluralType == Enums.PluralType.None ? "" :
						PluralType == Enums.PluralType.Es ? "es" :
						PluralType == Enums.PluralType.YIes ? "ies" :
						"s");
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetPluralName01(StringBuilder buf)
		{
			return GetPluralName(GetField("Name"), buf);
		}

		public virtual string GetDecoratedName(IField field, Enums.ArticleType articleType, bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			StringBuilder buf01;
			string result;
			long gc;

			if (field == null || buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(field.Name == "Name");

			buf.Clear();

			buf01 = new StringBuilder(Constants.BufSize);

			gc = groupCountOne ? 1 : GroupCount;

			switch (articleType)
			{
				case Enums.ArticleType.None:

					if (gc > 10)
					{
						buf01.AppendFormat("{0} ", gc);
					}
					else
					{
						buf01.Append(gc > 1 ? Globals.Engine.GetStringFromNumber(gc, true, new StringBuilder(Constants.BufSize)) : "");
					}

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						buf01.ToString(),
						gc > 1 ? GetPluralName(field, new StringBuilder(Constants.BufSize)) :
						Name,
						showStateDesc && StateDesc.Length > 0 ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
					);

					break;

				case Enums.ArticleType.The:

					if (gc > 10)
					{
						buf01.AppendFormat("{0}{1} ", "the ", gc);
					}
					else
					{
						buf01.AppendFormat
						(
							"{0}{1}",
							gc > 1 ? "the " :
							ArticleType == Enums.ArticleType.None ? "" :
							"the ",
							gc > 1 ? Globals.Engine.GetStringFromNumber(gc, true, new StringBuilder(Constants.BufSize)) : ""
						);
					}

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						buf01.ToString(),
						gc > 1 ? GetPluralName(field, new StringBuilder(Constants.BufSize)) :
						Name,
						showStateDesc && StateDesc.Length > 0 ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
					);

					break;

				default:

					if (gc > 10)
					{
						buf01.AppendFormat("{0} ", gc);
					}
					else
					{
						buf01.Append
						(
							gc > 1 ? Globals.Engine.GetStringFromNumber(gc, true, new StringBuilder(Constants.BufSize)) :
							ArticleType == Enums.ArticleType.None ? "" :
							ArticleType == Enums.ArticleType.The ? "the " :
							ArticleType == Enums.ArticleType.Some ? "some " :
							ArticleType == Enums.ArticleType.An ? "an " :
							"a "
						);
					}

					buf.AppendFormat
					(
						"{0}{1}{2}{3}",
						buf01.ToString(),
						gc > 1 ? GetPluralName(field, new StringBuilder(Constants.BufSize)) :
						Name,
						showStateDesc && StateDesc.Length > 0 ? " " : "",
						showStateDesc && StateDesc.Length > 0 ? StateDesc : ""
					);

					break;
			}

			if (buf.Length > 0 && upshift)
			{
				buf[0] = Char.ToUpper(buf[0]);
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetDecoratedName01(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), Enums.ArticleType.None, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName02(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), ArticleType, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual string GetDecoratedName03(bool upshift, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf)
		{
			return GetDecoratedName(GetField("Name"), Enums.ArticleType.The, upshift, showCharOwned, showStateDesc, groupCountOne, buf);
		}

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (showName)
			{
				buf.AppendFormat("{0}[{1}]",
					Environment.NewLine,
					GetDecoratedName02(true, true, false, false, new StringBuilder(Constants.BufSize)));
			}

			if (!string.IsNullOrWhiteSpace(Desc))
			{
				buf.AppendFormat("{0}{1}", Environment.NewLine, Desc);
			}

			if (showName || !string.IsNullOrWhiteSpace(Desc))
			{
				buf.Append(Environment.NewLine);
			}

		Cleanup:

			return rc;
		}

		public virtual IField GetNameField()
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
					x.GetValue = () => Name;
				});
			}

			return NameField;
		}

		#endregion

		#region Interface IEditable

		public override void ListErrorField(IValidateArgs args)
		{
			Debug.Assert(args != null && args.ErrorField != null && args.Buf != null);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Uid").GetPrintedName(), null), Uid);

			Globals.Out.Write("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Name").GetPrintedName(), null), Name);

			if (string.Equals(args.ErrorField.Name, "Desc", StringComparison.OrdinalIgnoreCase) || args.ShowDesc)
			{
				Globals.Out.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, GetField("Desc").GetPrintedName(), null), Desc);
			}

			if (string.Equals(args.ErrorField.Name, "PluralType", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), (long)PluralType);
			}
			else if (string.Equals(args.ErrorField.Name, "Location", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), Location);
			}
			else if (string.Equals(args.ErrorField.Name, "Weapon", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), Weapon);
			}
			else if (string.Equals(args.ErrorField.Name, "DeadBody", StringComparison.OrdinalIgnoreCase))
			{
				Globals.Out.WriteLine("{0}{1}{2}", Environment.NewLine, Globals.Engine.BuildPrompt(27, '.', 0, args.ErrorField.GetPrintedName(), null), DeadBody);
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IMonster monster)
		{
			return this.Uid.CompareTo(monster.Uid);
		}

		#endregion

		#region Interface IMonster

		public virtual bool IsDead()
		{
			return DmgTaken >= Hardiness;
		}

		public virtual bool IsCarryingWeapon()
		{
			return Weapon > 0 && Weapon < 1001;
		}

		public virtual bool HasDeadBody()
		{
			return DeadBody > 0 && DeadBody < 1001;
		}

		public virtual bool IsInRoom()
		{
			return Location > 0 && Location < 1001;
		}

		public virtual bool IsInLimbo()
		{
			return Location == 0;
		}

		public virtual bool IsInRoomUid(long roomUid)
		{
			return Location == roomUid;
		}

		public virtual bool IsInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			return IsInRoomUid(room.Uid);
		}

		public virtual bool CanMoveToRoom(bool fleeing)
		{
			return true;
		}

		public virtual bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			return CanMoveToRoom(fleeing);
		}

		public virtual bool CanMoveToRoom(IRoom room, bool fleeing)
		{
			Debug.Assert(room != null);

			return CanMoveToRoomUid(room.Uid, fleeing);
		}

		public virtual long GetCarryingWeaponUid()
		{
			return IsCarryingWeapon() ? Weapon : 0;
		}

		public virtual long GetDeadBodyUid()
		{
			return HasDeadBody() ? DeadBody : 0;
		}

		public virtual long GetInRoomUid()
		{
			return IsInRoom() ? Location : 0;
		}

		public virtual IRoom GetInRoom()
		{
			var uid = GetInRoomUid();

			return Globals.RDB[uid];
		}

		public virtual void SetInRoomUid(long roomUid)
		{
			Location = roomUid;
		}

		public virtual void SetInLimbo()
		{
			Location = 0;
		}

		public virtual void SetInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			SetInRoomUid(room.Uid);
		}

		public virtual bool IsInRoomLit()
		{
			var room = GetInRoom();

			return room != null && room.IsLit();
		}

		public virtual T EvalGender<T>(T maleValue, T femaleValue, T neutralValue)
		{
			return Gender == Enums.Gender.Male ? maleValue : Gender == Enums.Gender.Female ? femaleValue : neutralValue;
		}

		public virtual T EvalPlural<T>(T singularValue, T pluralValue)
		{
			return GroupCount > 1 ? pluralValue : singularValue;
		}

		public virtual T EvalInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsInRoomLit() ? lightValue : darkValue;
		}

		public virtual void ResolveFriendlinessPct(long charisma)
		{
			if (Globals.Engine.IsValidMonsterFriendlinessPct(Friendliness))
			{
				var rl = 0L;

				var f = (long)Friendliness - 100;

				var k = Enums.Friendliness.Friend;

				var rc = Globals.Engine.RollDice(1, 100, 0, ref rl);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				rl -= Globals.Engine.GetCharismaFactor(charisma);

				if (rl > f)
				{
					k--;

					rc = Globals.Engine.RollDice(1, 100, 0, ref rl);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					rl -= Globals.Engine.GetCharismaFactor(charisma);

					if (rl > f)
					{
						k--;
					}
				}

				Friendliness = k;
			}
		}

		public virtual void ResolveFriendlinessPct(ICharacter character)
		{
			if (character != null)
			{
				ResolveFriendlinessPct(character.GetStats(Enums.Stat.Charisma));
			}
		}

		public virtual bool IsCharacterMonster()
		{
			var gameState = Globals.Engine.GetGameState();

			return gameState != null && gameState.Cm == Uid;
		}

		public virtual long GetWeightCarryableGronds()
		{
			return Globals.Engine.GetWeightCarryableGronds(Hardiness);
		}

		public virtual IList<IArtifact> GetCarriedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (monsterFindFunc == null)
			{
				if (IsCharacterMonster())
				{
					monsterFindFunc = a => a.IsCarriedByCharacter();
				}
				else
				{
					monsterFindFunc = a => a.IsCarriedByMonster(this);
				}
			}

			var list = Globals.Engine.GetArtifactList(() => true, a => monsterFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.IsContainer())
					{
						list01.AddRange(a.GetContainedList(artifactFindFunc, recurse));
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual IList<IArtifact> GetWornList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (monsterFindFunc == null)
			{
				if (IsCharacterMonster())
				{
					monsterFindFunc = a => a.IsWornByCharacter();
				}
				else
				{
					monsterFindFunc = a => a.IsWornByMonster(this);
				}
			}

			var list = Globals.Engine.GetArtifactList(() => true, a => monsterFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.IsContainer())
					{
						list01.AddRange(a.GetContainedList(artifactFindFunc, recurse));
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual IList<IArtifact> GetContainedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (monsterFindFunc == null)
			{
				if (IsCharacterMonster())
				{
					monsterFindFunc = a => a.IsCarriedByCharacter() || a.IsWornByCharacter();
				}
				else
				{
					monsterFindFunc = a => a.IsCarriedByMonster(this) || a.IsWornByMonster(this);
				}
			}

			var list = Globals.Engine.GetArtifactList(() => true, a => monsterFindFunc(a));

			if (recurse && list.Count > 0)
			{
				var list01 = new List<IArtifact>();

				foreach (var a in list)
				{
					if (a.IsContainer())
					{
						list01.AddRange(a.GetContainedList(artifactFindFunc, recurse));
					}
				}

				list.AddRange(list01);
			}

			return list;
		}

		public virtual RetCode EnforceFullInventoryWeightLimits(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			RetCode rc;

			long c, w, mwt;

			rc = RetCode.Success;

			mwt = 0;

			var list = GetContainedList(monsterFindFunc, artifactFindFunc);

			foreach (var a in list)
			{
				c = 0;

				w = a.Weight;

				Debug.Assert(!Globals.Engine.IsUnmovable01(w));

				if (recurse && a.IsContainer())
				{
					rc = a.GetContainerInfo(ref c, ref w, recurse);

					if (Globals.Engine.IsFailure(rc))
					{
						// PrintError

						goto Cleanup;
					}
				}

				if (w <= 10 * Hardiness && mwt + w <= 10 * Hardiness * GroupCount)
				{
					mwt += w;
				}
				else
				{
					a.Location = Location >= 0 ? Location : 0;

					if (Weapon == a.Uid)
					{
						a.RemoveStateDesc(Globals.Engine.ReadyWeaponDesc);

						Weapon = -1;
					}
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode GetFullInventoryWeight(ref long weight, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			RetCode rc;

			rc = RetCode.Success;

			var list = GetContainedList(monsterFindFunc, artifactFindFunc, recurse);

			foreach (var a in list)
			{
				if (!a.IsUnmovable01())
				{
					weight += a.Weight;
				}
			}

			return rc;
		}

		public virtual void AddHealthStatus(StringBuilder buf, bool addNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (IsDead())
			{
				result = "dead!";
			}
			else
			{
				var x = DmgTaken;

				x = (((long)((double)(x * 5) / (double)Hardiness)) + 1) * (x > 0 ? 1 : 0);

				result = "at death's door, knocking loudly.";

				if (x == 4)
				{
					result = "badly injured.";
				}
				else if (x == 3)
				{
					result = "in pain.";
				}
				else if (x == 2)
				{
					result = "hurting.";
				}
				else if (x == 1)
				{
					result = "still in good shape.";
				}
				else if (x < 1)
				{
					result = "in perfect health.";
				}
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, addNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}

		public virtual string GetAttackDescString(IArtifact artifact)
		{
			var rl = Globals.Engine.RollDice01(1, 3, artifact == null && HasHumanNaturalAttackDescs() ? 3 : 0);

			var ac = artifact != null ? artifact.GetArtifactClass(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon }) : null;

			return Globals.Engine.GetAttackDescString((Enums.Weapon)(ac != null ? ac.Field6 : 0), rl);
		}

		public virtual string GetMissDescString(IArtifact artifact)
		{
			var i = 0L;

			var rc = Globals.Engine.RollDice(1, 2, 0, ref i);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			var ac = artifact != null ? artifact.GetArtifactClass(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon }) : null;

			return Globals.Engine.GetMissDescString((Enums.Weapon)(ac != null ? ac.Field6 : 0), i);
		}

		public virtual string GetArmorDescString()
		{
			return "armor";
		}

		#endregion

		#region Class Monster

		public Monster()
		{
			SetUidIfInvalid = SetMonsterUidIfInvalid;

			IsUidRecycled = true;

			Name = "";

			StateDesc = "";

			Desc = "";
		}

		#endregion

		#endregion
	}
}
