
// UseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Commands;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void PlayerProcessEvents(long eventType)
		{
			ICommand command;

			if (eventType == PpeBeforeArtifactUse)
			{
				switch (DobjArtifact.Uid)
				{
					case 48:
					case 50:

						// Display screen/Terminals

						command = Globals.CreateInstance<IReadCommand>();

						CopyCommandData(command);

						NextState = command;

						GotoCleanup = true;

						break;

					case 65:

						// Alphabet dial

						command = Globals.CreateInstance<Framework.Commands.ITurnCommand>();

						CopyCommandData(command);

						NextState = command;

						GotoCleanup = true;

						break;

					case 82:

						// Plastic card

						if (IobjArtifact != null && (IobjArtifact.Uid == 1 || IobjArtifact.Uid == 26))
						{
							command = Globals.CreateInstance<IPutCommand>();

							CopyCommandData(command);

							NextState = command;

							GotoCleanup = true;
						}
						else if (IobjArtifact == null && IobjMonster == null)
						{
							PrintBeMoreSpecific();

							GotoCleanup = true;
						}

						break;

					case 3:
					case 4:
					case 19:
					case 20:
					case 21:
					case 27:
					case 46:
					case 55:
					case 56:
					case 59:
					case 60:
					case 66:
					case 67:
					case 68:
					case 69:
					case 70:

						// Various buttons

						command = Globals.CreateInstance<Framework.Commands.IPushCommand>();

						CopyCommandData(command);

						NextState = command;

						GotoCleanup = true;

						break;

					default:

						base.PlayerProcessEvents(eventType);

						break;
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public UseCommand()
		{
			IsIobjEnabled = true;
		}
	}
}
