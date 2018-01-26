
// EffectDb.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IEffect>))]
	public class EffectDb : IRecordDb<IEffect>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public EffectDb()
		{
			CopyAddedRecord = true;
		}

		public virtual IEffect this[long uid]
		{
			get
			{
				return Globals.Database.FindEffect(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveEffect(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddEffect(value, CopyAddedRecord);
				}
			}
		}
	}
}
