
// SearchCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class SearchCommand : EamonRT.Game.Commands.Command, ISearchCommand
	{
		protected override void PrintCantVerbObj(IGameBase obj)
		{
			Debug.Assert(obj != null);

			Globals.Out.Write("{0}You can only {1} dead bodies.{0}", Environment.NewLine, Verb);
		}

		protected virtual void PrintNothingFound(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}Searching {1} reveals nothing of interest.{0}", Environment.NewLine, artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			switch (DobjArtifact.Uid)
			{
				case 89:

					// Dismantled worker android

					var artifact = Globals.ADB[82];

					Debug.Assert(artifact != null);

					if (artifact.IsInLimbo())
					{
						Globals.Out.Write("{0}{1}{0}", Environment.NewLine, artifact.Desc);

						artifact.SetInRoom(ActorRoom);

						artifact.Seen = true;

						var command = Globals.CreateInstance<EamonRT.Framework.Commands.IGetCommand>();

						CopyCommandData(command);

						command.DobjArtifact = artifact;

						NextState = command;
					}
					else
					{
						PrintNothingFound(DobjArtifact);
					}

					goto Cleanup;

				default:

					if (DobjArtifact.IsDeadBody() && DobjArtifact.Uid != 107)
					{
						PrintNothingFound(DobjArtifact);
					}
					else
					{ 
						PrintCantVerbObj(DobjArtifact);

						NextState = Globals.CreateInstance<EamonRT.Framework.States.IStartState>();
					}

					goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
		}

		protected override void PlayerFinishParsing()
		{
			PlayerResolveArtifact();
		}

		public SearchCommand()
		{
			SortOrder = 460;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "SearchCommand";

			Verb = "search";

			Type = Enums.CommandType.Interactive;
		}
	}
}
