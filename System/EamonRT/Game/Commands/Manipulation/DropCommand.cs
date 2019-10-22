
// DropCommand.cs

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
	public class DropCommand : Command, IDropCommand
	{
		public virtual bool DropAll { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> ArtifactList { get; set; }

		/// <summary></summary>
		public virtual IList<string> ContainerContentsList { get; set; }

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

			Globals.LastArtifactLocation = artifact.Location;

			artifact.SetInRoom(ActorRoom);

			Globals.Engine.RevealExtendedContainerContents(ActorRoom, artifact, ContainerContentsList);

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

			ContainerContentsList = new List<string>();

			if (DobjArtifact != null)
			{
				if (DobjArtifact.IsWornByCharacter())
				{
					PrintWearingRemoveFirst(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}
				else if (DobjArtifact.IsCarriedByCharacter(true) && DobjArtifact.IsCarriedByContainer())
				{
					PrintRemovingFirst(DobjArtifact);

					NextState = Globals.CreateInstance<IGetCommand>();

					CopyCommandData(NextState as ICommand);

					NextState.NextState = Globals.CreateInstance<IDropCommand>();

					CopyCommandData(NextState.NextState as ICommand);

					goto Cleanup;
				}
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

				foreach (var containerContentsDesc in ContainerContentsList)
				{
					Globals.Out.Write("{0}", containerContentsDesc);
				}
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
				monster.Armor -= (artifact.Wearable.Field1 > 1 ? (artifact.Wearable.Field1 / 2) + ((artifact.Wearable.Field1 / 2) >= 3 ? 2 : 0) : artifact.Wearable.Field1);
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
					a => a.IsWornByCharacter(),
					a => a.IsCarriedByContainerContainerTypeExposedToCharacter(Globals.Engine.ExposeContainersRecursively)
				};

				CommandParser.ObjData.ArtifactNotFoundFunc = PrintDontHaveIt;

				PlayerResolveArtifact();
			}
		}

		public override bool ShouldShowUnseenArtifacts()
		{
			return false;
		}

		public DropCommand()
		{
			SortOrder = 130;

			Name = "DropCommand";

			Verb = "drop";

			Type = CommandType.Manipulation;
		}
	}
}
