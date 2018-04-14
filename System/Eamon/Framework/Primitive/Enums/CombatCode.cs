
// CombatCode.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of monster Combat Codes.
	/// </summary>
	/// <remarks>
	/// These represent the behavior of monsters while in combat.  Their effect on gameplay is intended to parallel
	/// the Combat Code setting found in Eamon Deluxe.
	/// </remarks>
	public enum CombatCode : long
	{
		/// <summary>
		/// The monster will never fight.
		/// </summary>
		NeverFights = -2,

		/// <summary>
		/// The monster will favor weapons but fall back to natural weapons if necessary.
		/// </summary>
		NaturalWeapons,

		/// <summary>
		/// The monster will use either weapons or natural weapons (but never both).
		/// </summary>
		Weapons,

		/// <summary>
		/// The monster will be described as "attacking"; otherwise mirrors the Weapons setting.
		/// </summary>
		Attacks
	}
}
