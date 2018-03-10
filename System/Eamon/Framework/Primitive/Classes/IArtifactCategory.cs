
// IArtifactCategory.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface IArtifactCategory
	{
		IArtifact Parent { get; set; }

		Enums.ArtifactType Type { get; set; }

		long Field1 { get; set; }

		long Field2 { get; set; }

		long Field3 { get; set; }

		long Field4 { get; set; }

		bool IsOpenable();

		bool IsLockable();

		bool IsBreakable();

		bool IsEffectExposer();

		bool IsMonsterExposer();

		bool IsWeapon(Enums.Weapon weapon);

		bool IsWeapon01();

		bool IsOpen();

		void SetOpen(bool open);

		void SetKeyUid(long artifactUid);

		void SetBreakageStrength(long strength);

		void SetFirstEffect(long effectUid);

		void SetNumEffects(long numEffects);

		void SetMonsterUid(long monsterUid);

		long GetKeyUid();

		long GetBreakageStrength();

		long GetFirstEffect();

		long GetNumEffects();

		long GetMonsterUid();
	}
}
