
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : Command, IOpenCommand
	{
		/// <summary></summary>
		public const long PpeAfterArtifactOpenPrint = 1;

		/// <summary></summary>
		public const long PpeAfterArtifactOpen = 2;

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(gDobjArtifact != null);

			var inContainerAc = gDobjArtifact.InContainer;

			var doorGateAc = gDobjArtifact.DoorGate;

			var disguisedMonsterAc = gDobjArtifact.DisguisedMonster;

			var drinkableAc = gDobjArtifact.Drinkable;

			var edibleAc = gDobjArtifact.Edible;

			var readableAc = gDobjArtifact.Readable;

			var ac =	inContainerAc != null ? inContainerAc :
						doorGateAc != null ? doorGateAc :
						disguisedMonsterAc != null ? disguisedMonsterAc :
						drinkableAc != null ? drinkableAc :
						edibleAc != null ? edibleAc : 
						readableAc;

			if (ac != null)
			{
				if (gDobjArtifact.IsEmbeddedInRoom(gActorRoom))
				{
					gDobjArtifact.SetInRoom(gActorRoom);
				}

				if (ac.Type == ArtifactType.DoorGate)
				{
					ac.Field4 = 0;
				}

				if (ac.Type == ArtifactType.DisguisedMonster)
				{
					gEngine.RevealDisguisedMonster(gActorRoom, gDobjArtifact);

					goto Cleanup;
				}

				var keyUid = ac.GetKeyUid();

				var key = keyUid > 0 ? gADB[keyUid] : null;

				if (ac.IsOpen() || keyUid == -2)
				{
					PrintAlreadyOpen(gDobjArtifact);

					goto Cleanup;
				}

				if (ac.Type == ArtifactType.Drinkable || ac.Type == ArtifactType.Edible || ac.Type == ArtifactType.Readable)
				{
					ac.SetOpen(true);

					rc = gDobjArtifact.SyncArtifactCategories(ac);

					Debug.Assert(gEngine.IsSuccess(rc));

					PrintOpened(gDobjArtifact);

					goto Cleanup;
				}

				if (ac.Type == ArtifactType.InContainer && gDobjArtifact.OnContainer != null && gDobjArtifact.IsInContainerOpenedFromTop())
				{
					var contentsList = gDobjArtifact.GetContainedList(containerType: ContainerType.On);

					if (contentsList.Count > 0)
					{
						PrintContainerNotEmpty(gDobjArtifact, ContainerType.On, contentsList.Count > 1 || contentsList[0].IsPlural);

						goto Cleanup;
					}
				}

				if (keyUid == -1)
				{
					PrintWontOpen(gDobjArtifact);

					goto Cleanup;
				}

				if (key != null && !key.IsCarriedByCharacter() && !key.IsWornByCharacter() && !key.IsInRoom(gActorRoom))
				{
					PrintLocked(gDobjArtifact);

					goto Cleanup;
				}

				if (keyUid == 0 && ac.GetBreakageStrength() > 0)
				{
					PrintHaveToForceOpen(gDobjArtifact);

					goto Cleanup;
				}

				if (key != null)
				{
					PrintOpenObjWithKey(gDobjArtifact, key);
				}
				else
				{
					PrintOpened(gDobjArtifact);
				}

				PlayerProcessEvents(PpeAfterArtifactOpenPrint);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (ac.Type == ArtifactType.InContainer && gDobjArtifact.ShouldShowContentsWhenOpened())
				{
					NextState = Globals.CreateInstance<IInventoryCommand>();

					CopyCommandData(NextState as ICommand);
				}

				ac.SetKeyUid(0);

				ac.SetOpen(true);

				/*
				Note: duplicated above?

				if (Ac.Type != ArtifactType.InContainer)
				{
					Ac.Field4 = 0;
				}
				*/

				rc = gDobjArtifact.SyncArtifactCategories(ac);

				Debug.Assert(gEngine.IsSuccess(rc));

				PlayerProcessEvents(PpeAfterArtifactOpen);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{
				PrintCantVerbObj(gDobjArtifact);

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
			PlayerResolveArtifact();
		}

		public OpenCommand()
		{
			SortOrder = 180;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "OpenCommand";

			Verb = "open";

			Type = CommandType.Manipulation;
		}
	}
}
