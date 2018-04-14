
// Friendliness.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of monster friendliness values.  
	/// </summary>
	/// <remarks>
	/// Each monster in a game has a reaction to the player character, as defined by this enum.  This is pretty
	/// standard to Eamon in general and Eamon CS is no different.
	/// </remarks>
	public enum Friendliness : long
	{
		/// <summary>
		/// The monster is hostile to the player character and all Friend monsters.  The monster attacks on sight
		/// and flees the room or pursues based on courage.
		/// </summary>
		Enemy = 1,

		/// <summary>
		/// The monster ignores all other monsters and refuses to flee the room, or attack either Friend or Enemy.
		/// </summary>
		Neutral,

		/// <summary>
		/// The monster is friendly, follows the player character around and attacks Enemy monsters.
		/// </summary>
		Friend
	}
}
