
// IListExtensions.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

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
	}
}
