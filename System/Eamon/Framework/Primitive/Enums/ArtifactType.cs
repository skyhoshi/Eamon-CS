
// ArtifactType.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of Artifact Types.
	/// </summary>
	/// <remarks>
	/// These artifact types parallel those found in Eamon Deluxe, but Eamon CS supports a multiple
	/// artifact type paradigm.  You can take a look through the documentation for EDX or wait for
	/// the Eamon CS Dungeon Designer's Manual, which will have more details.
	/// </remarks>
	public enum ArtifactType : long
	{
		None = -1,
		Gold,
		Treasure,
		Weapon,
		MagicWeapon,
		Container,
		LightSource,
		Drinkable,
		Readable,
		DoorGate,
		Edible,
		BoundMonster,
		Wearable,
		DisguisedMonster,
		DeadBody,
		User1,
		User2,
		User3
	}
}
