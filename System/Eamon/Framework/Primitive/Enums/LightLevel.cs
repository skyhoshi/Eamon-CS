
// LightLevel.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of room Light Levels.
	/// </summary>
	/// <remarks>
	/// These represent the possible ambient light levels found in rooms in a game.  Each room has its
	/// own light level value which can be manipulated during gameplay if desired.
	/// </remarks>
	public enum LightLevel : long
	{
		/// <summary>
		/// Dark rooms are very restrictive on the kind of activities allowed.
		/// </summary>
		Dark = 0,

		/// <summary>
		/// Lit rooms allow the full range of commands.
		/// </summary>
		Light
	}
}
