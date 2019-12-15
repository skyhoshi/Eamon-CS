
// DigCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class DigCommand : EamonRT.Game.Commands.Command, Framework.Commands.IDigCommand
	{
		public override void PrintCantVerbHere()
		{
			if (Globals.GameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				PrintEnemiesNearby();
			}
			else
			{
				Globals.Out.Print("You cannot {0} here.", Verb);
			}
		}

		public override void PlayerExecute()
		{
			var buriedArtifacts = Globals.Engine.GetArtifactList(a => a.CastTo<Framework.IArtifact>().IsBuriedInRoom(ActorRoom));

			if (buriedArtifacts.Count > 0)
			{
				Globals.Out.Print("You found something!");

				buriedArtifacts[0].SetInRoom(ActorRoom);
			}
			else
			{
				Globals.Out.Print("You dig but find nothing.");
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override bool IsAllowedInRoom()
		{
			return Globals.GameState.GetNBTL(Friendliness.Enemy) <= 0 && ActorRoom.CastTo<Framework.IRoom>().IsDigCommandAllowedInRoom();
		}

		public DigCommand()
		{
			SortOrder = 440;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "DigCommand";

			Verb = "dig";

			Type = CommandType.Miscellaneous;
		}
	}
}
