
// Effect.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Effect : GameBase, IEffect
	{
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
				Globals.Database.FreeEffectUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IComparable

		public virtual int CompareTo(IEffect effect)
		{
			return this.Uid.CompareTo(effect.Uid);
		}

		#endregion

		#region Interface IEffect

		public virtual RetCode BuildPrintedFullDesc(StringBuilder buf)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (!string.IsNullOrWhiteSpace(Desc))
			{
				buf.AppendFormat("{0}{1}{0}", Environment.NewLine, Desc);
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Class Effect

		public Effect()
		{
			
		}

		#endregion

		#endregion
	}
}
