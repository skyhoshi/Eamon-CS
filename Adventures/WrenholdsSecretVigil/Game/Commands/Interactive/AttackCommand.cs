
// AttackCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IAttackCommand))]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		protected override void PrintAlreadyBrokeIt(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Swallower shark

			if (artifact.Uid == 31)
			{
				Globals.Out.Print("You already mangled {0}!", artifact.EvalPlural("it", "them"));
			}
			else
			{
				base.PrintAlreadyBrokeIt(artifact);
			}
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			var wpnArtifact = ActorMonster.Weapon > 0 ? Globals.ADB[ActorMonster.Weapon] : null;

			var ac = wpnArtifact != null ? wpnArtifact.GetArtifactCategory(new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon }) : null;

			if (BlastSpell)
			{
				// Blast rock

				if (DobjArtifact != null && DobjArtifact.Uid == 17)
				{
					Globals.Engine.PrintEffectDesc(14);

					DobjArtifact.SetInLimbo();

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
				}

				// Blast slime

				else if (DobjArtifact != null && (DobjArtifact.Uid == 24 || DobjArtifact.Uid == 25))
				{
					var slimeArtifact1 = Globals.ADB[24];

					Debug.Assert(slimeArtifact1 != null);

					var slimeArtifact2 = Globals.ADB[25];

					Debug.Assert(slimeArtifact2 != null);

					gameState.SlimeBlasts++;

					Globals.Engine.PrintEffectDesc(1 + gameState.SlimeBlasts);

					if (gameState.SlimeBlasts == 3)
					{
						slimeArtifact1.SetInLimbo();

						slimeArtifact2.SetInLimbo();
					}

					NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
				}
				else
				{
					base.PlayerExecute();
				}
			}

			// Attack slime will dissolve weapon (bows excluded)

			else if (DobjArtifact != null && (DobjArtifact.Uid == 24 || DobjArtifact.Uid == 25) && ac != null && ac.Field2 != (long)Enums.Weapon.Bow)
			{
				Globals.Engine.PrintEffectDesc(18);

				if (gameState.Ls == wpnArtifact.Uid)
				{
					Globals.Engine.LightOut(wpnArtifact);
				}

				Globals.Engine.RemoveWeight(wpnArtifact);

				wpnArtifact.SetInLimbo();

				var rc = wpnArtifact.RemoveStateDesc(wpnArtifact.GetReadyWeaponDesc());

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ActorMonster.Weapon = -1;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
