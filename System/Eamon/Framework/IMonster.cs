
// IMonster.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Text;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	public interface IMonster : IGameBase, IComparable<IMonster>
	{
		#region Properties

		string StateDesc { get; set; }

		bool IsListed { get; set; }

		Enums.PluralType PluralType { get; set; }

		long Hardiness { get; set; }

		long Agility { get; set; }

		long GroupCount { get; set; }

		long Courage { get; set; }

		long Location { get; set; }

		Enums.CombatCode CombatCode { get; set; }

		long Armor { get; set; }

		long Weapon { get; set; }

		long NwDice { get; set; }

		long NwSides { get; set; }

		long DeadBody { get; set; }

		Enums.Friendliness Friendliness { get; set; }

		Enums.Gender Gender { get; set; }

		long InitGroupCount { get; set; }

		long OrigGroupCount { get; set; }

		Enums.Friendliness OrigFriendliness { get; set; }

		long DmgTaken { get; set; }

		long Field1 { get; set; }

		long Field2 { get; set; }

		#endregion

		#region Methods

		bool IsDead();

		bool IsCarryingWeapon();

		bool IsWeaponless(bool includeWeaponFumble);

		bool HasDeadBody();

		bool HasWornInventory();

		bool HasCarriedInventory();

		bool IsInRoom();

		bool IsInLimbo();

		bool IsInRoomUid(long roomUid);

		bool IsInRoom(IRoom room);

		bool CanMoveToRoom(bool fleeing);

		bool CanMoveToRoomUid(long roomUid, bool fleeing);

		bool CanMoveToRoom(IRoom room, bool fleeing);

		long GetCarryingWeaponUid();

		long GetDeadBodyUid();

		long GetInRoomUid();

		IRoom GetInRoom();

		void SetInRoomUid(long roomUid);

		void SetInLimbo();

		void SetInRoom(IRoom room);

		bool IsInRoomLit();

		T EvalFriendliness<T>(T enemyValue, T neutralValue, T friendValue);

		T EvalGender<T>(T maleValue, T femaleValue, T neutralValue);

		T EvalPlural<T>(T singularValue, T pluralValue);

		T EvalInRoomLightLevel<T>(T darkValue, T lightValue);

		void ResolveFriendlinessPct(long charisma);

		void ResolveFriendlinessPct(ICharacter character);

		void CalculateGiftFriendlinessPct(long value);

		bool IsCharacterMonster();

		long GetWeightCarryableGronds();

		IList<IArtifact> GetCarriedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		IList<IArtifact> GetWornList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		IList<IArtifact> GetContainedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		RetCode EnforceFullInventoryWeightLimits(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		RetCode GetFullInventoryWeight(ref long weight, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		void AddHealthStatus(StringBuilder buf, bool addNewLine = true);

		string GetAttackDescString(IArtifact artifact);

		string GetMissDescString(IArtifact artifact);

		string GetArmorDescString();

		#endregion
	}
}
