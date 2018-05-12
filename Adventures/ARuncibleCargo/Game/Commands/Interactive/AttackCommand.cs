
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if ((BlastSpell || ActorMonster.Weapon > 0) && DobjArtifact != null)
			{
				switch (DobjArtifact.Uid)
				{
					case 30:
					case 80:

						// Can't attack oven or safe

						Globals.Engine.PrintEffectDesc(162);

						NextState = Globals.CreateInstance<IMonsterStartState>();

						break;

					case 41:

						// Attack cell = Open Jail

						var ac = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Container);

						Debug.Assert(ac != null);

						ac.SetOpen(false);

						var command = Globals.CreateInstance<IOpenCommand>();

						CopyCommandData(command);

						NextState = command;

						break;

					case 129:

						// Can't attack/blast the Runcible Cargo

						Globals.Out.Print("That sounds quite dangerous!");

						NextState = Globals.CreateInstance<IMonsterStartState>();

						break;

					default:

						base.PlayerExecute();

						break;
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}

		protected override bool IsAllowedInRoom()
		{
			// Disable AttackCommand in water rooms

			return !ActorRoom.CastTo<Framework.IRoom>().IsWaterRoom();
		}
	}
}
