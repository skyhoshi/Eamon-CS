
// FreeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
		/// <summary>
		/// This event fires before a check is made to see if a guard monster prevents
		/// a bound monster from being freed.
		/// </summary>
		public const long PpeBeforeGuardMonsterCheck = 1;

		public virtual IMonster Monster { get; set; }

		public virtual IMonster Guard { get; set; }

		public virtual IArtifact Key { get; set; }

		public virtual void PrintMonsterFreed()
		{
			Globals.Out.Print("You have freed {0}{1}.",
				Monster.GetDecoratedName03(false, true, false, false, Globals.Buf),
				Key != null ? string.Format(" with {0}", Key.GetDecoratedName03(false, true, false, false, Globals.Buf01)) : "");
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var ac = DobjArtifact.BoundMonster;

			if (ac != null)
			{
				var monsterUid = ac.GetMonsterUid();

				var keyUid = ac.GetKeyUid();

				var guardUid = ac.Field3;

				Monster = monsterUid > 0 ? Globals.MDB[monsterUid] : null;

				Key = keyUid > 0 ? Globals.ADB[keyUid] : null;

				Guard = guardUid > 0 ? Globals.MDB[guardUid] : null;

				Debug.Assert(Monster != null);

				PlayerProcessEvents(PpeBeforeGuardMonsterCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (Guard != null && Guard.IsInRoom(ActorRoom))
				{
					Globals.Out.Print("{0} won't let you!", Guard.GetDecoratedName03(true, true, false, false, Globals.Buf));

					goto Cleanup;
				}

				if (keyUid == -1)
				{
					Globals.Out.Print("There's no obvious way to do that.");

					goto Cleanup;
				}

				if (Key != null && !Key.IsCarriedByCharacter() && !Key.IsWornByCharacter() && !Key.IsInRoom(ActorRoom))
				{
					Globals.Out.Print("You don't have the key.");

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

		public override void PlayerFinishParsing()
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
