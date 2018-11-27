
// IListExtensions.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Eamon.Game.Extensions
{
	public static class IListExtensions
	{
		public static void AddRange<T>(this IList<T> list, IEnumerable<T> enu)
		{
			Debug.Assert(list != null && enu != null);

			if (list is List<T>)
			{
				((List<T>)list).AddRange(enu);
			}
			else
			{
				foreach (T obj in enu)
				{
					list.Add(obj);
				}
			}
		}

		public static int FindIndex<T>(this IList<T> list, Func<T, bool> predicate)
		{
			if (list == null) throw new ArgumentNullException("list");
			if (predicate == null) throw new ArgumentNullException("predicate");

			int retVal = 0;
			foreach (var item in list)
			{
				if (predicate(item)) return retVal;
				retVal++;
			}
			return -1;
		}
	}
}
