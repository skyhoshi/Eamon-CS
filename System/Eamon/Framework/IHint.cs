
// IHint.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;

namespace Eamon.Framework
{
	public interface IHint : IGameBase, IComparable<IHint>
	{
		#region Properties

		bool Active { get; set; }

		string Question { get; set; }

		long NumAnswers { get; set; }

		string[] Answers { get; set; }

		#endregion

		#region Methods

		string GetAnswers(long index);

		void SetAnswers(long index, string value);

		#endregion
	}
}
