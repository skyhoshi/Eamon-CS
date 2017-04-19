
// GameStateDb.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IGameState>))]
	public class GameStateDb : IRecordDb<IGameState>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public GameStateDb()
		{
			CopyAddedRecord = true;
		}

		public virtual IGameState this[long uid]
		{
			get
			{
				return Globals.Database.FindGameState(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveGameState(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddGameState(value, CopyAddedRecord);
				}
			}
		}
	}
}
