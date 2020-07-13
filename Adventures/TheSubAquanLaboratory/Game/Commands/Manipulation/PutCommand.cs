
// PutCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class PutCommand : EamonRT.Game.Commands.PutCommand, IPutCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null && IobjArtifact != null);

			if (DobjArtifact.Uid == 82)
			{
				if (IobjArtifact.Uid == 1)
				{
					gEngine.PrintEffectDesc(38);

					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
				else if (IobjArtifact.Uid == 26)
				{
					var ovalDoorArtifact = gADB[16];

					Debug.Assert(ovalDoorArtifact != null);

					if (gGameState.Sterilize && !ovalDoorArtifact.IsInLimbo())
					{
						gEngine.PrintEffectDesc(40);

						ovalDoorArtifact.SetInLimbo();
					}
					else
					{
						gEngine.PrintEffectDesc(39);
					}

					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
				else
				{
					base.PlayerExecute();
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
