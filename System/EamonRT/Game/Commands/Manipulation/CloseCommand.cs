
// CloseCommand.cs

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
	public class CloseCommand : Command, ICloseCommand
	{
		/// <summary></summary>
		public const long PpeAfterArtifactClose = 1;

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(gDobjArtifact != null);

			var inContainerAc = gDobjArtifact.InContainer;

			var doorGateAc = gDobjArtifact.DoorGate;

			var drinkableAc = gDobjArtifact.Drinkable;

			var edibleAc = gDobjArtifact.Edible;

			var readableAc = gDobjArtifact.Readable;

			var ac =	inContainerAc != null ? inContainerAc :
						doorGateAc != null ? doorGateAc :
						drinkableAc != null ? drinkableAc :
						edibleAc != null ? edibleAc :
						readableAc;

			if (ac != null)
			{
				if (ac.Type == ArtifactType.Drinkable || ac.Type == ArtifactType.Edible || ac.Type == ArtifactType.Readable || ac.GetKeyUid() == -1)
				{
					PrintDontNeedTo();

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (ac.Type == ArtifactType.DoorGate)
				{
					if (gDobjArtifact.Seen)
					{
						ac.Field4 = 0;
					}

					if (ac.Field4 == 1)
					{
						PrintDontFollowYou();

						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}
				}

				if (ac.GetKeyUid() == -2)
				{
					PrintBrokeIt(gDobjArtifact);

					goto Cleanup;
				}

				if (!ac.IsOpen())
				{
					PrintNotOpen(gDobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				PrintClosed(gDobjArtifact);

				ac.SetOpen(false);

				rc = gDobjArtifact.SyncArtifactCategories(ac);

				Debug.Assert(gEngine.IsSuccess(rc));

				PlayerProcessEvents(PpeAfterArtifactClose);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}
			else
			{
				PrintCantVerbIt(gDobjArtifact);

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

			Type = CommandType.Manipulation;
		}
	}
}
