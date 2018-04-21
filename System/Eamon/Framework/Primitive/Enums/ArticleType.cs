
// ArticleType.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of Article Types.
	/// </summary>
	/// <remarks>
	/// These article types are applied to artifacts and monsters and improve the aesthetics of the
	/// output, making it more natural to read.  You can modify any record at runtime to change its
	/// article type, if desired.
	/// </remarks>
	/// <seealso cref="Enums.PluralType"/>
	public enum ArticleType : long
	{
		/// <summary>
		/// No article is ever applied.  For example, "Trollsfire" or "Heinrich".
		/// </summary>
		None = 0,

		/// <summary>
		/// The name is preceded by "a".  For example, "a flashlight" or "a wolverine".
		/// </summary>
		A,

		/// <summary>
		/// The name is preceded by "an".  For example, "an axe" or "an orc".
		/// </summary>
		An,

		/// <summary>
		/// The name is preceded by "some".  For example, "some leather armor", "some silver cups" or "some green slime".
		/// </summary>
		/// <remarks>
		/// For artifacts, it is important to distinguish between singular artifacts (like the leather armor)
		/// and plural artifacts (the silver cups).  Plural artifacts should be named as singular; this article
		/// type combined with the right plural type will produce the correct name.  The situation for monsters
		/// is similar; it is important to distinguish between singular monsters (like the green slime) and group
		/// monsters.  Group monsters have their own plural syntax (eg, "seven kobolds").  Group monsters should
		/// be named as singular, but with a singular article type (eg, "a kobold"), and the right plural type.
		/// </remarks>
		Some,

		/// <summary>
		/// The name is preceded by "the".  For example, "the Rings of Xylo" or "the Emerald Warrior".
		/// </summary>
		/// <remarks>
		/// Typically only applied to unique artifacts or monsters, or those with special importance in the game.
		/// </remarks>
		The
	}
}
