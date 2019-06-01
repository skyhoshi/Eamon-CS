
// Engine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, Framework.IEngine
	{
		protected override void PrintTooManyWeapons()
		{
			Globals.Out.Print("As you enter the Main Hall, Lord William Crankhandle approaches you and says, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		protected override void PrintDeliverGoods()
		{
			Globals.Out.Print("You sell your goods to {0}the local buyer of treasure (under the sign of 3 balls).  He examines your items and pays you what they are worth.", string.Equals(Globals.Character.Name, "tom zucchini", StringComparison.OrdinalIgnoreCase) ? "" : "Tom Zucchini, ");
		}

		protected override void PlayerSpellCastBrainOverload(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s));

			Debug.Assert(spell != null);

			Globals.Out.Print("Spell backlash!  Your ability to cast {0} temporarily diminishes!", spell.Name);

			if (Globals.GameState.GetSa(s) > 10)
			{
				Globals.GameState.SetSa(s, 10);
			}
		}

		public override void AddPoundCharsToArtifactNames()
		{
			base.AddPoundCharsToArtifactNames();

			var secretDoorArtifact = Globals.ADB[10];     // Secret door #2

			Debug.Assert(secretDoorArtifact != null);

			secretDoorArtifact.Name = secretDoorArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 2, new string[] { "glasses" } },
				{ 4, new string[] { "door" } },
				{ 5, new string[] { "potion" } },
				{ 7, new string[] { "east gate", "east door", "portcullis", "gate", "door" } },
				{ 8, new string[] { "west gate", "west door", "portcullis", "gate", "door" } },
				{ 9, new string[] { "whitestone", "karamir" } },
				{ 10, new string[] { "door" } },
				{ 11, new string[] { "shelf" } },
				{ 12, new string[] { "gemstones", "stones" } },
				{ 13, new string[] { "coins" } },
				{ 14, new string[] { "boots", "levitation" } },
				{ 15, new string[] { "pouch" } },
				{ 16, new string[] { "key" } },
				{ 17, new string[] { "door" } },
				{ 18, new string[] { "amulet", "courage" } },
				{ 19, new string[] { "phial", "dragon", "spice" } },
				{ 20, new string[] { "weed" } },
				{ 21, new string[] { "stone" } },
				{ 22, new string[] { "powder" } },
				{ 23, new string[] { "scroll" } },
				{ 24, new string[] { "kettle", "pot" } },
				{ 25, new string[] { "wizard's helmet", "wizard helm", "wizard helmet", "helmet" } },
				{ 26, new string[] { "mirabelle", "niece", "woman", "girl" } },
				{ 27, new string[] { "sword" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "guard" } },
				{ 2, new string[] { "golem" } },
				{ 3, new string[] { "golem" } },
				{ 6, new string[] { "hydra" } },
				{ 8, new string[] { "evil ent", "tree ent", "ent" } },
				{ 9, new string[] { "evil ent", "tree ent", "ent" } },
				{ 10, new string[] { "evil ent", "tree ent", "ent" } },
				{ 11, new string[] { "guard" } },
				{ 12, new string[] { "guard" } },
				{ 13, new string[] { "guard" } },
				{ 14, new string[] { "jelly", "blob" } },
				{ 15, new string[] { "sharruk", "lich" } },
				{ 16, new string[] { "guard" } },
				{ 17, new string[] { "chieftain" } },
				{ 18, new string[] { "soldier" } },
				{ 19, new string[] { "scout" } },
				{ 20, new string[] { "brother" } },
				{ 22, new string[] { "wizard" } },
				{ 23, new string[] { "demonic snake", "serpent", "snake" } },
				{ 24, new string[] { "hound", "dog" } },
				{ 25, new string[] { "demon" } },
				{ 26, new string[] { "mirabelle", "niece", "girl" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true)
		{
			base.MonsterGetsAggravated(monster, printFinalNewLine);

			// If player aggravates Mirabelle, Jollifrud gets aggravated and vice versa

			if (monster.Uid == 20 || monster.Uid == 26)
			{
				var monster01 = Globals.MDB[monster.Uid == 20 ? 26 : 20];

				Debug.Assert(monster01 != null);

				base.MonsterGetsAggravated(monster01, printFinalNewLine);
			}
		}

		public override void MonsterSmiles(IMonster monster)
		{
			Debug.Assert(monster != null);

			Globals.Out.Write("{0}{1} {2}{3} at you.",
				Environment.NewLine,
				monster.GetDecoratedName03(true, true, false, false, Globals.Buf),
				monster.EvalFriendliness("growl", "look", "smile"),
				monster.EvalPlural("s", ""));
		}

		public override void MonsterDies(IMonster OfMonster, IMonster DfMonster)
		{
			base.MonsterDies(OfMonster, DfMonster);

			if (OfMonster != null && OfMonster.Uid == Globals.GameState.Cm)
			{
				// If player kills Mirabelle, Jollifrud gets angry and vice versa

				if (DfMonster.Uid == 20 || DfMonster.Uid == 26)
				{
					var monster = Globals.MDB[DfMonster.Uid == 20 ? 26 : 20];

					Debug.Assert(monster != null);

					if (monster.Friendliness > Friendliness.Enemy)
					{
						Globals.Out.WriteLine();
					}

					while (monster.Friendliness > Friendliness.Enemy)
					{
						base.MonsterGetsAggravated(monster, false);
					}
				}
			}
		}

		public override void MoveMonsters()
		{
			// Monsters can't move in/out of pit w/o magical help

			var pitMove = Globals.GameState.R3 == 84 && Globals.GameState.Ro == 94;

			if (!pitMove)
			{
				pitMove = Globals.GameState.R3 == 94 && Globals.GameState.Ro == 84;
			}

			if (!pitMove)
			{
				base.MoveMonsters();
			}
		}

		public virtual bool SpellReagentsInCauldron(IArtifact cauldronArtifact)
		{
			Debug.Assert(cauldronArtifact != null);

			var dragonSpiceArtifact = Globals.ADB[19];

			Debug.Assert(dragonSpiceArtifact != null);

			var kingswortWeedArtifact = Globals.ADB[20];

			Debug.Assert(kingswortWeedArtifact != null);

			var rubyStoneArtifact = Globals.ADB[21];

			Debug.Assert(rubyStoneArtifact != null);

			var residuumPowderArtifact = Globals.ADB[22];

			Debug.Assert(residuumPowderArtifact != null);

			return dragonSpiceArtifact.IsCarriedByContainer(cauldronArtifact) && kingswortWeedArtifact.IsCarriedByContainer(cauldronArtifact) && rubyStoneArtifact.IsCarriedByContainer(cauldronArtifact) && residuumPowderArtifact.IsCarriedByContainer(cauldronArtifact);
		}

		public virtual void SummonMonster(IRoom room, long monsterUid)
		{
			Debug.Assert(room != null);

			// Necromancer summons other monsters...

			var monster = Globals.MDB[monsterUid];

			Debug.Assert(monster != null);

			// Dead, hasn't been previously summoned, or is somewhere else

			if (monster.IsInLimbo() || !monster.IsInRoom(room))
			{
				// Preset 'Seen' flag for smoother effect

				monster.Seen = true;

				// Put monster in room

				monster.SetInRoom(room);

				// Reset group size to one

				monster.GroupCount = 1;

				monster.InitGroupCount = 1;

				monster.OrigGroupCount = 1;

				// Reset to 0 damage taken

				monster.DmgTaken = 0;
			}
			else
			{
				// Monster is already summoned and present so add another to the group

				monster.GroupCount++;

				monster.InitGroupCount++;

				monster.OrigGroupCount++;
			}

			CheckEnemies();
		}
		
		public Engine()
		{
			MissDescs[new Tuple<Weapon, long>(Weapon.Axe, 1)] = "Parried";

			MissDescs[new Tuple<Weapon, long>(Weapon.Club, 1)] = "Parried";

			MissDescs[new Tuple<Weapon, long>(Weapon.Spear, 1)] = "Parried";

			UseMonsterScaledHardinessValues = true;
		}
	}
}
