
// ModuleDb.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IModule>))]
	public class ModuleDb : IRecordDb<IModule>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public ModuleDb()
		{
			CopyAddedRecord = true;
		}

		public virtual IModule this[long uid]
		{
			get
			{
				return Globals.Database.FindModule(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveModule(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddModule(value, CopyAddedRecord);
				}
			}
		}
	}
}
