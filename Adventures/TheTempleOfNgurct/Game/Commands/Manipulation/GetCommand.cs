
// GetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IGetCommand))]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		protected override void PlayerExecute()
		{
			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (DobjArtifact != null && !DobjArtifact.IsWeapon01() && Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) > 0)
			{
				PrintEnemiesNearby();

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
			}
			else
			{
				base.PlayerExecute();

				var scimitarArtifact = Globals.ADB[41];

				Debug.Assert(scimitarArtifact != null);

				// Alignment conflict

				if (!gameState.AlignmentConflict && scimitarArtifact.IsCarriedByCharacter())
				{
					Globals.Engine.PrintEffectDesc(28);

					var combatSystem = Globals.CreateInstance<EamonRT.Framework.Combat.ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = ActorMonster;

						x.OmitArmor = true;
					});

					combatSystem.ExecuteCalculateDamage(1, 8, 1);

					gameState.AlignmentConflict = true;
				}
			}
		}

		protected override void PlayerFinishParsing()
		{
			CommandParser.ParseName();

			if (string.Equals(CommandParser.ObjData.Name, "all", StringComparison.OrdinalIgnoreCase))
			{
				if (Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) > 0)
				{
					PrintEnemiesNearby();

					CommandParser.NextState.Discarded = true;

					CommandParser.NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
				}
				else
				{
					GetAll = true;
				}
			}
			else if (ActorRoom.Type == Enums.RoomType.Indoors && CommandParser.ObjData.Name.IndexOf("torch", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				if (Globals.GameState.GetNBTL(Enums.Friendliness.Enemy) > 0)
				{
					PrintEnemiesNearby();

					CommandParser.NextState.Discarded = true;

					CommandParser.NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
				}
				else
				{
					Globals.Out.Print("They are bolted firmly to the walls.");

					CommandParser.NextState.Discarded = true;

					CommandParser.NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
				}
			}
			else
			{
				CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsInRoom(ActorRoom),
					a => a.IsEmbeddedInRoom(ActorRoom)
				};

				CommandParser.ObjData.ArtifactNotFoundFunc = PrintCantVerbThat;

				PlayerResolveArtifact();
			}
		}
	}
}
