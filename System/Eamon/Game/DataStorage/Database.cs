﻿
// Database.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Helpers;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage
{
	[ClassMappings]
	public class Database : IDatabase
	{
		public virtual IDbTable<IConfig> ConfigTable { get; set; }

		public virtual IDbTable<IFileset> FilesetTable { get; set; }

		public virtual IDbTable<ICharacter> CharacterTable { get; set; }

		public virtual IDbTable<IModule> ModuleTable { get; set; }

		public virtual IDbTable<IRoom> RoomTable { get; set; }

		public virtual IDbTable<IArtifact> ArtifactTable { get; set; }

		public virtual IDbTable<IEffect> EffectTable { get; set; }

		public virtual IDbTable<IMonster> MonsterTable { get; set; }

		public virtual IDbTable<IHint> HintTable { get; set; }

		public virtual IDbTable<ITrigger> TriggerTable { get; set; }

		public virtual IDbTable<IScript> ScriptTable { get; set; }

		public virtual IDbTable<IGameState> GameStateTable { get; set; }

		public virtual RetCode LoadRecords<T, U>(ref IDbTable<T> table, string fileName, bool validate = true, bool printOutput = true) where T : class, IGameBase where U : class, IHelper<T>
		{
			IDbTable<T> table01;
			RetCode rc;

			if (table == null || table.Records == null || string.IsNullOrWhiteSpace(fileName))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (printOutput)
			{
				gOut.Write("{0}Loading textfile [{1}] ... ", Environment.NewLine, fileName);
			}

			try
			{
				Globals.UpgradeTextfile(fileName);

				table01 = Globals.TextSerializer.Deserialize<IDbTable<T>>(fileName);
			}
			catch (FileNotFoundException)
			{
				table01 = null;
			}

			if (table01 == null || table01.Records == null)
			{
				table01 = Globals.CreateInstance<IDbTable<T>>();

				Debug.Assert(table01 != null && table01.Records != null);
			}

			if (validate)
			{
				var helper = Globals.CreateInstance<U>();

				long i = 1;

				foreach (var r in table01.Records)
				{
					helper.Record = r;

					if (helper.ValidateRecord() == false)
					{
						rc = RetCode.Failure;

						Globals.Error.WriteLine("{0}Error: expected valid [{1} value], found [{2}]", Environment.NewLine, helper.GetName(helper.ErrorFieldName), helper.GetValue(helper.ErrorFieldName) ?? "null");

						Globals.Error.WriteLine("Error: Validate function call failed for record number {0}", i);

						goto Cleanup;
					}

					i++;
				}
			}

			foreach (var r in table01.Records)
			{
				r.SetParentReferences();
			}

			rc = FreeRecords(table, false);

			if (gEngine.IsFailure(rc))
			{
				// PrintError
			}

			table = table01;

			if (printOutput)
			{
				gOut.Write("read {0} record{1}.", table.Records.Count, table.Records.Count != 1 ? "s" : "");
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode LoadConfigs(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = ConfigTable;

			rc = LoadRecords<IConfig, IConfigHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			ConfigTable = table;

			return rc;
		}

		public virtual RetCode LoadFilesets(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = FilesetTable;

			rc = LoadRecords<IFileset, IFilesetHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			FilesetTable = table;

			return rc;
		}

		public virtual RetCode LoadCharacters(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = CharacterTable;

			rc = LoadRecords<ICharacter, ICharacterHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			CharacterTable = table;

			return rc;
		}

		public virtual RetCode LoadModules(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = ModuleTable;

			rc = LoadRecords<IModule, IModuleHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			ModuleTable = table;

			return rc;
		}

		public virtual RetCode LoadRooms(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = RoomTable;

			rc = LoadRecords<IRoom, IRoomHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			RoomTable = table;

			return rc;
		}

		public virtual RetCode LoadArtifacts(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = ArtifactTable;

			rc = LoadRecords<IArtifact, IArtifactHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			ArtifactTable = table;

			return rc;
		}

		public virtual RetCode LoadEffects(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = EffectTable;

			rc = LoadRecords<IEffect, IEffectHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			EffectTable = table;

			return rc;
		}

		public virtual RetCode LoadMonsters(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = MonsterTable;

			rc = LoadRecords<IMonster, IMonsterHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			MonsterTable = table;

			return rc;
		}

		public virtual RetCode LoadHints(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = HintTable;

			rc = LoadRecords<IHint, IHintHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			HintTable = table;

			return rc;
		}

		public virtual RetCode LoadTriggers(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = TriggerTable;

			rc = LoadRecords<ITrigger, ITriggerHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			TriggerTable = table;

			return rc;
		}

		public virtual RetCode LoadScripts(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = ScriptTable;

			rc = LoadRecords<IScript, IScriptHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			ScriptTable = table;

			return rc;
		}

		public virtual RetCode LoadGameStates(string fileName, bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			var table = GameStateTable;

			rc = LoadRecords<IGameState, IGameStateHelper>(ref table, fileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRecords function call failed");
			}

			GameStateTable = table;

			return rc;
		}

		public virtual RetCode SaveRecords<T>(IDbTable<T> table, string fileName, bool printOutput = true) where T : class, IGameBase
		{
			RetCode rc;

			if (table == null || table.Records == null || string.IsNullOrWhiteSpace(fileName))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (printOutput)
			{
				gOut.Write("{0}Saving textfile [{1}] ... ", Environment.NewLine, fileName);
			}

			Globals.TextSerializer.Serialize(table, fileName);

			if (printOutput)
			{
				gOut.Write("wrote {0} record{1}.", table.Records.Count, table.Records.Count != 1 ? "s" : "");
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode SaveConfigs(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(ConfigTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveFilesets(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(FilesetTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveCharacters(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(CharacterTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveModules(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(ModuleTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveRooms(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(RoomTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveArtifacts(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(ArtifactTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveEffects(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(EffectTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveMonsters(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(MonsterTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveHints(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(HintTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveTriggers(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(TriggerTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveScripts(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(ScriptTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode SaveGameStates(string fileName, bool printOutput = true)
		{
			RetCode rc;

			rc = SaveRecords(GameStateTable, fileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRecords function call failed");
			}

			return rc;
		}

		public virtual RetCode FreeRecords<T>(IDbTable<T> table, bool dispose = true) where T : class, IGameBase
		{
			RetCode rc;

			if (table == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = table.FreeRecords(dispose);

		Cleanup:

			return rc;
		}

		public virtual RetCode FreeConfigs(bool dispose = true)
		{
			return FreeRecords(ConfigTable, dispose);
		}

		public virtual RetCode FreeFilesets(bool dispose = true)
		{
			return FreeRecords(FilesetTable, dispose);
		}

		public virtual RetCode FreeCharacters(bool dispose = true)
		{
			return FreeRecords(CharacterTable, dispose);
		}

		public virtual RetCode FreeModules(bool dispose = true)
		{
			return FreeRecords(ModuleTable, dispose);
		}

		public virtual RetCode FreeRooms(bool dispose = true)
		{
			return FreeRecords(RoomTable, dispose);
		}

		public virtual RetCode FreeArtifacts(bool dispose = true)
		{
			return FreeRecords(ArtifactTable, dispose);
		}

		public virtual RetCode FreeEffects(bool dispose = true)
		{
			return FreeRecords(EffectTable, dispose);
		}

		public virtual RetCode FreeMonsters(bool dispose = true)
		{
			return FreeRecords(MonsterTable, dispose);
		}

		public virtual RetCode FreeHints(bool dispose = true)
		{
			return FreeRecords(HintTable, dispose);
		}

		public virtual RetCode FreeTriggers(bool dispose = true)
		{
			return FreeRecords(TriggerTable, dispose);
		}

		public virtual RetCode FreeScripts(bool dispose = true)
		{
			return FreeRecords(ScriptTable, dispose);
		}

		public virtual RetCode FreeGameStates(bool dispose = true)
		{
			return FreeRecords(GameStateTable, dispose);
		}

		public virtual long GetRecordsCount<T>(IDbTable<T> table) where T : class, IGameBase
		{
			long result;

			if (table == null)
			{
				result = -1;

				// PrintError

				goto Cleanup;
			}

			result = table.GetRecordsCount();

		Cleanup:

			return result;
		}

		public virtual long GetConfigsCount()
		{
			return GetRecordsCount(ConfigTable);
		}

		public virtual long GetFilesetsCount()
		{
			return GetRecordsCount(FilesetTable);
		}

		public virtual long GetCharactersCount()
		{
			return GetRecordsCount(CharacterTable);
		}

		public virtual long GetModulesCount()
		{
			return GetRecordsCount(ModuleTable);
		}

		public virtual long GetRoomsCount()
		{
			return GetRecordsCount(RoomTable);
		}

		public virtual long GetArtifactsCount()
		{
			return GetRecordsCount(ArtifactTable);
		}

		public virtual long GetEffectsCount()
		{
			return GetRecordsCount(EffectTable);
		}

		public virtual long GetMonstersCount()
		{
			return GetRecordsCount(MonsterTable);
		}

		public virtual long GetHintsCount()
		{
			return GetRecordsCount(HintTable);
		}

		public virtual long GetTriggersCount()
		{
			return GetRecordsCount(TriggerTable);
		}

		public virtual long GetScriptsCount()
		{
			return GetRecordsCount(ScriptTable);
		}

		public virtual long GetGameStatesCount()
		{
			return GetRecordsCount(GameStateTable);
		}

		public virtual T FindRecord<T>(IDbTable<T> table, long uid) where T : class, IGameBase
		{
			T result;

			result = default(T);

			if (table == null)
			{
				// PrintError

				goto Cleanup;
			}

			result = table.FindRecord(uid);

		Cleanup:

			return result;
		}

		public virtual IConfig FindConfig(long uid)
		{
			return FindRecord(ConfigTable, uid);
		}

		public virtual IFileset FindFileset(long uid)
		{
			return FindRecord(FilesetTable, uid);
		}

		public virtual ICharacter FindCharacter(long uid)
		{
			return FindRecord(CharacterTable, uid);
		}

		public virtual IModule FindModule(long uid)
		{
			return FindRecord(ModuleTable, uid);
		}

		public virtual IRoom FindRoom(long uid)
		{
			return FindRecord(RoomTable, uid);
		}

		public virtual IArtifact FindArtifact(long uid)
		{
			return FindRecord(ArtifactTable, uid);
		}

		public virtual IEffect FindEffect(long uid)
		{
			return FindRecord(EffectTable, uid);
		}

		public virtual IMonster FindMonster(long uid)
		{
			return FindRecord(MonsterTable, uid);
		}

		public virtual IHint FindHint(long uid)
		{
			return FindRecord(HintTable, uid);
		}

		public virtual ITrigger FindTrigger(long uid)
		{
			return FindRecord(TriggerTable, uid);
		}

		public virtual IScript FindScript(long uid)
		{
			return FindRecord(ScriptTable, uid);
		}

		public virtual IGameState FindGameState(long uid)
		{
			return FindRecord(GameStateTable, uid);
		}

		public virtual T FindRecord<T>(IDbTable<T> table, Type type, bool exactMatch = false) where T : class, IGameBase
		{
			T result;

			result = default(T);

			if (table == null || type == null)
			{
				// PrintError

				goto Cleanup;
			}

			result = table.FindRecord(type, exactMatch);

		Cleanup:

			return result;
		}

		public virtual RetCode AddRecord<T>(IDbTable<T> table, T record, bool makeCopy = false) where T : class, IGameBase
		{
			RetCode rc;

			if (table == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = table.AddRecord(record, makeCopy);

		Cleanup:

			return rc;
		}

		public virtual RetCode AddConfig(IConfig config, bool makeCopy = false)
		{
			return AddRecord(ConfigTable, config, makeCopy);
		}

		public virtual RetCode AddFileset(IFileset fileset, bool makeCopy = false)
		{
			return AddRecord(FilesetTable, fileset, makeCopy);
		}

		public virtual RetCode AddCharacter(ICharacter character, bool makeCopy = false)
		{
			return AddRecord(CharacterTable, character, makeCopy);
		}

		public virtual RetCode AddModule(IModule module, bool makeCopy = false)
		{
			return AddRecord(ModuleTable, module, makeCopy);
		}

		public virtual RetCode AddRoom(IRoom room, bool makeCopy = false)
		{
			return AddRecord(RoomTable, room, makeCopy);
		}

		public virtual RetCode AddArtifact(IArtifact artifact, bool makeCopy = false)
		{
			return AddRecord(ArtifactTable, artifact, makeCopy);
		}

		public virtual RetCode AddEffect(IEffect effect, bool makeCopy = false)
		{
			return AddRecord(EffectTable, effect, makeCopy);
		}

		public virtual RetCode AddMonster(IMonster monster, bool makeCopy = false)
		{
			return AddRecord(MonsterTable, monster, makeCopy);
		}

		public virtual RetCode AddHint(IHint hint, bool makeCopy = false)
		{
			return AddRecord(HintTable, hint, makeCopy);
		}

		public virtual RetCode AddTrigger(ITrigger trigger, bool makeCopy = false)
		{
			return AddRecord(TriggerTable, trigger, makeCopy);
		}

		public virtual RetCode AddScript(IScript script, bool makeCopy = false)
		{
			return AddRecord(ScriptTable, script, makeCopy);
		}

		public virtual RetCode AddGameState(IGameState gameState, bool makeCopy = false)
		{
			return AddRecord(GameStateTable, gameState, makeCopy);
		}

		public virtual T RemoveRecord<T>(IDbTable<T> table, long uid) where T : class, IGameBase
		{
			T result;

			result = default(T);

			if (table == null)
			{
				// PrintError

				goto Cleanup;
			}

			result = table.RemoveRecord(uid);

		Cleanup:

			return result;
		}

		public virtual IConfig RemoveConfig(long uid)
		{
			return RemoveRecord(ConfigTable, uid);
		}

		public virtual IFileset RemoveFileset(long uid)
		{
			return RemoveRecord(FilesetTable, uid);
		}

		public virtual ICharacter RemoveCharacter(long uid)
		{
			return RemoveRecord(CharacterTable, uid);
		}

		public virtual IModule RemoveModule(long uid)
		{
			return RemoveRecord(ModuleTable, uid);
		}

		public virtual IRoom RemoveRoom(long uid)
		{
			return RemoveRecord(RoomTable, uid);
		}

		public virtual IArtifact RemoveArtifact(long uid)
		{
			return RemoveRecord(ArtifactTable, uid);
		}

		public virtual IEffect RemoveEffect(long uid)
		{
			return RemoveRecord(EffectTable, uid);
		}

		public virtual IMonster RemoveMonster(long uid)
		{
			return RemoveRecord(MonsterTable, uid);
		}

		public virtual IHint RemoveHint(long uid)
		{
			return RemoveRecord(HintTable, uid);
		}

		public virtual ITrigger RemoveTrigger(long uid)
		{
			return RemoveRecord(TriggerTable, uid);
		}

		public virtual IScript RemoveScript(long uid)
		{
			return RemoveRecord(ScriptTable, uid);
		}

		public virtual IGameState RemoveGameState(long uid)
		{
			return RemoveRecord(GameStateTable, uid);
		}

		public virtual T RemoveRecord<T>(IDbTable<T> table, Type type, bool exactMatch = false) where T : class, IGameBase
		{
			T result;

			result = default(T);

			if (table == null || type == null)
			{
				// PrintError

				goto Cleanup;
			}

			result = table.RemoveRecord(type, exactMatch);

		Cleanup:

			return result;
		}

		public virtual long GetRecordUid<T>(IDbTable<T> table, bool allocate = true) where T : class, IGameBase
		{
			long result;

			result = -1;

			if (table == null)
			{
				// PrintError

				goto Cleanup;
			}

			result = table.GetRecordUid(allocate);

		Cleanup:

			return result;
		}

		public virtual long GetConfigUid(bool allocate = true)
		{
			return GetRecordUid(ConfigTable, allocate);
		}

		public virtual long GetFilesetUid(bool allocate = true)
		{
			return GetRecordUid(FilesetTable, allocate);
		}

		public virtual long GetCharacterUid(bool allocate = true)
		{
			return GetRecordUid(CharacterTable, allocate);
		}

		public virtual long GetModuleUid(bool allocate = true)
		{
			return GetRecordUid(ModuleTable, allocate);
		}

		public virtual long GetRoomUid(bool allocate = true)
		{
			return GetRecordUid(RoomTable, allocate);
		}

		public virtual long GetArtifactUid(bool allocate = true)
		{
			return GetRecordUid(ArtifactTable, allocate);
		}

		public virtual long GetEffectUid(bool allocate = true)
		{
			return GetRecordUid(EffectTable, allocate);
		}

		public virtual long GetMonsterUid(bool allocate = true)
		{
			return GetRecordUid(MonsterTable, allocate);
		}

		public virtual long GetHintUid(bool allocate = true)
		{
			return GetRecordUid(HintTable, allocate);
		}

		public virtual long GetTriggerUid(bool allocate = true)
		{
			return GetRecordUid(TriggerTable, allocate);
		}

		public virtual long GetScriptUid(bool allocate = true)
		{
			return GetRecordUid(ScriptTable, allocate);
		}

		public virtual long GetGameStateUid(bool allocate = true)
		{
			return GetRecordUid(GameStateTable, allocate);
		}

		public virtual void FreeRecordUid<T>(IDbTable<T> table, long uid) where T : class, IGameBase
		{
			if (table == null)
			{
				// PrintError

				goto Cleanup;
			}

			table.FreeRecordUid(uid);

		Cleanup:

			;
		}

		public virtual void FreeConfigUid(long uid)
		{
			FreeRecordUid(ConfigTable, uid);
		}

		public virtual void FreeFilesetUid(long uid)
		{
			FreeRecordUid(FilesetTable, uid);
		}

		public virtual void FreeCharacterUid(long uid)
		{
			FreeRecordUid(CharacterTable, uid);
		}

		public virtual void FreeModuleUid(long uid)
		{
			FreeRecordUid(ModuleTable, uid);
		}

		public virtual void FreeRoomUid(long uid)
		{
			FreeRecordUid(RoomTable, uid);
		}

		public virtual void FreeArtifactUid(long uid)
		{
			FreeRecordUid(ArtifactTable, uid);
		}

		public virtual void FreeEffectUid(long uid)
		{
			FreeRecordUid(EffectTable, uid);
		}

		public virtual void FreeMonsterUid(long uid)
		{
			FreeRecordUid(MonsterTable, uid);
		}

		public virtual void FreeHintUid(long uid)
		{
			FreeRecordUid(HintTable, uid);
		}

		public virtual void FreeTriggerUid(long uid)
		{
			FreeRecordUid(TriggerTable, uid);
		}

		public virtual void FreeScriptUid(long uid)
		{
			FreeRecordUid(ScriptTable, uid);
		}

		public virtual void FreeGameStateUid(long uid)
		{
			FreeRecordUid(GameStateTable, uid);
		}

		public Database()
		{
			ConfigTable = Globals.CreateInstance<IDbTable<IConfig>>();

			FilesetTable = Globals.CreateInstance<IDbTable<IFileset>>();

			CharacterTable = Globals.CreateInstance<IDbTable<ICharacter>>();

			ModuleTable = Globals.CreateInstance<IDbTable<IModule>>();

			RoomTable = Globals.CreateInstance<IDbTable<IRoom>>();

			ArtifactTable = Globals.CreateInstance<IDbTable<IArtifact>>();

			EffectTable = Globals.CreateInstance<IDbTable<IEffect>>();

			MonsterTable = Globals.CreateInstance<IDbTable<IMonster>>();

			HintTable = Globals.CreateInstance<IDbTable<IHint>>();

			TriggerTable = Globals.CreateInstance<IDbTable<ITrigger>>();

			ScriptTable = Globals.CreateInstance<IDbTable<IScript>>();

			GameStateTable = Globals.CreateInstance<IDbTable<IGameState>>();
		}
	}
}
