
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Combat;
using EamonRT.Framework.States;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using RTEnums = EamonRT.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : ICombatSystem
	{
		protected long _odds = 0;

		protected long _rl = 0;

		protected long _d2 = 0;

		protected virtual RTEnums.CombatState CombatState { get; set; }

		protected virtual Classes.IArtifactCategory OfAc { get; set; }

		protected virtual Classes.IArtifactCategory DfAc { get; set; }

		protected virtual Classes.IArtifactCategory ArAc { get; set; }

		protected virtual IArtifact WpnArtifact { get; set; }

		protected virtual IArtifact OfWeapon { get; set; }

		protected virtual IArtifact DfWeapon { get; set; }

		protected virtual IArtifact DfArmor { get; set; }

		protected virtual Enums.Weapon OfWeaponType { get; set; }

		protected virtual Enums.Weapon DfWeaponType { get; set; }

		protected virtual string OfMonsterName { get; set; }

		protected virtual string DfMonsterName { get; set; }

		protected virtual string AttackDesc { get; set; }

		protected virtual string AttackDesc01 { get; set; }

		protected virtual string MissDesc { get; set; }

		protected virtual bool UseFractionalStrength { get; set; }

		protected virtual bool OmitBboaPadding { get; set; }

		protected virtual bool LightOut { get; set; }

		protected virtual long OfWeaponUid { get; set; }

		protected virtual long DfWeaponUid { get; set; }

		protected virtual long D { get; set; }

		protected virtual long S { get; set; }

		protected virtual long M { get; set; }

		protected virtual long A { get; set; }

		protected virtual long Af { get; set; }

		protected virtual double S2 { get; set; }

		public virtual Action<IState> SetNextStateFunc { get; set; }

		public virtual IMonster OfMonster { get; set; }

		public virtual IMonster DfMonster { get; set; }

		public virtual long MemberNumber { get; set; }

		public virtual long AttackNumber { get; set; }

		public virtual bool BlastSpell { get; set; }

		public virtual bool UseAttacks { get; set; }

		public virtual bool MaxDamage { get; set; }

		public virtual bool OmitArmor { get; set; }

		public virtual bool OmitSkillGains { get; set; }

		public virtual bool OmitMonsterStatus { get; set; }

		public virtual bool OmitFinalNewLine { get; set; }

		public virtual RTEnums.AttackResult FixedResult { get; set; }

		public virtual RTEnums.WeaponRevealType WeaponRevealType { get; set; }

		protected virtual void SetAttackDesc()
		{
			AttackDesc = "attack{0}";

			if (!UseAttacks)
			{
				if (OfMonster.IsCharacterMonster() || (OfMonster.IsInRoomLit() && OfMonster.CombatCode != Enums.CombatCode.Attacks))
				{
					AttackDesc = OfMonster.GetAttackDescString(OfWeapon);
				}
			}
		}

		protected virtual void PrintAttack()
		{
			SetAttackDesc();

			AttackDesc01 = string.Format(AttackDesc, OfMonster.IsCharacterMonster() ? "" : "s");

			OfMonsterName = OfMonster.IsCharacterMonster() ? "You" :
					OfMonster.EvalInRoomLightLevel(AttackNumber == 1 ? "An unseen offender" : "The unseen offender",
						OfMonster.InitGroupCount > 1 && AttackNumber == 1 ? OfMonster.GetDecoratedName02(true, true, false, true, Globals.Buf) : OfMonster.GetDecoratedName03(true, true, false, true, Globals.Buf));

			DfMonsterName = DfMonster.IsCharacterMonster() ? "you" :
					DfMonster.EvalInRoomLightLevel("an unseen defender",
					DfMonster.InitGroupCount > 1 ? DfMonster.GetDecoratedName02(false, true, false, true, Globals.Buf) : DfMonster.GetDecoratedName03(false, true, false, true, Globals.Buf));

			Globals.Out.Write("{0}{1} {2} {3}{4}.",
				Environment.NewLine,
				OfMonsterName,
				AttackDesc01,
				DfMonsterName,
					OfWeapon != null &&
					(WeaponRevealType == RTEnums.WeaponRevealType.Always ||
					(WeaponRevealType == RTEnums.WeaponRevealType.OnlyIfSeen && OfWeapon.Seen)) ?
						" with " + OfWeapon.GetDecoratedName02(false, true, false, false, Globals.Buf) :
						"");
		}

		protected virtual void PrintMiss()
		{
			MissDesc = DfMonster.GetMissDescString(DfWeapon);

			Globals.Out.Write("{0} --- {1}!", Environment.NewLine, MissDesc);
		}

		protected virtual void PrintFumble()
		{
			Globals.Out.Write("{0} ... A fumble!", Environment.NewLine);
		}

		protected virtual void PrintRecovered()
		{
			Globals.Out.Write("{0}  Recovered.", Environment.NewLine);
		}

		protected virtual void PrintWeaponDropped()
		{
			Globals.Out.Write("{0}  {1} {2} {3}!",
				Environment.NewLine,
				OfMonster.IsCharacterMonster() ? "You" :
				OfMonster.EvalInRoomLightLevel("The offender", OfMonster.GetDecoratedName03(true, true, false, true, Globals.Buf)),
				OfMonster.IsCharacterMonster() ? "drop" : "drops",
				OfMonster.IsCharacterMonster() || OfMonster.IsInRoomLit() ?
					(
						(WeaponRevealType == RTEnums.WeaponRevealType.Never || 
						(WeaponRevealType == RTEnums.WeaponRevealType.OnlyIfSeen && !OfWeapon.Seen)) ? 
							OfWeapon.GetDecoratedName02(false, true, false, false, Globals.Buf01) :
							OfWeapon.GetDecoratedName03(false, true, false, false, Globals.Buf01)
					) : 
					"a weapon");
		}

		protected virtual void PrintWeaponHitsUser()
		{
			Globals.Out.Write("{0}  Weapon hits user!", Environment.NewLine);
		}

		protected virtual void PrintSparksFly()
		{
			Globals.Out.Write("{0}  Sparks fly from {1}!",
				Environment.NewLine,
				OfMonster.IsCharacterMonster() || OfMonster.IsInRoomLit() ? 
					(
						(WeaponRevealType == RTEnums.WeaponRevealType.Never ||
						(WeaponRevealType == RTEnums.WeaponRevealType.OnlyIfSeen && !OfWeapon.Seen)) ?
							OfWeapon.GetDecoratedName02(false, true, false, false, Globals.Buf) :
							OfWeapon.GetDecoratedName03(false, true, false, false, Globals.Buf)
					) : 
					"a weapon");
		}

		protected virtual void PrintWeaponDamaged()
		{
			Globals.Out.Write("{0}  Weapon damaged!", Environment.NewLine);
		}

		protected virtual void PrintWeaponBroken()
		{
			Globals.Out.Write("{0}  Weapon broken!", Environment.NewLine);
		}

		protected virtual void PrintBrokenWeaponHitsUser()
		{
			Globals.Out.Write("{0}  Broken weapon hits user!", Environment.NewLine);
		}

		protected virtual void PrintStarPlus()
		{
			Globals.Out.Write("{0} {1} ", Environment.NewLine, DfMonster.IsCharacterMonster() ? "***" : "+++");
		}

		protected virtual void PrintHit()
		{
			Globals.Out.Write("A hit!");
		}

		protected virtual void PrintCriticalHit()
		{
			Globals.Out.Write("A critical hit!");
		}

		protected virtual void PrintBlowTurned()
		{
			if (DfMonster.Armor < 1)
			{
				Globals.Out.Write("{0}{1}Blow turned!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
			}
			else
			{
				var armorDesc = DfMonster.GetArmorDescString();

				Globals.Out.Write("{0}{1}Blow bounces off {2}!", Environment.NewLine, OmitBboaPadding ? "" : "  ", armorDesc);
			}
		}

		protected virtual void PrintHealthStatus()
		{
			DfMonsterName = DfMonster.IsCharacterMonster() ? "You" :
				BlastSpell && DfMonster.InitGroupCount > 1 ? DfMonster.EvalInRoomLightLevel(DfMonster == OfMonster ? "An offender" : "A defender", DfMonster.GetDecoratedName02(true, true, false, true, Globals.Buf)) :
				DfMonster.EvalInRoomLightLevel(DfMonster == OfMonster ? "The offender" : "The defender", DfMonster.GetDecoratedName03(true, true, false, true, Globals.Buf01));

			Globals.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				DfMonsterName,
				DfMonster.IsCharacterMonster() ? "are" : "is");

			DfMonster.AddHealthStatus(Globals.Buf, false);

			Globals.Out.Write("{0}", Globals.Buf);
		}

		protected virtual void PrintBlast()
		{
			Globals.Out.Print("{0}", Globals.Engine.GetBlastDesc());
		}

		protected virtual void RollToHitOrMiss()
		{
			if (FixedResult != RTEnums.AttackResult.None)
			{
				_odds = 50;

				switch (FixedResult)
				{
					case RTEnums.AttackResult.Fumble:

						_rl = 100;

						break;

					case RTEnums.AttackResult.Miss:

						_rl = _odds + 1;

						break;

					case RTEnums.AttackResult.Hit:

						_rl = _odds;

						break;

					case RTEnums.AttackResult.CriticalHit:

						_rl = 1;

						break;
				}
			}
			else
			{
				_rl = Globals.Engine.RollDice(1, 100, 0);
			}
		}

		protected virtual void BeginAttack()
		{
			Debug.Assert(OfMonster != null && OfMonster.CombatCode != Enums.CombatCode.NeverFights);

			Debug.Assert(DfMonster != null);

			Debug.Assert(MemberNumber > 0);

			OfWeaponUid = OfMonster.Weapon;

			if (OfWeaponUid > 0 && OfMonster.OrigGroupCount == 1 && AttackNumber > 1 && OfMonster.CanAttackWithMultipleWeapons())
			{
				var weaponList = OfMonster.GetCarriedList().Where(x => x.IsReadyableByMonster(OfMonster)).ToList();

				var weaponCount = weaponList.Count;

				if ((AttackNumber - 1) % weaponCount != 0)
				{
					OfWeapon = Globals.Engine.GetNthArtifact(weaponList, (AttackNumber - 1) % weaponCount, x => x.Uid != OfWeaponUid);

					OfWeaponUid = OfWeapon.Uid;
				}
				else
				{
					OfWeapon = Globals.ADB[OfWeaponUid];
				}
			}
			else if (OfWeaponUid > 0 && OfMonster.GroupCount > 1 && MemberNumber > 1)
			{
				OfWeapon = Globals.Engine.GetNthArtifact(OfMonster.GetCarriedList(), MemberNumber - 1, x => x.IsReadyableByMonster(OfMonster) && x.Uid != OfWeaponUid);

				OfWeaponUid = OfWeapon != null ? OfWeapon.Uid : OfMonster.NwDice > 0 && OfMonster.NwSides > 0 ? 0 : -1;

				if (OfWeaponUid < 0)
				{
					CombatState = RTEnums.CombatState.EndAttack;

					goto Cleanup;
				}
			}
			else
			{
				OfWeapon = OfWeaponUid > 0 ? Globals.ADB[OfWeaponUid] : null;
			}

			Debug.Assert(OfWeaponUid == 0 || (OfWeapon != null && OfWeapon.GeneralWeapon != null));

			OfAc = OfWeapon != null ? OfWeapon.GeneralWeapon : null;

			Af = Globals.Engine.GetArmorFactor(Globals.GameState.Ar, Globals.GameState.Sh);

			Globals.Engine.GetOddsToHit(OfMonster, DfMonster, OfAc, Af, ref _odds);

			RollToHitOrMiss();

			if (OfMonster.IsCharacterMonster() && _rl < 97 && (_rl < 5 || _rl <= _odds) && !OmitSkillGains)
			{
				Globals.Engine.CheckPlayerSkillGains(OfAc, Af);
			}

			OfWeaponType = (Enums.Weapon)(OfAc != null ? OfAc.Field2 : 0);

			PrintAttack();

			if (_rl < 97 && (_rl < 5 || _rl <= _odds))
			{
				CombatState = RTEnums.CombatState.AttackHit;
			}
			else
			{
				CombatState = RTEnums.CombatState.AttackMiss;
			}

		Cleanup:

			;
		}

		protected virtual void AttackMiss()
		{
			DfWeaponUid = DfMonster.Weapon;

			DfWeapon = DfWeaponUid > 0 ? Globals.ADB[DfWeaponUid] : null;

			DfAc = DfWeapon != null ? DfWeapon.GeneralWeapon : null;

			DfWeaponType = (Enums.Weapon)(DfAc != null ? DfAc.Field2 : 0);

			if (_rl < 97 || OfWeaponUid == 0)
			{
				PrintMiss();

				CombatState = RTEnums.CombatState.EndAttack;

				goto Cleanup;
			}

			CombatState = RTEnums.CombatState.AttackFumble;

		Cleanup:

			;
		}

		protected virtual void AttackFumble()
		{
			RetCode rc;

			PrintFumble();

			_rl = Globals.Engine.RollDice(1, 100, 0);

			if ((Globals.IsRulesetVersion(5) && _rl < 36) || (!Globals.IsRulesetVersion(5) && _rl < 41))
			{
				PrintRecovered();

				CombatState = RTEnums.CombatState.EndAttack;

				goto Cleanup;
			}

			if ((Globals.IsRulesetVersion(5) && _rl < 76) || (!Globals.IsRulesetVersion(5) && _rl < 81))
			{
				Globals.Engine.RemoveWeight(OfWeapon);

				if (Globals.GameState.Ls > 0 && Globals.GameState.Ls == OfWeaponUid)
				{
					LightOut = true;
				}

				OfWeapon.SetInRoom(OfMonster.GetInRoom());

				WpnArtifact = Globals.ADB[OfMonster.Weapon];

				Debug.Assert(WpnArtifact != null);

				rc = WpnArtifact.RemoveStateDesc(WpnArtifact.GetReadyWeaponDesc());

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				OfMonster.Weapon = !OfMonster.IsCharacterMonster() ? -OfWeaponUid - 1 : -1;

				PrintWeaponDropped();

				CombatState = RTEnums.CombatState.EndAttack;

				goto Cleanup;
			}

			if (_rl > 95)
			{
				PrintWeaponHitsUser();

				DfMonster = OfMonster;

				CombatState = RTEnums.CombatState.AttackHit;

				goto Cleanup;
			}

			if (OfAc.Type == Enums.ArtifactType.MagicWeapon)
			{
				PrintSparksFly();

				CombatState = RTEnums.CombatState.EndAttack;

				goto Cleanup;
			}

			if (_rl < 91)
			{
				OfAc.Field4--;

				if (OfAc.Field4 > 0)
				{
					PrintWeaponDamaged();

					CombatState = RTEnums.CombatState.EndAttack;

					goto Cleanup;
				}
			}

			PrintWeaponBroken();

			Globals.Engine.RemoveWeight(OfWeapon);

			if (Globals.GameState.Ls > 0 && Globals.GameState.Ls == OfWeaponUid)
			{
				LightOut = true;
			}

			OfWeapon.SetInLimbo();

			WpnArtifact = Globals.ADB[OfMonster.Weapon];

			Debug.Assert(WpnArtifact != null);

			rc = WpnArtifact.RemoveStateDesc(WpnArtifact.GetReadyWeaponDesc());

			Debug.Assert(Globals.Engine.IsSuccess(rc));

			OfMonster.Weapon = -1;

			_rl = Globals.Engine.RollDice(1, 100, 0);

			if (_rl > 50 || OfAc.Field4 <= 0)
			{
				CombatState = RTEnums.CombatState.EndAttack;

				goto Cleanup;
			}

			PrintBrokenWeaponHitsUser();

			DfMonster = OfMonster;

			_rl = Globals.Engine.RollDice(1, 5, 95);

			CombatState = RTEnums.CombatState.AttackHit;

		Cleanup:

			;
		}

		protected virtual void AttackHit()
		{
			if (OfAc != null)
			{
				D = OfAc.Field3;

				S = OfAc.Field4;
			}
			else
			{
				D = OfMonster.NwDice;

				S = OfMonster.NwSides;
			}

			A = OmitArmor ? 0 : 1;

			PrintStarPlus();

			if ((OfMonster != DfMonster && _rl >= 5) || (OfMonster == DfMonster && _rl < 100))
			{
				PrintHit();

				CombatState = RTEnums.CombatState.CalculateDamage;

				goto Cleanup;
			}

			PrintCriticalHit();

			if (OfMonster != DfMonster || !Globals.IsRulesetVersion(5))
			{
				_rl = Globals.Engine.RollDice(1, 100, 0);

				if (_rl == 100)
				{
					_d2 = DfMonster.Hardiness - DfMonster.DmgTaken - (Globals.IsRulesetVersion(5) ? 0 : 2);

					CombatState = RTEnums.CombatState.CheckArmor;

					goto Cleanup;
				}

				if (_rl < (Globals.IsRulesetVersion(5) ? 51 : 50))
				{
					A = 0;

					CombatState = RTEnums.CombatState.CalculateDamage;

					goto Cleanup;
				}

				if (_rl < 86 || !Globals.IsRulesetVersion(5))
				{
					S2 = S;

					S2 *= (1 + .5 + (_rl > 85 ? 1 : 0) + (_rl > 95 ? 1 : 0));

					S = (long)Math.Round(S2);
				}
				else
				{
					D *= (1 + (_rl > 85 ? 1 : 0) + (_rl > 95 ? 1 : 0));
				}
			}
			else
			{
				D *= 2;

				A = 0;
			}

			CombatState = RTEnums.CombatState.CalculateDamage;

		Cleanup:

			;
		}

		protected virtual void CalculateDamageForFractionalStrength()
		{
			Debug.Assert(OfMonster != null && UseFractionalStrength);

			_d2 = 0;

			var xx = (double)(OfMonster.Hardiness - OfMonster.DmgTaken) / (double)OfMonster.Hardiness;      // Fractional strength

			var yy = xx < .5 ? .5 + (xx * (.083 + (.833 * xx))) : (-.75) + (xx * (4.25 - (2.5 * xx)));

			if (yy > 1)
			{
				yy = 1;
			}

			for (var i = 0; i < D; i++)
			{
				_d2 += (long)Math.Round(yy * (MaxDamage ? S : Globals.Engine.RollDice(1, S, 0)));
			}

			_d2 += (long)Math.Round(yy * M);
		}

		protected virtual void CalculateDamage()
		{
			Debug.Assert(OfMonster != null || !UseFractionalStrength);

			Debug.Assert(DfMonster != null);

			Debug.Assert(D > 0 && S > 0 && A >= 0 && A <= 1);

			if (UseFractionalStrength)
			{
				CalculateDamageForFractionalStrength();
			}
			else
			{
				_d2 = MaxDamage ? (D * S) + M : Globals.Engine.RollDice(D, S, M);
			}

			_d2 -= (A * DfMonster.Armor);

			if (DfMonster.IsCharacterMonster())
			{
				DfArmor = Globals.GameState.Ar > 0 ? Globals.ADB[Globals.GameState.Ar] : null;

				ArAc = DfArmor != null ? DfArmor.Wearable : null;

				if (ArAc != null && (ArAc.Field1 / 2) > 2)
				{
					_d2 -= (A * 2);
				}
			}

			CombatState = RTEnums.CombatState.CheckArmor;
		}

		protected virtual void CheckArmor()
		{
			if (_d2 < 1)
			{
				PrintBlowTurned();

				CombatState = RTEnums.CombatState.EndAttack;

				goto Cleanup;
			}

			CombatState = RTEnums.CombatState.CheckMonsterStatus;

		Cleanup:

			;
		}

		protected virtual void CheckMonsterStatus()
		{
			Debug.Assert(DfMonster != null);

			Debug.Assert(_d2 >= 0);

			DfMonster.DmgTaken += _d2;

			if (Globals.IsRulesetVersion(5))
			{
				Globals.GameState.ModDTTL(DfMonster.Friendliness, _d2);
			}

			if (!OmitMonsterStatus || OfMonster == DfMonster)
			{
				PrintHealthStatus();
			}

			if (DfMonster.IsDead())
			{
				if (DfMonster.IsCharacterMonster())
				{
					Globals.GameState.Die = 1;

					if (SetNextStateFunc != null)
					{
						SetNextStateFunc(Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						}));
					}
				}
				else
				{
					Globals.Engine.MonsterDies(OfMonster, DfMonster);
				}
			}

			CombatState = RTEnums.CombatState.EndAttack;
		}

		protected virtual void ExecuteStateMachine()
		{
			Debug.Assert(CombatState == RTEnums.CombatState.BeginAttack || CombatState == RTEnums.CombatState.CalculateDamage || CombatState == RTEnums.CombatState.CheckMonsterStatus);

			while (true)
			{
				switch (CombatState)
				{
					case RTEnums.CombatState.BeginAttack:

						BeginAttack();

						break;

					case RTEnums.CombatState.AttackMiss:

						AttackMiss();

						break;

					case RTEnums.CombatState.AttackFumble:

						AttackFumble();

						break;

					case RTEnums.CombatState.AttackHit:

						AttackHit();

						break;

					case RTEnums.CombatState.CalculateDamage:

						CalculateDamage();

						break;

					case RTEnums.CombatState.CheckArmor:

						CheckArmor();

						break;

					case RTEnums.CombatState.CheckMonsterStatus:

						CheckMonsterStatus();

						break;

					case RTEnums.CombatState.EndAttack:

						goto Cleanup;

					default:

						Debug.Assert(false, "Invalid CombatState");

						break;
				}
			}

		Cleanup:

			if (!OmitFinalNewLine)
			{
				Globals.Out.WriteLine();
			}

			if (LightOut && OfWeapon != null)
			{
				Globals.Engine.LightOut(OfWeapon);
			}
		}

		public virtual void ExecuteCalculateDamage(long numDice, long numSides, long mod = 0)
		{
			CombatState = RTEnums.CombatState.CalculateDamage;

			D = numDice;

			S = numSides;

			M = mod;

			A = BlastSpell || OmitArmor ? 0 : 1;

			OmitBboaPadding = true;

			ExecuteStateMachine();
		}

		public virtual void ExecuteCheckMonsterStatus()
		{
			CombatState = RTEnums.CombatState.CheckMonsterStatus;

			_d2 = 0;

			ExecuteStateMachine();
		}

		public virtual void ExecuteAttack()
		{
			if (BlastSpell)
			{
				PrintBlast();

				if (Globals.IsRulesetVersion(5))
				{
					ExecuteCalculateDamage(1, 6);
				}
				else
				{
					ExecuteCalculateDamage(2, 5);
				}
			}
			else
			{
				CombatState = RTEnums.CombatState.BeginAttack;

				ExecuteStateMachine();
			}

			Globals.Thread.Sleep(Globals.GameState.PauseCombatMs);
		}

		public CombatSystem()
		{
			OfWeaponType = 0;

			DfWeaponType = 0;
		}
	}
}
