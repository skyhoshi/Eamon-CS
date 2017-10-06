
// IEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using Eamon.Framework.Args;
using Eamon.Framework.Commands;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface IEngine
	{
		#region Properties

		IDictionary<long, Func<string>> MacroFuncs { get; set; }

		string[] Preps { get; set; }

		string[] Articles { get; set; }

		long NumCacheItems { get; set; }

		string ProvidingLightDesc { get; set; }

		string ReadyWeaponDesc { get; set; }

		string BrokenDesc { get; set; }

		string EmptyDesc { get; set; }

		string BlastDesc { get; set; }

		string UnknownName { get; set; }

		#endregion

		#region Methods

		string GetPreps(long index);

		string GetArticles(long index);

		string GetNumberStrings(long index);

		string GetFieldDescNames(long index);

		string GetFieldDescNames(Enums.FieldDesc fieldDesc);

		string GetStatusNames(long index);

		string GetStatusNames(Enums.Status status);

		string GetClothingNames(long index);

		string GetClothingNames(Enums.Clothing clothing);

		string GetCombatCodeDescs(long index);

		string GetCombatCodeDescs(Enums.CombatCode combatCode);

		string GetLightLevelNames(long index);

		string GetLightLevelNames(Enums.LightLevel lightLevel);

		Classes.IStat GetStats(long index);

		Classes.IStat GetStats(Enums.Stat stat);

		Classes.ISpell GetSpells(long index);

		Classes.ISpell GetSpells(Enums.Spell spell);

		Classes.IWeapon GetWeapons(long index);

		Classes.IWeapon GetWeapons(Enums.Weapon weapon);

		Classes.IArmor GetArmors(long index);

		Classes.IArmor GetArmors(Enums.Armor armor);

		Classes.IDirection GetDirections(long index);

		Classes.IDirection GetDirections(Enums.Direction direction);

		Classes.IArtifactType GetArtifactTypes(long index);

		Classes.IArtifactType GetArtifactTypes(Enums.ArtifactType artifactType);

		bool IsSuccess(RetCode rc);

		bool IsFailure(RetCode rc);

		bool IsValidPluralType(Enums.PluralType pluralType);

		bool IsValidArtifactType(Enums.ArtifactType artifactType);

		bool IsValidArtifactArmor(long armor);

		bool IsValidMonsterArmor(long armor);

		bool IsValidMonsterCourage(long courage);

		bool IsValidMonsterFriendliness(Enums.Friendliness friendliness);

		bool IsValidMonsterFriendlinessPct(Enums.Friendliness friendliness);

		bool IsValidDirection(Enums.Direction dir);

		bool IsValidRoomUid01(long roomUid);

		bool IsValidRoomDirectionDoorUid01(long roomUid);

		bool IsArtifactFieldStrength(long value);

		bool IsUnmovable(long weight);

		bool IsUnmovable01(long weight);

		long GetWeightCarryableGronds(long hardiness);

		long GetWeightCarryableDos(long hardiness);

		long GetIntellectBonusPct(long intellect);

		long GetCharmMonsterPct(long charisma);

		long GetPluralTypeEffectUid(Enums.PluralType pluralType);

		long GetArmorFactor(long armorUid, long shieldUid);

		long GetCharismaFactor(long charisma);

		long GetMonsterFriendlinessPct(Enums.Friendliness friendliness);

		long GetArtifactFieldStrength(long value);

		long GetMerchantAskPrice(double price, double rtio);

		long GetMerchantBidPrice(double price, double rtio);

		long GetMerchantAdjustedCharisma(long charisma);

		double GetMerchantRtio(long charisma);

		bool IsCharYOrN(char ch);

		bool IsCharSOrTOrROrX(char ch);

		bool IsChar0Or1(char ch);

		bool IsChar0To2(char ch);

		bool IsChar0To3(char ch);

		bool IsChar1To3(char ch);

		bool IsCharDigit(char ch);

		bool IsCharDigitOrX(char ch);

		bool IsCharPlusMinusDigit(char ch);

		bool IsCharAlpha(char ch);

		bool IsCharAlphaSpace(char ch);

		bool IsCharAlnum(char ch);

		bool IsCharAlnumSpace(char ch);

		bool IsCharPrint(char ch);

		bool IsCharPound(char ch);

		bool IsCharQuote(char ch);

		bool IsCharAny(char ch);

		bool IsCharAnyButDquoteCommaColon(char ch);

		char ModifyCharToUpper(char ch);

		char ModifyCharToNullOrX(char ch);

		char ModifyCharToNull(char ch);

		Enums.Direction GetDirection(string printedName);

		IConfig GetConfig();

		IGameState GetGameState();

		IModule GetModule();

		T EvalFriendliness<T>(Enums.Friendliness friendliness, T enemyValue, T neutralValue, T friendValue);

		T EvalGender<T>(Enums.Gender gender, T maleValue, T femaleValue, T neutralValue);

		T EvalRoomType<T>(Enums.RoomType roomType, T indoorsValue, T outdoorsValue);

		string BuildPrompt(long bufSize, char fillChar, long number, string msg, string emptyVal);

		string BuildValue(long bufSize, char fillChar, long offset, long longVal, string stringVal, string lookupMsg);

		string WordWrap(string str, StringBuilder buf, long margin, IWordWrapArgs args, bool clearBuf = true);

		string WordWrap(string str, StringBuilder buf, bool clearBuf = true);

		string LineWrap(string str, StringBuilder buf, long startColumn, bool clearBuf = true);

		string GetStringFromNumber(long num, bool addSpace, StringBuilder buf);

		long GetNumberFromString(string str);

		string GetAttackDescString(Enums.Weapon weapon, long roll);

		string GetMissDescString(Enums.Weapon weapon, long roll);

		RetCode RollDice(long numDice, long numSides, ref long[] dieRolls);

		RetCode RollDice(long numDice, long numSides, long modifier, ref long result);

		long RollDice01(long numDice, long numSides, long modifier);

		RetCode SumHighestRolls(long[] dieRolls, long numDieRolls, long numRollsToSum, ref long result);

		RetCode BuildCommandList(IList<ICommand> commands, Enums.CommandType cmdType, StringBuilder buf, ref bool newSeen);

		string Capitalize(string str);

		void UnlinkOnFailure();

		void TruncatePluralTypeEffectDesc(Enums.PluralType pluralType, long maxSize);

		void TruncatePluralTypeEffectDesc(IEffect effect);

		void GetPossessiveName(StringBuilder buf);

		long FindIndex<T>(T[] array, long startIndex, long count, Predicate<T> match);

		long FindIndex<T>(T[] array, long startIndex, Predicate<T> match);

		long FindIndex<T>(T[] array, Predicate<T> match);

		RetCode SplitPath(string fullPath, ref string directory, ref string fileName, ref string extension, bool appendDirectorySeparatorChar = true);

		RetCode StripPrepsAndArticles(StringBuilder buf, ref bool mySeen);

		void PrintTitle(string title, bool inBox);

		void PrintEffectDesc(IEffect effect, bool printFinalNewLine = true);

		void PrintEffectDesc(long effectUid, bool printFinalNewLine = true);

		RetCode GetRecordNameList(IList<IHaveListedName> records, Enums.ArticleType articleType, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		RetCode GetRecordNameCount(IList<IHaveListedName> records, string name, bool exactMatch, ref long count);

		RetCode ListRecords(IList<IHaveListedName> records, bool capitalize, bool showExtraInfo, StringBuilder buf);

		RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse, ref long invalidUid);

		RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse);

		double GetWeaponPriceOrValue(string name, long complexity, Enums.Weapon type, long dice, long sides, bool calcPrice, ref bool isMarcosWeapon);

		double GetWeaponPriceOrValue(Classes.ICharacterWeapon weapon, bool calcPrice, ref bool isMarcosWeapon);

		double GetArmorPriceOrValue(Enums.Armor armor, bool calcPrice, ref bool isMarcosArmor);

		void AppendFieldDesc(IPrintDescArgs args, StringBuilder fullDesc, StringBuilder briefDesc);

		void AppendFieldDesc(IPrintDescArgs args, string fullDesc, string briefDesc);

		void CopyArtifactClassFields(Classes.IArtifactClass destAc, Classes.IArtifactClass sourceAc);

		IList<IArtifact> GetArtifactList(Func<bool> shouldQueryFunc, params Func<IArtifact, bool>[] whereClauseFuncs);

		IList<IMonster> GetMonsterList(Func<bool> shouldQueryFunc, params Func<IMonster, bool>[] whereClauseFuncs);

		IList<IHaveListedName> GetRecordList(Func<bool> shouldQueryFunc, params Func<IHaveListedName, bool>[] whereClauseFuncs);

		IArtifact GetNthArtifact(IList<IArtifact> artifactList, long which, Func<IArtifact, bool> whereClauseFunc);

		IMonster GetNthMonster(IList<IMonster> monsterList, long which, Func<IMonster, bool> whereClauseFunc);

		IHaveListedName GetNthRecord(IList<IHaveListedName> recordList, long which, Func<IHaveListedName, bool> whereClauseFunc);

		void StripPoundCharsFromRecordNames(IList<IHaveListedName> recordList);

		void AddPoundCharsToRecordNames(IList<IHaveListedName> recordList);

		void ConvertWeaponToGoldOrTreasure(IArtifact artifact, bool convertToGold);

		#endregion
	}
}
