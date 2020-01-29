
// BurnDownLightSourceState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BurnDownLightSourceState : State, IBurnDownLightSourceState
	{
		public virtual void PrintLightAlmostOutCheck(IArtifact artifact, IArtifactCategory ac)
		{
			Debug.Assert(artifact != null && ac != null);

			if (ac.Field1 <= 20)
			{
				gOut.Print("{0}{1}", artifact.GetTheName(true, buf: Globals.Buf01), ac.Field1 <= 10 ? " is almost out!" : " grows dim!");
			}
		}

		public virtual void DecrementLightTurnCounter(IArtifact artifact, IArtifactCategory ac)
		{
			Debug.Assert(artifact != null && ac != null);

			ac.Field1--;
		}

		public override void Execute()
		{
			var artUid = gGameState.Ls;

			if (artUid > 0 && ShouldPreTurnProcess())
			{
				var artifact = gADB[artUid];

				Debug.Assert(artifact != null);

				var ac = artifact.LightSource;

				if (ac != null && ac.Field1 != -1)
				{
					if (ac.Field1 > 0)
					{
						PrintLightAlmostOutCheck(artifact, ac);

						DecrementLightTurnCounter(artifact, ac);

						if (ac.Field1 < 0)
						{
							ac.Field1 = 0;
						}
					}
					else
					{
						gEngine.LightOut(artifact);
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
