
// DropCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

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

		public virtual void ProcessLightSource()
		{
			if (gGameState.Ls > 0)
			{
				var lsArtifact = gADB[gGameState.Ls];

				Debug.Assert(lsArtifact != null && lsArtifact.LightSource != null);

				if ((DropAll || lsArtifact == gDobjArtifact) && lsArtifact.IsCarriedByCharacter())
				{
					gEngine.LightOut(lsArtifact);
				}
			}
		}

		public virtual void ProcessArtifact(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			artifact.SetInRoom(gActorRoom);

			if (gActorMonster.Weapon == artifact.Uid)
			{
				Debug.Assert(artifact.GeneralWeapon != null);

				var rc = artifact.RemoveStateDesc(artifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				gActorMonster.Weapon = -1;
			}

			PrintDropped(artifact);
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DropAll || gDobjArtifact != null);

			if (gDobjArtifact != null)
			{
				if (gDobjArtifact.IsWornByCharacter())
				{
					PrintWearingRemoveFirst(gDobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (!gDobjArtifact.IsCarriedByCharacter())
				{
					if (!GetCommandCalled)
					{
						if (gDobjArtifact.IsCarriedByCharacter(true) && gDobjArtifact.IsCarriedByContainer())
						{
							RedirectToGetCommand<IDropCommand>(gDobjArtifact);
						}
						else
						{
							NextState = Globals.CreateInstance<IStartState>();
						}
					}
					else if (gDobjArtifact.DisguisedMonster == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}
			}

			ProcessLightSource();

			ArtifactList = DropAll ? gActorMonster.GetCarriedList() : new List<IArtifact>() { gDobjArtifact };

			if (ArtifactList.Count > 0)
			{
				foreach (var artifact in ArtifactList)
				{
					ProcessArtifact(artifact);
				}

				gOut.WriteLine();
			}
			else
			{
				gOut.Print("There's nothing for you to drop.");

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
			gCommandParser.ParseName();

			if (string.Equals(gCommandParser.ObjData.Name, "all", StringComparison.OrdinalIgnoreCase))
			{
				DropAll = true;
			}
			else
			{
				gCommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
				{
					a => a.IsCarriedByCharacter(),
					a => a.IsWornByCharacter(),
					a => a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively)
				};

				gCommandParser.ObjData.ArtifactNotFoundFunc = PrintDontHaveIt;

				PlayerResolveArtifact();
			}
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			return artifact.IsWornByCharacter();
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
