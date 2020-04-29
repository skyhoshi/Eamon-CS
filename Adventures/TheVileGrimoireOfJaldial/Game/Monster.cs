
// Monster.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IMonster))]
	public class Monster : Eamon.Game.Monster, Framework.IMonster
	{
		public override long Agility
		{
			get
			{
				var result = base.Agility;

				// Beholder's clumsiness spell causes decreased Agility

				if (Globals.EnableGameOverrides && gGameState != null && gGameState.ClumsyTargets.ContainsKey(Uid))
				{
					var roundsList = gGameState.ClumsyTargets[Uid];

					Debug.Assert(roundsList != null && roundsList.Count > 0);

					result -= (roundsList.Count * 3);

					if (result < 1)			// TODO: use Agility.MinValue
					{
						result = 1;
					}
				}

				return result;
			}

			set
			{
				base.Agility = value;
			}
		}

		public virtual string AttackDesc { get; set; }

		public override bool CanMoveToRoom(bool fleeing)
		{
			// Parent griffin will never abandon the griffin cubs

			if (Uid == 40)
			{
				var smallGriffinMonster = Globals.MDB[41];

				Debug.Assert(smallGriffinMonster != null);

				return GetInRoomUid() != smallGriffinMonster.GetInRoomUid() ? base.CanMoveToRoom(fleeing) : false;
			}

			// Dark hood and griffin cubs will only flee, never follow

			else if (Uid == 21 || Uid == 41)
			{
				return fleeing ? base.CanMoveToRoom(fleeing) : false;
			}
			else
			{
				// Flora monsters and water weird can't flee or follow

				return Uid != 18 && Uid != 19 && Uid != 20 && Uid != 22 && Uid != 38 ? base.CanMoveToRoom(fleeing) : false;
			}
		}

		public override bool ShouldProcessInGameLoop()
		{
			// When a monster has initiative nobody else can react this round

			return (Globals.InitiativeMonsterUid == 0 || Uid == Globals.InitiativeMonsterUid) && base.ShouldProcessInGameLoop();
		}

		public override string[] GetWeaponAttackDescs(IArtifact artifact)
		{
			var attackDescs = base.GetWeaponAttackDescs(artifact);

			if (!string.IsNullOrWhiteSpace(AttackDesc))
			{
				attackDescs = new string[] { AttackDesc };
			}

			return attackDescs;
		}

		public override string[] GetNaturalAttackDescs()
		{
			var attackDescs = base.GetNaturalAttackDescs();

			if (!string.IsNullOrWhiteSpace(AttackDesc))
			{
				attackDescs = new string[] { AttackDesc };
			}

			return attackDescs;
		}

		public override string GetArmorDescString()
		{
			var armorDesc = base.GetArmorDescString();

			if (IsInRoomLit())
			{
				if (Uid == 1 || Uid == 15)
				{
					armorDesc = "its course fur";
				}
				else if (Uid == 2)
				{
					armorDesc = "his leather armor";
				}
				else if (Uid == 3)
				{
					armorDesc = "its brittle bones";
				}
				else if (Uid == 4 || Uid == 12)
				{
					armorDesc = "its rotting flesh";
				}
				else if (Uid == 5 || Uid == 6 || Uid == 7 || Uid == 8)
				{
					armorDesc = "its tough hide";
				}
				else if (Uid == 9 || Uid == 14 || Uid == 16 || Uid == 21 || Uid == 38)
				{
					armorDesc = "its transparent form";
				}
				else if (Uid == 10)
				{
					armorDesc = "its glowing forcefield-like aura";
				}
				else if (Uid == 11 || Uid == 36 || Uid == 37 || Uid == 39)
				{
					armorDesc = "its chitinous carapace";
				}
				else if (Uid == 13 || Uid == 25)
				{
					armorDesc = "its jelly-like form";
				}
				else if (Uid == 19 || Uid == 20 || Uid == 22)
				{
					armorDesc = "its plant-skin armor";
				}
				else if (Uid == 23 || Uid == 24)
				{
					armorDesc = "its armor-like plating";
				}
				else if (Uid == 26)
				{
					armorDesc = "its greasy skin";
				}
				else if (Uid == 31)
				{
					armorDesc = "the weapon itself";
				}
				else if (Uid == 32)
				{
					armorDesc = "its jade-stone skin";
				}
				else if (Uid == 40 || Uid == 41 || Uid == 43)
				{
					armorDesc = "its armor-like hide";
				}
				else if (Uid == 44)
				{
					armorDesc = "its smooth fur";
				}
				else if (Uid == 50)
				{
					armorDesc = "its fiery skin";
				}
			}

			return armorDesc;
		}

		public virtual void SetAttackModality()
		{
			var rl = gEngine.RollDice(1, 100, 0);

			switch (Uid)
			{
				case 1:

					AttackDesc = rl > 50 ? "claw{0} at" : "bite{0} at";

					break;

				case 3:

					AttackDesc = rl > 50 ? "claw{0} at" : "slashes at";

					break;

				case 4:

					AttackDesc = rl > 50 ? "grapples with" : "bludgeon{0}";

					break;

				case 6:

					if (rl > 50)
					{
						AttackDesc = "claw{0} at";

						NwDice = 1;

						NwSides = 3;
					}
					else
					{
						AttackDesc = "bite{0} at";

						NwDice = 1;

						NwSides = 6;
					}

					break;

				case 7:

					if (rl > 50)
					{
						AttackDesc = "claw{0} at";

						NwDice = 1;

						NwSides = 4;
					}
					else
					{
						AttackDesc = "bite{0} at";

						NwDice = 1;

						NwSides = 8;
					}

					break;

				case 8:

					if (rl > 70)
					{
						AttackDesc = "gore{0}";

						NwDice = 1;

						NwSides = 6;
					}
					else if (rl > 50)
					{
						AttackDesc = "bite{0} at";

						NwDice = 1;

						NwSides = 4;
					}
					else
					{
						AttackDesc = "claw{0} at";

						NwDice = 1;

						NwSides = 3;
					}

					break;

				case 10:

					AttackDesc = "zap{0}";

					break;

				case 12:

					AttackDesc = "bludgeon{0}";

					break;

				case 13:

					AttackDesc = "spit{0} nauseating gunk at";

					break;

				case 15:
				case 17:

					if (rl > 50)
					{
						AttackDesc = "claw{0} at";

						NwDice = 1;

						NwSides = 8;
					}
					else
					{
						AttackDesc = "bite{0} at";

						NwDice = 1;

						NwSides = 6;
					}

					break;

				case 19:

					if (rl > 60)
					{
						AttackDesc = "maul{0}";

						NwDice = 2;

						NwSides = 4;
					}
					else
					{
						AttackDesc = "grapples with";

						NwDice = 1;

						NwSides = 4;
					}

					break;

				case 20:

					if (gGameState.BloodnettleVictimUid != 0)
					{
						AttackDesc = "drain{0} blood from";

						NwDice = 1;

						NwSides = 8;
					}
					else
					{
						AttackDesc = "prick{0}";

						NwDice = 1;

						NwSides = 4;
					}

					break;

				case 22:

					AttackDesc = "whiplashes";

					break;

				case 23:

					if (rl > 60)
					{
						AttackDesc = "kick{0} at";

						NwDice = 2;

						NwSides = 3;
					}
					else
					{
						AttackDesc = "throw{0} a punch at";

						NwDice = 1;

						NwSides = 4;
					}

					break;

				case 24:

					AttackDesc = "bite{0} at";

					break;

				case 25:

					AttackDesc = "club{0} at";

					break;

				case 26:
				case 31:

					AttackDesc = rl > 50 ? "thrust{0} at" : "slashes at";

					break;

				case 32:

					AttackDesc = rl > 50 ? "bludgeon{0}" : "throw{0} a punch at";

					break;

				case 36:

					if (rl > 80 && gGameState.ClumsySpells < 4)
					{
						AttackDesc = "cast{0} a clumsiness spell on";

						NwDice = 0;

						NwSides = 0;

						gGameState.ClumsySpells++;
					}
					else if (rl > 50 && gGameState.FireBalls < 7)
					{
						AttackDesc = "cast{0} a fireball at";

						NwDice = 1;

						NwSides = 8;

						gGameState.FireBalls++;
					}
					else if (rl > 20 && gGameState.MysticMissiles < 5)
					{
						AttackDesc = "cast{0} a mystic missile at";

						NwDice = 2;

						NwSides = 7;

						gGameState.MysticMissiles++;
					}
					else
					{
						AttackDesc = "bite{0} at";

						NwDice = 1;

						NwSides = 5;
					}

					break;

				case 37:

					AttackDesc = "pinches";

					break;

				case 38:

					if (rl > 80)
					{
						AttackDesc = "envelop{0}";

						NwDice = 0;

						NwSides = 0;
					}
					else
					{
						AttackDesc = "bludgeon{0}";

						NwDice = 1;

						NwSides = 6;
					}

					break;

				case 39:

					if (rl > 60)
					{
						AttackDesc = "sting{0}";

						NwDice = 1;

						NwSides = 4;
					}
					else
					{
						AttackDesc = "pinches";

						NwDice = 1;

						NwSides = 10;
					}

					break;

				case 40:

					if (rl > 40)
					{
						AttackDesc = "bite{0} at";

						if (gGameState.GriffinAngered)
						{
							NwDice = 2;

							NwSides = 8;
						}
						else
						{
							NwDice = 1;

							NwSides = 8;
						}
					}
					else
					{
						AttackDesc = "claw{0} at";

						if (gGameState.GriffinAngered)
						{
							NwDice = 2;

							NwSides = 3;
						}
						else
						{
							NwDice = 1;

							NwSides = 4;
						}
					}

					break;

				case 41:

					if (rl > 50)
					{
						AttackDesc = "bite{0} at";

						NwDice = 2;

						NwSides = 3;
					}
					else
					{
						AttackDesc = "claw{0} at";

						NwDice = 1;

						NwSides = 2;
					}

					break;

				case 43:

					if (rl > 73 && gGameState.LightningBolts < 7)
					{
						AttackDesc = "cast{0} a lightning bolt at";

						NwDice = 3;

						NwSides = 4;

						gGameState.LightningBolts++;
					}
					else if (rl > 46 && gGameState.IceBolts < 7)
					{
						AttackDesc = "cast{0} an ice bolt at";

						NwDice = 3;

						NwSides = 5;

						gGameState.IceBolts++;
					}
					else if (rl > 19 && gGameState.MentalBlasts < 3)
					{
						AttackDesc = "mentally blast{0}";

						NwDice = 3;

						NwSides = 5;

						gGameState.MentalBlasts++;
					}
					else
					{
						AttackDesc = "attack{0}";

						NwDice = 1;

						NwSides = 7;
					}

					break;

				case 44:

					if (rl > 50)
					{
						AttackDesc = "bite{0} at";

						NwDice = 3;

						NwSides = 5;
					}
					else
					{
						AttackDesc = "claw{0} at";

						NwDice = 1;

						NwSides = 7;
					}

					break;

				case 9:
				case 14:
				case 16:
				case 50:

					AttackDesc = "touches";

					break;

				default:

					var wpnArtifact = Weapon > 0 ? gADB[Weapon] : null;

					if (wpnArtifact == null)
					{
						AttackDesc = "attack{0}";
					}
					else if (wpnArtifact.GeneralWeapon.Field2 == (long)Enums.Weapon.Axe)
					{
						AttackDesc = rl > 50 ? "hew{0} at" : "hack{0} at";
					}
					else if (wpnArtifact.GeneralWeapon.Field2 == (long)Enums.Weapon.Bow)
					{
						AttackDesc = "shoot{0} at";
					}
					else if (wpnArtifact.GeneralWeapon.Field2 == (long)Enums.Weapon.Club)
					{
						AttackDesc = rl > 66 ? "bashes" : rl > 33 ? "smashes" : "bludgeon{0}";
					}
					else if (wpnArtifact.GeneralWeapon.Field2 == (long)Enums.Weapon.Spear)
					{
						AttackDesc = "stab{0} at";
					}
					else
					{
						AttackDesc = rl > 50 ? "thrust{0} at" : "slashes at";
					}

					break;
			}
		}
	}
}
