
// CloseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
		/// <summary>
		/// This event fires after the player closes an artifact.
		/// </summary>
		protected const long PpeAfterArtifactClose = 1;

		public override void PlayerExecute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			var containerAc = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Container);

			var doorGateAc = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.DoorGate);

			var drinkableAc = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Drinkable);

			var edibleAc = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Edible);

			var readableAc = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Readable);

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

				rc = DobjArtifact.SyncArtifactCategories(ac);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				PlayerProcessEvents(PpeAfterArtifactClose);

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

			Type = Enums.CommandType.Manipulation;
		}
	}
}
