
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Remove magical amulet in forest

			if (Globals.GameState.Ro >= 65 && Globals.GameState.Ro != 92 && Globals.GameState.Ro != 93 && DobjArtifact.Uid == 18)
			{
				Globals.Out.Print("If you remove {0}, you'll be paralysed with fear!", DobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf));

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
