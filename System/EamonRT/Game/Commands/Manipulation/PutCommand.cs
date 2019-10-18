
// PutCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
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
	public class PutCommand : Command, IPutCommand
	{
		/// <summary></summary>
		public const long PpeAfterArtifactPut = 1;

		/// <summary></summary>
		public virtual bool GetCommandCalled { get; set; }

		/// <summary></summary>
		public virtual ContainerType ContainerType { get; set; }

		/// <summary></summary>
		public virtual IList<string> ContainerContentsList { get; set; }

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null && IobjArtifact != null);

			ContainerContentsList = new List<string>();

			if (!DobjArtifact.IsCarriedByCharacter() && !GetCommandCalled)
			{
				if (DobjArtifact.IsCarriedByContainer())
				{
					PrintRemovingFirst(DobjArtifact);
				}
				else
				{
					PrintTakingFirst(DobjArtifact);
				}

				NextState = Globals.CreateInstance<IGetCommand>(x =>
				{
					x.PreserveNextState = true;

					x.OmitWeightCheck = DobjArtifact.IsCarriedByCharacter(true);
				});

				CopyCommandData(NextState as ICommand, false);

				NextState.NextState = Globals.CreateInstance<IPutCommand>(x =>
				{
					x.GetCommandCalled = true;

					x.ContainerType = ContainerType;
				});

				CopyCommandData(NextState.NextState as ICommand);

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByCharacter())
			{
				if (DobjArtifact.DisguisedMonster == null || !GetCommandCalled)
				{
					NextState = Globals.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			var ac = Globals.Engine.EvalContainerType(ContainerType, IobjArtifact.InContainer, IobjArtifact.OnContainer, IobjArtifact.UnderContainer, IobjArtifact.BehindContainer);

			var containedList = DobjArtifact.GetContainedList(containerType: (ContainerType)(-1), recurse: true);

			containedList.Add(DobjArtifact);

			if (containedList.Contains(IobjArtifact) || ac == null)
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if ((IobjArtifact.IsCarriedByCharacter() && !IobjArtifact.ShouldAddContentsWhenCarried(ContainerType)) || (IobjArtifact.IsWornByCharacter() && !IobjArtifact.ShouldAddContentsWhenWorn(ContainerType)))
			{
				if (IobjArtifact.IsCarriedByCharacter())
				{
					PrintNotWhileCarryingObj(IobjArtifact);
				}
				else
				{
					PrintNotWhileWearingObj(IobjArtifact);
				}

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (ac == IobjArtifact.InContainer && !ac.IsOpen())
			{
				PrintMustFirstOpen(IobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if ((ac == IobjArtifact.InContainer && ac.GetKeyUid() == -2) || (ac == IobjArtifact.OnContainer && IobjArtifact.InContainer != null && IobjArtifact.InContainer.GetKeyUid() == -2 && IobjArtifact.IsInContainerOpenedFromTop()))
			{
				PrintBrokeIt(IobjArtifact);

				goto Cleanup;
			}

			if (ac == IobjArtifact.OnContainer && IobjArtifact.InContainer != null && IobjArtifact.InContainer.IsOpen() && IobjArtifact.IsInContainerOpenedFromTop())
			{
				PrintMustFirstClose(IobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			var count = 0L;

			var weight = 0L;

			rc = IobjArtifact.GetContainerInfo(ref count, ref weight, ContainerType, false);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (ac.Field3 < 1 || ac.Field4 < 1)
			{
				PrintDontNeedTo();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			var maxItemsReached = count >= ac.Field4;

			if ((!maxItemsReached && weight + DobjArtifact.Weight > ac.Field3) || !IobjArtifact.ShouldAddContents(DobjArtifact, ContainerType))
			{
				PrintWontFit(DobjArtifact);

				goto Cleanup;
			}

			if (maxItemsReached || weight + DobjArtifact.Weight > ac.Field3)
			{
				if (ac == IobjArtifact.InContainer)
				{
					PrintFull(IobjArtifact);
				}
				else
				{
					PrintOutOfSpace(IobjArtifact);
				}

				goto Cleanup;
			}

			Globals.LastArtifactLocation = DobjArtifact.Location;

			DobjArtifact.SetCarriedByContainer(IobjArtifact, ContainerType);

			Globals.Engine.RevealExtendedContainerContents(ActorRoom, DobjArtifact, ContainerContentsList);

			if (Globals.GameState.Ls == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.LightSource != null);

				Globals.Engine.LightOut(DobjArtifact);
			}

			if (ActorMonster.Weapon == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.GeneralWeapon != null);

				rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ActorMonster.Weapon = -1;
			}

			Globals.Out.Print("Done.");

			foreach (var containerContentsDesc in ContainerContentsList)
			{
				Globals.Out.Write("{0}", containerContentsDesc);
			}

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

			if (DobjArtifact != null)
			{
				CommandParser.ObjData = CommandParser.IobjData;

				CommandParser.ObjData.QueryDesc = string.Format("{0}Put {1} {2} what? ", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"), Enum.IsDefined(typeof(ContainerType), ContainerType) ? Globals.Engine.EvalContainerType(ContainerType, "inside", "on", "under", "behind") : "in");

				PlayerResolveArtifact();

				if (IobjArtifact != null)
				{
					if (!Enum.IsDefined(typeof(ContainerType), ContainerType))
					{
						var artTypes = new ArtifactType[] { ArtifactType.InContainer, ArtifactType.OnContainer };

						var defaultAc = IobjArtifact.GetArtifactCategory(artTypes);

						ContainerType = defaultAc != null ? Globals.Engine.GetContainerType(defaultAc.Type) : ContainerType.In;
					}
				}
			}
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

			ContainerType = (ContainerType)(-1);
		}
	}
}
