
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using Eamon;
using Eamon.Framework;
using EamonRT.Framework.Commands;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework
{
	public interface IEngine : EamonDD.Framework.IEngine
	{
		long StartRoom { get; set; }

		long NumSaveSlots { get; set; }

		long ScaledHardinessUnarmedMaxDamage { get; set; }

		double ScaledHardinessMaxDamageDivisor { get; set; }

		bool EnforceMonsterWeightLimits { get; set; }

		bool UseMonsterScaledHardinessValues { get; set; }

		Enums.PoundCharPolicy PoundCharPolicy { get; set; }

		void PrintMonsterAlive(IArtifact artifact);

		long WeaponPowerCompare(IArtifact artifact1, IArtifact artifact2);

		long WeaponPowerCompare(long artifactUid1, long artifactUid2);

		IArtifact GetMostPowerfulWeapon(IList<IArtifact> artifactList);

		long GetMostPowerfulWeaponUid(IList<IArtifact> artifactList);

		void InitWtValueAndEnforceLimits();

		void AddPoundCharsToArtifactNames();

		void AddMissingDescs();

		void InitSaArray();

		void CreateCommands();

		void InitArtifacts();

		void InitMonsters();

		void InitMonsterScaledHardinessValues();

		IArtifact ConvertWeaponToArtifact(Classes.ICharacterArtifact weapon);

		Classes.ICharacterArtifact ConvertArtifactToWeapon(IArtifact artifact);

		IMonster ConvertArtifactToMonster(IArtifact artifact, Action<IMonster> initialize = null, bool addToDatabase = false);

		IMonster ConvertCharacterToMonster();

		void ConvertMonsterToCharacter(IMonster monster, IList<IArtifact> weaponList);

		void ResetMonsterStats(IMonster monster);

		void SetArmorClass();

		void ConvertToCarriedInventory(IList<IArtifact> weaponList);

		void SellExcessWeapons(IList<IArtifact> weaponList);

		void SellInventoryToMerchant(bool sellInventory = true);

		void DeadMenu(IMonster monster, bool printLineSep, ref bool restoreGame);

		void LightOut(IArtifact artifact);

		void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true);

		void MonsterSmiles(IMonster monster);

		void MonsterDies(IMonster OfMonster, IMonster DfMonster);

		void ProcessMonsterDeathEvents(IMonster monster);

		void RevealDisguisedMonster(IArtifact artifact);

		void RevealEmbeddedArtifact(IRoom room, IArtifact artifact);

		void RemoveWeight(IArtifact artifact);

		IArtifact GetBlockedDirectionArtifact(long ro, long r2, Enums.Direction dir);

		void CheckDoor(IRoom room, IArtifact artifact, ref bool found, ref long roomUid);

		void CheckNumberOfExits(IRoom room, IMonster monster, bool fleeing, ref long numExits);

		void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Enums.Direction direction, ref bool found, ref long roomUid);

		void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Enums.Direction direction);

		IList<IMonster> GetRandomMonsterList(long numMonsters, params Func<IMonster, bool>[] whereClauseFuncs);

		IList<IArtifact> FilterArtifactList(IList<IArtifact> artifactList, string name);

		IList<IMonster> FilterMonsterList(IList<IMonster> monsterList, string name);

		IList<IGameBase> FilterRecordList(IList<IGameBase> recordList, string name);

		IList<IArtifact> GetReadyableWeaponList(IMonster monster);

		IList<IMonster> GetHostileMonsterList(IMonster monster);

		RetCode BuildCommandList(IList<ICommand> commands, Enums.CommandType cmdType, StringBuilder buf, ref bool newSeen);

		bool ResurrectDeadBodies(params Func<IArtifact, bool>[] whereClauseFuncs);

		bool MakeArtifactsVanish(params Func<IArtifact, bool>[] whereClauseFuncs);

		bool CheckNBTLHostility(IMonster monster);

		bool CheckCourage(IMonster monster);

		bool CheckPlayerSpellCast(Enums.Spell spellValue, bool shouldAllowSkillGains);

		void CheckPlayerSkillGains(Classes.IArtifactCategory ac, long af);

		/// <summary>
		/// Checks to see if the player command should be allowed to proceed.
		/// </summary>
		/// <param name="command">The command to check.</param>
		/// <param name="afterFinishParsing">A flag indicating whether the direct/indirect objects have been resolved.</param>
		/// <remarks>
		/// This method takes the command identified during the parsing process and checks to see if it
		/// should be allowed to proceed.  If afterFinishParsing is false, the method is called immediately
		/// once the command is identified; the direct/indirect objects have not been parsed or resolved at
		/// this point.  If the flag is true, the direct/indirect objects have been parsed and resolved.
		/// You can examine the command passed and take whatever actions you deem necessary, including
		/// outputting text to the player.  If you want to abort or redirect the command you should set
		/// its CommandParser.NextState property to some other state (or null).
		/// </remarks>
		void CheckPlayerCommand(ICommand command, bool afterFinishParsing);

		void CheckToExtinguishLightSource();

		void TransportRoomContentsBetweenRooms(IRoom oldRoom, IRoom newRoom, bool includeEmbedded = true);

		void TransportPlayerBetweenRooms(IRoom oldRoom, IRoom newRoom, IEffect effect);

		/// <summary>
		/// Prints the string passed in, after doing full macro replacement on it (if necessary).  If the
		/// string contains page separators it will be printed a page at a time.
		/// </summary>
		/// <param name="str">The string to process and print.</param>
		/// <param name="buf">The buffer used during string processing.</param>
		/// <remarks>
		/// Macro replacement is done using <see cref="Eamon.Framework.IEngine.ResolveUidMacros"/>.  If the
		/// printed string contains the page break macro <see cref="Plugin.IPluginConstants.PageSep"/> it will
		/// be split into pages and each page will be displayed, followed by a required user keypress.  The
		/// passed in buffer is used during processing and its original contents will be overwritten.  The
		/// passed in string will not be modified.  This method is used to print the intro story as well as
		/// hints, such as the author's notes.
		/// </remarks>
		void PrintMacroReplacedPagedString(string str, StringBuilder buf);

		void CreateArtifactSynonyms(long artifactUid, params string[] synonyms);

		void CreateMonsterSynonyms(long monsterUid, params string[] synonyms);

		void GetOddsToHit(IMonster ofMonster, IMonster dfMonster, Classes.IArtifactCategory ac, long af, ref long oddsToHit);

		void CreateInitialState(bool printLineSep);

		void CheckEnemies();

		void MoveMonsters();

		void RtProcessArgv(bool secondPass, ref bool nlFlag);
	};
}
