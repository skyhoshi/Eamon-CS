
// FreeCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : Command, IFreeCommand
	{
		protected virtual IMonster Monster { get; set; }

		protected virtual IMonster Guard { get; set; }

		protected virtual IArtifact Key { get; set; }

		protected virtual void PrintMonsterFreed()
		{
			Globals.Out.Write("{0}You have freed {1}{2}.{0}",
				Environment.NewLine,
				Monster.GetDecoratedName03(false, true, false, false, Globals.Buf),
				Key != null ? string.Format(" with {0}", Key.GetDecoratedName03(false, true, false, false, Globals.Buf01)) : "");
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var ac = DobjArtifact.GetArtifactClass(Enums.ArtifactType.BoundMonster);

			if (ac != null)
			{
				var monsterUid = ac.GetMonsterUid();

				var keyUid = ac.GetKeyUid();

				var guardUid = ac.Field7;

				Monster = monsterUid > 0 ? Globals.MDB[monsterUid] : null;

				Key = keyUid > 0 ? Globals.ADB[keyUid] : null;

				Guard = guardUid > 0 ? Globals.MDB[guardUid] : null;

				Debug.Assert(Monster != null);

				if (Guard != null && Guard.IsInRoom(ActorRoom))
				{
					Globals.Out.Write("{0}{1} won't let you!{0}", Environment.NewLine, Guard.GetDecoratedName03(true, true, false, false, Globals.Buf));

					goto Cleanup;
				}

				if (keyUid == -1)
				{
					Globals.Out.WriteLine("{0}There's no obvious way to do that.", Environment.NewLine);

					goto Cleanup;
				}

				if (Key != null && !Key.IsCarriedByCharacter() && !Key.IsWornByCharacter() && !Key.IsInRoom(ActorRoom))
				{
					Globals.Out.WriteLine("{0}You don't have the key.", Environment.NewLine);

					goto Cleanup;
				}

				PrintMonsterFreed();

				Monster.SetInRoom(ActorRoom);

				Globals.Engine.RemoveWeight(DobjArtifact);

				DobjArtifact.SetInLimbo();

				Globals.Engine.CheckEnemies();
			}
			else
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			CommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

			CommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch01;

			PlayerResolveArtifact();
		}

		public FreeCommand()
		{
			SortOrder = 270;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "FreeCommand";

			Verb = "free";

			Type = Enums.CommandType.Interactive;
		}
	}
}
