
// PutCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
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
				Globals.Out.Print("You mangled {0}!", artifact.EvalPlural("it", "them"));
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

				if (IobjArtifact.Uid == 24 || IobjArtifact.Uid == 25)
				{
					Globals.Out.Print("{0} start{1} dissolving on contact with {2}!", DobjArtifact.GetDecoratedName03(true, true, false, false, Globals.Buf), DobjArtifact.EvalPlural("s", ""), IobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf01));

					Globals.Out.Print("{0} {1} destroyed!", DobjArtifact.GetDecoratedName03(true, true, false, false, Globals.Buf), DobjArtifact.EvalPlural("is", "are"));

					DobjArtifact.SetInLimbo();
				}

				// Put orb in metal pedestal

				else if (DobjArtifact.Uid == 4 && IobjArtifact.Uid == 43)
				{
					Globals.Engine.PrintEffectDesc(43);

					Globals.Engine.PrintEffectDesc(44);

					var adjacentRoom = Globals.RDB[45];

					Debug.Assert(adjacentRoom != null);

					var newRoom = Globals.RDB[15];

					Debug.Assert(newRoom != null);

					adjacentRoom.SetDirs(Enums.Direction.South, 15);

					IobjArtifact.IsListed = false;

					Globals.Engine.TransportPlayerBetweenRooms(ActorRoom, newRoom, null);

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
			var ac = IobjArtifact.Treasure;

			Debug.Assert(ac != null);

			ac.Type = Enums.ArtifactType.Container;

			ac.Field1 = 0;

			ac.Field2 = 1;

			ac.Field3 = 9999;

			ac.Field4 = 1;

			ac.Field5 = 0;
		}

		public virtual void ConvertSlimeToTreasure()
		{
			var ac = IobjArtifact.Container;

			Debug.Assert(ac != null);

			ac.Type = Enums.ArtifactType.Treasure;

			ac.Field1 = 0;

			ac.Field2 = 0;

			ac.Field3 = 0;

			ac.Field4 = 0;

			ac.Field5 = 0;
		}

		public override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null && IobjArtifact != null);

			if (IobjArtifact.Uid == 24 || IobjArtifact.Uid == 25)
			{
				ConvertSlimeToContainer();
			}

			base.PlayerExecute();

			if (IobjArtifact.Uid == 24 || IobjArtifact.Uid == 25)
			{
				ConvertSlimeToTreasure();
			}
		}
	}
}
