
// IDatabase.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Framework.DataStorage.Generic;

namespace Eamon.Framework.DataStorage
{
	public interface IDatabase
	{
		IDbTable<IConfig> ConfigTable { get; set; }

		IDbTable<IFileset> FilesetTable { get; set; }

		IDbTable<ICharacter> CharacterTable { get; set; }

		IDbTable<IModule> ModuleTable { get; set; }

		IDbTable<IRoom> RoomTable { get; set; }

		IDbTable<IArtifact> ArtifactTable { get; set; }

		IDbTable<IEffect> EffectTable { get; set; }

		IDbTable<IMonster> MonsterTable { get; set; }

		IDbTable<IHint> HintTable { get; set; }

		IDbTable<IGameState> GameStateTable { get; set; }

		RetCode LoadRecords<T>(ref IDbTable<T> table, string fileName, bool validate = true, bool printOutput = true) where T : class, IGameBase;

		RetCode LoadConfigs(string fileName, bool validate = true, bool printOutput = true);

		RetCode LoadFilesets(string fileName, bool validate = true, bool printOutput = true);

		RetCode LoadCharacters(string fileName, bool validate = true, bool printOutput = true);

		RetCode LoadModules(string fileName, bool validate = true, bool printOutput = true);

		RetCode LoadRooms(string fileName, bool validate = true, bool printOutput = true);

		RetCode LoadArtifacts(string fileName, bool validate = true, bool printOutput = true);

		RetCode LoadEffects(string fileName, bool validate = true, bool printOutput = true);

		RetCode LoadMonsters(string fileName, bool validate = true, bool printOutput = true);

		RetCode LoadHints(string fileName, bool validate = true, bool printOutput = true);

		RetCode LoadGameStates(string fileName, bool validate = true, bool printOutput = true);

		RetCode SaveRecords<T>(IDbTable<T> table, string fileName, bool printOutput = true) where T : class, IGameBase;

		RetCode SaveConfigs(string fileName, bool printOutput = true);

		RetCode SaveFilesets(string fileName, bool printOutput = true);

		RetCode SaveCharacters(string fileName, bool printOutput = true);

		RetCode SaveModules(string fileName, bool printOutput = true);

		RetCode SaveRooms(string fileName, bool printOutput = true);

		RetCode SaveArtifacts(string fileName, bool printOutput = true);

		RetCode SaveEffects(string fileName, bool printOutput = true);

		RetCode SaveMonsters(string fileName, bool printOutput = true);

		RetCode SaveHints(string fileName, bool printOutput = true);

		RetCode SaveGameStates(string fileName, bool printOutput = true);

		RetCode FreeRecords<T>(IDbTable<T> table, bool dispose = true) where T : class, IGameBase;

		RetCode FreeConfigs(bool dispose = true);

		RetCode FreeFilesets(bool dispose = true);

		RetCode FreeCharacters(bool dispose = true);

		RetCode FreeModules(bool dispose = true);

		RetCode FreeRooms(bool dispose = true);

		RetCode FreeArtifacts(bool dispose = true);

		RetCode FreeEffects(bool dispose = true);

		RetCode FreeMonsters(bool dispose = true);

		RetCode FreeHints(bool dispose = true);

		RetCode FreeGameStates(bool dispose = true);

		long GetRecordsCount<T>(IDbTable<T> table) where T : class, IGameBase;

		long GetConfigsCount();

		long GetFilesetsCount();

		long GetCharactersCount();

		long GetModulesCount();

		long GetRoomsCount();

		long GetArtifactsCount();

		long GetEffectsCount();

		long GetMonstersCount();

		long GetHintsCount();

		long GetGameStatesCount();

		T FindRecord<T>(IDbTable<T> table, long uid) where T : class, IGameBase;

		IConfig FindConfig(long uid);

		IFileset FindFileset(long uid);

		ICharacter FindCharacter(long uid);

		IModule FindModule(long uid);

		IRoom FindRoom(long uid);

		IArtifact FindArtifact(long uid);

		IEffect FindEffect(long uid);

		IMonster FindMonster(long uid);

		IHint FindHint(long uid);

		IGameState FindGameState(long uid);

		T FindRecord<T>(IDbTable<T> table, Type type, bool exactMatch = false) where T : class, IGameBase;

		RetCode AddRecord<T>(IDbTable<T> table, T record, bool makeCopy = false) where T : class, IGameBase;

		RetCode AddConfig(IConfig config, bool makeCopy = false);

		RetCode AddFileset(IFileset fileset, bool makeCopy = false);

		RetCode AddCharacter(ICharacter character, bool makeCopy = false);

		RetCode AddModule(IModule module, bool makeCopy = false);

		RetCode AddRoom(IRoom room, bool makeCopy = false);

		RetCode AddArtifact(IArtifact artifact, bool makeCopy = false);

		RetCode AddEffect(IEffect effect, bool makeCopy = false);

		RetCode AddMonster(IMonster monster, bool makeCopy = false);

		RetCode AddHint(IHint hint, bool makeCopy = false);

		RetCode AddGameState(IGameState gameState, bool makeCopy = false);

		T RemoveRecord<T>(IDbTable<T> table, long uid) where T : class, IGameBase;

		IConfig RemoveConfig(long uid);

		IFileset RemoveFileset(long uid);

		ICharacter RemoveCharacter(long uid);

		IModule RemoveModule(long uid);

		IRoom RemoveRoom(long uid);

		IArtifact RemoveArtifact(long uid);

		IEffect RemoveEffect(long uid);

		IMonster RemoveMonster(long uid);

		IHint RemoveHint(long uid);

		IGameState RemoveGameState(long uid);

		T RemoveRecord<T>(IDbTable<T> table, Type type, bool exactMatch = false) where T : class, IGameBase;

		long GetRecordUid<T>(IDbTable<T> table, bool allocate = true) where T : class, IGameBase;

		long GetConfigUid(bool allocate = true);

		long GetFilesetUid(bool allocate = true);

		long GetCharacterUid(bool allocate = true);

		long GetModuleUid(bool allocate = true);

		long GetRoomUid(bool allocate = true);

		long GetArtifactUid(bool allocate = true);

		long GetEffectUid(bool allocate = true);

		long GetMonsterUid(bool allocate = true);

		long GetHintUid(bool allocate = true);

		long GetGameStateUid(bool allocate = true);

		void FreeRecordUid<T>(IDbTable<T> table, long uid) where T : class, IGameBase;

		void FreeConfigUid(long uid);

		void FreeFilesetUid(long uid);

		void FreeCharacterUid(long uid);

		void FreeModuleUid(long uid);

		void FreeRoomUid(long uid);

		void FreeArtifactUid(long uid);

		void FreeEffectUid(long uid);

		void FreeMonsterUid(long uid);

		void FreeHintUid(long uid);

		void FreeGameStateUid(long uid);
	}
}
