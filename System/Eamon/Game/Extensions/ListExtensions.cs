
// ListExtensions.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Eamon.Game.Extensions
{
	public static class ListExtensions
	{
		public static void Sort<T>(this IList<T> list, IComparer comparer)
		{
			Debug.Assert(list != null && comparer != null);

			if (list.Count > 0)        // +++ VERIFY +++
			{
				var array = new T[list.Count];

				list.CopyTo(array, 0);

				array.Sort(comparer);

				list.Clear();

				list.AddRange(array);
			}
		}
	}
}
