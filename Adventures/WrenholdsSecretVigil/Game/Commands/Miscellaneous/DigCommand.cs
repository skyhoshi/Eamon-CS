
// DigCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using WrenholdsSecretVigil.Framework;
using WrenholdsSecretVigil.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class DigCommand : EamonRT.Game.Commands.Command, IDigCommand
	{
		protected override void PrintCantVerbHere()
		{
			Globals.Out.Write("{0}You cannot {1} here.{0}", Environment.NewLine, Verb);
		}

		protected override void PlayerExecute()
		{
			var buriedArtifacts = Globals.Engine.GetArtifactList(() => true, a => a.CastTo<IArtifact>().IsBuriedInRoom(ActorRoom));

			if (buriedArtifacts.Count > 0)
			{
				Globals.Out.Write("{0}You found something!{0}", Environment.NewLine);

				buriedArtifacts[0].SetInRoom(ActorRoom);
			}
			else
			{
				Globals.Out.Write("{0}You dig but find nothing.{0}", Environment.NewLine);
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
		}

		protected override bool IsAllowedInRoom()
		{
			return ActorRoom.CastTo<IRoom>().IsDigCommandAllowedInRoom();
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
