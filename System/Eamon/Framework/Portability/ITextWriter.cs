
// ITextWriter.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Text;

namespace Eamon.Framework.Portability
{
	public class Coord
	{
		public int X { get; set; }

		public int Y { get; set; }
	}

	public interface ITextWriter
	{
		bool EnableOutput { get; set; }

		bool ResolveUidMacros { get; set; }

		bool WordWrap { get; set; }

		bool SuppressNewLines { get; set; }

		bool Stdout { get; set; }

		Encoding Encoding { get; }

		bool CursorVisible { get; set; }

		void SetCursorPosition(Coord coord);

		void SetWindowTitle(string title);

		void SetWindowSize(long width, long height);

		void SetBufferSize(long width, long height);

		Coord GetCursorPosition();

		long GetLargestWindowWidth();

		long GetLargestWindowHeight();

		long GetWindowHeight();

		long GetBufferHeight();

		void Print(string format, params object[] arg);

		void Write(object value);

		void Write(string value);

		void Write(decimal value);

		void Write(double value);

		void Write(float value);

		void Write(long value);

		void Write(uint value);

		void Write(int value);

		void Write(bool value);

		void Write(char[] buffer);

		void Write(char value);

		void Write(ulong value);

		void Write(string format, object arg0);

		void Write(string format, params object[] arg);

		void Write(string format, object arg0, object arg1);

		void Write(char[] buffer, int index, int count);

		void Write(string format, object arg0, object arg1, object arg2);

		void WriteLine();

		void WriteLine(object value);

		void WriteLine(string value);

		void WriteLine(decimal value);

		void WriteLine(float value);

		void WriteLine(ulong value);

		void WriteLine(double value);

		void WriteLine(uint value);

		void WriteLine(int value);

		void WriteLine(bool value);

		void WriteLine(char[] buffer);

		void WriteLine(char value);

		void WriteLine(long value);

		void WriteLine(string format, object arg0);

		void WriteLine(string format, params object[] arg);

		void WriteLine(char[] buffer, int index, int count);

		void WriteLine(string format, object arg0, object arg1);

		void WriteLine(string format, object arg0, object arg1, object arg2);
	}
}
