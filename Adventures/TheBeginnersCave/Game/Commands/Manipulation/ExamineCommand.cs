
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void PlayerExecute()
		{
			Debug.Assert(gDobjArtifact != null || gDobjMonster != null);

			// don't show bites/drinks left for spices

			if (gDobjArtifact != null && gDobjArtifact.Uid == 8)
			{
				var ac = gDobjArtifact.Edible;

				Debug.Assert(ac != null);

				var field2 = ac.Field2;

				ac.Field2 = Constants.InfiniteDrinkableEdible;

				base.PlayerExecute();

				ac.Field2 = field2;
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
