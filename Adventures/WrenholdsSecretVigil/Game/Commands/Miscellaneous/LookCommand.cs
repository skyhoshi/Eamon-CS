
// LookCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using WrenholdsSecretVigil.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.ILookCommand))]
	public class LookCommand : EamonRT.Game.Commands.LookCommand, ILookCommand
	{
		protected override void PlayerExecute()
		{
			ActorRoom.Seen = false;

			var artClasses = new Enums.ArtifactType[] { Enums.ArtifactType.Treasure, Enums.ArtifactType.DoorGate };

			var goldCurtainArtifact = Globals.ADB[40];

			Debug.Assert(goldCurtainArtifact != null);

			var ac = goldCurtainArtifact.GetArtifactClass(artClasses);

			Debug.Assert(ac != null);

			if (ActorRoom.Uid != 67 || ac.Type == Enums.ArtifactType.Treasure || ac.GetKeyUid() == 0)
			{
				var numRooms = Globals.Module.NumRooms;

				var directionValues = EnumUtil.GetValues<Enums.Direction>();

				foreach (var dv in directionValues)
				{
					if (ActorRoom.GetDirs(dv) < 0 && ActorRoom.GetDirs(dv) >= -numRooms)
					{
						var direction = Globals.Engine.GetDirections(dv);

						Debug.Assert(direction != null);

						Globals.Out.Write("{0}You found a secret passage {1}!{0}", Environment.NewLine, direction.Name.ToLower());

						ActorRoom.SetDirs(dv, -ActorRoom.GetDirs(dv));
					}
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<EamonRT.Framework.States.IMonsterStartState>();
			}
		}
	}
}
