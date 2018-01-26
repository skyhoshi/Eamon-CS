
// Artifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework;

namespace StrongholdOfKahrDur.Game
{
	[ClassMappings(typeof(Eamon.Framework.IArtifact))]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool IsAttackable()
		{
			return Uid != 3 && Uid != 11 && Uid != 15 ? base.IsAttackable() : false;
		}
	}
}
