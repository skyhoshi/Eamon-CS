
// PutCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class PutCommand : Command, IPutCommand
	{
		/// <summary>
		/// An event that fires after the player puts an <see cref="IArtifact">Artifact</see> into a container.
		/// </summary>
		public const long PpeAfterArtifactPut = 1;

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(gDobjArtifact != null && gIobjArtifact != null);

			if (!gDobjArtifact.IsCarriedByCharacter())
			{
				if (!GetCommandCalled)
				{
					RedirectToGetCommand<IPutCommand>(gDobjArtifact);
				}
				else if (gDobjArtifact.DisguisedMonster == null)
				{
					NextState = Globals.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			var ac = gEngine.EvalContainerType(ContainerType, gIobjArtifact.InContainer, gIobjArtifact.OnContainer, gIobjArtifact.UnderContainer, gIobjArtifact.BehindContainer);

			var containedList = gDobjArtifact.GetContainedList(containerType: (ContainerType)(-1), recurse: true);

			containedList.Add(gDobjArtifact);

			if (containedList.Contains(gIobjArtifact) || ac == null)
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if ((gIobjArtifact.IsCarriedByCharacter() && !gIobjArtifact.ShouldAddContentsWhenCarried(ContainerType)) || (gIobjArtifact.IsWornByCharacter() && !gIobjArtifact.ShouldAddContentsWhenWorn(ContainerType)))
			{
				if (gIobjArtifact.IsCarriedByCharacter())
				{
					PrintNotWhileCarryingObj(gIobjArtifact);
				}
				else
				{
					PrintNotWhileWearingObj(gIobjArtifact);
				}

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (ac == gIobjArtifact.InContainer && !ac.IsOpen())
			{
				PrintMustFirstOpen(gIobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if ((ac == gIobjArtifact.InContainer && ac.GetKeyUid() == -2) || (ac == gIobjArtifact.OnContainer && gIobjArtifact.InContainer != null && gIobjArtifact.InContainer.GetKeyUid() == -2 && gIobjArtifact.IsInContainerOpenedFromTop()))
			{
				PrintBrokeIt(gIobjArtifact);

				goto Cleanup;
			}

			if (ac == gIobjArtifact.OnContainer && gIobjArtifact.InContainer != null && gIobjArtifact.InContainer.IsOpen() && gIobjArtifact.IsInContainerOpenedFromTop())
			{
				PrintMustFirstClose(gIobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			var count = 0L;

			var weight = 0L;

			rc = gIobjArtifact.GetContainerInfo(ref count, ref weight, ContainerType, false);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (ac.Field3 < 1 || ac.Field4 < 1)
			{
				PrintDontNeedTo();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			var maxItemsReached = count >= ac.Field4;

			if ((!maxItemsReached && weight + gDobjArtifact.Weight > ac.Field3) || !gIobjArtifact.ShouldAddContents(gDobjArtifact, ContainerType))
			{
				PrintWontFit(gDobjArtifact);

				goto Cleanup;
			}

			if (maxItemsReached || weight + gDobjArtifact.Weight > ac.Field3)
			{
				if (ac == gIobjArtifact.InContainer)
				{
					PrintFull(gIobjArtifact);
				}
				else
				{
					PrintOutOfSpace(gIobjArtifact);
				}

				goto Cleanup;
			}

			gDobjArtifact.SetCarriedByContainer(gIobjArtifact, ContainerType);

			if (gGameState.Ls == gDobjArtifact.Uid)
			{
				Debug.Assert(gDobjArtifact.LightSource != null);

				gEngine.LightOut(gDobjArtifact);
			}

			if (gActorMonster.Weapon == gDobjArtifact.Uid)
			{
				Debug.Assert(gDobjArtifact.GeneralWeapon != null);

				rc = gDobjArtifact.RemoveStateDesc(gDobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				gActorMonster.Weapon = -1;
			}

			gOut.Print("Done.");

			PlayerProcessEvents(PpeAfterArtifactPut);

			if (GotoCleanup)
			{
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

			ContainerType = Prep != null ? Prep.ContainerType : (ContainerType)(-1);

			if (gDobjArtifact != null)
			{
				gCommandParser.ObjData = gCommandParser.IobjData;

				gCommandParser.ObjData.QueryDescFunc = () => string.Format("{0}Put {1} {2} what? ", Environment.NewLine, gDobjArtifact.EvalPlural("it", "them"), Enum.IsDefined(typeof(ContainerType), ContainerType) ? gEngine.EvalContainerType(ContainerType, "inside", "on", "under", "behind") : "in");

				PlayerResolveArtifact();

				if (gIobjArtifact != null)
				{
					if (!Enum.IsDefined(typeof(ContainerType), ContainerType))
					{
						var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer };

						var defaultAc = gIobjArtifact.GetArtifactCategory(artTypes);

						ContainerType = defaultAc != null ? gEngine.GetContainerType(defaultAc.Type) : ContainerType.In;
					}
				}
			}
		}

		public override bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			return artifact.IsCarriedByCharacter();
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			var prepNames = new string[] { "in", "into", "on", "onto", "under", "behind" };

			return prepNames.FirstOrDefault(pn => string.Equals(prep.Name, pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public PutCommand()
		{
			SortOrder = 190;

			IsIobjEnabled = true;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "PutCommand";

			Verb = "put";

			Type = CommandType.Manipulation;
		}
	}
}
