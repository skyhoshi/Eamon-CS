
// PlayerResurrectedState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class PlayerResurrectedState : EamonRT.Game.States.State, Framework.States.IPlayerResurrectedState
	{
		public override void Execute()
		{
			var characterMonster = gMDB[gGameState.Cm];

			Debug.Assert(characterMonster != null);

			var room = characterMonster.GetInRoom();

			Debug.Assert(room != null);

			gOut.Print("You feel yourself spinning in infinity, and then re-embodied in your former self.  You slowly open your eyes, with a sense of bewilderment, and find yourself lying in an open grave some six feet in the ground.");

			gGameState.Die = 0;

			characterMonster.DmgTaken = 0;

			gEngine.ResetMonsterStats(characterMonster);

			var artifactList = characterMonster.GetContainedList();

			gOut.EnableOutput = false;

			foreach (var artifact in artifactList)
			{
				if (gEngine.RollDice(1, 100, 0) < 25)
				{
					if (artifact.IsWornByCharacter())
					{
						Globals.CurrState = Globals.CreateInstance<IRemoveCommand>(x =>
						{
							x.ActorMonster = characterMonster;

							x.ActorRoom = room;

							x.Dobj = artifact;
						});

						Globals.CurrCommand.PlayerExecute();
					}

					Globals.CurrState = Globals.CreateInstance<IDropCommand>(x =>
					{
						x.ActorMonster = characterMonster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					Globals.CurrCommand.PlayerExecute();

					Globals.CurrState = this;
				}
			}

			gOut.EnableOutput = true;

			gGameState.Ro = 19;

			gGameState.R2 = gGameState.Ro;

			NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
			{
				x.MoveMonsters = false;
			});

			Globals.NextState = NextState;
		}
	}
}
