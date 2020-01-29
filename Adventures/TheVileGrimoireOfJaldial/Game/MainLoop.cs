
// MainLoop.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			var torchArtifact = gADB[1];

			Debug.Assert(torchArtifact != null);

			// Scale torch value based on rounds remaining

			torchArtifact.Value = (long)Math.Round((double)torchArtifact.Value * ((double)torchArtifact.LightSource.Field1 / (double)gGameState.TorchRounds));

			var parchmentArtifact = gADB[33];

			Debug.Assert(parchmentArtifact != null);

			// TODO: if ancient parchment is still carried, exit text should indicate it is returned to wizard's care

			parchmentArtifact.SetInLimbo();

			var armorArtifact = gGameState.Ar > 0 ? gADB[gGameState.Ar] : null;

			// Crimson cloak (if worn with armor) goes with the player character - move to limbo but let armor bonus remain

			var cloakArtifact = gADB[19];

			Debug.Assert(cloakArtifact != null);

			if (armorArtifact != null && armorArtifact.Uid != 19 && cloakArtifact.IsWornByCharacter())
			{
				if (armorArtifact.Desc.Length + cloakArtifact.Desc.Length + 2 <= Constants.ArtDescLen)
				{
					armorArtifact.Desc = string.Format("{0}  {1}", armorArtifact.Desc, cloakArtifact.Desc);
				}

				cloakArtifact.SetInLimbo();
			}

			// Steel gauntlets (if worn) go with the player character - move to limbo but let skill bonuses remain

			var gauntletsArtifact = gADB[16];

			Debug.Assert(gauntletsArtifact != null);

			if (gauntletsArtifact.IsWornByCharacter())
			{
				if (armorArtifact != null && armorArtifact.Desc.Length + gauntletsArtifact.Desc.Length + 2 <= Constants.ArtDescLen)
				{
					armorArtifact.Desc = string.Format("{0}  {1}", armorArtifact.Desc, gauntletsArtifact.Desc);
				}

				gauntletsArtifact.SetInLimbo();
			}

			base.Shutdown();
		}
	}
}
