
// IValidator.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.Args;

namespace Eamon.Framework.Validation
{
	public interface IValidator
	{
		#region Methods

		bool ValidateRecord(IValidateArgs args);

		bool ValidateField(IField field, IValidateArgs args);

		bool ValidateRecordInterdependencies(IValidateArgs args);

		bool ValidateFieldInterdependencies(IField field, IValidateArgs args);

		#endregion
	}
}
