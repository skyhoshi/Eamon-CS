
// ObjectExtensions.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Diagnostics;

namespace Eamon.Game.Extensions
{
	public static class ObjectExtensions
	{
		public static T CastTo<T>(this object obj) where T : class
		{
			Debug.Assert(obj != null);

			return obj as T;
		}
	}
}
