
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

		bool IsGold();

		bool IsTreasure();

		bool IsWeapon();

		bool IsWeapon01();

		bool IsMagicWeapon();

		bool IsContainer();

		bool IsLightSource();

		bool IsDrinkable();

		bool IsReadable();

		bool IsDoorGate();

		bool IsEdible();

		bool IsBoundMonster();

		bool IsWearable();

		bool IsArmor();

		bool IsShield();

		bool IsDisguisedMonster();

		bool IsDeadBody();

		bool IsUser1();

		bool IsUser2();

		bool IsUser3();

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
