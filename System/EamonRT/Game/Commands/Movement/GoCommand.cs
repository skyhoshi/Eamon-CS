
// GoCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
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
	public class GoCommand : Command, IGoCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			var ac = DobjArtifact.DoorGate;

			if (ac != null)
			{
				NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
				{
					x.Artifact = DobjArtifact;
				});
			}
			else
			{
				PrintDontFollowYou();

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
			CommandParser.ParseName();

			CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
			{
				a => a.IsInRoom(ActorRoom),
				a => a.IsEmbeddedInRoom(ActorRoom)
			};

			CommandParser.ObjData.ArtifactNotFoundFunc = PrintNothingHereByThatName;

			PlayerResolveArtifact();
		}

		public GoCommand()
		{
			SortOrder = 95;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "GoCommand";

			Verb = "go";

			Type = CommandType.Movement;
		}
	}
}
