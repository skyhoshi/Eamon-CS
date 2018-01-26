
// ArtifactDb.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IArtifact>))]
	public class ArtifactDb : IRecordDb<IArtifact>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public ArtifactDb()
		{
			CopyAddedRecord = true;
		}

		public virtual IArtifact this[long uid]
		{
			get
			{
				return Globals.Database.FindArtifact(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveArtifact(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddArtifact(value, CopyAddedRecord);
				}
			}
		}
	}
}
