
// RoomDb.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IRoom>))]
	public class RoomDb : IRecordDb<IRoom>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public RoomDb()
		{
			CopyAddedRecord = true;
		}

		public virtual IRoom this[long uid]
		{
			get
			{
				return Globals.Database.FindRoom(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveRoom(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddRoom(value, CopyAddedRecord);
				}
			}
		}
	}
}
