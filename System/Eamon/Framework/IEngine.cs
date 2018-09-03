
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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

		/// <summary>
		/// A collection of functions used to resolve macros embedded in Desc and StateDesc properties for various record types.
		/// </summary>
		/// <remarks>
		/// You can embed macros in the Desc or StateDesc properties of the various record types to produce dynamically
		/// generated strings for output during gameplay.  Eamon CS supports the standard macros found in Eamon Deluxe,
		/// plus a new type based on lookups done in the MacroFuncs dictionary:
		/// <list type="table">
		/// <listheader><term>Macro</term><description>Result</description></listheader>
		/// <item><term>*XXX</term><description>Injects [CRLF + CRLF + effect Uid XXX] into the property string (no brackets)</description></item>
		/// <item><term>**XXX</term><description>Injects [effect Uid XXX] into the property string</description></item>
		/// <item><term>@XXX</term><description>Injects [CRLF + CRLF + string returned by Func w/Key XXX] into the property string</description></item>
		/// <item><term>@@XXX</term><description>Injects [string returned by Func w/Key XXX] into the property string</description></item>
		/// </list>
		/// The functions stored in MacroFuncs are typically lambdas but can be any valid function returning a string.  This
		/// mechanism is incredibly powerful as it can exploit all facets of the game engine to calculate the string returned.
		/// Also, the dictionary can be manipulated during gameplay with old functions removed and/or new functions added if
		/// beneficial.
		/// <para>
		/// However, there are a few things to keep in mind.  You can run EamonDD in a mode where it will display resolved macros;
		/// the option is called Resolve Effects and it exists off the CONFIG RECORD MENU.  If you load the MacroFuncs dictionary
		/// during gameplay (typically in InitArtifacts or InitMonsters, but this isn't a requirement), EamonDD will NOT be able
		/// to resolve any function macro so it will remain.  But there are scenarios where you can load MacroFuncs in such a
		/// way that functions are available to EamonDD, which will then use them to resolve macros.  The upshot of this is that
		/// you should always write the functions installed in MacroFuncs in such a way that they are insensitive to whether they
		/// are being executed by EamonRT or EamonDD.  The best way to do this is to be aggressive about checking for null at all
		/// times and return default strings when appropriate.  There are many examples of macro functions written for the various
		/// Eamon CS adventures; the See Also list below is a good place to start.
		/// </para>
		/// </remarks>
		/// <seealso cref="WrenholdsSecretVigil.Game.Engine.Engine()"/>
		/// <seealso cref="ARuncibleCargo.Game.Engine.InitArtifacts()"/>
		/// <seealso cref="TheTempleOfNgurct.Game.Engine.InitMonsters()"/>
		IDictionary<long, Func<string>> MacroFuncs { get; set; }

		/// <summary>
		/// An array of sentence prepositions (eg, "to", "from", "inside", etc).
		/// </summary>
		/// <remarks>
		/// Other than to check the Length, you should never access this array directly; always use <see cref="GetPreps(long)"/>.
		/// </remarks>
		string[] Preps { get; set; }

		/// <summary>
		/// An array of sentence articles (eg, "a", "some", "the", etc).
		/// </summary>
		/// <remarks>
		/// Other than to check the Length, you should never access this array directly; always use <see cref="GetArticles(long)"/>.
		/// </remarks>
		string[] Articles { get; set; }

		/// <summary>
		/// Each record type has a quick-lookup cache in the database that stores this many items.
		/// </summary>
		/// <remarks>
		/// The database uses B-Plus Trees as the backing store, one tree and one quick-lookup cache per record type.  Using a
		/// cache speeds up the record access times since the actual "search" on the tree is more like a full-table scan (this
		/// might get fixed at some point).  This value is initialized using <see cref="Plugin.IPluginConstants.NumCacheItems"/>.
		/// </remarks>
		/// <seealso cref="DataStorage.Generic.IDbTable{T}"/>
		/// <seealso cref="DataStorage.IDatabase"/>
		long NumCacheItems { get; set; }

		/// <summary>
		/// A generic string representing an unknown name (eg, "???").
		/// </summary>
		/// <remarks>
		/// Typically the fallback value when a record with a given Uid is looked up for its Name property, but not found.
		/// </remarks>
		string UnknownName { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Gets the sentence preposition (eg, "to", "from", "inside", etc).
		/// </summary>
		/// <param name="index">The index into the Preps array.</param>
		/// <remarks>
		/// This method looks up and returns the preposition associated with a given index.  It is a Getter method that
		/// can be overridden in a subclass to intercept array accesses or provide other specialized behavior.  You must
		/// ensure the passed index is valid for the Preps array or an exception will be thrown.
		/// </remarks>
		/// <returns>The preposition associated with a given index.</returns>
		string GetPreps(long index);

		/// <summary>
		/// Gets the sentence article (eg, "a", "some", "the", etc).
		/// </summary>
		/// <param name="index">The index into the Articles array.</param>
		/// <remarks>
		/// This method looks up and returns the article associated with a given index.  It is a Getter method that
		/// can be overridden in a subclass to intercept array accesses or provide other specialized behavior.  You must
		/// ensure the passed index is valid for the Articles array or an exception will be thrown.
		/// </remarks>
		/// <returns>The article associated with a given index.</returns>
		string GetArticles(long index);

		string GetNumberStrings(long index);

		string GetFieldDescNames(long index);

		string GetFieldDescNames(Enums.FieldDesc fieldDesc);

		/// <summary>
		/// Gets the name for a given Status.
		/// </summary>
		/// <param name="index">The index into the StatusNames array.</param>
		/// <remarks>
		/// This method looks up and returns the name associated with a given <see cref="Enums.Status"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The name associated with a given Status.</returns>
		/// <seealso cref="GetStatusNames(Enums.Status)"/>
		string GetStatusNames(long index);

		/// <summary>
		/// Gets the name for a given Status.
		/// </summary>
		/// <param name="status">The Status value.</param>
		/// <remarks>
		/// This method looks up and returns the name associated with a given <see cref="Enums.Status"/>.  It is a convenience
		/// wrapper around the <see cref="GetStatusNames(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in Status to a long.
		/// </remarks>
		/// <returns>The name associated with a given Status.</returns>
		string GetStatusNames(Enums.Status status);

		/// <summary>
		/// Gets the name for a given Clothing.
		/// </summary>
		/// <param name="index">The index into the ClothingNames array.</param>
		/// <remarks>
		/// This method looks up and returns the name associated with a given <see cref="Enums.Clothing"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The name associated with a given Clothing.</returns>
		/// <seealso cref="GetClothingNames(Enums.Clothing)"/>
		string GetClothingNames(long index);

		/// <summary>
		/// Gets the name for a given Clothing.
		/// </summary>
		/// <param name="clothing">The Clothing value.</param>
		/// <remarks>
		/// This method looks up and returns the name associated with a given <see cref="Enums.Clothing"/>.  It is a convenience
		/// wrapper around the <see cref="GetClothingNames(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in Clothing to a long.
		/// </remarks>
		/// <returns>The name associated with a given Clothing.</returns>
		string GetClothingNames(Enums.Clothing clothing);

		/// <summary>
		/// Gets the description for a given CombatCode.
		/// </summary>
		/// <param name="index">The index into the CombatCodeDescs array.</param>
		/// <remarks>
		/// This method looks up and returns the description associated with a given <see cref="Enums.CombatCode"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The description associated with a given CombatCode.</returns>
		/// <seealso cref="GetCombatCodeDescs(Enums.CombatCode)"/>
		string GetCombatCodeDescs(long index);

		/// <summary>
		/// Gets the description for a given CombatCode.
		/// </summary>
		/// <param name="combatCode">The CombatCode value.</param>
		/// <remarks>
		/// This method looks up and returns the description associated with a given <see cref="Enums.CombatCode"/>.  It is a convenience
		/// wrapper around the <see cref="GetCombatCodeDescs(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in CombatCode to a long.
		/// </remarks>
		/// <returns>The description associated with a given CombatCode.</returns>
		string GetCombatCodeDescs(Enums.CombatCode combatCode);

		/// <summary>
		/// Gets the name for a given LightLevel.
		/// </summary>
		/// <param name="index">The index into the LightLevelNames array.</param>
		/// <remarks>
		/// This method looks up and returns the name associated with a given <see cref="Enums.LightLevel"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The name associated with a given LightLevel.</returns>
		/// <seealso cref="GetLightLevelNames(Enums.LightLevel)"/>
		string GetLightLevelNames(long index);

		/// <summary>
		/// Gets the name for a given LightLevel.
		/// </summary>
		/// <param name="lightLevel">The LightLevel value.</param>
		/// <remarks>
		/// This method looks up and returns the name associated with a given <see cref="Enums.LightLevel"/>.  It is a convenience
		/// wrapper around the <see cref="GetLightLevelNames(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in LightLevel to a long.
		/// </remarks>
		/// <returns>The name associated with a given LightLevel.</returns>
		string GetLightLevelNames(Enums.LightLevel lightLevel);

		/// <summary>
		/// Gets the data for a given Stat.
		/// </summary>
		/// <param name="index">The index into the Stats array.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Stat"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The data associated with a given Stat.</returns>
		/// <seealso cref="GetStats(Enums.Stat)"/>
		Classes.IStat GetStats(long index);

		/// <summary>
		/// Gets the data for a given Stat.
		/// </summary>
		/// <param name="stat">The Stat value.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Stat"/>.  It is a convenience
		/// wrapper around the <see cref="GetStats(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in Stat to a long.
		/// </remarks>
		/// <returns>The data associated with a given Stat.</returns>
		Classes.IStat GetStats(Enums.Stat stat);

		/// <summary>
		/// Gets the data for a given Spell.
		/// </summary>
		/// <param name="index">The index into the Spells array.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Spell"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The data associated with a given Spell.</returns>
		/// <seealso cref="GetSpells(Enums.Spell)"/>
		Classes.ISpell GetSpells(long index);

		/// <summary>
		/// Gets the data for a given Spell.
		/// </summary>
		/// <param name="spell">The Spell value.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Spell"/>.  It is a convenience
		/// wrapper around the <see cref="GetSpells(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in Spell to a long.
		/// </remarks>
		/// <returns>The data associated with a given Spell.</returns>
		Classes.ISpell GetSpells(Enums.Spell spell);

		/// <summary>
		/// Gets the data for a given Weapon.
		/// </summary>
		/// <param name="index">The index into the Weapons array.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Weapon"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The data associated with a given Weapon.</returns>
		/// <seealso cref="GetWeapons(Enums.Weapon)"/>
		Classes.IWeapon GetWeapons(long index);

		/// <summary>
		/// Gets the data for a given Weapon.
		/// </summary>
		/// <param name="weapon">The Weapon value.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Weapon"/>.  It is a convenience
		/// wrapper around the <see cref="GetWeapons(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in Weapon to a long.
		/// </remarks>
		/// <returns>The data associated with a given Weapon.</returns>
		Classes.IWeapon GetWeapons(Enums.Weapon weapon);

		/// <summary>
		/// Gets the data for a given Armor.
		/// </summary>
		/// <param name="index">The index into the Armors array.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Armor"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The data associated with a given Armor.</returns>
		/// <seealso cref="GetArmors(Enums.Armor)"/>
		Classes.IArmor GetArmors(long index);

		/// <summary>
		/// Gets the data for a given Armor.
		/// </summary>
		/// <param name="armor">The Armor value.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Armor"/>.  It is a convenience
		/// wrapper around the <see cref="GetArmors(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in Armor to a long.
		/// </remarks>
		/// <returns>The data associated with a given Armor.</returns>
		Classes.IArmor GetArmors(Enums.Armor armor);

		/// <summary>
		/// Gets the data for a given Direction.
		/// </summary>
		/// <param name="index">The index into the Directions array.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Direction"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The data associated with a given Direction.</returns>
		/// <seealso cref="GetDirections(Enums.Direction)"/>
		Classes.IDirection GetDirections(long index);

		/// <summary>
		/// Gets the data for a given Direction.
		/// </summary>
		/// <param name="direction">The Direction value.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.Direction"/>.  It is a convenience
		/// wrapper around the <see cref="GetDirections(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in Direction to a long.
		/// </remarks>
		/// <returns>The data associated with a given Direction.</returns>
		Classes.IDirection GetDirections(Enums.Direction direction);

		/// <summary>
		/// Gets the data for a given ArtifactType.
		/// </summary>
		/// <param name="index">The index into the ArtifactTypes array.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.ArtifactType"/>.  It is a Getter
		/// method that can be overridden in a subclass to intercept array accesses or provide other specialized behavior.
		/// </remarks>
		/// <returns>The data associated with a given ArtifactType.</returns>
		/// <seealso cref="GetArtifactTypes(Enums.ArtifactType)"/>
		Classes.IArtifactType GetArtifactTypes(long index);

		/// <summary>
		/// Gets the data for a given ArtifactType.
		/// </summary>
		/// <param name="artifactType">The ArtifactType value.</param>
		/// <remarks>
		/// This method looks up and returns the data associated with a given <see cref="Enums.ArtifactType"/>.  It is a convenience
		/// wrapper around the <see cref="GetArtifactTypes(long)"/> Getter method; as a rule it shouldn't be overridden.  The lower-level
		/// method is called by casting the passed in ArtifactType to a long.
		/// </remarks>
		/// <returns>The data associated with a given ArtifactType.</returns>
		Classes.IArtifactType GetArtifactTypes(Enums.ArtifactType artifactType);

		/// <summary>
		/// Check whether an operation succeeded.
		/// </summary>
		/// <param name="rc">The RetCode value.</param>
		/// <returns>If the operation succeeded, returns true; else returns false.</returns>
		bool IsSuccess(RetCode rc);

		/// <summary>
		/// Check whether an operation failed.
		/// </summary>
		/// <param name="rc">The RetCode value.</param>
		/// <returns>If the operation failed, returns true; else returns false.</returns>
		bool IsFailure(RetCode rc);

		/// <summary>
		/// Check whether a plural type is valid.
		/// </summary>
		/// <param name="pluralType">The PluralType value.</param>
		/// <remarks>
		/// This method checks to see if the supplied plural type is either (1) a member of <see cref="Enums.PluralType"/> or (2) a
		/// customized plural name built using an effect record (of the form 1000 + effectUid).
		/// </remarks>
		/// <returns>If the plural type is valid, returns true; else returns false.</returns>
		bool IsValidPluralType(Enums.PluralType pluralType);

		/// <summary>
		/// Check whether an artifact type is valid.
		/// </summary>
		/// <param name="artifactType">The ArtifactType value.</param>
		/// <remarks>
		/// This method checks to see if the supplied artifact type is a member of <see cref="Enums.ArtifactType"/>.  The None value
		/// is not considered valid.
		/// </remarks>
		/// <returns>If the artifact type is valid, returns true; else returns false.</returns>
		bool IsValidArtifactType(Enums.ArtifactType artifactType);

		/// <summary>
		/// Check whether an armor value is valid for an wearable artifact.
		/// </summary>
		/// <param name="armor">The armor value.</param>
		/// <remarks>
		/// This method checks to see if the supplied armor value is valid for a wearable artifact.  The armor value must be a member
		/// of <see cref="Enums.Armor"/> and also either (1) a shield only or (2) armor only.  This excludes "combo armors" that contain
		/// both armor and shield.
		/// </remarks>
		/// <returns>If the armor value is valid, returns true; else returns false.</returns>
		bool IsValidArtifactArmor(long armor);

		/// <summary>
		/// Check whether an armor value is valid for a monster.
		/// </summary>
		/// <param name="armor">The armor value.</param>
		/// <remarks>
		/// This method checks to see if the supplied armor value is valid for a monster; always the case for any armor value >= 0.
		/// </remarks>
		/// <returns>If the armor value is valid, returns true; else returns false.</returns>
		bool IsValidMonsterArmor(long armor);

		/// <summary>
		/// Check whether a courage value is valid for a monster.
		/// </summary>
		/// <param name="courage">The courage value.</param>
		/// <remarks>
		/// This method checks to see if the supplied courage value is valid for a monster; always the case for any courage value >= 0
		/// and &lt;= 200.
		/// </remarks>
		/// <returns>If the courage value is valid, returns true; else returns false.</returns>
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

		/// <summary>
		/// Check whether a character is one of ['Y', 'N'].
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharYOrN(char ch);

		/// <summary>
		/// Check whether a character is one of ['S', 'T', 'R', 'X'].
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharSOrTOrROrX(char ch);

		/// <summary>
		/// Check whether a character is one of ['0', '1'].
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsChar0Or1(char ch);

		/// <summary>
		/// Check whether a character is one of ['0', '1', '2'].
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsChar0To2(char ch);

		/// <summary>
		/// Check whether a character is one of ['0', '1', '2', '3'].
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsChar0To3(char ch);

		/// <summary>
		/// Check whether a character is one of ['1', '2', '3'].
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsChar1To3(char ch);

		/// <summary>
		/// Check whether a character is a numeric digit.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharDigit(char ch);

		/// <summary>
		/// Check whether a character is a numeric digit or 'X'.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharDigitOrX(char ch);

		/// <summary>
		/// Check whether a character is a numeric digit or one of ['+', '-'].
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharPlusMinusDigit(char ch);

		/// <summary>
		/// Check whether a character is alphabetic.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharAlpha(char ch);

		/// <summary>
		/// Check whether a character is alphabetic or space.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharAlphaSpace(char ch);

		/// <summary>
		/// Check whether a character is alphabetic or numeric digit.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharAlnum(char ch);

		/// <summary>
		/// Check whether a character is alphabetic, numeric digit or space.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharAlnumSpace(char ch);

		/// <summary>
		/// Check whether a character is alphabetic, numeric digit, period or underscore.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharAlnumPeriodUnderscore(char ch);

		/// <summary>
		/// Check whether a character is printable.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharPrint(char ch);

		/// <summary>
		/// Check whether a character is '#'.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharPound(char ch);

		/// <summary>
		/// Check whether a character is a quote.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharQuote(char ch);

		/// <summary>
		/// Check whether a character is any character at all.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>For any valid character, returns true; else returns false.</returns>
		bool IsCharAny(char ch);

		/// <summary>
		/// Check whether a character is any character but one of ['"', ',', ':'].
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharAnyButDquoteCommaColon(char ch);

		/// <summary>
		/// Check whether a character is any character but one of ['\', '/'].
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input validation and termination.
		/// </remarks>
		/// <returns>Based on the character, either true or false.</returns>
		bool IsCharAnyButBackForwardSlash(char ch);

		/// <summary>
		/// Given a character, produce its upper case equivalent, if any.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input modification.
		/// </remarks>
		/// <returns>If the character has an upper case equivalent, returns that; else returns the original character.</returns>
		char ModifyCharToUpper(char ch);

		/// <summary>
		/// Given a character, produce either 'X' or '\0'.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input modification.
		/// </remarks>
		/// <returns>If the character is 'X', returns that; else returns '\0'.</returns>
		char ModifyCharToNullOrX(char ch);

		/// <summary>
		/// Given a character, produce '\0'.
		/// </summary>
		/// <param name="ch">The character value.</param>
		/// <remarks>
		/// This method is used by <see cref="Portability.ITextReader.ReadField"/> for input modification.
		/// </remarks>
		/// <returns>For any character, returns '\0'.</returns>
		char ModifyCharToNull(char ch);

		Enums.Direction GetDirection(string printedName);

		IConfig GetConfig();

		/// <summary>
		/// Gets the IGameState record.
		/// </summary>
		/// <remarks>
		/// This method returns a record of type <see cref="IGameState"/>; there should be at most one of these in the database when
		/// running a game using EamonRT (or a derivative).  Note that when running an EamonRT derivative the returned record may be
		/// cast to a derived IGameState defined in the derived game library, if any.  There are many examples of this, one of
		/// which is <see cref="StrongholdOfKahrDur.Framework.IGameState"/>.  This allows you to easily access any game-specific
		/// methods or properties you've defined in your customized IGameState.
		/// <para>
		/// In other circumstances, this method will return null if no IGameState record is found in the database.  When running
		/// EamonDD this record should never exist and thus null will always be returned.</para>
		/// </remarks>
		/// <returns>
		/// A record of type IGameState (or a game-specific subclass of it) or null.
		/// </returns>
		IGameState GetGameState();

		/// <summary>
		/// Gets the IModule record.
		/// </summary>
		/// <remarks>
		/// This method returns a record of type <see cref="IModule"/>; there should be at most one of these in the database when
		/// editing an adventure using EamonDD or running a game using EamonRT (or a derivative).  Note that when running an EamonRT
		/// derivative the returned record may be cast to a derived IModule defined in the derived game library, if any (it would
		/// have been loaded out of the game's MODULE.XML file).  This allows you to easily access any game-specific methods or
		/// properties you've defined in your customized IModule.
		/// <para>
		/// In other circumstances, this method will return null if no IModule record is found in the database.  This can occur
		/// in EamonDD when an adventure is not being edited.</para>
		/// </remarks>
		/// <returns>
		/// A record of type IModule (or a game-specific subclass of it) or null.
		/// </returns>
		IModule GetModule();

		/// <summary>
		/// Evaluates the friendliness, returning a value of type T.
		/// </summary>
		/// <typeparam name="T">The type of the value returned.</typeparam>
		/// <param name="friendliness">The friendliness.</param>
		/// <param name="enemyValue">The enemy value.</param>
		/// <param name="neutralValue">The neutral value.</param>
		/// <param name="friendValue">The friend value.</param>
		/// <remarks>
		/// This is a convenience macro that switches on the input friendliness value, returning
		/// the corresponding value of type T.
		/// </remarks>
		/// <returns>If friendliness is Enemy, returns enemyValue; if Neutral, returns neutralValue; else returns friendValue.</returns>
		T EvalFriendliness<T>(Enums.Friendliness friendliness, T enemyValue, T neutralValue, T friendValue);

		/// <summary>
		/// Evaluates the gender, returning a value of type T.
		/// </summary>
		/// <typeparam name="T">The type of the value returned.</typeparam>
		/// <param name="gender">The gender.</param>
		/// <param name="maleValue">The male value.</param>
		/// <param name="femaleValue">The female value.</param>
		/// <param name="neutralValue">The neutral value.</param>
		/// <remarks>
		/// This is a convenience macro that switches on the input gender value, returning
		/// the corresponding value of type T.
		/// </remarks>
		/// <returns>If gender is Male, returns maleValue; if Female, returns femaleValue; else returns neutralValue.</returns>
		T EvalGender<T>(Enums.Gender gender, T maleValue, T femaleValue, T neutralValue);

		/// <summary>
		/// Evaluates the room type, returning a value of type T.
		/// </summary>
		/// <typeparam name="T">The type of the value returned.</typeparam>
		/// <param name="roomType">The room type.</param>
		/// <param name="indoorsValue">The indoors value.</param>
		/// <param name="outdoorsValue">The outdoors value.</param>
		/// <remarks>
		/// This is a convenience macro that switches on the input roomType value, returning
		/// the corresponding value of type T.
		/// </remarks>
		/// <returns>If roomType is Indoors, returns indoorsValue; else returns outdoorsValue.</returns>
		T EvalRoomType<T>(Enums.RoomType roomType, T indoorsValue, T outdoorsValue);

		/// <summary>
		/// Evaluates the light level, returning a value of type T.
		/// </summary>
		/// <typeparam name="T">The type of the value returned.</typeparam>
		/// <param name="lightLevel">The light level.</param>
		/// <param name="darkValue">The dark value.</param>
		/// <param name="lightValue">The light value.</param>
		/// <remarks>
		/// This is a convenience macro that switches on the input lightLevel value, returning
		/// the corresponding value of type T.
		/// </remarks>
		/// <returns>If lightLevel is Dark, returns darkValue; else returns lightValue.</returns>
		T EvalLightLevel<T>(Enums.LightLevel lightLevel, T darkValue, T lightValue);

		/// <summary>
		/// Evaluates the plural value, returning a value of type T.
		/// </summary>
		/// <typeparam name="T">The type of the value returned.</typeparam>
		/// <param name="isPlural">If set to <c>true</c> then is plural.</param>
		/// <param name="singularValue">The singular value.</param>
		/// <param name="pluralValue">The plural value.</param>
		/// <remarks>
		/// This is a convenience macro that switches on the input isPlural value, returning
		/// the corresponding value of type T.
		/// </remarks>
		/// <returns>If isPlural is false, returns singularValue; else returns pluralValue.</returns>
		T EvalPlural<T>(bool isPlural, T singularValue, T pluralValue);

		string BuildPrompt(long bufSize, char fillChar, long number, string msg, string emptyVal);

		string BuildValue(long bufSize, char fillChar, long offset, long longVal, string stringVal, string lookupMsg);

		string WordWrap(string str, StringBuilder buf, long margin, IWordWrapArgs args, bool clearBuf = true);

		string WordWrap(string str, StringBuilder buf, bool clearBuf = true);

		string LineWrap(string str, StringBuilder buf, long startColumn, bool clearBuf = true);

		string GetStringFromNumber(long num, bool addSpace, StringBuilder buf);

		long GetNumberFromString(string str);

		/// <summary>
		/// Gets the Blast spell description.
		/// </summary>
		/// <remarks>
		/// This method, like all others, can be overridden in a game to provide customization.
		/// </remarks>
		/// <returns>The string printed when a Blast spell is successfully cast.</returns>
		string GetBlastDesc();

		string GetAttackDescString(Enums.Weapon weapon, long roll);

		string GetMissDescString(Enums.Weapon weapon, long roll);

		/// <summary>
		/// Rolls a number of dice, storing the resulting values in an array.
		/// </summary>
		/// <param name="numDice">The number dice to roll.</param>
		/// <param name="numSides">The number sides per die.</param>
		/// <param name="dieRolls">The array of die roll results.</param>
		/// <returns>Success, InvalidArg</returns>
		RetCode RollDice(long numDice, long numSides, ref long[] dieRolls);

		/// <summary>
		/// Rolls a number of dice, returning a sum of the results.
		/// </summary>
		/// <param name="numDice">The number dice to roll.</param>
		/// <param name="numSides">The number sides per die.</param>
		/// <param name="modifier">An optional modifier to add to the sum; may be any value (including 0).</param>
		/// <param name="result">The summed result of the die rolls, plus the modifier (if any).</param>
		/// <remarks>
		/// This is the dice-rolling function for Eamon CS.  It rolls a number of dice, each with a given number of sides
		/// and sums the results.  An optional modifier is then added in; this modifier may be negative, positive or zero.
		/// The common roleplaying game nomenclature would be to roll XdY+Z.  While you can use this method directly, there
		/// is a convenience method <see cref="RollDice01(long, long, long)"/> that is recommended.</remarks>
		/// <returns>Success, InvalidArg</returns>
		RetCode RollDice(long numDice, long numSides, long modifier, ref long result);

		/// <summary>
		/// Rolls a number of dice, returning a sum of the results.
		/// </summary>
		/// <param name="numDice">The number dice to roll.</param>
		/// <param name="numSides">The number sides per die.</param>
		/// <param name="modifier">An optional modifier to add to the sum; may be any value (including 0).</param>
		/// <remarks>
		/// This is a convenience wrapper around <see cref="RollDice(long, long, long, ref long)"/> which should be used
		/// whenever possible.</remarks>
		/// <returns>The summed result of the die rolls, plus the modifier (if any).</returns>
		long RollDice01(long numDice, long numSides, long modifier);

		/// <summary>
		/// Given an array of die rolls, sum the highest of them and return the result.
		/// </summary>
		/// <param name="dieRolls">The array of die rolls.</param>
		/// <param name="numRollsToSum">The number of die rolls to sum.</param>
		/// <param name="result">The summed result of the highest die rolls.</param>
		/// <remarks>
		/// This method takes an array of die rolls, sorts the array in place from lowest to highest roll, then sums the
		/// requested number of rolls and returns the result.
		/// <para>
		/// It is intended to be used in conjunction with the <see cref="RollDice(long, long, ref long[])"/> method.</para>
		/// </remarks>
		/// <returns>Success, InvalidArg</returns>
		RetCode SumHighestRolls(long[] dieRolls, long numRollsToSum, ref long result);

		RetCode BuildCommandList(IList<ICommand> commands, Enums.CommandType cmdType, StringBuilder buf, ref bool newSeen);

		string Capitalize(string str);

		/// <summary>
		/// Deletes a set of game-related files from the filesystem.
		/// </summary>
		/// <remarks>
		/// This method is called by EamonRT during game startup if any of the following are true:
		/// <list type="bullet">
		/// <item><description>The Character record read from FRESHMEAT.XML is missing.</description></item>
		/// <item><description>The Module record read from MODULE.XML is missing.</description></item>
		/// <item><description>The Room record (where the game begins) read from ROOMS.XML is missing.</description></item>
		/// </list>
		/// The above cases are considered fatal errors and this method is called to clean up the filesystem before the
		/// game aborts.  The exact files deleted are EAMONCFG.XML, FRESHMEAT.XML and SAVEGAME.XML.  These files are created
		/// during the transfer of the player into the adventure and are no longer needed once shutdown completes.
		/// </remarks>
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

		RetCode GetRecordNameList(IList<IGameBase> records, Enums.ArticleType articleType, bool showCharOwned, bool showStateDesc, bool groupCountOne, StringBuilder buf);

		RetCode GetRecordNameCount(IList<IGameBase> records, string name, bool exactMatch, ref long count);

		RetCode ListRecords(IList<IGameBase> records, bool capitalize, bool showExtraInfo, StringBuilder buf);

		RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse, ref long invalidUid);

		RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse);

		double GetWeaponPriceOrValue(string name, long complexity, Enums.Weapon type, long dice, long sides, bool calcPrice, ref bool isMarcosWeapon);

		double GetWeaponPriceOrValue(Classes.ICharacterWeapon weapon, bool calcPrice, ref bool isMarcosWeapon);

		double GetArmorPriceOrValue(Enums.Armor armor, bool calcPrice, ref bool isMarcosArmor);

		void AppendFieldDesc(Enums.FieldDesc fieldDesc, StringBuilder buf, StringBuilder fullDesc, StringBuilder briefDesc);

		void AppendFieldDesc(Enums.FieldDesc fieldDesc, StringBuilder buf, string fullDesc, string briefDesc);

		void CopyArtifactCategoryFields(Classes.IArtifactCategory destAc, Classes.IArtifactCategory sourceAc);

		IList<IArtifact> GetArtifactList(Func<bool> shouldQueryFunc, params Func<IArtifact, bool>[] whereClauseFuncs);

		IList<IMonster> GetMonsterList(Func<bool> shouldQueryFunc, params Func<IMonster, bool>[] whereClauseFuncs);

		IList<IGameBase> GetRecordList(Func<bool> shouldQueryFunc, params Func<IGameBase, bool>[] whereClauseFuncs);

		IArtifact GetNthArtifact(IList<IArtifact> artifactList, long which, Func<IArtifact, bool> whereClauseFunc);

		IMonster GetNthMonster(IList<IMonster> monsterList, long which, Func<IMonster, bool> whereClauseFunc);

		IGameBase GetNthRecord(IList<IGameBase> recordList, long which, Func<IGameBase, bool> whereClauseFunc);

		void StripPoundCharsFromRecordNames(IList<IGameBase> recordList);

		void AddPoundCharsToRecordNames(IList<IGameBase> recordList);

		void ConvertWeaponToGoldOrTreasure(IArtifact artifact, bool convertToGold);

		#endregion
	}
}
