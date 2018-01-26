
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework;
using TheTempleOfNgurct.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IOpenCommand))]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		protected override void PlayerProcessEvents01()
		{
			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			// If chest opened reveal cobra

			if (DobjArtifact.Uid == 54 && !gameState.CobraAppeared)
			{
				var cobraMonster = Globals.MDB[52];

				Debug.Assert(cobraMonster != null);

				cobraMonster.SetInRoom(ActorRoom);

				Globals.Engine.CheckEnemies();

				gameState.CobraAppeared = true;

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();

				GotoCleanup = true;
			}
			else
			{
				base.PlayerProcessEvents01();
			}
		}

		protected override void PrintOpened(Eamon.Framework.IArtifact artifact)
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

		protected override void PrintLocked(Eamon.Framework.IArtifact artifact)
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

		protected override void PrintOpenObjWithKey(Eamon.Framework.IArtifact artifact, Eamon.Framework.IArtifact key)
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

		protected override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		protected override bool IsAllowedInRoom()
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
