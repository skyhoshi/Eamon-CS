
// ValidateArgs.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Args
{
	[ClassMappings]
	public class ValidateArgs : IValidateArgs
	{
		public virtual IField ErrorField { get; set; }

		public virtual string ErrorMessage { get; set; }

		public virtual StringBuilder Buf { get; set; }

		public virtual Type RecordType { get; set; }

		public virtual IGameBase EditRecord { get; set; }

		public virtual long NewRecordUid { get; set; }

		public virtual bool ShowDesc { get; set; }

		public virtual void Clear()
		{
			ErrorField = null;

			ErrorMessage = "";

			Buf.Clear();

			RecordType = null;

			EditRecord = null;

			NewRecordUid = 0;

			ShowDesc = false;
		}

		public ValidateArgs()
		{
			Buf = new StringBuilder(Constants.BufSize);

			Clear();
		}
	}
}
