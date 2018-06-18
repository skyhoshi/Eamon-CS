
// ArtifactHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Helpers.Generic;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace WalledCityOfDarkness.Game.Helpers
{
	[ClassMappings(typeof(IHelper<IArtifact>))]
	public class ArtifactHelper : Eamon.Game.Helpers.ArtifactHelper
	{
		protected override bool ValidateCategoriesField1(IField field, IValidateArgs args)
		{
			Debug.Assert(field != null && field.UserData != null);

			var i = Convert.ToInt64(field.UserData);

			// Unusually high weapon complexity for blue light/star wand

			if (i == 0 && (Record.Type == Enums.ArtifactType.Weapon || Record.Type == Enums.ArtifactType.MagicWeapon))
			{
				return Record.Field1 >= -50 && Record.Field1 <= 80;
			}
			else
			{
				return base.ValidateCategoriesField1(field, args);
			}
		}
	}
}
