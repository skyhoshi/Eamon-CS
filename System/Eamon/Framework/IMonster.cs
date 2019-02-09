
// IMonster.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IMonster : IGameBase, IComparable<IMonster>
	{
		#region Properties

		/// <summary></summary>
		string StateDesc { get; set; }

		/// <summary></summary>
		bool IsListed { get; set; }

		/// <summary></summary>
		Enums.PluralType PluralType { get; set; }

		/// <summary></summary>
		long Hardiness { get; set; }

		/// <summary></summary>
		long Agility { get; set; }

		/// <summary></summary>
		long GroupCount { get; set; }

		/// <summary></summary>
		long AttackCount { get; set; }

		/// <summary></summary>
		long Courage { get; set; }

		/// <summary></summary>
		long Location { get; set; }

		/// <summary></summary>
		Enums.CombatCode CombatCode { get; set; }

		/// <summary></summary>
		long Armor { get; set; }

		/// <summary></summary>
		long Weapon { get; set; }

		/// <summary></summary>
		long NwDice { get; set; }

		/// <summary></summary>
		long NwSides { get; set; }

		/// <summary></summary>
		long DeadBody { get; set; }

		/// <summary></summary>
		Enums.Friendliness Friendliness { get; set; }

		/// <summary></summary>
		Enums.Gender Gender { get; set; }

		/// <summary></summary>
		long InitGroupCount { get; set; }

		/// <summary></summary>
		long OrigGroupCount { get; set; }

		/// <summary></summary>
		Enums.Friendliness OrigFriendliness { get; set; }

		/// <summary></summary>
		long DmgTaken { get; set; }

		/// <summary></summary>
		long Field1 { get; set; }

		/// <summary></summary>
		long Field2 { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <returns></returns>
		bool IsDead();

		/// <summary></summary>
		/// <returns></returns>
		bool IsCarryingWeapon();

		/// <summary></summary>
		/// <param name="includeWeaponFumble"></param>
		/// <returns></returns>
		bool IsWeaponless(bool includeWeaponFumble);

		/// <summary></summary>
		/// <returns></returns>
		bool HasDeadBody();

		/// <summary></summary>
		/// <returns></returns>
		bool HasWornInventory();

		/// <summary></summary>
		/// <returns></returns>
		bool HasCarriedInventory();

		/// <summary></summary>
		/// <returns></returns>
		bool IsInRoom();

		/// <summary></summary>
		/// <returns></returns>
		bool IsInLimbo();

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		bool IsInRoomUid(long roomUid);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <returns></returns>
		bool IsInRoom(IRoom room);

		/// <summary></summary>
		/// <param name="fleeing"></param>
		/// <returns></returns>
		bool CanMoveToRoom(bool fleeing);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <param name="fleeing"></param>
		/// <returns></returns>
		bool CanMoveToRoomUid(long roomUid, bool fleeing);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="fleeing"></param>
		/// <returns></returns>
		bool CanMoveToRoom(IRoom room, bool fleeing);

		/// <summary></summary>
		/// <returns></returns>
		bool CanAttackWithMultipleWeapons();

		/// <summary></summary>
		/// <returns></returns>
		long GetCarryingWeaponUid();

		/// <summary></summary>
		/// <returns></returns>
		long GetDeadBodyUid();

		/// <summary></summary>
		/// <returns></returns>
		long GetInRoomUid();

		/// <summary></summary>
		/// <returns></returns>
		IRoom GetInRoom();

		/// <summary></summary>
		/// <param name="roomUid"></param>
		void SetInRoomUid(long roomUid);

		/// <summary></summary>
		void SetInLimbo();

		/// <summary></summary>
		/// <param name="room"></param>
		void SetInRoom(IRoom room);

		/// <summary></summary>
		/// <returns></returns>
		bool IsInRoomLit();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldShowContentsWhenExamined();

		/// <summary></summary>
		/// <param name="enemyValue"></param>
		/// <param name="neutralValue"></param>
		/// <param name="friendValue"></param>
		/// <returns></returns>
		T EvalFriendliness<T>(T enemyValue, T neutralValue, T friendValue);

		/// <summary></summary>
		/// <param name="maleValue"></param>
		/// <param name="femaleValue"></param>
		/// <param name="neutralValue"></param>
		/// <returns></returns>
		T EvalGender<T>(T maleValue, T femaleValue, T neutralValue);

		/// <summary></summary>
		/// <param name="singularValue"></param>
		/// <param name="pluralValue"></param>
		/// <returns></returns>
		T EvalPlural<T>(T singularValue, T pluralValue);

		/// <summary></summary>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalInRoomLightLevel<T>(T darkValue, T lightValue);

		/// <summary></summary>
		/// <param name="charisma"></param>
		void ResolveFriendlinessPct(long charisma);

		/// <summary></summary>
		/// <param name="character"></param>
		void ResolveFriendlinessPct(ICharacter character);

		/// <summary></summary>
		/// <param name="value"></param>
		void CalculateGiftFriendlinessPct(long value);

		/// <summary></summary>
		/// <returns></returns>
		bool IsCharacterMonster();

		/// <summary></summary>
		/// <returns></returns>
		long GetWeightCarryableGronds();

		/// <summary></summary>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetCarriedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetWornList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		IList<IArtifact> GetContainedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		RetCode EnforceFullInventoryWeightLimits(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="weight"></param>
		/// <param name="monsterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		RetCode GetFullInventoryWeight(ref long weight, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="addNewLine"></param>
		void AddHealthStatus(StringBuilder buf, bool addNewLine = true);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		string GetAttackDescString(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		string GetMissDescString(IArtifact artifact);

		/// <summary></summary>
		/// <returns></returns>
		string GetArmorDescString();

		#endregion
	}
}
