
// PutCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class PutCommand : EamonRT.Game.Commands.PutCommand, IPutCommand
	{
		public override void PrintBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Swallower shark

			if (artifact.Uid == 31)
			{
				gOut.Print("You mangled {0}!", artifact.EvalPlural("it", "them"));
			}
			else
			{
				base.PrintBrokeIt(artifact);
			}
		}

		public override void PlayerProcessEvents(long eventType)
		{
			if (eventType == PpeAfterArtifactPut)
			{
				// Put anything in slime destroys it

				if (gIobjArtifact.Uid == 24 || gIobjArtifact.Uid == 25)
				{
					gOut.Print("{0} start{1} dissolving on contact with {2}!", gDobjArtifact.GetTheName(true), gDobjArtifact.EvalPlural("s", ""), gIobjArtifact.GetTheName(buf: Globals.Buf01));

					gOut.Print("{0} {1} destroyed!", gDobjArtifact.GetTheName(true), gDobjArtifact.EvalPlural("is", "are"));

					gDobjArtifact.SetInLimbo();
				}

				// Put orb in metal pedestal

				else if (gDobjArtifact.Uid == 4 && gIobjArtifact.Uid == 43)
				{
					gEngine.PrintEffectDesc(43);

					gEngine.PrintEffectDesc(44);

					var adjacentRoom = gRDB[45];

					Debug.Assert(adjacentRoom != null);

					var newRoom = gRDB[15];

					Debug.Assert(newRoom != null);

					adjacentRoom.SetDirs(Direction.South, 15);

					gIobjArtifact.IsListed = false;

					gEngine.TransportPlayerBetweenRooms(gActorRoom, newRoom, null);

					NextState = Globals.CreateInstance<IAfterPlayerMoveState>();
				}
				else
				{
					base.PlayerProcessEvents(eventType);
				}
			}
			else
			{
				base.PlayerProcessEvents(eventType);
			}
		}

		public virtual void ConvertSlimeToContainer()
		{
			var ac = gIobjArtifact.Treasure;

			Debug.Assert(ac != null);

			ac.Type = ArtifactType.InContainer;

			ac.Field1 = 0;

			ac.Field2 = 1;

			ac.Field3 = 9999;

			ac.Field4 = 1;

			ac.Field5 = 0;
		}

		public virtual void ConvertSlimeToTreasure()
		{
			var ac = gIobjArtifact.InContainer;

			Debug.Assert(ac != null);

			ac.Type = ArtifactType.Treasure;

			ac.Field1 = 0;

			ac.Field2 = 0;

			ac.Field3 = 0;

			ac.Field4 = 0;

			ac.Field5 = 0;
		}

		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null && gIobjArtifact != null);

			if (gIobjArtifact.Uid == 24 || gIobjArtifact.Uid == 25)
			{
				ConvertSlimeToContainer();
			}

			base.PlayerExecute();

			if (gIobjArtifact.Uid == 24 || gIobjArtifact.Uid == 25)
			{
				ConvertSlimeToTreasure();
			}
		}
	}
}
