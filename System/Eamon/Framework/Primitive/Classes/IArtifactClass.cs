
// IArtifactClass.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Primitive.Classes
{
	public interface IArtifactClass
	{
		IArtifact Parent { get; set; }

		Enums.ArtifactType Type { get; set; }

		long Field5 { get; set; }

		long Field6 { get; set; }

		long Field7 { get; set; }

		long Field8 { get; set; }

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
