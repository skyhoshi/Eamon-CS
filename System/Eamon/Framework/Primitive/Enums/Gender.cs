
// Gender.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of character/monster genders.
	/// </summary>
	/// <remarks>
	/// Each character or monster in a game is assigned a gender value.  For player characters this
	/// is of course set during creation in the Main Hall.  As with all other properties, the gender
	/// value can be manipulated during gameplay (and, in at least one ECS adventure, actually is!)
	/// </remarks>
	public enum Gender : long
	{
		/// <summary>
		/// The character or monster is male.
		/// </summary>
		Male = 0,

		/// <summary>
		/// The character or monster is female.
		/// </summary>
		Female,

		/// <summary>
		/// The monster is neutral/indeterminate.  Not available to player characters.
		/// </summary>
		Neutral
	}
}
