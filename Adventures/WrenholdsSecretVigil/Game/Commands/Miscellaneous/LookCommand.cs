
// LookCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class LookCommand : EamonRT.Game.Commands.LookCommand, EamonRT.Framework.Commands.ILookCommand
	{
		protected override void PlayerExecute()
		{
			ActorRoom.Seen = false;

			var artTypes = new Enums.ArtifactType[] { Enums.ArtifactType.Treasure, Enums.ArtifactType.DoorGate };

			var goldCurtainArtifact = Globals.ADB[40];

			Debug.Assert(goldCurtainArtifact != null);

			var ac = goldCurtainArtifact.GetArtifactCategory(artTypes);

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

						Globals.Out.Print("You found a secret passage {0}!", direction.Name.ToLower());

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
