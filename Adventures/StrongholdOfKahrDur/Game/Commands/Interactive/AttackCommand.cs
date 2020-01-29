
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null || gDobjMonster != null);

			// Can't attack armoire/bookshelf/pouch

			if ((BlastSpell || gActorMonster.Weapon > 0) && gDobjArtifact != null && (gDobjArtifact.Uid == 3 || gDobjArtifact.Uid == 11 || gDobjArtifact.Uid == 15))
			{
				var ac = gDobjArtifact.GetArtifactCategory(new ArtifactType[] { ArtifactType.InContainer, ArtifactType.User1 });

				Debug.Assert(ac != null);

				var type = ac.Type;

				ac.Type = ArtifactType.Gold;

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
