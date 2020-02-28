
// FreeCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : Command, IFreeCommand
	{
		/// <summary>
		/// An event that fires before a guard <see cref="IMonster">Monster</see> prevents a bound Monster from being freed.
		/// </summary>
		public const long PpeBeforeGuardMonsterCheck = 1;

		public virtual IMonster Monster { get; set; }

		public virtual IMonster Guard { get; set; }

		public virtual IArtifact Key { get; set; }

		public virtual void PrintMonsterFreed()
		{
			gOut.Print("You have freed {0}{1}.",
				Monster.GetTheName(),
				Key != null ? string.Format(" with {0}", Key.GetTheName(buf: Globals.Buf01)) : "");
		}

		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			var ac = gDobjArtifact.BoundMonster;

			if (ac != null)
			{
				var monsterUid = ac.GetMonsterUid();

				var keyUid = ac.GetKeyUid();

				var guardUid = ac.Field3;

				Monster = monsterUid > 0 ? gMDB[monsterUid] : null;

				Key = keyUid > 0 ? gADB[keyUid] : null;

				Guard = guardUid > 0 ? gMDB[guardUid] : null;

				Debug.Assert(Monster != null);

				PlayerProcessEvents(PpeBeforeGuardMonsterCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (Guard != null && Guard.IsInRoom(gActorRoom))
				{
					gOut.Print("{0} won't let you!", Guard.GetTheName(true));

					goto Cleanup;
				}

				if (keyUid == -1)
				{
					gOut.Print("There's no obvious way to do that.");

					goto Cleanup;
				}

				if (Key != null && !Key.IsCarriedByCharacter() && !Key.IsWornByCharacter() && !Key.IsInRoom(gActorRoom))
				{
					gOut.Print("You don't have the key.");

					goto Cleanup;
				}

				PrintMonsterFreed();

				Monster.SetInRoom(gActorRoom);

				gDobjArtifact.SetInLimbo();
			}
			else
			{
				PrintCantVerbObj(gDobjArtifact);

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
			gCommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch01;

			gCommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch01;

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

			Type = CommandType.Interactive;
		}
	}
}
