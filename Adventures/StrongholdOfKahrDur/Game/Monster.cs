
// Monster.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			rc = base.BuildPrintedFullDesc(buf, showName);

			// Lich will ask to be freed

			if (Globals.Engine.IsSuccess(rc) && Uid == 15 && !Seen)
			{
				var effect = Globals.EDB[53];

				Debug.Assert(effect != null);

				buf.AppendPrint("{0}", effect.Desc);
			}

			return rc;
		}

		public override bool CanMoveToRoom(bool fleeing)
		{
			// Tree ents can't flee or follow

			return Uid < 8 || Uid > 10 ? base.CanMoveToRoom(fleeing) : false;
		}

		public override bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			// Necromancer can't move into a dark area

			return Uid != 22 || roomUid != 54 ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
		}

		public override void AddHealthStatus(StringBuilder buf, bool addNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (IsDead())
			{
				result = "dead!";
			}
			else
			{
				var x = DmgTaken;

				x = (((long)((double)(x * 5) / (double)Hardiness)) + 1) * (x > 0 ? 1 : 0);

				result = "at death's door, knocking loudly.";

				if (x == 4)
				{
					result = "gravely injured.";
				}
				else if (x == 3)
				{
					result = "badly hurt.";
				}
				else if (x == 2)
				{
					result = "hurt.";
				}
				else if (x == 1)
				{
					result = "still in good shape.";
				}
				else if (x < 1)
				{
					result = "in perfect health.";
				}
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, addNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}
	}
}
