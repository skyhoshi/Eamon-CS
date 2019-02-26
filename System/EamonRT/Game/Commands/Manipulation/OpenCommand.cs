
// OpenCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
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
		public const long PpeAfterArtifactOpenPrint = 1;

		public const long PpeAfterArtifactOpen = 2;

		public virtual bool ShouldPrintContainerInventory()
		{
			return true;
		}

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var containerAc = DobjArtifact.Container;

			var doorGateAc = DobjArtifact.DoorGate;

			var disguisedMonsterAc = DobjArtifact.DisguisedMonster;

			var drinkableAc = DobjArtifact.Drinkable;

			var edibleAc = DobjArtifact.Edible;

			var readableAc = DobjArtifact.Readable;

			var ac =	containerAc != null ? containerAc :
						doorGateAc != null ? doorGateAc :
						disguisedMonsterAc != null ? disguisedMonsterAc :
						drinkableAc != null ? drinkableAc :
						edibleAc != null ? edibleAc : 
						readableAc;

			if (ac != null)
			{
				if (DobjArtifact.IsEmbeddedInRoom(ActorRoom))
				{
					DobjArtifact.SetInRoom(ActorRoom);
				}

				if (ac.Type == ArtifactType.DoorGate)
				{
					ac.Field4 = 0;
				}

				if (ac.Type == ArtifactType.DisguisedMonster)
				{
					Globals.Engine.RevealDisguisedMonster(DobjArtifact);

					goto Cleanup;
				}

				var keyUid = ac.GetKeyUid();

				var key = keyUid > 0 ? Globals.ADB[keyUid] : null;

				if (ac.IsOpen() || keyUid == -2)
				{
					PrintAlreadyOpen(DobjArtifact);

					goto Cleanup;
				}

				if (ac.Type == ArtifactType.Drinkable || ac.Type == ArtifactType.Edible || ac.Type == ArtifactType.Readable)
				{
					ac.SetOpen(true);

					rc = DobjArtifact.SyncArtifactCategories(ac);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					PrintOpened(DobjArtifact);

					goto Cleanup;
				}

				if (keyUid == -1)
				{
					PrintWontOpen(DobjArtifact);

					goto Cleanup;
				}

				if (key != null && !key.IsCarriedByCharacter() && !key.IsWornByCharacter() && !key.IsInRoom(ActorRoom))
				{
					PrintLocked(DobjArtifact);

					goto Cleanup;
				}

				if (keyUid == 0 && ac.GetBreakageStrength() > 0)
				{
					PrintHaveToForceOpen(DobjArtifact);

					goto Cleanup;
				}

				if (key != null)
				{
					PrintOpenObjWithKey(DobjArtifact, key);
				}
				else
				{
					PrintOpened(DobjArtifact);
				}

				PlayerProcessEvents(PpeAfterArtifactOpenPrint);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (ac.Type == ArtifactType.Container && ShouldPrintContainerInventory())
				{
					NextState = Globals.CreateInstance<IInventoryCommand>();

					CopyCommandData(NextState as ICommand);
				}

				ac.SetKeyUid(0);

				ac.SetOpen(true);

				/*
				Note: duplicated above?

				if (Ac.Type != ArtifactType.Container)
				{
					Ac.Field4 = 0;
				}
				*/

				rc = DobjArtifact.SyncArtifactCategories(ac);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				PlayerProcessEvents(PpeAfterArtifactOpen);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{
				PrintCantVerbObj(DobjArtifact);

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
