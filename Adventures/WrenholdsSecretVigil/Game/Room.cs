
// Room.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game
{
	[ClassMappings(typeof(IRoom))]
	public class Room : Eamon.Game.Room, Framework.IRoom
	{
		protected override bool IsDirectionInObviousExitsList(long index)
		{
			// Suppress up/down in obvious exits list while hiking

			return base.IsDirectionInObviousExitsList(index) && (Uid < 8 ? index != 5 && index != 6 : true);
		}

		public virtual bool IsDigCommandAllowedInRoom()
		{
			var roomUids = new List<long>()
			{
				3, 8, 13, 14, 18, 25, 39, 40, 
			};

			return roomUids.Contains(Uid);
		}

		public virtual bool IsDirectionEffect(long index)
		{
			return GetDirs(index) > 5000 && GetDirs(index) < 6001;
		}

		public virtual bool IsDirectionEffect(Enums.Direction dir)
		{
			return IsDirectionEffect((long)dir);
		}

		public virtual long GetDirectionEffectUid(Enums.Direction dir)
		{
			return IsDirectionEffect(dir) ? GetDirs(dir) - 5000 : 0;
		}

		public virtual IEffect GetDirectionEffect(Enums.Direction dir)
		{
			var uid = GetDirectionEffectUid(dir);

			return Globals.EDB[uid];
		}
	}
}
