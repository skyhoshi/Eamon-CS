
// PutCommand.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using WrenholdsSecretVigil.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IPutCommand))]
	public class PutCommand : EamonRT.Game.Commands.PutCommand, IPutCommand
	{
		protected override void PrintBrokeIt(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Swallower shark

			if (artifact.Uid == 31)
			{
				Globals.Out.WriteLine("{0}You mangled {1}!", Environment.NewLine, artifact.EvalPlural("it", "them"));
			}
			else
			{
				base.PrintBrokeIt(artifact);
			}
		}

		protected override void PlayerProcessEvents()
		{
			// Put anything in slime destroys it

			if (IobjArtifact.Uid == 24 || IobjArtifact.Uid == 25)
			{
				Globals.Out.Write("{0}{1} start{2} dissolving on contact with {3}!{0}", Environment.NewLine, DobjArtifact.GetDecoratedName03(true, true, false, false, Globals.Buf),	DobjArtifact.EvalPlural("s", ""), IobjArtifact.GetDecoratedName03(false, true, false, false, Globals.Buf01));

				Globals.Out.Write("{0}{1} {2} destroyed!{0}", Environment.NewLine, DobjArtifact.GetDecoratedName03(true, true, false, false, Globals.Buf), DobjArtifact.EvalPlural("is", "are"));

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

				NextState = Globals.CreateInstance<EamonRT.Framework.States.IAfterPlayerMoveState>();
			}
			else
			{
				base.PlayerProcessEvents();
			}
		}

		protected virtual void ConvertSlimeToContainer()
		{
			var ac = IobjArtifact.GetArtifactClass(Enums.ArtifactType.Treasure);

			Debug.Assert(ac != null);

			ac.Type = Enums.ArtifactType.Container;

			ac.Field5 = 0;

			ac.Field6 = 1;

			ac.Field7 = 9999;

			ac.Field8 = 1;
		}

		protected virtual void ConvertSlimeToTreasure()
		{
			var ac = IobjArtifact.GetArtifactClass(Enums.ArtifactType.Container);

			Debug.Assert(ac != null);

			ac.Type = Enums.ArtifactType.Treasure;

			ac.Field5 = 0;

			ac.Field6 = 0;

			ac.Field7 = 0;

			ac.Field8 = 0;
		}

		protected override void PlayerExecute()
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
