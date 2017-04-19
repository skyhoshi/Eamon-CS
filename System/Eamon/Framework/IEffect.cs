
// IEffect.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Text;
using Eamon.Framework.DataEntry;
using Eamon.Framework.Validation;

namespace Eamon.Framework
{
	public interface IEffect : IHaveUid, IHaveFields, IValidator, IEditable, IComparable<IEffect>
	{
		#region Properties

		string Desc { get; set; }

		#endregion

		#region Methods

		RetCode BuildPrintedFullDesc(StringBuilder buf);

		#endregion
	}
}
