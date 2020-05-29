
// Artifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IArtifact))]
	public class Artifact : Eamon.Game.Artifact, Framework.IArtifact
	{
		public override string Desc
		{
			get
			{
				var result = base.Desc;

				var room = GetInRoom(true) as Framework.IRoom;

				if (Globals.EnableGameOverrides && gGameState != null && room != null && room.Uid == gGameState.Ro && room.IsDimLightRoom() && gGameState.Ls <= 0)
				{
					result = string.Format("You can vaguely make out {0} in the {1}.", GetTheName(buf: Globals.Buf01), gGameState.IsNightTime() ? "darkness" : "white haze");
				}

				return result;
			}

			set
			{
				base.Desc = value;
			}
		}

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			var result = base.BuildPrintedFullDesc(buf, showName);

			// Reset solitary tombstone's Desc value after initial viewing

			if (Uid == 10 && !Seen)
			{
				Desc = "This tombstone is very old, possibly several hundred years.  Why is it all by itself, you pause to wonder?";
			}

			return result;
		}

		public virtual bool IsDecoration()
		{
			return Uid == 41 || Uid == 42;
		}

		public virtual long GetLeverageBonus()
		{
			return Uid == 7 ? 5 : Uid == 28 ? 9 : GeneralWeapon != null && GeneralWeapon.Field2 > 3 ? GeneralWeapon.Field2 : 0;
		}
	}
}
