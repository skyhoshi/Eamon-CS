
// GetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void PlayerExecute()
		{
			if (gDobjArtifact != null && gDobjArtifact.GeneralWeapon == null && gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				PrintEnemiesNearby();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.PlayerExecute();

				var scimitarArtifact = gADB[41];

				Debug.Assert(scimitarArtifact != null);

				// Alignment conflict

				if (!gGameState.AlignmentConflict && scimitarArtifact.IsCarriedByCharacter())
				{
					gEngine.PrintEffectDesc(28);

					var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.DfMonster = gActorMonster;

						x.OmitArmor = true;
					});

					combatSystem.ExecuteCalculateDamage(1, 8, 1);

					gGameState.AlignmentConflict = true;
				}
			}
		}

		public override void PlayerFinishParsing()
		{
			gCommandParser.ParseName();

			if (string.Equals(gCommandParser.ObjData.Name, "all", StringComparison.OrdinalIgnoreCase))
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
				{
					PrintEnemiesNearby();

					gCommandParser.NextState = Globals.CreateInstance<IStartState>();
				}
				else
				{
					GetAll = true;
				}
			}
			else if (gActorRoom.Type == RoomType.Indoors && gCommandParser.ObjData.Name.IndexOf("torch", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
				{
					PrintEnemiesNearby();

					gCommandParser.NextState = Globals.CreateInstance<IStartState>();
				}
				else
				{
					gOut.Print("They are bolted firmly to the walls.");

					gCommandParser.NextState = Globals.CreateInstance<IMonsterStartState>();
				}
			}
			else
			{
				gCommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsInRoom(gActorRoom),
					a => a.IsEmbeddedInRoom(gActorRoom),
					a => a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(gActorRoom, gEngine.ExposeContainersRecursively)
				};

				gCommandParser.ObjData.ArtifactNotFoundFunc = PrintCantVerbThat;

				PlayerResolveArtifact();
			}
		}
	}
}
