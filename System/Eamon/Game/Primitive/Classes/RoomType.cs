
// RoomType.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class RoomType : IRoomType
	{
		public virtual string Name { get; set; }

		public virtual string RoomDesc { get; set; }

		public virtual string ExitDesc { get; set; }

		public virtual string FleeDesc { get; set; }
	}
}
