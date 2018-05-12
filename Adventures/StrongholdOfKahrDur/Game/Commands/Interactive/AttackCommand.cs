
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// Can't attack armoire/bookshelf/pouch

			if ((BlastSpell || ActorMonster.Weapon > 0) && DobjArtifact != null && (DobjArtifact.Uid == 3 || DobjArtifact.Uid == 11 || DobjArtifact.Uid == 15))
			{
				var ac = DobjArtifact.GetArtifactCategory(new Enums.ArtifactType[] { Enums.ArtifactType.Container, Enums.ArtifactType.User1 });

				Debug.Assert(ac != null);

				var type = ac.Type;

				ac.Type = Enums.ArtifactType.Gold;

				base.PlayerExecute();

				ac.Type = type;
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
