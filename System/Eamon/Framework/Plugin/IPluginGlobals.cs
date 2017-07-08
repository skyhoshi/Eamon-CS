
// IPluginGlobals.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.IO;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;

namespace Eamon.Framework.Plugin
{
	public interface IPluginGlobals
	{
		IDatabase Database { get; }

		IDictionary<Type, Type> ClassMappingsDictionary { get; }

		ITextReader In { get; set; }

		ITextWriter Out { get; set; }

		ITextWriter Error { get; set; }

		IMutex Mutex { get; set; }

		ITransferProtocol TransferProtocol { get; set; }

		IDirectory Directory { get; set; }

		IFile File { get; set; }

		IPath Path { get; set; }

		ISharpSerializer SharpSerializer { get; set; }

		IThread Thread { get; set; }

		MemoryStream CloneStream { get; set; }

		IEngine Engine { get; set; }

		string WorkDir { get; set; }

		string FilePrefix { get; set; }

		string LineSep { get; set; }

		bool LineWrapUserInput { get; set; }

		bool RunGameEditor { get; set; }

		bool DeleteGameStateFromMainHall { get; set; }

		Coord CursorPosition { get; set; }

		IRecordDb<IConfig> CFGDB { get; set; }

		IRecordDb<IFileset> FSDB { get; set; }

		IRecordDb<ICharacter> CHRDB { get; set; }

		IRecordDb<IModule> MODDB { get; set; }

		IRecordDb<IRoom> RDB { get; set; }

		IRecordDb<IArtifact> ADB { get; set; }

		IRecordDb<IEffect> EDB { get; set; }

		IRecordDb<IMonster> MDB { get; set; }

		IRecordDb<IHint> HDB { get; set; }

		IRecordDb<IGameState> GSDB { get; set; }

		void HandleException(Exception ex, string stackTraceFile, string errorMessage);

		RetCode PushDatabase();

		RetCode PushDatabase(IDatabase database);

		RetCode PopDatabase(bool freeDatabase = true);

		RetCode GetDatabase(long index, ref IDatabase database);

		RetCode SaveDatabase(string fileName);

		RetCode RestoreDatabase(string fileName);

		RetCode ClearDbStack();

		RetCode GetDbStackTop(ref long dbStackTop);

		RetCode GetDbStackSize(ref long dbStackSize);

		void InitSystem();

		void DeinitSystem();

		T CreateInstance<T>(Type ifaceType, Action<T> initialize = null) where T : class;

		T CreateInstance<T>(Action<T> initialize = null) where T : class;

		T CloneInstance<T>(T source) where T : class;

		bool CompareInstances<T>(T object1, T object2) where T : class;

		string GetPrefixedFileName(string fileName);
	}
}
