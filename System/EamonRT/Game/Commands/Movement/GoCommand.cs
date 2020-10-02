﻿
// GoCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Classes;
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
		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			DobjArtAc = DobjArtifact.DoorGate;

			if (DobjArtAc != null)
			{
				NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
				{
					x.DoorGateArtifact = DobjArtifact;
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

		public GoCommand()
		{
			Synonyms = new string[] { "enter" };

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
