
// TextWriter.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Threading;
using System.Text;
using Xamarin.Forms;
using Eamon.Framework.Portability;
using Eamon.Game.Extensions;
using Eamon.Mobile;
using static Eamon.Game.Plugin.PluginContext;

namespace EamonPM.Game.Portability
{
	public class TextWriter : ITextWriter
	{
		protected virtual StringBuilder Buf { get; set; }

		protected virtual StringBuilder Buf01 { get; set; }

		public virtual bool EnableOutput { get; set; }

		public virtual bool ResolveUidMacros { get; set; }

		public virtual bool WordWrap { get; set; }

		public virtual bool Stdout { get; set; }

		protected virtual void EnforceOutputBufMaxSize()
		{
			if (App.OutputBuf.Length - (App.OutputBufStartIndex + 1) > App.SettingsViewModel.OutputBufMaxSize)
			{
				App.OutputBufMutex.ReleaseMutex();

				System.Threading.Thread.Sleep(1000);

				App.OutputBufMutex.WaitOne();

				while (App.OutputBuf.Length - (App.OutputBufStartIndex + 1) > App.SettingsViewModel.OutputBufMaxSize)
				{
					App.OutputBufStartIndex += (long)(App.SettingsViewModel.OutputBufMaxSize * 0.25);
				}

				var nlChar = Environment.NewLine[Environment.NewLine.Length - 1];

				while (App.OutputBufStartIndex > -1 && App.OutputBuf[(int)App.OutputBufStartIndex] != nlChar)
				{
					App.OutputBufStartIndex--;
				}

				RefreshOutputText();

				App.OutputBufMutex.ReleaseMutex();

				System.Threading.Thread.Sleep(1000);

				App.OutputBufMutex.WaitOne();
			}
		}

		protected virtual void RefreshOutputText()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				try
				{
					App.OutputBufMutex.WaitOne();

					var startIndex = (int)(App.OutputBufStartIndex + 1);

					var length = App.OutputBuf.Length - startIndex;

					App.PluginLauncherViewModel.OutputText = App.OutputBuf.ToString(startIndex, length);
				}
				catch (Exception ex)
				{
					// do something
				}
				finally
				{
					App.OutputBufMutex.ReleaseMutex();
				}
			});
		}

		public virtual Encoding Encoding
		{
			get
			{
				// +++ IMPLEMENT +++

				return Encoding.Unicode;
			}
		}

		public virtual bool CursorVisible
		{
			get
			{
				// +++ IMPLEMENT +++

				return true;
			}

			set
			{
				// +++ IMPLEMENT +++
			}
		}

		public virtual void SetCursorPosition(Coord coord)
		{
			Debug.Assert(coord != null);

			try
			{
				App.OutputBufMutex.WaitOne();

				EnforceOutputBufMaxSize();

				App.OutputBuf.Length = coord.X;

				RefreshOutputText();
			}
			catch (Exception ex)
			{
				// do something
			}
			finally
			{
				App.OutputBufMutex.ReleaseMutex();
			}
		}

		public virtual void SetWindowTitle(string title)
		{
			Debug.Assert(title != null);

			// +++ IMPLEMENT +++
		}

		public virtual void SetWindowSize(long width, long height)
		{
			// +++ IMPLEMENT +++
		}

		public virtual void SetBufferSize(long width, long height)
		{
			// +++ IMPLEMENT +++
		}

		public virtual Coord GetCursorPosition()
		{
			var coord = new Coord();

			try
			{
				App.OutputBufMutex.WaitOne();

				coord.X = App.OutputBuf.Length;

				coord.Y = -1;
			}
			catch (Exception ex)
			{
				// do something
			}
			finally
			{
				App.OutputBufMutex.ReleaseMutex();
			}

			return coord;
		}

		public virtual long GetLargestWindowWidth()
		{
			// +++ IMPLEMENT +++

			return 0;
		}

		public virtual long GetLargestWindowHeight()
		{
			// +++ IMPLEMENT +++

			return 0;
		}

		public virtual long GetWindowHeight()
		{
			// +++ IMPLEMENT +++

			return 0;
		}

		public virtual long GetBufferHeight()
		{
			// +++ IMPLEMENT +++

			return 0;
		}

		public virtual void Write(object value)
		{
			Write("{0}", value);
		}

		public virtual void Write(string value)
		{
			Write("{0}", value);
		}

		public virtual void Write(decimal value)
		{
			Write("{0}", value);
		}

		public virtual void Write(double value)
		{
			Write("{0}", value);
		}

		public virtual void Write(float value)
		{
			Write("{0}", value);
		}

		public virtual void Write(long value)
		{
			Write("{0}", value);
		}

		public virtual void Write(uint value)
		{
			Write("{0}", value);
		}

		public virtual void Write(int value)
		{
			Write("{0}", value);
		}

		public virtual void Write(bool value)
		{
			Write("{0}", value);
		}

		public virtual void Write(char[] buffer)
		{
			Debug.Assert(buffer != null);

			Write("{0}", buffer.ToString());
		}

		public virtual void Write(char value)
		{
			Write("{0}", value);
		}

		public virtual void Write(ulong value)
		{
			Write("{0}", value);
		}

		public virtual void Write(string format, object arg0)
		{
			Write(format, new object[] { arg0 });
		}

		public virtual void Write(string format, params object[] arg)
		{
			Debug.Assert(format != null);

			Buf.SetFormat(format, arg);

			if (ResolveUidMacros && Globals?.Engine != null)
			{
				Buf01.Clear();

				var rc = Globals.Engine.ResolveUidMacros(Buf.ToString(), Buf01, true, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Buf.SetFormat("{0}", Buf01);
			}

			if (WordWrap && Globals?.Engine != null)
			{
				Globals.Engine.WordWrap(Buf.ToString(), Buf);
			}

			if (EnableOutput)
			{
				try
				{
					App.OutputBufMutex.WaitOne();

					EnforceOutputBufMaxSize();

					if (Stdout)
					{
						App.OutputBuf.Append(Buf);
					}
					else
					{
						App.OutputBuf.Append(Buf);
					}

					RefreshOutputText();
				}
				catch (Exception ex)
				{
					// do something
				}
				finally
				{
					App.OutputBufMutex.ReleaseMutex();
				}
			}
		}

		public virtual void Write(string format, object arg0, object arg1)
		{
			Write(format, new object[] { arg0, arg1 });
		}

		public virtual void Write(char[] buffer, int index, int count)
		{
			Debug.Assert(buffer != null);

			Write("{0}", buffer.ToString().Substring(index, count));
		}

		public virtual void Write(string format, object arg0, object arg1, object arg2)
		{
			Write(format, new object[] { arg0, arg1, arg2 });
		}

		public virtual void WriteLine()
		{
			WriteLine("{0}", "");
		}

		public virtual void WriteLine(object value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(string value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(decimal value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(float value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(ulong value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(double value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(uint value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(int value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(bool value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(char[] buffer)
		{
			Debug.Assert(buffer != null);

			WriteLine("{0}", buffer.ToString());
		}

		public virtual void WriteLine(char value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(long value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(string format, object arg0)
		{
			WriteLine(format, new object[] { arg0 });
		}

		public virtual void WriteLine(string format, params object[] arg)
		{
			Debug.Assert(format != null);

			Buf.SetFormat(format, arg);

			if (ResolveUidMacros && Globals?.Engine != null)
			{
				Buf01.Clear();

				var rc = Globals.Engine.ResolveUidMacros(Buf.ToString(), Buf01, true, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Buf.SetFormat("{0}", Buf01);
			}

			if (WordWrap && Globals?.Engine != null)
			{
				Globals.Engine.WordWrap(Buf.ToString(), Buf);
			}

			if (EnableOutput)
			{
				try
				{
					App.OutputBufMutex.WaitOne();

					EnforceOutputBufMaxSize();

					if (Stdout)
					{
						App.OutputBuf.AppendFormat("{0}{1}", Buf, Environment.NewLine);
					}
					else
					{
						App.OutputBuf.AppendFormat("{0}{1}", Buf, Environment.NewLine);
					}

					RefreshOutputText();
				}
				catch (Exception ex)
				{
					// do something
				}
				finally
				{
					App.OutputBufMutex.ReleaseMutex();
				}
			}
		}

		public virtual void WriteLine(char[] buffer, int index, int count)
		{
			Debug.Assert(buffer != null);

			WriteLine("{0}", buffer.ToString().Substring(index, count));
		}

		public virtual void WriteLine(string format, object arg0, object arg1)
		{
			WriteLine(format, new object[] { arg0, arg1 });
		}

		public virtual void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			WriteLine(format, new object[] { arg0, arg1, arg2 });
		}

		public TextWriter()
		{
			Buf = new StringBuilder(Constants.BufSize);

			Buf01 = new StringBuilder(Constants.BufSize);

			EnableOutput = true;

			ResolveUidMacros = true;

			WordWrap = true;

			Stdout = true;
		}
	}
}