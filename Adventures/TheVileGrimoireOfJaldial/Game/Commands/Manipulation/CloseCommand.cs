﻿
// CloseCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		public override void PrintDontNeedTo()
		{
			Debug.Assert(DobjArtifact != null);

			if (DobjArtifact.Uid == 3 || DobjArtifact.Uid == 4 || DobjArtifact.Uid == 5 || DobjArtifact.Uid == 13 || DobjArtifact.Uid == 35)
			{
				PrintNotOpen(DobjArtifact);
			}
			else
			{
				base.PrintDontNeedTo();
			}
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null);

			// Crypts can't be closed unless they can also be opened

			if ((DobjArtifact.Uid == 3 || DobjArtifact.Uid == 4 || DobjArtifact.Uid == 5) && DobjArtifact.DoorGate.IsOpen())
			{
				var keyList = gADB.Records.Cast<Framework.IArtifact>().Where(a => (a.IsInRoom(ActorRoom) || a.IsCarriedByCharacter()) && a.GetLeverageBonus() > 0).ToList();

				var key = keyList.Count > 0 ? keyList[0] : null;

				if (key == null)
				{
					gOut.Print("You don't have the right tools for that.");

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
