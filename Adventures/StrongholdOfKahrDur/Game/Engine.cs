
// Engine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
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

		protected override void PlayerSpellCastBrainOverload(Enums.Spell s, Classes.ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Enums.Spell), s));

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

			var artifact = Globals.ADB[10];     // Secret door #2

			Debug.Assert(artifact != null);

			artifact.Name = artifact.Name.TrimEnd('#');
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

					if (monster.Friendliness > Enums.Friendliness.Enemy)
					{
						Globals.Out.WriteLine();
					}

					while (monster.Friendliness > Enums.Friendliness.Enemy)
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

		public virtual bool SpellReagentsInCauldron(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var artifact01 = Globals.ADB[19];

			Debug.Assert(artifact01 != null);

			var artifact02 = Globals.ADB[20];

			Debug.Assert(artifact02 != null);

			var artifact03 = Globals.ADB[21];

			Debug.Assert(artifact03 != null);

			var artifact04 = Globals.ADB[22];

			Debug.Assert(artifact04 != null);

			return artifact01.IsCarriedByContainer(artifact) && artifact02.IsCarriedByContainer(artifact) && artifact03.IsCarriedByContainer(artifact) && artifact04.IsCarriedByContainer(artifact);
		}

		public Engine()
		{
			MissDescs[new Tuple<Enums.Weapon, long>(Enums.Weapon.Axe, 1)] = "Parried";

			MissDescs[new Tuple<Enums.Weapon, long>(Enums.Weapon.Club, 1)] = "Parried";

			MissDescs[new Tuple<Enums.Weapon, long>(Enums.Weapon.Spear, 1)] = "Parried";

			UseMonsterScaledHardinessValues = true;
		}
	}
}
