
// RoomType.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of Room Types.
	/// </summary>
	/// <remarks>
	/// These represent the possible types of rooms found in a game.  Each room has its own room type value
	/// which can be manipulated during gameplay if desired.
	/// </remarks>
	public enum RoomType : long
	{
		/// <summary>
		/// The room is considered indoors; the adjacent rooms list is prefixed with "Obvious exits".
		/// </summary>
		Indoors = 0,

		/// <summary>
		/// The room is considered outdoors; the adjacent rooms list is prefixed with "Obvious paths".
		/// </summary>
		Outdoors
	}
}
