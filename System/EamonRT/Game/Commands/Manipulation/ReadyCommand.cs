
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ReadyCommand : Command, IReadyCommand
	{
		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var artTypes = Globals.IsRulesetVersion(5) ? 
				new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon } : 
				new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon, ArtifactType.Wearable };

			var ac = DobjArtifact.GetArtifactCategory(artTypes, false);

			if (ac != null)
			{
				if (ac.Type == ArtifactType.Wearable)
				{
					NextState = Globals.CreateInstance<IWearCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				if (!DobjArtifact.IsReadyableByCharacter())
				{
					PrintNotReadyableWeapon(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByCharacter())
				{
					if (DobjArtifact.IsCarriedByContainer())
					{
						PrintRemovingFirst(DobjArtifact);
					}
					else
					{
						PrintTakingFirst(DobjArtifact);
					}

					NextState = Globals.CreateInstance<IGetCommand>(x =>
					{
						x.OmitWeightCheck = DobjArtifact.IsCarriedByCharacter(true);
					});

					CopyCommandData(NextState as ICommand);

					NextState.NextState = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(NextState.NextState as ICommand);

					goto Cleanup;
				}

				// can't use two-handed weapon while wearing shield

				if (Globals.GameState.Sh > 0 && ac.Field5 > 1)
				{
					var shield = Globals.ADB[Globals.GameState.Sh];

					Debug.Assert(shield != null);

					PrintCantReadyWeaponWithShield(DobjArtifact, shield);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				var wpnArtifact = Globals.ADB[ActorMonster.Weapon];

				if (wpnArtifact != null)
				{
					rc = wpnArtifact.RemoveStateDesc(wpnArtifact.GetReadyWeaponDesc());

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}

				ActorMonster.Weapon = DobjArtifact.Uid;

				rc = DobjArtifact.AddStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.Print("{0} readied.", DobjArtifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
			}
			else
			{
				PrintNotWeapon(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override void MonsterExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			if (DobjArtifact.IsReadyableByMonster(ActorMonster) && DobjArtifact.IsCarriedByMonster(ActorMonster))
			{
				var wpnArtifact = Globals.ADB[ActorMonster.Weapon];

				if (wpnArtifact != null)
				{
					rc = wpnArtifact.RemoveStateDesc(wpnArtifact.GetReadyWeaponDesc());

					Debug.Assert(Globals.Engine.IsSuccess(rc));
				}

				ActorMonster.Weapon = DobjArtifact.Uid;

				rc = DobjArtifact.AddStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				var charMonster = Globals.MDB[Globals.GameState.Cm];

				Debug.Assert(charMonster != null);

				if (charMonster.IsInRoom(ActorRoom))
				{
					if (ActorRoom.IsLit())
					{
						var monsterName = ActorMonster.EvalPlural(ActorMonster.GetDecoratedName03(true, true, false, false, Globals.Buf), ActorMonster.GetDecoratedName02(true, true, false, true, Globals.Buf01));

						Globals.Out.Print("{0} readies {1}.", monsterName, DobjArtifact.GetDecoratedName02(false, true, false, false, Globals.Buf));
					}
					else
					{
						var monsterName = string.Format("An unseen {0}", ActorMonster.CheckNBTLHostility() ? "offender" : "entity");

						Globals.Out.Print("{0} readies {1}.", monsterName, "a weapon");
					}

					if (ActorMonster.CheckNBTLHostility())
					{
						Globals.Thread.Sleep(Globals.GameState.PauseCombatMs);
					}
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();
		}

		public ReadyCommand()
		{
			SortOrder = 210;

			Name = "ReadyCommand";

			Verb = "ready";

			Type = CommandType.Manipulation;
		}
	}
}
