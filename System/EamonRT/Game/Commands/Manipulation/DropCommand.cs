
// DropCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
		public virtual IList<IArtifact> ArtifactList { get; set; }

		public virtual bool DropAll { get; set; }

		public virtual void ProcessLightSource()
		{
			if (Globals.GameState.Ls > 0)
			{
				var lsArtifact = Globals.ADB[Globals.GameState.Ls];

				Debug.Assert(lsArtifact != null && lsArtifact.LightSource != null);

				if ((DropAll || lsArtifact == DobjArtifact) && lsArtifact.IsCarriedByCharacter())
				{
					Globals.Engine.LightOut(lsArtifact);
				}
			}
		}

		public virtual void ProcessArtifact(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Engine.RemoveWeight(artifact);

			artifact.SetInRoom(ActorRoom);

			if (ActorMonster.Weapon == artifact.Uid)
			{
				Debug.Assert(artifact.GeneralWeapon != null);

				var rc = artifact.RemoveStateDesc(artifact.GetReadyWeaponDesc());

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ActorMonster.Weapon = -1;
			}

			PrintDropped(artifact);
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DropAll || DobjArtifact != null);

			if (DobjArtifact != null && DobjArtifact.IsWornByCharacter())
			{
				PrintWearingRemoveFirst(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			ProcessLightSource();

			ArtifactList = DropAll ? ActorMonster.GetCarriedList() : new List<IArtifact>() { DobjArtifact };

			if (ArtifactList.Count > 0)
			{
				foreach (var artifact in ArtifactList)
				{
					ProcessArtifact(artifact);
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

		/*
		public override void MonsterExecute()
		{
			// Note: just the starting point for a real implementation

			Debug.Assert(artifact.IsCarriedByMonster(monster) || artifact.IsWornByMonster(monster));

			if (artifact.IsWornByMonster(monster) && artifact.Wearable != null && artifact.Wearable.Field1 > 0)
			{
				monster.Armor -= (artifact.Wearable.Field1 > 1 ? artifact.Wearable.Field1 / 2 : artifact.Wearable.Field1);
			}

			Debug.Assert(IsValidMonsterArmor(monster.Armor));

			artifact.SetInRoom(room);

			if (monster.Weapon == artifact.Uid)
			{
				Debug.Assert(artifact.GeneralWeapon != null);

				var rc = artifact.RemoveStateDesc(artifact.GetReadyWeaponDesc());

				Debug.Assert(IsSuccess(rc));

				monster.Weapon = -1;
			}
		}
		*/

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
