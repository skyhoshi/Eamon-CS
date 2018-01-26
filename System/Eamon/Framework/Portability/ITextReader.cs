
// ITextReader.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Text;

namespace Eamon.Framework.Portability
{
	public interface ITextReader
	{
		bool EnableInput { get; set; }

		RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, char fillChar, char maskChar, bool emptyAllowed, string emptyVal, Func<char, char> modifyCharFunc, Func<char, bool> validCharFunc, Func<char, bool> termCharFunc);

		RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, string emptyVal, Func<char, bool> validCharFunc);

		RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, string emptyVal);

		string ReadLine();

		char ReadKey(bool intercept);

		void KeyPress(StringBuilder buf, bool initialNewLine = true);
	}
}
