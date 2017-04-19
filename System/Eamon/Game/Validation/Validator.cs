
// Validator.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Linq;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Args;

namespace Eamon.Game.Validation
{
	public abstract class Validator
	{
		#region Protected Properties

		[ExcludeFromSerialization]
		protected virtual IList<IField> Fields { get; set; }

		#endregion

		#region Protected Methods

		#region Interface IDisposable

		protected abstract void Dispose(bool disposing);

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public void Dispose()      // virtual intentionally omitted
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion

		#region Interface IHaveFields

		public virtual void FreeFields()
		{
			Fields = null;
		}

		public abstract IList<IField> GetFields();

		public virtual IField GetField(string name)
		{
			IField result;

			if (string.IsNullOrWhiteSpace(name))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			result = GetFields().FirstOrDefault(f => f.Name == name);

		Cleanup:

			return result;
		}

		public virtual IField GetField(long listNum)
		{
			IField result;

			if (listNum == 0)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			result = GetFields().FirstOrDefault(f => f.ListNum == listNum);

		Cleanup:

			return result;
		}

		#endregion

		#region Interface IValidator

		public virtual bool ValidateRecord(IValidateArgs args)
		{
			bool result;

			if (args == null)
			{
				result = false;

				// PrintError

				goto Cleanup;
			}

			result = true;

			var fields = GetFields();

			foreach (var f in fields)
			{
				result = ValidateField(f, args);

				if (result == false)
				{
					break;
				}
			}

		Cleanup:

			return result;
		}

		public virtual bool ValidateField(IField field, IValidateArgs args)
		{
			bool result;

			if (field == null || args == null)
			{
				result = false;

				// PrintError

				goto Cleanup;
			}

			result = true;

			if (field.Validate != null)
			{
				args.Clear();

				result = field.Validate(field, args);

				if (result == false)
				{
					args.ErrorField = field;
				}
			}

		Cleanup:

			return result;
		}

		public virtual bool ValidateRecordInterdependencies(IValidateArgs args)
		{
			bool result;

			if (args == null)
			{
				result = false;

				// PrintError

				goto Cleanup;
			}

			result = true;

			var fields = GetFields();

			foreach (var f in fields)
			{
				result = ValidateFieldInterdependencies(f, args);

				if (result == false)
				{
					break;
				}
			}

		Cleanup:

			return result;
		}

		public virtual bool ValidateFieldInterdependencies(IField field, IValidateArgs args)
		{
			bool result;

			if (field == null || args == null)
			{
				result = false;

				// PrintError

				goto Cleanup;
			}

			result = true;

			if (field.ValidateInterdependencies != null)
			{
				args.Clear();

				result = field.ValidateInterdependencies(field, args);

				if (result == false)
				{
					args.ErrorField = field;
				}
			}

		Cleanup:

			return result;
		}

		#endregion

		#endregion
	}
}
