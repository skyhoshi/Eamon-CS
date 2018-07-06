
// ArtifactHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;

namespace WalledCityOfDarkness.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IArtifact>))]
	public class ArtifactHelper : Eamon.Game.Helpers.ArtifactHelper
	{
		protected override bool ValidateCategoriesField1()
		{
			var i = Index;

			// Unusually high weapon complexity for blue light/star wand

			if (i == 0 && (Record.Uid == 40 || Record.Uid == 50))
			{
				return Record.Field1 >= -50 && Record.Field1 <= 80;
			}
			else
			{
				return base.ValidateCategoriesField1();
			}
		}
	}
}
