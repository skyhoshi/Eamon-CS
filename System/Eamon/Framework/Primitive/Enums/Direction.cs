
// Direction.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of (compass) Directions.
	/// </summary>
	/// <remarks>
	/// These represent the possible directional links between rooms in a game.  Each room contains an
	/// array that is indexed using these Direction values.  The array will always be created assuming
	/// a 10-direction game; for 6-direction games, the last four (4) elements will be unused.
	/// </remarks>
	public enum Direction : long
	{
		North = 1,
		South,
		East,
		West,
		Up,
		Down,
		Northeast,
		Northwest,
		Southeast,
		Southwest
	}
}
