﻿
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : Command, IOpenCommand
	{
		/// <summary></summary>
		public virtual IList<IArtifact> OnContainerArtifactList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory InContainerAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DoorGateAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DisguisedMonsterAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DrinkableAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory EdibleAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ReadableAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact KeyArtifact { get; set; }

		/// <summary></summary>
		public virtual long KeyArtifactUid { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			InContainerAc = DobjArtifact.InContainer;

			DoorGateAc = DobjArtifact.DoorGate;

			DisguisedMonsterAc = DobjArtifact.DisguisedMonster;

			DrinkableAc = DobjArtifact.Drinkable;

			EdibleAc = DobjArtifact.Edible;

			ReadableAc = DobjArtifact.Readable;

			DobjArtAc =	InContainerAc != null ? InContainerAc :
						DoorGateAc != null ? DoorGateAc :
						DisguisedMonsterAc != null ? DisguisedMonsterAc :
						DrinkableAc != null ? DrinkableAc :
						EdibleAc != null ? EdibleAc : 
						ReadableAc;

			if (DobjArtAc != null)
			{
				if (DobjArtifact.IsEmbeddedInRoom(ActorRoom))
				{
					DobjArtifact.SetInRoom(ActorRoom);
				}

				if (DobjArtAc.Type == ArtifactType.DoorGate)
				{
					DobjArtAc.Field4 = 0;
				}

				if (DobjArtAc.Type == ArtifactType.DisguisedMonster)
				{
					gEngine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

					goto Cleanup;
				}

				KeyArtifactUid = DobjArtAc.GetKeyUid();

				KeyArtifact = KeyArtifactUid > 0 ? gADB[KeyArtifactUid] : null;

				if (DobjArtAc.IsOpen() || KeyArtifactUid == -2)
				{
					PrintAlreadyOpen(DobjArtifact);

					goto Cleanup;
				}

				if (DobjArtAc.Type == ArtifactType.Drinkable || DobjArtAc.Type == ArtifactType.Edible || DobjArtAc.Type == ArtifactType.Readable)
				{
					DobjArtAc.SetOpen(true);

					rc = DobjArtifact.SyncArtifactCategories(DobjArtAc);

					Debug.Assert(gEngine.IsSuccess(rc));

					PrintOpened(DobjArtifact);

					goto Cleanup;
				}

				if (DobjArtAc.Type == ArtifactType.InContainer && DobjArtifact.OnContainer != null && DobjArtifact.IsInContainerOpenedFromTop())
				{
					OnContainerArtifactList = DobjArtifact.GetContainedList(containerType: ContainerType.On);

					if (OnContainerArtifactList.Count > 0)
					{
						PrintContainerNotEmpty(DobjArtifact, ContainerType.On, OnContainerArtifactList.Count > 1 || OnContainerArtifactList[0].IsPlural);

						goto Cleanup;
					}
				}

				if (KeyArtifactUid == -1)
				{
					PrintWontOpen(DobjArtifact);

					goto Cleanup;
				}

				if (KeyArtifact != null && !KeyArtifact.IsCarriedByCharacter() && !KeyArtifact.IsWornByCharacter() && !KeyArtifact.IsInRoom(ActorRoom))
				{
					PrintLocked(DobjArtifact);

					goto Cleanup;
				}

				if (KeyArtifactUid == 0 && DobjArtAc.GetBreakageStrength() > 0)
				{
					PrintHaveToForceOpen(DobjArtifact);

					goto Cleanup;
				}

				if (KeyArtifact != null)
				{
					PrintOpenObjWithKey(DobjArtifact, KeyArtifact);
				}
				else
				{
					PrintOpened(DobjArtifact);
				}

				PlayerProcessEvents(EventType.AfterArtifactOpenPrint);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (DobjArtAc.Type == ArtifactType.InContainer && DobjArtifact.ShouldShowContentsWhenOpened())
				{
					NextState = Globals.CreateInstance<IInventoryCommand>();

					CopyCommandData(NextState as ICommand);
				}

				DobjArtAc.SetKeyUid(0);

				DobjArtAc.SetOpen(true);

				/*
				Note: duplicated above?

				if (DobjArtAc.Type != ArtifactType.InContainer)
				{
					DobjArtAc.Field4 = 0;
				}
				*/

				rc = DobjArtifact.SyncArtifactCategories(DobjArtAc);

				Debug.Assert(gEngine.IsSuccess(rc));

				PlayerProcessEvents(EventType.AfterArtifactOpen);

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
