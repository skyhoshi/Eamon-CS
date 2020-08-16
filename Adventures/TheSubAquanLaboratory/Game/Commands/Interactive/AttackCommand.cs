
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public virtual IArtifactCategory DobjArtZeroAc { get; set; }

		public virtual long WallDamage { get; set; }

		public override void PlayerExecute()
		{
			var effectUid = 0L;
			var n = 0L;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if ((BlastSpell || ActorMonster.Weapon > 0) && DobjArtifact != null)
			{
				switch (DobjArtifact.Uid)
				{
					case 83:

						// Fake-looking back wall

						ProcessWallAttack();

						if (WallDamage > 17)
						{
							WallDamage = 17;
						}

						DobjArtZeroAc.Field4 -= WallDamage;

						if (DobjArtZeroAc.Field4 < 0)
						{
							DobjArtZeroAc.Field4 = 0;
						}

						n = (long)Math.Round((double)DobjArtZeroAc.Field4 / 16);

						if (n > 5)
						{
							n = 5;
						}

						effectUid = 63 - n * (n > 0 ? 1 : 0);

						gEngine.PrintEffectDesc(effectUid);

						// First attack

						if (!gGameState.FloorAttack)
						{
							gEngine.PrintEffectDesc(64);

							var electrifiedFloorArtifact = gADB[85];

							Debug.Assert(electrifiedFloorArtifact != null);

							electrifiedFloorArtifact.SetInRoom(ActorRoom);

							gGameState.FloorAttack = true;
						}

						// Broken!

						if (effectUid == 63)
						{
							DobjArtifact.SetInLimbo();

							var engravingArtifact = gADB[2];

							Debug.Assert(engravingArtifact != null);

							engravingArtifact.SetInLimbo();

							var rubbleArtifact = gADB[106];

							Debug.Assert(rubbleArtifact != null);

							rubbleArtifact.SetInRoom(ActorRoom);
						}

						NextState = Globals.CreateInstance<IMonsterStartState>();

						break;

					case 84:

						// Glass walls

						ProcessWallAttack();

						if (WallDamage > 9)
						{
							WallDamage = 9;
						}

						DobjArtZeroAc.Field4 -= WallDamage;

						if (DobjArtZeroAc.Field4 < 0)
						{
							DobjArtZeroAc.Field4 = 0;
						}

						n = (long)Math.Round((double)DobjArtZeroAc.Field4 / 12);

						if (n > 3)
						{
							n = 3;
						}

						effectUid = 69 - n * (n > 0 ? 1 : 0);

						gEngine.PrintEffectDesc(effectUid);

						// Broken!

						if (effectUid == 69)
						{
							DobjArtifact.SetInLimbo();

							var ovalDoorArtifact = gADB[16];

							Debug.Assert(ovalDoorArtifact != null);

							ovalDoorArtifact.SetInLimbo();

							var shatteredGlassWallsArtifact = gADB[105];

							Debug.Assert(shatteredGlassWallsArtifact != null);

							shatteredGlassWallsArtifact.SetInRoom(ActorRoom);

							gGameState.Sterilize = false;

							gEngine.PrintEffectDesc(70);

							gOut.Print("Enemies storm into the room!");

							var monsterList = gEngine.GetMonsterList(m => m.Uid >= 20 && m.Uid <= 22);

							foreach (var monster in monsterList)
							{
								monster.SetInRoom(ActorRoom);
							}

							NextState = Globals.CreateInstance<IStartState>();
						}
						else
						{
							NextState = Globals.CreateInstance<IMonsterStartState>();
						}

						break;

					case 85:

						// Electrified floor

						gEngine.PrintEffectDesc(65);

						DobjArtifact.SetInLimbo();

						var brokenFloorTrapArtifact = gADB[107];

						Debug.Assert(brokenFloorTrapArtifact != null);

						brokenFloorTrapArtifact.SetInRoom(ActorRoom);

						NextState = Globals.CreateInstance<IMonsterStartState>();

						break;

					case 89:

						// Dismantled worker android

						base.PlayerExecute();

						var plasticCardArtifact = gADB[82];

						Debug.Assert(plasticCardArtifact != null);

						if (plasticCardArtifact.IsInLimbo())
						{
							plasticCardArtifact.Desc = "Destroying the remains of the android reveals a small featureless card made out of a durable plastic.";

							gOut.Print("{0}", plasticCardArtifact.Desc);

							plasticCardArtifact.SetInRoom(ActorRoom);

							plasticCardArtifact.Seen = true;
						}

						break;

					default:

						base.PlayerExecute();

						break;
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}

		public virtual void ProcessWallAttack()
		{
			DobjArtZeroAc = DobjArtifact.GetCategories(0);

			Debug.Assert(DobjArtZeroAc != null);

			var whereClauseFuncs = gGameState.GetNBTL(Friendliness.Enemy) <= 0 ?
				new Func<IMonster, bool>[] { m => m == ActorMonster, m => m.IsInRoom(ActorRoom) && m.Friendliness == Friendliness.Friend && ((m.Weapon > -1 && m.Weapon <= Globals.Database.GetArtifactsCount()) || m.CombatCode == CombatCode.NaturalWeapons) && m != ActorMonster } :
				new Func<IMonster, bool>[] { m => m == ActorMonster };

			var monsterList = gEngine.GetMonsterList(whereClauseFuncs);

			for (var i = 0; i < monsterList.Count; i++)
			{
				var monster = monsterList[i];

				gOut.Write("{0}{1} {2}{3} the {4}!{5}",
					Environment.NewLine,
					monster == ActorMonster ? "You" : monster.GetTheName(true, true, false, true),
					monster == ActorMonster && BlastSpell ? "blast" : "attack",
					monster == ActorMonster ? "" : "s",
					DobjArtifact.Uid == 83 ? "back wall" : "glass walls",
					true /* monster == ActorMonster || i == monsters.Count - 1 */ ? Environment.NewLine : "");

				var dice = 0L;

				var sides = 0L;

				if (monster == ActorMonster && BlastSpell)
				{
					dice = 2;

					sides = 5;
				}
				else
				{
					var weapon = monster.Weapon > 0 ? gADB[monster.Weapon] : null;

					var wpnAc = weapon != null ? weapon.GetCategories(0) : null;

					dice = wpnAc != null ? wpnAc.Field3 : monster.NwDice;

					sides = wpnAc != null ? wpnAc.Field4 : monster.NwSides;
				}

				WallDamage += gEngine.RollDice(dice, sides, 0);
			}
		}
	}
}
