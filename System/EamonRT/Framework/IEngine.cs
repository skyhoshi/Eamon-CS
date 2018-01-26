
// IEngine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework;
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

		IArtifact ConvertWeaponToArtifact(Classes.ICharacterWeapon weapon);

		Classes.ICharacterWeapon ConvertArtifactToWeapon(IArtifact artifact);

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

		bool ResurrectDeadBodies(params Func<IArtifact, bool>[] whereClauseFuncs);

		bool MakeArtifactsVanish(params Func<IArtifact, bool>[] whereClauseFuncs);

		bool CheckNBTLHostility(IMonster monster);

		bool CheckCourage(IMonster monster);

		bool CheckPlayerSpellCast(Enums.Spell spellValue, bool shouldAllowSkillGains);

		void CheckPlayerSkillGains(Classes.IArtifactClass ac, long af);

		void TransportRoomContentsBetweenRooms(IRoom oldRoom, IRoom newRoom, bool includeEmbedded = true);

		void TransportPlayerBetweenRooms(IRoom oldRoom, IRoom newRoom, IEffect effect);

		void CreateArtifactSynonyms(long artifactUid, params string[] synonyms);

		void CreateMonsterSynonyms(long monsterUid, params string[] synonyms);

		void GetOddsToHit(IMonster ofMonster, IMonster dfMonster, Classes.IArtifactClass ac, long af, ref long oddsToHit);

		void CreateInitialState(bool printLineSep);

		void CheckEnemies();

		void MoveMonsters();

		void RtProcessArgv(bool secondPass, ref bool nlFlag);
	};
}
