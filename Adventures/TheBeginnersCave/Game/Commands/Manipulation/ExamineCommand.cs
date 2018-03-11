
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheBeginnersCave.Framework.Commands;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings(typeof(EamonRT.Framework.Commands.IExamineCommand))]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		protected override void PlayerExecute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// don't show bites/drinks left for spices

			if (DobjArtifact != null && DobjArtifact.Uid == 8)
			{
				var ac = DobjArtifact.GetArtifactCategory(Enums.ArtifactType.Edible);

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
