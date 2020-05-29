﻿
// CombatSystem.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Combat;
using Enums = Eamon.Framework.Primitive.Enums;
using RTEnums = EamonRT.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Combat
{
	[ClassMappings(typeof(ICombatSystem))]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, Framework.Combat.ICombatSystem
	{
		public virtual bool CrossbowTrap { get; set; }

		protected virtual bool ScoredCriticalHit { get; set; }

		protected override void PrintMiss()
		{
			MissDesc = null;

			var rl = gEngine.RollDice(1, 100, 0);

			// Beholder

			if (OfMonster?.Uid == 36)
			{
				var beholderMonster = OfMonster as Framework.IMonster;

				Debug.Assert(beholderMonster != null);

				switch (beholderMonster.AttackDesc)
				{
					case "cast{0} a clumsiness spell on":

						MissDesc = "Ineffective";

						break;

					case "cast{0} a fireball at":

						MissDesc = rl > 50 ? "Dodged" : "Missed";

						break;

					case "cast{0} a mystic missile at":

						MissDesc = "Missed";

						break;
				}
			}

			// Jaldi'al the lich

			else if (OfMonster?.Uid == 43)
			{
				var jaldialMonster = OfMonster as Framework.IMonster;

				Debug.Assert(jaldialMonster != null);

				switch (jaldialMonster.AttackDesc)
				{
					case "cast{0} a lightning bolt at":

						MissDesc = "Missed";

						break;

					case "cast{0} an ice bolt at":

						MissDesc = rl > 50 ? "Dodged" : "Missed";

						break;

					case "mentally blast{0}":

						MissDesc = "Ineffective";

						break;
				}
			}

			if (!string.IsNullOrWhiteSpace(MissDesc))
			{
				gOut.Write("{0} --- {1}!", Environment.NewLine, MissDesc);
			}
			else
			{
				base.PrintMiss();
			}
		}

		protected override void PrintCriticalHit()
		{
			ScoredCriticalHit = true;

			base.PrintCriticalHit();
		}

		protected override void RollToHitOrMiss()
		{
			ScoredCriticalHit = false;

			base.RollToHitOrMiss();

			// Bloodnettle always hits when draining blood (and ignores armor)

			if (OfMonster?.Uid == 20 && DfMonster.Uid == gGameState.BloodnettleVictimUid)
			{
				if (_rl > _odds)
				{
					_rl = _odds;
				}

				OmitArmor = true;
			}
		}

		protected override void CalculateDamage()
		{
			var beholderMonster = gMDB[36] as Framework.IMonster;

			Debug.Assert(beholderMonster != null);

			var waterWeirdMonster = gMDB[38] as Framework.IMonster;

			Debug.Assert(waterWeirdMonster != null);

			// Bypass damage calculation for beholder clumsiness spell and water weird envelopment

			if ((OfMonster?.Uid == 36 && string.Equals(beholderMonster.AttackDesc, "cast{0} a clumsiness spell on", StringComparison.OrdinalIgnoreCase)) || (OfMonster?.Uid == 38 && string.Equals(waterWeirdMonster.AttackDesc, "envelop{0}", StringComparison.OrdinalIgnoreCase)))
			{
				CombatState = RTEnums.CombatState.CheckMonsterStatus;
			}
			else
			{
				base.CalculateDamage();
			}
		}

		protected override void CheckArmor()
		{
			var beholderMonster = gMDB[36] as Framework.IMonster;

			Debug.Assert(beholderMonster != null);

			var waterWeirdMonster = gMDB[38] as Framework.IMonster;

			Debug.Assert(waterWeirdMonster != null);

			// Bypass armor check for beholder clumsiness spell and water weird envelopment

			if ((OfMonster?.Uid == 36 && string.Equals(beholderMonster.AttackDesc, "cast{0} a clumsiness spell on", StringComparison.OrdinalIgnoreCase)) || (OfMonster?.Uid == 38 && string.Equals(waterWeirdMonster.AttackDesc, "envelop{0}", StringComparison.OrdinalIgnoreCase)))
			{
				CombatState = RTEnums.CombatState.CheckMonsterStatus;
			}
			else
			{
				var room = gRDB[gGameState.Ro];

				Debug.Assert(room != null);

				var artTypes = new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon };

				var immuneMonsterUids = new long[] { 8, 9, 14, 15, 16, 17 };

				var ac = OfWeapon != null ? OfWeapon.GetArtifactCategory(artTypes) : null;

				// Apply special defenses

				if (OfMonster?.Uid != 50 && !BlastSpell)
				{
					// Some monsters are immune to non-magical weapons

					if (immuneMonsterUids.Contains(DfMonster.Uid))
					{
						if (ac == null || ac.Field1 < 20)
						{
							if (DfMonster.IsInRoom(room))
							{
								gOut.Write("{0}{1}Weapon doesn't seem to affect {2}!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.IsLit() ? DfMonster.GetTheName() : "the defender");
							}

							CombatState = RTEnums.CombatState.EndAttack;

							goto Cleanup;
						}
					}

					// Skeleton and crimson amoeba are resistant to non-club weapons (half damage)

					else if (DfMonster.Uid == 3 || DfMonster.Uid == 25)
					{
						if (ac == null || ac.Field2 != (long)Enums.Weapon.Club)
						{
							_d2 = (long)Math.Round((double)_d2 / 2.0);
						}
					}

					// Water weird is extremely resistant to non-club weapons (minimum damage)

					else if (DfMonster.Uid == 38)
					{
						if (ac == null || ac.Field2 != (long)Enums.Weapon.Club)
						{
							if (_d2 > 1)
							{
								_d2 = 1;
							}
						}
					}
				}

				// Bloodnettle always injures when draining blood

				if (OfMonster?.Uid == 20 && DfMonster.Uid == gGameState.BloodnettleVictimUid && _d2 < 1)
				{
					_d2 = 1;
				}

				base.CheckArmor();

			Cleanup:

				;
			}
		}

		protected override void CheckMonsterStatus()
		{
			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			var rl = gEngine.RollDice(1, 100, 0);

			// Apply special attacks

			if (OfMonster?.Uid == 9)
			{
				if (DfMonster.Uid > 17 && rl > 50)
				{
					if (DfMonster.IsCharacterMonster() || room.IsLit())
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, OmitBboaPadding ? "" : "  ", DfMonster.IsCharacterMonster() ? "You suddenly feel weaker!" : DfMonster.GetTheName(true) + " suddenly looks weaker!");
					}

					var stat = gEngine.GetStats(Stat.Hardiness);

					Debug.Assert(stat != null);

					DfMonster.Hardiness--;

					if (DfMonster.Hardiness < stat.MinValue)
					{
						DfMonster.Hardiness = stat.MinValue;
					}
					else if (DfMonster.IsCharacterMonster())
					{
						gGameState.PlayerHardinessPointsDrained++;
					}

					if (DfMonster.DmgTaken > DfMonster.Hardiness)
					{
						DfMonster.DmgTaken = DfMonster.Hardiness;
					}
				}
			}
			else if (OfMonster?.Uid == 11)
			{
				var carrionCrawlerMonster = OfMonster as Framework.IMonster;

				Debug.Assert(carrionCrawlerMonster != null);

				if (DfMonster.Uid != 50 && string.Equals(carrionCrawlerMonster.AttackDesc, "flail{0} at", StringComparison.OrdinalIgnoreCase) && rl > 50)
				{
					var saved = DfMonster.IsCharacterMonster() ? gEngine.SaveThrow(Stat.Hardiness) : rl > 80;

					if (!saved)
					{
						var rl02 = gEngine.RollDice(2, 2, 1);

						if (ScoredCriticalHit)
						{
							rl02 *= 2;
						}

						var firstParalyzed = !gGameState.ParalyzedTargets.ContainsKey(DfMonster.Uid);

						if (firstParalyzed)
						{
							gGameState.ParalyzedTargets[DfMonster.Uid] = 0;
						}

						if (DfMonster.IsCharacterMonster())
						{
							gOut.Write("{0}{1}You are{2} paralyzed!", Environment.NewLine, OmitBboaPadding ? "" : "  ", !firstParalyzed ? " more" : "");
						}
						else if (room.IsLit())
						{
							gOut.Write("{0}{1}{2} is{3} paralyzed!", Environment.NewLine, OmitBboaPadding ? "" : "  ", DfMonster.GetTheName(true), !firstParalyzed ? " more" : "");
						}
						else if (firstParalyzed)
						{
							gOut.Write("{0}{1}The defender falls silent!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
						}

						gGameState.ParalyzedTargets[DfMonster.Uid] += rl02;
					}
				}
			}
			else if (OfMonster?.Uid == 14 || OfMonster?.Uid == 16)
			{
				if (DfMonster.Uid > 17 && rl > 60)
				{
					if (DfMonster.IsCharacterMonster() || room.IsLit())
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, OmitBboaPadding ? "" : "  ", DfMonster.IsCharacterMonster() ? "You suddenly feel less skillful!" : DfMonster.GetTheName(true) + " suddenly looks less skillful!");
					}

					// Only apply skill loss to the player character

					if (DfMonster.IsCharacterMonster())
					{
						var weaponValues = EnumUtil.GetValues<Weapon>();

						foreach (var wv in weaponValues)
						{
							var weapon = gEngine.GetWeapons(wv);

							Debug.Assert(weapon != null);

							gCharacter.ModWeaponAbilities(wv, -gEngine.RollDice(1, OfMonster?.Uid == 14 ? 4 : 2, 0));

							if (gCharacter.GetWeaponAbilities(wv) < weapon.MinValue)
							{
								gCharacter.SetWeaponAbilities(wv, weapon.MinValue);
							}
						}
					}
				}
			}
			else if (OfMonster?.Uid == 36)
			{
				var beholderMonster = OfMonster as Framework.IMonster;

				Debug.Assert(beholderMonster != null);

				if (string.Equals(beholderMonster.AttackDesc, "cast{0} a clumsiness spell on", StringComparison.OrdinalIgnoreCase))
				{
					var saved = DfMonster.IsCharacterMonster() ? gEngine.SaveThrow(Stat.Intellect) : rl > 50;

					if (!saved)
					{
						var rl02 = gEngine.RollDice(1, 5, 2);

						if (ScoredCriticalHit)
						{
							rl02 *= 2;
						}

						IList<long> roundsList = null;

						if (gGameState.ClumsyTargets.ContainsKey(DfMonster.Uid))
						{
							roundsList = gGameState.ClumsyTargets[DfMonster.Uid];
						}
						else
						{
							roundsList = new List<long>();

							gGameState.ClumsyTargets[DfMonster.Uid] = roundsList;
						}

						roundsList.Add(rl02);

						if (DfMonster.IsCharacterMonster())
						{
							gOut.Write("{0}{1}You suddenly feel {2}less agile!", Environment.NewLine, OmitBboaPadding ? "" : "  ", ScoredCriticalHit ? "much " : "");
						}
						else
						{
							gOut.Write("{0}{1}{2} suddenly {3} {4}less agile!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("The defender", DfMonster.GetTheName(true)), room.EvalLightLevel("sounds", "looks"), ScoredCriticalHit ? "much " : "");
						}
					}
					else
					{
						if (DfMonster.IsCharacterMonster() || room.IsLit())
						{
							gOut.Write("{0}{1}Spell resisted!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
						}
					}

					CombatState = RTEnums.CombatState.EndAttack;

					goto Cleanup;
				}
			}
			else if (OfMonster?.Uid == 38)
			{
				var waterWeirdMonster = OfMonster as Framework.IMonster;

				Debug.Assert(waterWeirdMonster != null);

				if (string.Equals(waterWeirdMonster.AttackDesc, "envelop{0}", StringComparison.OrdinalIgnoreCase))
				{
					var saved = DfMonster.IsCharacterMonster() ? gEngine.SaveThrow(Stat.Hardiness) : rl > 40;

					if (!saved)
					{
						if (DfMonster.IsCharacterMonster())
						{
							gOut.Write("{0}{1}You struggle, but {2} holds you down, drowning you!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("the offender", OfMonster?.GetTheName()));
						}
						else
						{
							gOut.Write("{0}{1}{2} is enveloped by {3}, and drowns immediately!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("The defender", DfMonster.GetTheName(true)), room.EvalLightLevel("the offender", OfMonster?.GetTheName()));
						}

						DfMonster.DmgTaken = DfMonster.Hardiness;
					}
					else
					{
						if (DfMonster.IsCharacterMonster())
						{
							gOut.Write("{0}{1}You manage to break free!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
						}
						else
						{
							gOut.Write("{0}{1}{2} manages to break free of {3}'s grip!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("The defender", DfMonster.GetTheName(true)), room.EvalLightLevel("the offender", OfMonster?.GetTheName()));
						}

						CombatState = RTEnums.CombatState.EndAttack;

						goto Cleanup;
					}
				}
			}

			if (_d2 > 0 && gGameState.ShowCombatDamage && room.IsLit())
			{
				gOut.Write("{0}{1}Blow does {2} point{3} of damage.{4}", Environment.NewLine, OmitBboaPadding ? "" : "  ", _d2, _d2 != 1 ? "s" : "", BlastSpell || CrossbowTrap ? Environment.NewLine : "");
			}

			base.CheckMonsterStatus();

			// Bloodnettle selects its next victim

			if (OfMonster?.Uid == 20 && !DfMonster.IsInLimbo() && gGameState.BloodnettleVictimUid == 0)
			{
				gGameState.BloodnettleVictimUid = DfMonster.Uid;
			}

		Cleanup:

			;
		}

		public override void ExecuteAttack()
		{
			var griffinMonster = Globals.MDB[40];

			Debug.Assert(griffinMonster != null);

			// Attacking baby griffins makes the parent angry

			if (DfMonster.Uid == 41 && !griffinMonster.IsInLimbo() && !gGameState.GriffinAngered)
			{
				if (griffinMonster.IsInRoomUid(gGameState.Ro) && griffinMonster.GetInRoom().IsLit())
				{
					gEngine.PrintEffectDesc(82);
				}

				gGameState.GriffinAngered = true;
			}

			base.ExecuteAttack();
		}
	}
}
