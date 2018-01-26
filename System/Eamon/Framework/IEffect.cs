
// IEffect.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Text;

namespace Eamon.Framework
{
	public interface IEffect : IGameBase, IComparable<IEffect>
	{
		#region Methods

		RetCode BuildPrintedFullDesc(StringBuilder buf);

		#endregion
	}
}
