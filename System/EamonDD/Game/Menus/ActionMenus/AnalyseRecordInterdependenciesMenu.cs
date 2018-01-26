
// AnalyseRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Framework.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AnalyseRecordInterdependenciesMenu<T> : RecordMenu<T>, IAnalyseRecordInterdependenciesMenu<T> where T : class, IGameBase
	{
		public virtual IList<string> SkipFieldNames { get; set; }

		public virtual IValidateArgs ValidateArgs { get; set; }

		public virtual T ErrorRecord { get; set; }

		public virtual bool ClearSkipFieldNames { get; set; }

		public virtual bool ModifyFlag { get; set; }

		public virtual bool ExitFlag { get; set; }

		public virtual void ProcessInterdependency()
		{
			Debug.Assert(ErrorRecord != null);

			var helper = Globals.CreateInstance<IHelper<T>>(x =>
			{
				x.Record = ErrorRecord;
			});

			helper.ListErrorField(ValidateArgs);

			Globals.Out.Print("{0}", ValidateArgs.ErrorMessage);

			Globals.Out.Print("{0}", Globals.LineSep);

			Globals.Out.Write("{0}S=Skip field, T=Edit this record, R={1} referred to record, X=Exit: ",
				Environment.NewLine,
				ValidateArgs.NewRecordUid > 0 ? "Add" : "Edit");

			ValidateArgs.Buf.Clear();

			var rc = Globals.In.ReadField(ValidateArgs.Buf, Constants.BufSize02, null, ' ', '\0', false, null, Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharSOrTOrROrX, Globals.Engine.IsCharSOrTOrROrX);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (ValidateArgs.Buf.Length == 0 || ValidateArgs.Buf[0] == 'X')
			{
				ExitFlag = true;
			}
			else if (ValidateArgs.Buf[0] == 'S')
			{
				var uniqueFieldName = string.Format("{0}_{1}_{2}", typeof(T).Name, ErrorRecord.Uid, helper.GetErrorFieldName(ValidateArgs));

				SkipFieldNames.Add(uniqueFieldName);
			}
			else if (ValidateArgs.Buf[0] == 'T')
			{
				IMenu menu;

				ModifyFlag = true;

				if (ErrorRecord is IArtifact)
				{
					menu = Globals.CreateInstance<IEditArtifactRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IArtifact)ErrorRecord;
					});
				}
				else if (ErrorRecord is IEffect)
				{
					menu = Globals.CreateInstance<IEditEffectRecordMenu>(x =>
					{
						x.EditRecord = (IEffect)ErrorRecord;
					});
				}
				else if (ErrorRecord is IHint)
				{
					menu = Globals.CreateInstance<IEditHintRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IHint)ErrorRecord;
					});
				}
				else if (ErrorRecord is IModule)
				{
					menu = Globals.CreateInstance<IEditModuleRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IModule)ErrorRecord;
					});
				}
				else if (ErrorRecord is IMonster)
				{
					menu = Globals.CreateInstance<IEditMonsterRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IMonster)ErrorRecord;
					});
				}
				else
				{
					Debug.Assert(ErrorRecord is IRoom);

					menu = Globals.CreateInstance<IEditRoomRecordManyFieldsMenu>(x =>
					{
						x.EditRecord = (IRoom)ErrorRecord;
					});
				}

				menu.Execute();
			}
			else if (ValidateArgs.Buf[0] == 'R')
			{
				IMenu menu;

				ModifyFlag = true;

				if (ValidateArgs.NewRecordUid > 0)
				{
					if (ValidateArgs.RecordType == typeof(IArtifact))
					{
						menu = Globals.CreateInstance<IAddArtifactRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateArgs.NewRecordUid;
						});
					}
					else if (ValidateArgs.RecordType == typeof(IEffect))
					{
						menu = Globals.CreateInstance<IAddEffectRecordMenu>(x =>
						{
							x.NewRecordUid = ValidateArgs.NewRecordUid;
						});
					}
					else if (ValidateArgs.RecordType == typeof(IHint))
					{
						menu = Globals.CreateInstance<IAddHintRecordMenu>(x =>
						{
							x.NewRecordUid = ValidateArgs.NewRecordUid;
						});
					}
					else if (ValidateArgs.RecordType == typeof(IModule))
					{
						menu = Globals.CreateInstance<IAddModuleRecordMenu>(x =>
						{
							x.NewRecordUid = ValidateArgs.NewRecordUid;
						});
					}
					else if (ValidateArgs.RecordType == typeof(IMonster))
					{
						menu = Globals.CreateInstance<IAddMonsterRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateArgs.NewRecordUid;
						});
					}
					else
					{
						Debug.Assert(ValidateArgs.RecordType == typeof(IRoom));

						menu = Globals.CreateInstance<IAddRoomRecordManualMenu>(x =>
						{
							x.NewRecordUid = ValidateArgs.NewRecordUid;
						});
					}
				}
				else
				{
					if (ValidateArgs.RecordType == typeof(IArtifact))
					{
						menu = Globals.CreateInstance<IEditArtifactRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IArtifact)ValidateArgs.EditRecord;
						});
					}
					else if (ValidateArgs.RecordType == typeof(IEffect))
					{
						menu = Globals.CreateInstance<IEditEffectRecordMenu>(x =>
						{
							x.EditRecord = (IEffect)ValidateArgs.EditRecord;
						});
					}
					else if (ValidateArgs.RecordType == typeof(IHint))
					{
						menu = Globals.CreateInstance<IEditHintRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IHint)ValidateArgs.EditRecord;
						});
					}
					else if (ValidateArgs.RecordType == typeof(IModule))
					{
						menu = Globals.CreateInstance<IEditModuleRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IModule)ValidateArgs.EditRecord;
						});
					}
					else if (ValidateArgs.RecordType == typeof(IMonster))
					{
						menu = Globals.CreateInstance<IEditMonsterRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IMonster)ValidateArgs.EditRecord;
						});
					}
					else
					{
						Debug.Assert(ValidateArgs.RecordType == typeof(IRoom));

						menu = Globals.CreateInstance<IEditRoomRecordManyFieldsMenu>(x =>
						{
							x.EditRecord = (IRoom)ValidateArgs.EditRecord;
						});
					}
				}

				menu.Execute();
			}

			Globals.Out.Print("{0}", Globals.LineSep);
		}

		public override void Execute()
		{
			Globals.Out.WriteLine();

			Globals.Engine.PrintTitle(Title, true);

			var helper = Globals.CreateInstance<IHelper<T>>();

			if (ClearSkipFieldNames)
			{
				SkipFieldNames.Clear();
			}

			ValidateArgs.Clear();

			ModifyFlag = false;

			ExitFlag = false;

			while (true)
			{
				ErrorRecord = default(T);

				foreach (var record in RecordTable.Records)
				{
					helper.Record = record;

					var fieldNames = helper.GetFieldNames((fn) =>
					{
						var uniqueFieldName = string.Format("{0}_{1}_{2}", typeof(T).Name, record.Uid, fn);

						return !SkipFieldNames.Contains(uniqueFieldName);
					});

					foreach (var fieldName in fieldNames)
					{
						if (!helper.ValidateFieldInterdependencies(fieldName, ValidateArgs))
						{
							ErrorRecord = record;

							goto ProcessError;
						}
					}
				}

			ProcessError:

				if (ErrorRecord != null)
				{
					ValidateArgs.ShowDesc = Globals.Config.ShowDesc;

					ProcessInterdependency();

					if (ExitFlag)
					{
						break;
					}
				}
				else
				{
					break;
				}
			}

			Globals.Out.Print("Done analysing {0} records.", RecordTypeName);
		}

		public AnalyseRecordInterdependenciesMenu()
		{
			SkipFieldNames = new List<string>();

			ValidateArgs = Globals.CreateInstance<IValidateArgs>();

			ClearSkipFieldNames = true;
		}
	}
}
