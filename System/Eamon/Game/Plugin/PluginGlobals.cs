
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Plugin
{
	public class PluginGlobals : IPluginGlobals
	{
		/// <summary></summary>
		protected virtual IDatabase[] Databases { get; set; }

		/// <summary></summary>
		protected virtual long DbStackTop { get; set; }

		public virtual IDatabase Database
		{
			get
			{
				return DbStackTop >= 0 && DbStackTop < Databases.Length ? Databases[DbStackTop] : null;
			}
		}

		public virtual IDictionary<Type, Type> ClassMappingsDictionary
		{
			get
			{
				return ClassMappings.ClassMappingsDictionary;
			}
		}

		public virtual ITextReader In
		{
			get
			{
				return ClassMappings.In;
			}
			set
			{
				ClassMappings.In = value;
			}
		}

		public virtual ITextWriter Out
		{
			get
			{
				return ClassMappings.Out;
			}
			set
			{
				ClassMappings.Out = value;
			}
		}

		public virtual ITextWriter Error
		{
			get
			{
				return ClassMappings.Error;
			}
			set
			{
				ClassMappings.Error = value;
			}
		}

		public virtual IMutex Mutex
		{
			get
			{
				return ClassMappings.Mutex;
			}
			set
			{
				ClassMappings.Mutex = value;
			}
		}

		public virtual ITransferProtocol TransferProtocol
		{
			get
			{
				return ClassMappings.TransferProtocol;
			}
			set
			{
				ClassMappings.TransferProtocol = value;
			}
		}

		public virtual IDirectory Directory
		{
			get
			{
				return ClassMappings.Directory;
			}
			set
			{
				ClassMappings.Directory = value;
			}
		}

		public virtual IFile File
		{
			get
			{
				return ClassMappings.File;
			}
			set
			{
				ClassMappings.File = value;
			}
		}

		public virtual IPath Path
		{
			get
			{
				return ClassMappings.Path;
			}
			set
			{
				ClassMappings.Path = value;
			}
		}

		public virtual ISharpSerializer SharpSerializer
		{
			get
			{
				return ClassMappings.SharpSerializer;
			}
			set
			{
				ClassMappings.SharpSerializer = value;
			}
		}

		public virtual IThread Thread
		{
			get
			{
				return ClassMappings.Thread;
			}
			set
			{
				ClassMappings.Thread = value;
			}
		}

		public virtual MemoryStream CloneStream
		{
			get
			{
				return ClassMappings.CloneStream;
			}
			set
			{
				ClassMappings.CloneStream = value;
			}
		}

		public virtual IEngine Engine { get; set; }

		public virtual string WorkDir
		{
			get
			{
				return ClassMappings.WorkDir;
			}
			set
			{
				ClassMappings.WorkDir = value;
			}
		}

		public virtual string FilePrefix
		{
			get
			{
				return ClassMappings.FilePrefix;
			}
			set
			{
				ClassMappings.FilePrefix = value;
			}
		}

		public virtual string LineSep { get; set; } = new string('-', (int)Constants.RightMargin);

		public virtual long RulesetVersion
		{
			get
			{
				return ClassMappings.RulesetVersion;
			}
			set
			{
				ClassMappings.RulesetVersion = value;
			}
		}

		public virtual bool EnableGameOverrides
		{
			get
			{
				return ClassMappings.EnableGameOverrides;
			}
		}

		public virtual bool LineWrapUserInput { get; set; }

		public virtual bool RunGameEditor
		{
			get
			{
				return ClassMappings.RunGameEditor;
			}
			set
			{
				ClassMappings.RunGameEditor = value;
			}
		}

		public virtual bool DeleteGameStateFromMainHall
		{
			get
			{
				return ClassMappings.DeleteGameStateFromMainHall;
			}
			set
			{
				ClassMappings.DeleteGameStateFromMainHall = value;
			}
		}

		public virtual Coord CursorPosition { get; set; }

		public virtual IRecordDb<IConfig> CFGDB { get; set; }

		public virtual IRecordDb<IFileset> FSDB { get; set; }

		public virtual IRecordDb<ICharacter> CHRDB { get; set; }

		public virtual IRecordDb<IModule> MODDB { get; set; }

		public virtual IRecordDb<IRoom> RDB { get; set; }

		public virtual IRecordDb<IArtifact> ADB { get; set; }

		public virtual IRecordDb<IEffect> EDB { get; set; }

		public virtual IRecordDb<IMonster> MDB { get; set; }

		public virtual IRecordDb<IHint> HDB { get; set; }

		public virtual IRecordDb<IGameState> GSDB { get; set; }

		/// <summary></summary>
		/// <param name="records"></param>
		protected virtual void RestoreRecords(IList<IGameBase> records)
		{
			if (records != null)
			{
				foreach (var r in records)
				{
					r.SetParentReferences();

					// Note: may want to be really rigorous here and also validate record

					if (r is IMonster)
					{
						// Note: may want to be really rigorous here and also validate weapon/shield combo
					}
				}
			}
		}

		public virtual void HandleException(Exception ex, string stackTraceFile, string errorMessage)
		{
			ClassMappings.HandleException(ex, stackTraceFile, errorMessage);
		}

		public virtual RetCode PushDatabase()
		{
			RetCode rc;

			rc = RetCode.Success;

			if (DbStackTop + 1 >= Databases.Length)
			{
				rc = RetCode.IsFull;

				goto Cleanup;
			}

			Databases[++DbStackTop] = ClassMappings.CreateInstance<IDatabase>();

		Cleanup:

			return rc;
		}

		public virtual RetCode PushDatabase(IDatabase database)
		{
			RetCode rc;

			if (database == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (DbStackTop + 1 >= Databases.Length)
			{
				rc = RetCode.IsFull;

				goto Cleanup;
			}

			Databases[++DbStackTop] = database;

			Cleanup:

			return rc;
		}

		public virtual RetCode PopDatabase(bool freeDatabase = true)
		{
			RetCode rc;

			rc = RetCode.Success;

			if (DbStackTop < 0)
			{
				rc = RetCode.IsEmpty;

				goto Cleanup;
			}

			if (freeDatabase)
			{
				Database.FreeConfigs();

				Database.FreeFilesets();

				Database.FreeCharacters();

				Database.FreeModules();

				Database.FreeRooms();

				Database.FreeArtifacts();

				Database.FreeEffects();

				Database.FreeMonsters();

				Database.FreeHints();

				Database.FreeGameStates();
			}

			Databases[DbStackTop--] = null;

		Cleanup:

			return rc;
		}

		public virtual RetCode GetDatabase(long index, ref IDatabase database)
		{
			RetCode rc;

			if (index < 0 || index > DbStackTop)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			database = Databases[index];

		Cleanup:

			return rc;
		}

		public virtual RetCode SaveDatabase(string fileName)
		{
			RetCode rc;

			if (string.IsNullOrWhiteSpace(fileName))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (DbStackTop < 0)
			{
				rc = RetCode.IsEmpty;

				goto Cleanup;
			}

			SharpSerializer.Serialize(Database, fileName);

		Cleanup:

			return rc;
		}

		public virtual RetCode RestoreDatabase(string fileName)
		{
			RetCode rc;

			if (string.IsNullOrWhiteSpace(fileName))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (DbStackTop + 1 >= Databases.Length)
			{
				rc = RetCode.IsFull;

				goto Cleanup;
			}

			UpgradeTextfile(fileName);

			var database = SharpSerializer.Deserialize(fileName) as IDatabase;

			if (database == null)
			{
				rc = RetCode.Failure;

				goto Cleanup;
			}

			RestoreRecords(database?.ConfigTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.FilesetTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.CharacterTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.ModuleTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.RoomTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.ArtifactTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.EffectTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.MonsterTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.HintTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.GameStateTable?.Records?.Cast<IGameBase>().ToList());

			Databases[++DbStackTop] = database;

		Cleanup:

			return rc;
		}

		public virtual RetCode ClearDbStack()
		{
			RetCode rc;

			rc = RetCode.Success;

			while (DbStackTop >= 0)
			{
				rc = PopDatabase();

				if (rc != RetCode.Success)
				{
					break;
				}
			}

			return rc;
		}

		public virtual RetCode GetDbStackTop(ref long dbStackTop)
		{
			RetCode rc;

			rc = RetCode.Success;

			dbStackTop = DbStackTop;

			return rc;
		}

		public virtual RetCode GetDbStackSize(ref long dbStackSize)
		{
			RetCode rc;

			rc = RetCode.Success;

			dbStackSize = Databases.Length;

			return rc;
		}

		public virtual void InitSystem()
		{
			if (!ClassMappings.IgnoreMutex)
			{
				ClassMappings.Mutex.CreateAndWaitOne();
			}

			Engine = ClassMappings.CreateInstance<IEngine>();

			Databases = new IDatabase[Constants.NumDatabases];

			DbStackTop = -1;

			PushDatabase();

			CFGDB = ClassMappings.CreateInstance<IRecordDb<IConfig>>();

			FSDB = ClassMappings.CreateInstance<IRecordDb<IFileset>>();

			CHRDB = ClassMappings.CreateInstance<IRecordDb<ICharacter>>();

			MODDB = ClassMappings.CreateInstance<IRecordDb<IModule>>();

			RDB = ClassMappings.CreateInstance<IRecordDb<IRoom>>();

			ADB = ClassMappings.CreateInstance<IRecordDb<IArtifact>>();

			EDB = ClassMappings.CreateInstance<IRecordDb<IEffect>>();

			MDB = ClassMappings.CreateInstance<IRecordDb<IMonster>>();

			HDB = ClassMappings.CreateInstance<IRecordDb<IHint>>();

			GSDB = ClassMappings.CreateInstance<IRecordDb<IGameState>>();
		}

		public virtual void DeinitSystem()
		{
			ClearDbStack();
		}

		public virtual T CreateInstance<T>(Type ifaceType, Action<T> initialize = null) where T : class
		{
			return ClassMappings.CreateInstance(ifaceType, initialize);
		}

		public virtual T CreateInstance<T>(Action<T> initialize = null) where T : class
		{
			return ClassMappings.CreateInstance(initialize);
		}

		public virtual T CloneInstance<T>(T source) where T : class
		{
			return ClassMappings.CloneInstance(source);
		}

		public virtual bool CompareInstances<T>(T object1, T object2) where T : class
		{
			return ClassMappings.CompareInstances(object1, object2);
		}

		public virtual bool IsRulesetVersion(params long[] versions)
		{
			return ClassMappings.IsRulesetVersion(versions);
		}

		public virtual string GetPrefixedFileName(string fileName)
		{
			return ClassMappings.GetPrefixedFileName(fileName);
		}

		public virtual void ReplaceTextfileValues(string fileName, string[] oldValues, string[] newValues)
		{
			if (string.IsNullOrWhiteSpace(fileName) || oldValues == null || newValues == null || oldValues.Length != newValues.Length)
			{
				// PrintError

				goto Cleanup;
			}

			var contents = Globals.File.ReadAllText(fileName);

			for (var i = 0; i < oldValues.Length; i++)
			{
				contents = contents.Replace(oldValues[i], newValues[i]);
			}

			Globals.File.WriteAllText(fileName, contents);

		Cleanup:

			;
		}

		public virtual void UpgradeTextfile(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				// PrintError

				goto Cleanup;
			}

			var needsUpgrade = true;

			while (needsUpgrade)
			{
				var firstLine = Globals.File.ReadFirstLine(fileName);

				if (firstLine.Contains("Version=1.3.0.0"))
				{
					if (firstLine.Contains("Game.DataStorage.ArtifactDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{
						ReplaceTextfileValues
						(
							fileName,
							new string[]
							{
								"Version=1.3.0.0",
								"EAMON CS 1.3",
								"<SingleArray name=\"Classes\">",
								"Eamon.Game.Primitive.Classes.ArtifactClass",
								"<Simple name=\"Field5\"",
								"<Simple name=\"Field6\"",
								"<Simple name=\"Field7\"",
								"<Simple name=\"Field8\"",
							},
							new string[]
							{
								"Version=1.4.0.0",
								"EAMON CS 1.4",
								"<SingleArray name=\"Categories\">",
								"Eamon.Game.Primitive.Classes.ArtifactCategory",
								"<Simple name=\"Field1\"",
								"<Simple name=\"Field2\"",
								"<Simple name=\"Field3\"",
								"<Simple name=\"Field4\"",
							}
						);
					}
					else
					{
						ReplaceTextfileValues
						(
							fileName,
							new string[]
							{
								"Version=1.3.0.0",
								"EAMON CS 1.3",
							},
							new string[]
							{
								"Version=1.4.0.0",
								"EAMON CS 1.4",
							}
						);
					}
				}
				else if (firstLine.Contains("Version=1.4.0.0"))
				{
					if (firstLine.Contains("Game.DataStorage.CharacterDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{
						ReplaceTextfileValues
						(
							fileName,
							new string[]
							{
								"Version=1.4.0.0",
								"EAMON CS 1.4",
								"Eamon.Game.Primitive.Classes.CharacterWeapon",
								"<Simple name=\"Complexity\"",
								"<Simple name=\"Type\" value=\"Axe\"",
								"<Simple name=\"Type\" value=\"Bow\"",
								"<Simple name=\"Type\" value=\"Club\"",
								"<Simple name=\"Type\" value=\"Spear\"",
								"<Simple name=\"Type\" value=\"Sword\"",
								"<Simple name=\"Dice\"",
								"<Simple name=\"Sides\"",
							},
							new string[]
							{
								"Version=1.5.0.0",
								"EAMON CS 1.5",
								"Eamon.Game.Primitive.Classes.CharacterArtifact",
								"<Simple name=\"Field1\"",
								"<Simple name=\"Field2\" value=\"1\"",
								"<Simple name=\"Field2\" value=\"2\"",
								"<Simple name=\"Field2\" value=\"3\"",
								"<Simple name=\"Field2\" value=\"4\"",
								"<Simple name=\"Field2\" value=\"5\"",
								"<Simple name=\"Field3\"",
								"<Simple name=\"Field4\"",
							}
						);
					}
					else
					{
						ReplaceTextfileValues
						(
							fileName,
							new string[]
							{
								"Version=1.4.0.0",
								"EAMON CS 1.4",
							},
							new string[]
							{
								"Version=1.5.0.0",
								"EAMON CS 1.5",
							}
						);
					}
				}
				else
				{
					needsUpgrade = false;
				}
			}

		Cleanup:

			;
		}
	}
}
