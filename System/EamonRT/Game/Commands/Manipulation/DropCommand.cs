
// DropCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class DropCommand : Command, IDropCommand
	{
		public virtual bool DropAll { get; set; }

		protected override void PlayerExecute()
		{
			Debug.Assert(DropAll || DobjArtifact != null);

			if (DobjArtifact != null && DobjArtifact.IsWornByCharacter())
			{
				PrintWearingRemoveFirst(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (Globals.GameState.Ls > 0)
			{
				var lsArtifact = Globals.ADB[Globals.GameState.Ls];

				Debug.Assert(lsArtifact != null && lsArtifact.IsLightSource());

				if ((DropAll || lsArtifact == DobjArtifact) && lsArtifact.IsCarriedByCharacter())
				{
					Globals.Engine.LightOut(lsArtifact);
				}
			}

			var artifactList = DropAll ? ActorMonster.GetCarriedList() : new List<IArtifact>() { DobjArtifact };

			if (artifactList.Count > 0)
			{
				foreach (var artifact in artifactList)
				{
					Globals.Engine.RemoveWeight(artifact);

					artifact.SetInRoom(ActorRoom);

					if (ActorMonster.Weapon == artifact.Uid)
					{
						Debug.Assert(artifact.IsWeapon01());

						var rc = artifact.RemoveStateDesc(Globals.Engine.ReadyWeaponDesc);

						Debug.Assert(Globals.Engine.IsSuccess(rc));

						ActorMonster.Weapon = -1;
					}

					PrintDropped(artifact);
				}

				Globals.Out.WriteLine();
			}
			else
			{
				Globals.Out.WriteLine("{0}There's nothing for you to drop.", Environment.NewLine);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
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
