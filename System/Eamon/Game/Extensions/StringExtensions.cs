
// StringExtensions.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;

namespace Eamon.Game.Extensions
{
	public static class StringExtensions
	{
		public static string FirstCharToUpper(this string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				return Char.ToUpper(str[0]) + str.Substring(1);
			}
			else
			{
				return str;
			}
		}

		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			if (source != null && toCheck != null)
			{
				return source.IndexOf(toCheck, comp) >= 0;
			}
			else
			{
				return false;
			}
		}

		public static string PadTLeft(this string text, int totalWidth, char paddingChar)
		{
			Debug.Assert(text != null);

			string arg = text.Length > totalWidth ? text.Substring(0, totalWidth) : text;

			return arg.PadLeft(totalWidth, paddingChar);
		}

		public static string PadTRight(this string text, int totalWidth, char paddingChar)
		{
			Debug.Assert(text != null);

			string arg = text.Length > totalWidth ? text.Substring(0, totalWidth) : text;

			return arg.PadRight(totalWidth, paddingChar);
		}

		public static string Truncate(this string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value)) return value;
			return value.Length <= maxLength ? value : value.Substring(0, maxLength);
		}
	}
}
