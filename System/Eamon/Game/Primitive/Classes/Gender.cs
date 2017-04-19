
// Gender.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class Gender : IGender
	{
		public virtual string Name { get; set; }

		public virtual string MightyDesc { get; set; }

		public virtual string BoyDesc { get; set; }

		public virtual string HimDesc { get; set; }

		public virtual string HisDesc { get; set; }

		public virtual string HeDesc { get; set; }
	}
}
