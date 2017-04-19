
// IntroStory.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game
{
	[ClassMappings]
	public class IntroStory : IIntroStory
	{
		public virtual StringBuilder Buf { get; set; }

		public virtual bool ShouldPrintOutput
		{
			get
			{
				var result = true;

				if (Globals.Database.GetFilesetsCount() > 0)
				{
					Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

					Globals.Out.WriteLine("{0}Welcome back to {1}!", Environment.NewLine, Globals.Module.Name);

					Globals.Out.Write("{0}Would you like to see the introduction story again (Y/N) [N]: ", Environment.NewLine);

					Buf.Clear();

					var rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, "N", Globals.Engine.ModifyCharToUpper, Globals.Engine.IsCharYOrN, null);

					Debug.Assert(Globals.Engine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					if (Buf.Length > 0 && Buf[0] != 'Y')
					{
						result = false;
					}
				}

				return result;
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public virtual void PrintOutput()
		{
			RetCode rc;

			Globals.Out.WriteLine("{0}{1}", Environment.NewLine, Globals.LineSep);

			var effect = Globals.EDB[Globals.Module.IntroStory];

			if (effect != null)
			{
				Buf.Clear();

				rc = Globals.Engine.ResolveUidMacros(effect.Desc, Buf, true, true);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WriteLine();

				var pages = Buf.ToString().Split(new string[] { Constants.PageSep }, StringSplitOptions.RemoveEmptyEntries);

				for (var i = 0; i < pages.Length; i++)
				{
					if (i > 0)
					{
						Globals.Out.WriteLine("{0}{1}{0}", Environment.NewLine, Globals.LineSep);
					}

					Globals.Out.Write("{0}", pages[i]);

					if (i < pages.Length - 1)
					{
						Globals.Out.WriteLine();

						Globals.In.KeyPress(Buf);
					}
				}

				Globals.Out.WriteLine();
			}
			else
			{
				Globals.Out.WriteLine("{0}There is no introduction story for this adventure.", Environment.NewLine);
			}
		}

		public IntroStory()
		{
			Buf = Globals.Buf;
		}
	}
}
