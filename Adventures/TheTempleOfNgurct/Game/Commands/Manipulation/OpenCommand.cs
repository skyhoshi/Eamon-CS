
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			// If chest opened reveal cobra

			if (eventType == PpeAfterArtifactOpen && DobjArtifact.Uid == 54 && !gameState.CobraAppeared)
			{
				var cobraMonster = Globals.MDB[52];

				Debug.Assert(cobraMonster != null);

				cobraMonster.SetInRoom(ActorRoom);

				Globals.Engine.CheckEnemies();

				gameState.CobraAppeared = true;

				NextState = Globals.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public override void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Chest

			if (artifact.Uid == 54)
			{
				Globals.Out.Print("{0} is open!", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
			}

			// Oak door

			else if (artifact.Uid == 85)
			{
				Globals.Out.Print("{0} swings open to your gentle touch.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
			}

			// Cell doors

			else if (artifact.Uid >= 86 && artifact.Uid <= 88)
			{
				Globals.Out.Print("{0} squeaks open.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
			}
			else
			{
				base.PrintOpened(artifact);
			}
		}

		public override void PrintLocked(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Chest

			if (artifact.Uid == 54)
			{
				Globals.Out.Print("{0} is locked -- what do you think that padlock is... chopped liver?",	artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
			}

			// Oak door

			else if (artifact.Uid == 85)
			{
				Globals.Out.Print("{0} is locked shut!", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
			}

			// Cell doors

			else if (artifact.Uid >= 86 && artifact.Uid <= 88)
			{
				Globals.Out.Print("{0} is locked!", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
			}
			else
			{
				base.PrintLocked(artifact);
			}
		}

		public override void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			// Chest/oak door/cell doors

			if (artifact.Uid == 54 || artifact.Uid == 85 || (artifact.Uid >= 86 && artifact.Uid <= 88))
			{
				Globals.Out.Print("You unlock {0} with {1}.", artifact.EvalPlural("it", "them"), key.GetDecoratedName03(false, true, false, false, Globals.Buf));

				PrintOpened(artifact);
			}
			else
			{
				base.PrintOpenObjWithKey(artifact, key);
			}
		}

		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override bool IsAllowedInRoom()
		{
			return Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) <= 0;
		}

		public OpenCommand()
		{
			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
