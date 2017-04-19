
// UseCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework.Commands;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IUseCommand))]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		protected override void PlayerProcessEvents()
		{
			Eamon.Framework.Commands.ICommand command;

			switch (DobjArtifact.Uid)
			{
				case 48:
				case 50:

					// Display screen/Terminals

					command = Globals.CreateInstance<EamonRT.Framework.Commands.IReadCommand>();

					CopyCommandData(command);

					NextState = command;

					GotoCleanup = true;

					break;

				case 65:

					// Alphabet dial

					command = Globals.CreateInstance<ITurnCommand>();

					CopyCommandData(command);

					NextState = command;

					GotoCleanup = true;

					break;

				case 82:

					// Plastic card

					if (IobjArtifact != null && (IobjArtifact.Uid == 1 || IobjArtifact.Uid == 26))
					{
						command = Globals.CreateInstance<EamonRT.Framework.Commands.IPutCommand>();

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

					command = Globals.CreateInstance<IPushCommand>();

					CopyCommandData(command);

					NextState = command;

					GotoCleanup = true;

					break;

				default:

					base.PlayerProcessEvents();

					break;
			}
		}

		public UseCommand()
		{
			IobjSupport = true;
		}
	}
}
