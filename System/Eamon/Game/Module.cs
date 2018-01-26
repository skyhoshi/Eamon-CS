
// Module.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Module : GameBase, IModule
	{
		#region Public Properties

		#region Interface IModule

		public virtual string Author { get; set; }

		public virtual string VolLabel { get; set; }

		public virtual string SerialNum { get; set; }

		public virtual DateTime LastMod { get; set; }

		public virtual long IntroStory { get; set; }

		public virtual long NumDirs { get; set; }

		public virtual long NumRooms { get; set; }

		public virtual long NumArtifacts { get; set; }

		public virtual long NumEffects { get; set; }

		public virtual long NumMonsters { get; set; }

		public virtual long NumHints { get; set; }

		#endregion

		#endregion

		#region Protected Methods

		#region Interface IDisposable

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeModuleUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IComparable

		public virtual int CompareTo(IModule module)
		{
			return this.Uid.CompareTo(module.Uid);
		}

		#endregion

		#region Interface IModule

		public virtual void PrintInfo()
		{
			var buf = new StringBuilder(Constants.BufSize);

			buf.AppendPrint("This is {0}, by {1}.",
				Name,
				Author);

			buf.AppendFormat("{0}Serial Number:  {1}{0}Volume  Label:  {2}{0}Last Modified:  {3}{0}{0}",
				Environment.NewLine,
				SerialNum,
				VolLabel,
				LastMod.ToString("MM/dd/yyyy HH:mm:ss"));

			buf.AppendFormat("{0}{1}", Desc, Environment.NewLine);

			Globals.Out.Write("{0}", buf);
		}

		#endregion

		#region Class Module

		public Module()
		{
			Author = "";

			VolLabel = "";

			SerialNum = "";

			LastMod = DateTime.Now;
		}

		#endregion

		#endregion
	}
}
