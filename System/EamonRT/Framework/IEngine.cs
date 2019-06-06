﻿
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Commands;

namespace EamonRT.Framework
{
	/// <summary></summary>
	public interface IEngine : EamonDD.Framework.IEngine
	{
		/// <summary></summary>
		long StartRoom { get; set; }

		/// <summary></summary>
		long NumSaveSlots { get; set; }

		/// <summary></summary>
		long ScaledHardinessUnarmedMaxDamage { get; set; }

		/// <summary></summary>
		double ScaledHardinessMaxDamageDivisor { get; set; }

		/// <summary></summary>
		bool EnforceMonsterWeightLimits { get; set; }

		/// <summary></summary>
		bool UseMonsterScaledHardinessValues { get; set; }

		/// <summary></summary>
		PoundCharPolicy PoundCharPolicy { get; set; }

		/// <summary></summary>
		void PrintPlayerRoom();

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintMonsterAlive(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact1"></param>
		/// <param name="artifact2"></param>
		/// <returns></returns>
		long WeaponPowerCompare(IArtifact artifact1, IArtifact artifact2);

		/// <summary></summary>
		/// <param name="artifactUid1"></param>
		/// <param name="artifactUid2"></param>
		/// <returns></returns>
		long WeaponPowerCompare(long artifactUid1, long artifactUid2);

		/// <summary></summary>
		/// <param name="artifactList"></param>
		/// <returns></returns>
		IArtifact GetMostPowerfulWeapon(IList<IArtifact> artifactList);

		/// <summary></summary>
		/// <param name="artifactList"></param>
		/// <returns></returns>
		long GetMostPowerfulWeaponUid(IList<IArtifact> artifactList);

		/// <summary></summary>
		void EnforceCharacterWeightLimits();

		/// <summary></summary>
		void AddPoundCharsToArtifactNames();

		/// <summary></summary>
		void AddMissingDescs();

		/// <summary></summary>
		void InitSaArray();

		/// <summary></summary>
		void CreateCommands();

		/// <summary></summary>
		void InitArtifacts();

		/// <summary></summary>
		void InitMonsters();

		/// <summary></summary>
		void InitMonsterScaledHardinessValues();

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <returns></returns>
		IArtifact ConvertWeaponToArtifact(ICharacterArtifact weapon);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		ICharacterArtifact ConvertArtifactToWeapon(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="initialize"></param>
		/// <param name="addToDatabase"></param>
		/// <returns></returns>
		IMonster ConvertArtifactToMonster(IArtifact artifact, Action<IMonster> initialize = null, bool addToDatabase = false);

		/// <summary></summary>
		/// <returns></returns>
		IMonster ConvertCharacterToMonster();

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="weaponList"></param>
		void ConvertMonsterToCharacter(IMonster monster, IList<IArtifact> weaponList);

		/// <summary></summary>
		/// <param name="monster"></param>
		void ResetMonsterStats(IMonster monster);

		/// <summary></summary>
		void SetArmorClass();

		/// <summary></summary>
		/// <param name="weaponList"></param>
		void ConvertToCarriedInventory(IList<IArtifact> weaponList);

		/// <summary></summary>
		/// <param name="weaponList"></param>
		void SellExcessWeapons(IList<IArtifact> weaponList);

		/// <summary></summary>
		/// <param name="sellInventory"></param>
		void SellInventoryToMerchant(bool sellInventory = true);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="printLineSep"></param>
		/// <param name="restoreGame"></param>
		void DeadMenu(IMonster monster, bool printLineSep, ref bool restoreGame);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void LightOut(IArtifact artifact);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="printFinalNewLine"></param>
		void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true);

		/// <summary></summary>
		/// <param name="monster"></param>
		void MonsterSmiles(IMonster monster);

		/// <summary></summary>
		/// <param name="OfMonster"></param>
		/// <param name="DfMonster"></param>
		void MonsterDies(IMonster OfMonster, IMonster DfMonster);

		/// <summary></summary>
		/// <param name="monster"></param>
		void ProcessMonsterDeathEvents(IMonster monster);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void RevealDisguisedMonster(IArtifact artifact);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		void RevealEmbeddedArtifact(IRoom room, IArtifact artifact);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		/// <param name="containerContentsList"></param>
		void RevealExtendedContainerContents(IRoom room, IArtifact artifact, IList<string> containerContentsList = null);

		/// <summary></summary>
		/// <param name="ro"></param>
		/// <param name="r2"></param>
		/// <param name="dir"></param>
		/// <returns></returns>
		IArtifact GetBlockedDirectionArtifact(long ro, long r2, Direction dir);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		/// <param name="found"></param>
		/// <param name="roomUid"></param>
		void CheckDoor(IRoom room, IArtifact artifact, ref bool found, ref long roomUid);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="fleeing"></param>
		/// <param name="numExits"></param>
		void CheckNumberOfExits(IRoom room, IMonster monster, bool fleeing, ref long numExits);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="fleeing"></param>
		/// <param name="direction"></param>
		/// <param name="found"></param>
		/// <param name="roomUid"></param>
		void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction, ref bool found, ref long roomUid);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="fleeing"></param>
		/// <param name="direction"></param>
		void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction);

		/// <summary></summary>
		/// <param name="numMonsters"></param>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		IList<IMonster> GetRandomMonsterList(long numMonsters, params Func<IMonster, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="artifactList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		IList<IArtifact> FilterArtifactList(IList<IArtifact> artifactList, string name);

		/// <summary></summary>
		/// <param name="monsterList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		IList<IMonster> FilterMonsterList(IList<IMonster> monsterList, string name);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		IList<IGameBase> FilterRecordList(IList<IGameBase> recordList, string name);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		IList<IArtifact> GetReadyableWeaponList(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		IList<IMonster> GetHostileMonsterList(IMonster monster);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <returns></returns>
		IList<IMonster> GetSmilingMonsterList(IRoom room, IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		IList<IArtifact> BuildLoopArtifactList(IMonster monster);

		/// <summary></summary>
		/// <param name="commands"></param>
		/// <param name="cmdType"></param>
		/// <param name="buf"></param>
		/// <param name="newSeen"></param>
		/// <returns></returns>
		RetCode BuildCommandList(IList<ICommand> commands, CommandType cmdType, StringBuilder buf, ref bool newSeen);

		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		bool ResurrectDeadBodies(params Func<IArtifact, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		bool MakeArtifactsVanish(params Func<IArtifact, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="spellValue"></param>
		/// <param name="shouldAllowSkillGains"></param>
		/// <returns></returns>
		bool CheckPlayerSpellCast(Spell spellValue, bool shouldAllowSkillGains);

		/// <summary></summary>
		/// <param name="ac"></param>
		/// <param name="af"></param>
		void CheckPlayerSkillGains(IArtifactCategory ac, long af);

		/// <summary></summary>
		/// <param name="command"></param>
		/// <param name="afterFinishParsing"></param>
		void CheckPlayerCommand(ICommand command, bool afterFinishParsing);

		/// <summary></summary>
		void CheckToExtinguishLightSource();

		/// <summary></summary>
		/// <param name="oldRoom"></param>
		/// <param name="newRoom"></param>
		/// <param name="includeEmbedded"></param>
		void TransportRoomContentsBetweenRooms(IRoom oldRoom, IRoom newRoom, bool includeEmbedded = true);

		/// <summary></summary>
		/// <param name="oldRoom"></param>
		/// <param name="newRoom"></param>
		/// <param name="effect"></param>
		void TransportPlayerBetweenRooms(IRoom oldRoom, IRoom newRoom, IEffect effect);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		void PrintMacroReplacedPagedString(string str, StringBuilder buf);

		/// <summary></summary>
		/// <param name="artifactUid"></param>
		/// <param name="synonyms"></param>
		void CreateArtifactSynonyms(long artifactUid, params string[] synonyms);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <param name="synonyms"></param>
		void CreateMonsterSynonyms(long monsterUid, params string[] synonyms);

		/// <summary></summary>
		/// <param name="ofMonster"></param>
		/// <param name="dfMonster"></param>
		/// <param name="ac"></param>
		/// <param name="af"></param>
		/// <param name="oddsToHit"></param>
		void GetOddsToHit(IMonster ofMonster, IMonster dfMonster, IArtifactCategory ac, long af, ref long oddsToHit);

		/// <summary></summary>
		/// <param name="printLineSep"></param>
		void CreateInitialState(bool printLineSep);

		/// <summary></summary>
		void MoveMonsters();

		/// <summary></summary>
		/// <param name="secondPass"></param>
		/// <param name="nlFlag"></param>
		void RtProcessArgv(bool secondPass, ref bool nlFlag);
	};
}
