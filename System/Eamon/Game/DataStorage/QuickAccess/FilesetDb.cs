
// FilesetDb.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IFileset>))]
	public class FilesetDb : IRecordDb<IFileset>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public FilesetDb()
		{
			CopyAddedRecord = true;
		}

		public virtual IFileset this[long uid]
		{
			get
			{
				return Globals.Database.FindFileset(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveFileset(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddFileset(value, CopyAddedRecord);
				}
			}
		}
	}
}
