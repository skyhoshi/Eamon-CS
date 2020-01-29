
// BurnDownLightSourceState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class BurnDownLightSourceState : EamonRT.Game.States.BurnDownLightSourceState, IBurnDownLightSourceState
	{
		public override void PrintLightAlmostOutCheck(IArtifact artifact, IArtifactCategory ac)
		{
			Debug.Assert(artifact != null && ac != null);

			if (artifact.Uid == 1)
			{
				if (ac.Field1 <= 10 && gEngine.RollDice(1, 100, 0) > 50)
				{
					gOut.Print("{0} flickers momentarily.", artifact.GetTheName(true, buf: Globals.Buf01));
				}
			}
			else
			{
				base.PrintLightAlmostOutCheck(artifact, ac);
			}
		}
	}
}
