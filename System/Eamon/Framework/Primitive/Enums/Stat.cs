
// Stat.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of character Stats.  
	/// </summary>
	/// <remarks>
	/// The last three should be familiar to anyone who has been involved with Eamon before; Intellect
	/// is unique to Eamon CS, and represents the character's mental capacity, wisdom and/or intelligence.
	/// </remarks>
	public enum Stat : long
	{
		Intellect = 1,
		Hardiness,
		Agility,
		Charisma
	}
}
