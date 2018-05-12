
// DigCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class DigCommand : EamonRT.Game.Commands.Command, Framework.Commands.IDigCommand
	{
		protected override void PrintCantVerbHere()
		{
			Globals.Out.Print("You cannot {0} here.", Verb);
		}

		protected override void PlayerExecute()
		{
			var buriedArtifacts = Globals.Engine.GetArtifactList(() => true, a => a.CastTo<Framework.IArtifact>().IsBuriedInRoom(ActorRoom));

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

		protected override bool IsAllowedInRoom()
		{
			return ActorRoom.CastTo<Framework.IRoom>().IsDigCommandAllowedInRoom();
		}

		public DigCommand()
		{
			SortOrder = 440;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "DigCommand";

			Verb = "dig";

			Type = Enums.CommandType.Miscellaneous;
		}
	}
}
