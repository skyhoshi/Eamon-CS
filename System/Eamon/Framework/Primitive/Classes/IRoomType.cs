
// IRoomType.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface IRoomType
	{
		string Name { get; set; }

		string RoomDesc { get; set; }

		string ExitDesc { get; set; }

		string FleeDesc { get; set; }
	}
}
