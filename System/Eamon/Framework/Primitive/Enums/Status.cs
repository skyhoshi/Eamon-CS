
// Status.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of (character) Statuses.
	/// </summary>
	/// <remarks>
	/// These represent the possible states that a player character can be in.  Each character has an
	/// associated Status that is set based on various game activities.
	/// </remarks>
	public enum Status : long
	{
		/// <summary>
		/// The state of the character is indeterminate (in practice, this Status is unused).
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// The character is available for use; this is the default Status.
		/// </summary>
		Alive,

		/// <summary>
		/// The character is not available for use; this is trivially reversible, however.
		/// </summary>
		Dead,

		/// <summary>
		/// The character is out on an adventure.
		/// </summary>
		Adventuring
	}
}
