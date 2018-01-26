
// Artifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings(typeof(Eamon.Framework.IArtifact))]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool IsReadyableByMonsterUid(long monsterUid)
		{
			// Only player can wield fireball wand

			return Uid != 63 && base.IsReadyableByMonsterUid(monsterUid);
		}
	}
}
