
// DropCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class DropCommand : Command, IDropCommand
	{
		public virtual IList<IArtifact> ArtifactList { get; set; }

		public virtual bool DropAll { get; set; }

		public override void PlayerExecute()
		{
			Debug.Assert(DropAll || DobjArtifact != null);

			if (DobjArtifact != null && DobjArtifact.IsWornByCharacter())
			{
				PrintWearingRemoveFirst(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			ArtifactList = DropAll ? ActorMonster.GetCarriedList() : new List<IArtifact>() { DobjArtifact };

			if (ArtifactList.Count > 0)
			{
				var lsIndex = ArtifactList.FindIndex(a => a.Uid == Globals.GameState.Ls);

				if (lsIndex > 0)
				{
					var lsArtifact = ArtifactList[lsIndex];
					
					ArtifactList.RemoveAt(lsIndex);

					ArtifactList.Insert(0, lsArtifact);
				}

				foreach (var artifact in ArtifactList)
				{
					Globals.Engine.DropArtifact(ActorMonster, artifact, ActorRoom, (m, a) => PrintDropped(a));
				}

				Globals.Out.WriteLine();
			}
			else
			{
				Globals.Out.Print("There's nothing for you to drop.");

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

			if (string.Equals(CommandParser.ObjData.Name, "all", StringComparison.OrdinalIgnoreCase))
			{
				DropAll = true;
			}
			else
			{
				CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByCharacter(),
					a => a.IsWornByCharacter()
				};

				CommandParser.ObjData.ArtifactNotFoundFunc = PrintDontHaveIt;

				PlayerResolveArtifact();
			}
		}

		public DropCommand()
		{
			SortOrder = 130;

			Name = "DropCommand";

			Verb = "drop";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
