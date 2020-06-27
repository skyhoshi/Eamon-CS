
// Hint.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using TheVileGrimoireOfJaldial.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings]
	public class Hint : Eamon.Game.Hint, IHint
	{
		public override bool Active
		{
			get
			{
				if (Globals.EnableGameOverrides)
				{
					Framework.IRoom room = null;

					switch (Uid)
					{
						case 5:

							var woodenBucketArtifact = gADB[6];

							Debug.Assert(woodenBucketArtifact != null);

							return woodenBucketArtifact.Seen;

						case 6:

							var shovelArtifact = gADB[7];

							Debug.Assert(shovelArtifact != null);

							return shovelArtifact.Seen;

						case 7:

							var ropeArtifact = gADB[38];

							Debug.Assert(ropeArtifact != null);

							return ropeArtifact.Seen;

						case 8:

							var bronzeCrossArtifact = gADB[37];

							Debug.Assert(bronzeCrossArtifact != null);

							return bronzeCrossArtifact.Seen;

						case 9:

							var torchArtifact = gADB[1];

							Debug.Assert(torchArtifact != null && torchArtifact.LightSource != null);

							return torchArtifact.LightSource.Field1 <= 10;

						case 10:

							room = (gGameState != null ? gRDB[gGameState.Ro] : null) as Framework.IRoom;

							return room != null && room.IsCryptRoom();

						case 11:

							var waterWeirdMonster = gMDB[38];

							Debug.Assert(waterWeirdMonster != null);

							return waterWeirdMonster.Seen;

						case 12:

							var jaldialRemainsArtifact = gADB[57];

							Debug.Assert(jaldialRemainsArtifact != null);

							return jaldialRemainsArtifact.Seen;

						case 13:

							room = (gGameState != null ? gRDB[gGameState.Ro] : null) as Framework.IRoom;

							return room != null && room.IsGroundsRoom() && gGameState.Day > 0;

						case 14:

							room = (gGameState != null ? gRDB[gGameState.Ro] : null) as Framework.IRoom;

							return room != null && room.IsGroundsRoom() && gGameState.WeatherType != WeatherType.None;

						case 15:

							var tombstoneArtifact = gADB[10];

							Debug.Assert(tombstoneArtifact != null);

							return tombstoneArtifact.Seen;

						case 16:

							room = gRDB[110] as Framework.IRoom;

							Debug.Assert(room != null);

							var jaldialMonster = gMDB[43];

							Debug.Assert(jaldialMonster != null);

							return room.Seen && jaldialMonster.Seen;

						default:

							return base.Active;
					}
				}
				else
				{
					return base.Active;
				}			
			}

			set
			{
				base.Active = value;
			}
		}
	}
}
