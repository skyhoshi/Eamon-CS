
// CharacterDb.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<ICharacter>))]
	public class CharacterDb : IRecordDb<ICharacter>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public CharacterDb()
		{
			CopyAddedRecord = true;
		}

		public virtual ICharacter this[long uid]
		{
			get
			{
				return Globals.Database.FindCharacter(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveCharacter(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddCharacter(value, CopyAddedRecord);
				}
			}
		}
	}
}
