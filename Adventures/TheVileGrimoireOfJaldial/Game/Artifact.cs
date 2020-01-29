
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

		public virtual long GetLeverageBonus()
		{
			return Uid == 7 ? 5 : Uid == 28 ? 9 : GeneralWeapon != null && GeneralWeapon.Field2 > 3 ? GeneralWeapon.Field2 : 0;
		}
	}
}
