
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

		string StateDesc { get; set; }

		bool IsCharOwned { get; set; }

		bool IsPlural { get; set; }

		bool IsListed { get; set; }

		Enums.PluralType PluralType { get; set; }

		long Value { get; set; }

		long Weight { get; set; }

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
