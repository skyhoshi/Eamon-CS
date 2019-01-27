
// IArtifact.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface IArtifact : IGameBase, IComparable<IArtifact>
	{
		#region Properties

		/// <summary>
		/// A description shown after the artifact's name in various lists that indicates the state
		/// of the artifact.
		/// </summary>
		/// <remarks>
		/// This property is shown after the artifact's name in the various artifact lists and should
		/// describe the artifact's state.  See, for example, the deep red ruby in Test Adventure 
		/// "that glows with an inner fire".  This property can mutate during the adventure if the
		/// artifact's state changes.
		/// </remarks>
		string StateDesc { get; set; }

		/// <summary>
		/// A flag indicating whether the artifact is owned by the player character.
		/// </summary>
		/// <remarks>
		/// This property is used to indicate an artifact is owned by the player; it is usually set
		/// to true when the artifact is stored in the Character class, but this is not a requirement.
		/// The game sometimes uses "your" as the article for player owned artifacts.  When building
		/// a game, you can set this to true if you want an artifact to be player owned.
		/// </remarks>
		bool IsCharOwned { get; set; }

		/// <summary>
		/// A flag indicating whether the artifact represents a group of objects.
		/// </summary>
		/// <remarks>
		/// This property is false when an artifact is a singular object and true for a group of objects.
		/// It determines whether the game refers to the artifact as "it" or "them".
		/// </remarks>
		bool IsPlural { get; set; }

		/// <summary>
		/// A flag indicating whether the artifact should be displayed in various artifact lists.
		/// </summary>
		/// <remarks>
		/// This property is false when an artifact should be omitted from various artifact lists in the
		/// game; if true, the artifact will be included in those lists.  The vast majority of artifacts
		/// will have this set to true.  A common use of this flag is to permanently suppress the display
		/// of embedded artifacts that have been "discovered", but you can use it with regular artifacts
		/// as well.  Any artifact with this set to false should be referred to in some description to
		/// ensure the player knows of its existence (if desired).  See, for example, the boat in The
		/// Beginner's Cave.
		/// </remarks>
		bool IsListed { get; set; }

		/// <summary>
		/// A property indicating how the artifact's singular name is modified to produce its plural name.
		/// </summary>
		/// <remarks>
		/// This property is used to modify an artifact's singular name to produce its plural.  The name
		/// of an artifact should always be stored as singular, even when <see cref="IsPlural"/>) is true.
		/// You can also use a special code to indicate the plural name is stored as an effect record - use
		/// EamonDD to get a list of valid PluralType codes.
		/// </remarks>
		Enums.PluralType PluralType { get; set; }

		/// <summary>
		/// The base value of the artifact in gold pieces.
		/// </summary>
		/// <remarks>
		/// This property gives the value of the artifact in gold pieces.  It can be any number between and
		/// including <see cref="Eamon.Game.Plugin.PluginConstants.MinGoldValue"/> and
		/// <see cref="Eamon.Game.Plugin.PluginConstants.MaxGoldValue"/>.  When a Value is negative it typically
		/// indicates some unique circumstance - eg, a cursed item or when used by the game as a special code.
		/// Various other factors influence the actual amount of gold a character can get for the artifact, so
		/// this is considered a base value and is expected to conform to Eamon Deluxe standards.
		/// </remarks>
		long Value { get; set; }

		/// <summary>
		/// The weight of the artifact in Gronds.
		/// </summary>
		/// <remarks>
		/// This property gives the weight of the artifact in Gronds, a unit of measure unique to the Eamon
		/// gaming system.  There has never been a conversion between real-world weight systems and Gronds (that 
		/// I know of), so as such Weight is expected to conform to Eamon Deluxe standards.  The weight can be any
		/// value, with negative values reserved for special circumstances.  You can use EamonDD to get a list of
		/// the special built-in codes for Weight.
		/// </remarks>
		long Weight { get; set; }

		/// <summary>
		/// This property indicates where the artifact is located - the containing room/container, or the
		/// carrying/wearing monster.
		/// </summary>
		/// <remarks>
		/// This property gives the location of the artifact in the game.  The original Eamon called this Room #
		/// but it has been renamed to make it more general in Eamon CS.  There are a variety of special codes 
		/// associated with Location, so it is best to use EamonDD to get a list of valid Location values.
		/// </remarks>
		long Location { get; set; }

		/// <summary>
		/// A convenience property representing the artifact type.
		/// </summary>
		/// <remarks>
		/// This property is designed to mimic the behavior of Eamon Deluxe, allowing easy access to
		/// the artifact's type data field.  It uses the 0'th element of the <see cref="Categories"/>
		/// array, which is consistent with how older Eamon games are implemented.
		/// </remarks>
		Enums.ArtifactType Type { get; set; }

		/// <summary>
		/// A convenience property representing the artifact Field1 data field.
		/// </summary>
		/// <remarks>
		/// This property is designed to mimic the behavior of Eamon Deluxe, allowing easy access to
		/// the artifact's Field1 data field.  It uses the 0'th element of the <see cref="Categories"/>
		/// array, which is consistent with how older Eamon games are implemented.
		/// </remarks>
		long Field1 { get; set; }

		/// <summary>
		/// A convenience property representing the artifact Field2 data field.
		/// </summary>
		/// <remarks>
		/// This property is designed to mimic the behavior of Eamon Deluxe, allowing easy access to
		/// the artifact's Field2 data field.  It uses the 0'th element of the <see cref="Categories"/>
		/// array, which is consistent with how older Eamon games are implemented.
		/// </remarks>
		long Field2 { get; set; }

		/// <summary>
		/// A convenience property representing the artifact Field3 data field.
		/// </summary>
		/// <remarks>
		/// This property is designed to mimic the behavior of Eamon Deluxe, allowing easy access to
		/// the artifact's Field3 data field.  It uses the 0'th element of the <see cref="Categories"/>
		/// array, which is consistent with how older Eamon games are implemented.
		/// </remarks>
		long Field3 { get; set; }

		/// <summary>
		/// A convenience property representing the artifact Field4 data field.
		/// </summary>
		/// <remarks>
		/// This property is designed to mimic the behavior of Eamon Deluxe, allowing easy access to
		/// the artifact's Field4 data field.  It uses the 0'th element of the <see cref="Categories"/>
		/// array, which is consistent with how older Eamon games are implemented.
		/// </remarks>
		long Field4 { get; set; }

		/// <summary>
		/// A convenience property representing the artifact Field5 data field.
		/// </summary>
		/// <remarks>
		/// This property is designed to mimic the behavior of Eamon Deluxe, allowing easy access to
		/// the artifact's Field5 data field.  It uses the 0'th element of the <see cref="Categories"/>
		/// array, which is consistent with how older Eamon games are implemented.
		/// </remarks>
		long Field5 { get; set; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType Gold.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the Gold
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory Gold { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType Treasure.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the Treasure
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory Treasure { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType Weapon.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the Weapon
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory Weapon { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType MagicWeapon.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the MagicWeapon
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory MagicWeapon { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for either ArtifactType
		/// Weapon or MagicWeapon.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the Weapon
		/// or MagicWeapon ArtifactType (which ever exists will be returned).  It is fully compatible
		/// with the EDX convenience properties, but more intended for new games.  If the artifact
		/// has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory GeneralWeapon { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType Container.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the Container
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory Container { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType LightSource.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the LightSource
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory LightSource { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType Drinkable.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the Drinkable
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory Drinkable { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType Readable.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the Readable
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory Readable { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType DoorGate.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the DoorGate
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory DoorGate { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType Edible.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the Edible
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory Edible { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType BoundMonster.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the BoundMonster
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory BoundMonster { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType Wearable.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the Wearable
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory Wearable { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType DisguisedMonster.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the DisguisedMonster
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory DisguisedMonster { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType DeadBody.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the DeadBody
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory DeadBody { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType User1.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the User1
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory User1 { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType User2.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the User2
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory User2 { get; }

		/// <summary>
		/// A convenience property representing the ArtifactCategory data for ArtifactType User3.
		/// </summary>
		/// <remarks>
		/// This property can be used to obtain the ArtifactCategory data associated with the User3
		/// ArtifactType.  It is fully compatible with the EDX convenience properties, but more
		/// intended for new games.  If the artifact has no such data the property returns null.
		/// </remarks>
		Classes.IArtifactCategory User3 { get; }

		/// <summary>
		/// An array of ArtifactCategory objects that define the artifact's behavior in the game.
		/// </summary>
		/// <remarks>
		/// This property is a collection of ArtifactCategory objects.  An ArtifactCategory object
		/// stores data that makes the artifact behave a certain way in the game - it is the equivalent
		/// of Eamon Deluxe's Type/Field5-Field8 logic.  Unique to Eamon CS, however, is the ability to
		/// have multi-categoried artifacts that can behave like different artifact types depending on
		/// how you use them.  The Categories array length is always between 1 and 4, depending on the
		/// number of ArtifactCategories needed, and the elements will always contain consecutive valid
		/// references starting at index 0.  Older games ported from BASIC only need one ArtifactCategory
		/// but new games can vary (see, for example, the backpack in Test Adventure).  You can look at
		/// the text file Documentation\ARTIFACT_TYPES.txt to get an idea of how ECS handles artifact
		/// behavior.  This directly parallels the Eamon Deluxe documentation, which is also worth
		/// examining as it is a more comprehensive reference.
		/// </remarks>
		Classes.IArtifactCategory[] Categories { get; set; }

		#endregion

		#region Methods

		Classes.IArtifactCategory GetCategories(long index);

		string GetSynonyms(long index);

		void SetCategories(long index, Classes.IArtifactCategory value);

		void SetSynonyms(long index, string value);

		bool IsCarriedByCharacter();

		bool IsCarriedByMonster();

		bool IsCarriedByContainer();

		bool IsWornByCharacter();

		bool IsWornByMonster();

		bool IsReadyableByCharacter();

		bool IsInRoom();

		bool IsEmbeddedInRoom();

		bool IsInLimbo();

		bool IsCarriedByMonsterUid(long monsterUid);

		bool IsCarriedByContainerUid(long containerUid);

		bool IsWornByMonsterUid(long monsterUid);

		bool IsReadyableByMonsterUid(long monsterUid);

		bool IsInRoomUid(long roomUid);

		bool IsEmbeddedInRoomUid(long roomUid);

		bool IsCarriedByMonster(IMonster monster);

		bool IsCarriedByContainer(IArtifact container);

		bool IsWornByMonster(IMonster monster);

		bool IsReadyableByMonster(IMonster monster);

		bool IsInRoom(IRoom room);

		bool IsEmbeddedInRoom(IRoom room);

		long GetCarriedByMonsterUid();

		long GetCarriedByContainerUid();

		long GetWornByMonsterUid();

		long GetInRoomUid();

		long GetEmbeddedInRoomUid();

		IMonster GetCarriedByMonster();

		IArtifact GetCarriedByContainer();

		IMonster GetWornByMonster();

		IRoom GetInRoom();

		IRoom GetEmbeddedInRoom();

		void SetCarriedByCharacter();

		void SetCarriedByMonsterUid(long monsterUid);

		void SetCarriedByContainerUid(long containerUid);

		void SetWornByCharacter();

		void SetWornByMonsterUid(long monsterUid);

		void SetInRoomUid(long roomUid);

		void SetEmbeddedInRoomUid(long roomUid);

		void SetInLimbo();

		void SetCarriedByMonster(IMonster monster);

		void SetCarriedByContainer(IArtifact container);

		void SetWornByMonster(IMonster monster);

		void SetInRoom(IRoom room);

		void SetEmbeddedInRoom(IRoom room);

		bool IsInRoomLit();

		bool IsEmbeddedInRoomLit();

		bool IsFieldStrength(long value);

		long GetFieldStrength(long value);

		bool IsWeapon(Enums.Weapon weapon);

		bool IsAttackable();

		bool IsAttackable01(ref Classes.IArtifactCategory ac);

		bool IsUnmovable();

		bool IsUnmovable01();

		bool IsArmor();

		bool IsShield();

		bool ShouldShowContentsWhenExamined();

		string GetProvidingLightDesc();

		string GetReadyWeaponDesc();

		string GetBrokenDesc();

		string GetEmptyDesc();

		T EvalPlural<T>(T singularValue, T pluralValue);

		T EvalInRoomLightLevel<T>(T darkValue, T lightValue);

		T EvalEmbeddedInRoomLightLevel<T>(T darkValue, T lightValue);

		Classes.IArtifactCategory GetArtifactCategory(Enums.ArtifactType artifactType);

		Classes.IArtifactCategory GetArtifactCategory(Enums.ArtifactType[] artifactTypes, bool categoryArrayPrecedence = true);

		IList<Classes.IArtifactCategory> GetArtifactCategories(Enums.ArtifactType[] artifactTypes);

		RetCode SetArtifactCategoryCount(long count);

		RetCode SyncArtifactCategories(Classes.IArtifactCategory artifactCategory);

		RetCode SyncArtifactCategories();

		RetCode AddStateDesc(string stateDesc, bool dupAllowed = false);

		RetCode RemoveStateDesc(string stateDesc);

		IList<IArtifact> GetContainedList(Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		RetCode GetContainerInfo(ref long count, ref long weight, bool recurse = false);

		#endregion
	}
}
