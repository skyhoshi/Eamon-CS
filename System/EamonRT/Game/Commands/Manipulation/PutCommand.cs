
// PutCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class PutCommand : Command, IPutCommand
	{
		public virtual bool GetCommandCalled { get; set; }

		protected override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null && IobjArtifact != null);

			if (!DobjArtifact.IsCarriedByCharacter() && !GetCommandCalled)
			{
				PrintTakingFirst(DobjArtifact);

				NextState = Globals.CreateInstance<IGetCommand>(x =>
				{
					x.PreserveNextState = true;
				});

				CopyCommandData(NextState as ICommand, false);

				NextState.NextState = Globals.CreateInstance<IPutCommand>(x =>
				{
					x.GetCommandCalled = true;
				});

				CopyCommandData(NextState.NextState as ICommand);

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByCharacter())
			{
				if (!DobjArtifact.IsDisguisedMonster() || !GetCommandCalled)
				{
					NextState = Globals.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			var ac = IobjArtifact.GetArtifactClass(Enums.ArtifactType.Container);

			if (DobjArtifact == IobjArtifact || ac == null)
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!ac.IsOpen())
			{
				PrintMustFirstOpen(IobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (ac.GetKeyUid() == -2)
			{
				PrintBrokeIt(IobjArtifact);

				goto Cleanup;
			}

			var count = 0L;

			var weight = 0L;

			rc = IobjArtifact.GetContainerInfo(ref count, ref weight, false);

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			if (ac.Field7 < 1 || ac.Field8 < 1)
			{
				PrintDontNeedTo();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			var maxItemsReached = count >= ac.Field8;

			if (!maxItemsReached && weight + DobjArtifact.Weight > ac.Field7)
			{
				PrintWontFit(DobjArtifact);

				goto Cleanup;
			}

			if (maxItemsReached || weight + DobjArtifact.Weight > ac.Field7)
			{
				PrintFull(IobjArtifact);

				goto Cleanup;
			}

			if (!IobjArtifact.IsCarriedByCharacter())
			{
				count = 0;

				weight = DobjArtifact.Weight;

				rc = DobjArtifact.GetContainerInfo(ref count, ref weight, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.GameState.Wt -= weight;
			}

			DobjArtifact.SetCarriedByContainer(IobjArtifact);

			if (Globals.GameState.Ls == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.IsLightSource());

				Globals.RtEngine.LightOut(DobjArtifact);
			}

			if (ActorMonster.Weapon == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.IsWeapon01());

				rc = DobjArtifact.RemoveStateDesc(Globals.Engine.ReadyWeaponDesc);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				ActorMonster.Weapon = -1;
			}

			Globals.Out.WriteLine("{0}Done.", Environment.NewLine);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();

			if (DobjArtifact != null)
			{
				CommandParser.ObjData = CommandParser.IobjData;

				CommandParser.ObjData.QueryDesc = string.Format("{0}Put {1} in what? ", Environment.NewLine, DobjArtifact.EvalPlural("it", "them"));

				PlayerResolveArtifact();
			}
		}

		public PutCommand()
		{
			SortOrder = 190;

			if (Globals.IsClassicVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "PutCommand";

			Verb = "put";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
