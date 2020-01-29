
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
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
		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(gDobjArtifact != null);

			var ac = gDobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (ac != null)
			{
				if (ac.Type == ArtifactType.Wearable)
				{
					NextState = Globals.CreateInstance<IWearCommand>();

					CopyCommandData(NextState as ICommand);

					goto Cleanup;
				}

				if (!gDobjArtifact.IsReadyableByCharacter())
				{
					PrintNotReadyableWeapon(gDobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (!gDobjArtifact.IsCarriedByCharacter())
				{
					if (!GetCommandCalled)
					{
						RedirectToGetCommand<IReadyCommand>(gDobjArtifact);
					}
					else if (gDobjArtifact.DisguisedMonster == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				// can't use two-handed weapon while wearing shield

				if (gGameState.Sh > 0 && ac.Field5 > 1)
				{
					var shield = gADB[gGameState.Sh];

					Debug.Assert(shield != null);

					PrintCantReadyWeaponWithShield(gDobjArtifact, shield);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				var wpnArtifact = gADB[gActorMonster.Weapon];

				if (wpnArtifact != null)
				{
					rc = wpnArtifact.RemoveStateDesc(wpnArtifact.GetReadyWeaponDesc());

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				gActorMonster.Weapon = gDobjArtifact.Uid;

				rc = gDobjArtifact.AddStateDesc(gDobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Print("{0} readied.", gDobjArtifact.GetNoneName(true, false));
			}
			else
			{
				PrintNotWeapon(gDobjArtifact);

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

			Debug.Assert(gDobjArtifact != null);

			if (gDobjArtifact.IsReadyableByMonster(gActorMonster) && gDobjArtifact.IsCarriedByMonster(gActorMonster))
			{
				var wpnArtifact = gADB[gActorMonster.Weapon];

				if (wpnArtifact != null)
				{
					rc = wpnArtifact.RemoveStateDesc(wpnArtifact.GetReadyWeaponDesc());

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				gActorMonster.Weapon = gDobjArtifact.Uid;

				rc = gDobjArtifact.AddStateDesc(gDobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				var charMonster = gMDB[gGameState.Cm];

				Debug.Assert(charMonster != null);

				if (charMonster.IsInRoom(gActorRoom))
				{
					if (gActorRoom.IsLit())
					{
						var monsterName = gActorMonster.EvalPlural(gActorMonster.GetTheName(true), gActorMonster.GetArticleName(true, true, false, true, Globals.Buf01));

						gOut.Print("{0} readies {1}.", monsterName, gDobjArtifact.GetArticleName());
					}
					else
					{
						var monsterName = string.Format("An unseen {0}", gActorMonster.CheckNBTLHostility() ? "offender" : "entity");

						gOut.Print("{0} readies {1}.", monsterName, "a weapon");
					}

					if (gActorMonster.CheckNBTLHostility())
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

		public override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var ac = artifact.GetArtifactCategory(ArtTypes, false);

			if (ac != null)
			{
				if (ac.Type == ArtifactType.Wearable)
				{
					return artifact.IsCarriedByCharacter();
				}
				else		
				{
					return !artifact.IsReadyableByCharacter() || artifact.IsCarriedByCharacter();
				}
			}
			else
			{
				return true;
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
