
// StateImpl.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class StateImpl : EamonRT.Game.States.StateImpl, IStateImpl
	{
		public override void PrintObjBlocksTheWay(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Tombs and crypts need to be opened before entry

			if (artifact.Uid == 3 || artifact.Uid == 4 || artifact.Uid == 5)
			{
				gOut.Print("{0}'s door will have to be opened first!", artifact.GetTheName(true));
			}
			else
			{
				base.PrintObjBlocksTheWay(artifact);
			}
		}

		public override void PrintEnemiesNearby()
		{
			gOut.Print("You don't want to turn your back here!");
		}
	}
}
