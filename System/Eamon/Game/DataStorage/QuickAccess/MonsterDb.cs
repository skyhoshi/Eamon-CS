
// MonsterDb.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IMonster>))]
	public class MonsterDb : IRecordDb<IMonster>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public MonsterDb()
		{
			CopyAddedRecord = true;
		}

		public virtual IMonster this[long uid]
		{
			get
			{
				return Globals.Database.FindMonster(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveMonster(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddMonster(value, CopyAddedRecord);
				}
			}
		}
	}
}
