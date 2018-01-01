
// ReadyCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ReadyCommand : Command, IReadyCommand
	{
		protected override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var artClasses = Globals.IsRulesetVersion(5) ? 
				new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon } : 
				new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon, Enums.ArtifactType.Wearable };

			var ac = DobjArtifact.GetArtifactClass(artClasses, false);

			if (ac != null)
			{
				if (ac.Type == Enums.ArtifactType.Wearable)
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
					PrintTakingFirst(DobjArtifact);

					NextState = Globals.CreateInstance<IGetCommand>();

					CopyCommandData(NextState as ICommand);

					NextState.NextState = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(NextState.NextState as ICommand);

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

				Globals.Out.Write("{0}{1} readied.{0}", Environment.NewLine, DobjArtifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
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

		protected override void MonsterExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			if (ActorRoom.IsLit() && DobjArtifact.IsReadyableByMonster(ActorMonster) && DobjArtifact.IsCarriedByMonster(ActorMonster))
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

				var monsters = Globals.Engine.GetMonsterList(() => true, m => m.IsInRoom(ActorRoom) && m != ActorMonster);

				if (monsters.Contains(charMonster))
				{
					var monsterName = ActorMonster.EvalPlural(ActorMonster.GetDecoratedName03(true, true, false, false, Globals.Buf), ActorMonster.GetDecoratedName02(true, true, false, true, Globals.Buf01));

					Globals.Out.Write("{0}{1} readies {2}.{0}", Environment.NewLine, monsterName, DobjArtifact.GetDecoratedName02(false, true, false, false, Globals.Buf));
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

		protected override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();
		}

		public ReadyCommand()
		{
			SortOrder = 210;

			Name = "ReadyCommand";

			Verb = "ready";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
