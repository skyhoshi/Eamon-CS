﻿
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
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
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact ShieldArtifact { get; set; }

		/// <summary></summary>
		public virtual IArtifact PlayerActorWeapon { get; set; }

		/// <summary></summary>
		public virtual IArtifact MonsterActorWeapon { get; set; }

		/// <summary></summary>
		public virtual string MonsterName { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (DobjArtAc != null)
			{
				if (DobjArtAc.Type == ArtifactType.Wearable)
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
					if (!GetCommandCalled)
					{
						RedirectToGetCommand<IReadyCommand>(DobjArtifact);
					}
					else if (DobjArtifact.DisguisedMonster == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				// can't use two-handed weapon while wearing shield

				if (gGameState.Sh > 0 && DobjArtAc.Field5 > 1)
				{
					ShieldArtifact = gADB[gGameState.Sh];

					Debug.Assert(ShieldArtifact != null);

					PrintCantReadyWeaponWithShield(DobjArtifact, ShieldArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				PlayerActorWeapon = gADB[ActorMonster.Weapon];

				if (PlayerActorWeapon != null)
				{
					rc = PlayerActorWeapon.RemoveStateDesc(PlayerActorWeapon.GetReadyWeaponDesc());

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				ActorMonster.Weapon = DobjArtifact.Uid;

				rc = DobjArtifact.AddStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Print("{0} readied.", DobjArtifact.GetNoneName(true, false));
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
				MonsterActorWeapon = gADB[ActorMonster.Weapon];

				if (MonsterActorWeapon != null)
				{
					rc = MonsterActorWeapon.RemoveStateDesc(MonsterActorWeapon.GetReadyWeaponDesc());

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				ActorMonster.Weapon = DobjArtifact.Uid;

				rc = DobjArtifact.AddStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				Debug.Assert(gCharMonster != null);

				if (gCharMonster.IsInRoom(ActorRoom))
				{
					if (ActorRoom.IsLit())
					{
						MonsterName = ActorMonster.EvalPlural(ActorMonster.GetTheName(true), ActorMonster.GetArticleName(true, true, false, true, Globals.Buf01));

						gOut.Print("{0} readies {1}.", MonsterName, DobjArtifact.GetArticleName());
					}
					else
					{
						MonsterName = string.Format("An unseen {0}", ActorMonster.CheckNBTLHostility() ? "offender" : "entity");

						gOut.Print("{0} readies {1}.", MonsterName, "a weapon");
					}

					if (ActorMonster.CheckNBTLHostility())
					{
						Globals.Thread.Sleep(gGameState.PauseCombatMs);
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

		public ReadyCommand()
		{
			SortOrder = 210;

			Name = "ReadyCommand";

			Verb = "ready";

			Type = CommandType.Manipulation;

			ArtTypes = Globals.IsRulesetVersion(5) ?
				new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon } :
				new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon, ArtifactType.Wearable };
		}
	}
}
