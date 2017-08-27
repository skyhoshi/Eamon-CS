
// CloseCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : Command, ICloseCommand
	{
		protected virtual void PlayerProcessEvents()
		{

		}

		protected override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var containerAc = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Container);

			var doorGateAc = DobjArtifact.GetArtifactClass(Enums.ArtifactType.DoorGate);

			var drinkableAc = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Drinkable);

			var edibleAc = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Edible);

			var readableAc = DobjArtifact.GetArtifactClass(Enums.ArtifactType.Readable);

			var ac =	containerAc != null ? containerAc :
						doorGateAc != null ? doorGateAc :
						drinkableAc != null ? drinkableAc :
						edibleAc != null ? edibleAc :
						readableAc;

			if (ac != null)
			{
				if (ac.Type == Enums.ArtifactType.Drinkable || ac.Type == Enums.ArtifactType.Edible || ac.Type == Enums.ArtifactType.Readable || ac.GetKeyUid() == -1)
				{
					PrintDontNeedTo();

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (ac.Type == Enums.ArtifactType.DoorGate)
				{
					if (DobjArtifact.Seen)
					{
						ac.Field8 = 0;
					}

					if (ac.Field8 == 1)
					{
						PrintDontFollowYou();

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}
				}

				if (ac.GetKeyUid() == -2)
				{
					PrintBrokeIt(DobjArtifact);

					goto Cleanup;
				}

				if (!ac.IsOpen())
				{
					PrintNotOpen(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				PrintClosed(DobjArtifact);

				ac.SetOpen(false);

				rc = DobjArtifact.SyncArtifactClasses(ac);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				PlayerProcessEvents();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{
				PrintCantVerbIt(DobjArtifact);

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
			PlayerResolveArtifact();
		}

		public CloseCommand()
		{
			SortOrder = 110;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;

				IsMonsterEnabled = false;
			}

			Name = "CloseCommand";

			Verb = "close";

			Type = Enums.CommandType.Manipulation;
		}
	}
}
