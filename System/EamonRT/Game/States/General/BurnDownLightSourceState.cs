
// BurnDownLightSourceState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BurnDownLightSourceState : State, IBurnDownLightSourceState
	{
		public override void Execute()
		{
			var artUid = Globals.GameState.Ls;

			if (artUid > 0 && ShouldPreTurnProcess())
			{
				var artifact = Globals.ADB[artUid];

				Debug.Assert(artifact != null);

				var ac = artifact.GetArtifactCategory(Enums.ArtifactType.LightSource);

				if (ac != null && ac.Field1 != -1)
				{
					if (ac.Field1 > 0)
					{
						ac.Field1--;
					}

					if (ac.Field1 > 0)
					{
						if (ac.Field1 < 20)
						{
							Globals.Out.Print("{0}{1}", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf01), ac.Field1 < 10 ? " is almost out!" : " grows dim!");
						}
					}
					else
					{
						Globals.Engine.LightOut(artifact);
					}
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IBurnDownSpeedSpellState>();
			}

			Globals.NextState = NextState;
		}

		public BurnDownLightSourceState()
		{
			Name = "BurnDownLightSourceState";
		}
	}
}
