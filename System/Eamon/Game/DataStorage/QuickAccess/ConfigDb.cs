
// ConfigDb.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IConfig>))]
	public class ConfigDb : IRecordDb<IConfig>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public ConfigDb()
		{
			CopyAddedRecord = true;
		}

		public virtual IConfig this[long uid]
		{
			get
			{
				return Globals.Database.FindConfig(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveConfig(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddConfig(value, CopyAddedRecord);
				}
			}
		}
	}
}
