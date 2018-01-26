
// ArtifactClass.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class ArtifactClass : IArtifactClass
	{
		[ExcludeFromSerialization]
		public virtual IArtifact Parent { get; set; }

		public virtual Enums.ArtifactType Type { get; set; }

		public virtual long Field5 { get; set; }

		public virtual long Field6 { get; set; }

		public virtual long Field7 { get; set; }

		public virtual long Field8 { get; set; }

		public virtual bool IsOpenable()
		{
			return Type == Enums.ArtifactType.Container || Type == Enums.ArtifactType.Drinkable || Type == Enums.ArtifactType.Edible || Type == Enums.ArtifactType.Readable || Type == Enums.ArtifactType.DoorGate;
		}

		public virtual bool IsLockable()
		{
			return Type == Enums.ArtifactType.Container || Type == Enums.ArtifactType.DoorGate || Type == Enums.ArtifactType.BoundMonster;
		}

		public virtual bool IsBreakable()
		{
			return Type == Enums.ArtifactType.Container || Type == Enums.ArtifactType.DoorGate;
		}

		public virtual bool IsEffectExposer()
		{
			return Type == Enums.ArtifactType.Readable || Type == Enums.ArtifactType.DisguisedMonster;
		}

		public virtual bool IsMonsterExposer()
		{
			return Type == Enums.ArtifactType.BoundMonster || Type == Enums.ArtifactType.DisguisedMonster;
		}

		public virtual bool IsWeapon(Enums.Weapon weapon)
		{
			Debug.Assert(Enum.IsDefined(typeof(Enums.Weapon), weapon));

			return IsWeapon01() && Field6 == (long)weapon;
		}

		public virtual bool IsWeapon01()
		{
			return Type == Enums.ArtifactType.Weapon || Type == Enums.ArtifactType.MagicWeapon;
		}

		public virtual bool IsOpen()
		{
			var result = false;

			if (Type == Enums.ArtifactType.Container)
			{
				result = Field6 == 1;
			}
			else if (Type == Enums.ArtifactType.Drinkable || Type == Enums.ArtifactType.Edible || Type == Enums.ArtifactType.Readable)
			{
				result = Field7 == 1;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				result = Field7 == 0;
			}

			return result;
		}

		public virtual void SetOpen(bool open)
		{
			if (Type == Enums.ArtifactType.Container)
			{
				Field6 = open ? 1 : 0;
			}
			else if (Type == Enums.ArtifactType.Drinkable || Type == Enums.ArtifactType.Edible || Type == Enums.ArtifactType.Readable)
			{
				Field7 = open ? 1 : 0;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				Field7 = open ? 0 : 1;
			}
		}

		public virtual void SetKeyUid(long artifactUid)
		{
			Debug.Assert(artifactUid >= -2);         // -2=Broken

			if (Type == Enums.ArtifactType.Container)
			{
				Field5 = artifactUid;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				Field6 = artifactUid;
			}
			else if (Type == Enums.ArtifactType.BoundMonster)
			{
				if (artifactUid == -2)
				{
					artifactUid = 0;
				}

				Field6 = artifactUid;
			}
		}

		public virtual void SetBreakageStrength(long strength)
		{
			Debug.Assert(Globals.Engine.IsArtifactFieldStrength(strength));

			if (Type == Enums.ArtifactType.Container)
			{
				Field6 = strength;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				Field7 = strength;
			}
		}

		public virtual void SetFirstEffect(long effectUid)
		{
			Debug.Assert(effectUid > 0);

			if (Type == Enums.ArtifactType.Readable)
			{
				Field5 = effectUid;
			}
			else if (Type == Enums.ArtifactType.DisguisedMonster)
			{
				Field6 = effectUid;
			}
		}

		public virtual void SetNumEffects(long numEffects)
		{
			Debug.Assert(numEffects > 0);

			if (Type == Enums.ArtifactType.Readable)
			{
				Field6 = numEffects;
			}
			else if (Type == Enums.ArtifactType.DisguisedMonster)
			{
				Field7 = numEffects;
			}
		}

		public virtual void SetMonsterUid(long monsterUid)
		{
			Debug.Assert(monsterUid > 0);

			if (Type == Enums.ArtifactType.BoundMonster || Type == Enums.ArtifactType.DisguisedMonster)
			{
				Field5 = monsterUid;
			}
		}

		public virtual long GetKeyUid()
		{
			long result = 0;

			if (Type == Enums.ArtifactType.Container)
			{
				result = Field5;
			}
			else if (Type == Enums.ArtifactType.DoorGate || Type == Enums.ArtifactType.BoundMonster)
			{
				result = Field6;
			}

			return result;
		}

		public virtual long GetBreakageStrength()
		{
			var result = 0L;

			if (Type == Enums.ArtifactType.Container)
			{
				result = Globals.Engine.IsArtifactFieldStrength(Field6) ? Field6 : 0L;
			}
			else if (Type == Enums.ArtifactType.DoorGate)
			{
				result = Globals.Engine.IsArtifactFieldStrength(Field7) ? Field7 : 0L;
			}

			return result;
		}

		public virtual long GetFirstEffect()
		{
			var result = 0L;

			if (Type == Enums.ArtifactType.Readable)
			{
				result = Field5;
			}
			else if (Type == Enums.ArtifactType.DisguisedMonster)
			{
				result = Field6;
			}

			return result;
		}

		public virtual long GetNumEffects()
		{
			var result = 0L;

			if (Type == Enums.ArtifactType.Readable)
			{
				result = Field6;
			}
			else if (Type == Enums.ArtifactType.DisguisedMonster)
			{
				result = Field7;
			}

			return result;
		}

		public virtual long GetMonsterUid()
		{
			var result = 0L;

			if (Type == Enums.ArtifactType.BoundMonster || Type == Enums.ArtifactType.DisguisedMonster)
			{
				result = Field5;
			}

			return result;
		}

		public ArtifactClass()
		{
			Type = Enums.ArtifactType.None;
		}
	}
}
