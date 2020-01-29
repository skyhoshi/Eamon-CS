
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null || gDobjMonster != null);

			if ((BlastSpell || gActorMonster.Weapon > 0) && gDobjArtifact != null)
			{
				switch (gDobjArtifact.Uid)
				{
					case 30:
					case 80:

						// Can't attack oven or safe

						gEngine.PrintEffectDesc(162);

						NextState = Globals.CreateInstance<IMonsterStartState>();

						break;

					case 41:

						// Attack cell = Open Jail

						var ac = gDobjArtifact.InContainer;

						Debug.Assert(ac != null);

						ac.SetOpen(false);

						var command = Globals.CreateInstance<IOpenCommand>();

						CopyCommandData(command);

						NextState = command;

						break;

					case 129:

						// Can't attack/blast the Runcible Cargo

						gOut.Print("That sounds quite dangerous!");

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

		public override bool IsAllowedInRoom()
		{
			// Disable AttackCommand in water rooms

			return !gActorRoom.IsWaterRoom();
		}
	}
}
