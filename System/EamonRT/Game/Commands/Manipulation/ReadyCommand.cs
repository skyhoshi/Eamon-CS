
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var artTypes = Globals.IsRulesetVersion(5) ? 
				new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon } : 
				new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon, Enums.ArtifactType.Wearable };

			var ac = DobjArtifact.GetArtifactCategory(artTypes, false);

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

				var monsters = Globals.Engine.GetMonsterList(() => true, m => m.IsInRoom(ActorRoom) && m != ActorMonster);

				if (monsters.Contains(charMonster))
				{
					var monsterName = ActorRoom.EvalLightLevel("An unseen offender", ActorMonster.EvalPlural(ActorMonster.GetDecoratedName03(true, true, false, false, Globals.Buf), ActorMonster.GetDecoratedName02(true, true, false, true, Globals.Buf01)));

					Globals.Out.Print("{0} readies {1}.", monsterName, ActorRoom.EvalLightLevel("a weapon", DobjArtifact.GetDecoratedName02(false, true, false, false, Globals.Buf)));
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

			Type = Enums.CommandType.Manipulation;
		}
	}
}

/* EamonCsCodeTemplate

// ReadyCommand.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static YourAdventureName.Game.Plugin.PluginContext;

namespace YourAdventureName.Game.Commands
{
	[ClassMappings]
	public class ReadyCommand : EamonRT.Game.Commands.ReadyCommand, IReadyCommand
	{

	}
}
EamonCsCodeTemplate */
