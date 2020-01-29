
// GetCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void PlayerFinishParsing()
		{
			gCommandParser.ParseName();

			if (string.Equals(gCommandParser.ObjData.Name, "all", StringComparison.OrdinalIgnoreCase))
			{
				GetAll = true;
			}
			else if ((gActorRoom.Uid == 4 || gActorRoom.Uid == 20 || gActorRoom.Uid == 22) && gCommandParser.ObjData.Name.IndexOf("torch", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				gOut.Print("All torches are bolted to the wall and cannot be removed.");

				gCommandParser.NextState = Globals.CreateInstance<IMonsterStartState>();
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
