
// SearchCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class SearchCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISearchCommand
	{
		public override void PrintCantVerbObj(IGameBase obj)
		{
			Debug.Assert(obj != null);

			gOut.Print("You can only {0} dead bodies.", Verb);
		}

		public virtual void PrintNothingFound(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("Searching {0} reveals nothing of interest.", artifact.GetTheName());
		}

		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null);

			switch (gDobjArtifact.Uid)
			{
				case 89:

					// Dismantled worker android

					var plasticCardArtifact = gADB[82];

					Debug.Assert(plasticCardArtifact != null);

					if (plasticCardArtifact.IsInLimbo())
					{
						gOut.Print("{0}", plasticCardArtifact.Desc);

						plasticCardArtifact.SetInRoom(gActorRoom);

						plasticCardArtifact.Seen = true;

						var command = Globals.CreateInstance<IGetCommand>();

						CopyCommandData(command);

						command.Dobj = plasticCardArtifact;

						NextState = command;
					}
					else
					{
						PrintNothingFound(gDobjArtifact);
					}

					goto Cleanup;

				default:

					if (gDobjArtifact.DeadBody != null && gDobjArtifact.Uid != 107)
					{
						PrintNothingFound(gDobjArtifact);
					}
					else
					{ 
						PrintCantVerbObj(gDobjArtifact);

						NextState = Globals.CreateInstance<IStartState>();
					}

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

		public SearchCommand()
		{
			SortOrder = 460;

			IsNew = true;

			IsMonsterEnabled = false;

			Name = "SearchCommand";

			Verb = "search";

			Type = CommandType.Interactive;
		}
	}
}
