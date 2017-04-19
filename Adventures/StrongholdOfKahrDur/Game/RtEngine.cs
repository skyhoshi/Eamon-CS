
// RtEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Game.Attributes;
using StrongholdOfKahrDur.Framework;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IRtEngine))]
	public class RtEngine : EamonRT.Game.RtEngine, IRtEngine
	{
		protected override void PrintTooManyWeapons()
		{
			Globals.Out.Write("{0}As you enter the Main Hall, Lord William Crankhandle approaches you and says, \"You have too many weapons to keep them all, four is the legal limit.\"{0}", Environment.NewLine);
		}

		protected override void PrintDeliverGoods()
		{
			Globals.Out.Write("{0}You sell your goods to {1}the local buyer of treasure (under the sign of 3 balls).  He examines your items and pays you what they are worth.{0}", Environment.NewLine, string.Equals(Globals.Character.Name, "tom zucchini", StringComparison.OrdinalIgnoreCase) ? "" : "Tom Zucchini, ");
		}

		protected override void PlayerSpellCastBrainOverload(Enums.Spell s, Classes.ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Enums.Spell), s));

			Debug.Assert(spell != null);

			Globals.Out.Write("{0}Spell backlash!  Your ability to cast {1} temporarily diminishes!{0}", Environment.NewLine, spell.Name);

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

			var artUids = new long[]
			{
				2, 4, 5, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27
			};

			var synonyms = new List<string[]>()
			{
				new string[] { "glasses" },
				new string[] { "door" },
				new string[] { "potion" },
				new string[] { "east gate", "east door", "portcullis", "gate", "door" },
				new string[] { "west gate", "west door", "portcullis", "gate", "door" },
				new string[] { "whitestone", "karamir" },
				new string[] { "door" },
				new string[] { "shelf" },
				new string[] { "gemstones", "stones" },
				new string[] { "coins" },
				new string[] { "boots", "levitation" },
				new string[] { "pouch" },
				new string[] { "key" },
				new string[] { "door" },
				new string[] { "amulet", "courage" },
				new string[] { "phial", "dragon", "spice" },
				new string[] { "weed" },
				new string[] { "stone" },
				new string[] { "powder" },
				new string[] { "scroll" },
				new string[] { "kettle", "pot" },
				new string[] { "wizard's helmet", "wizard helm", "wizard helmet", "helmet" },
				new string[] { "mirabelle", "niece", "woman", "girl" },
				new string[] { "sword" }
			};

			for (var i = 0; i < artUids.Length; i++)
			{
				Globals.RtEngine.CreateArtifactSynonyms(artUids[i], synonyms[i]);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			var monUids = new long[] 
			{
				1, 2, 3, 6, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 22, 23, 24, 25, 26
			};

			var synonyms = new List<string[]>()
			{
				new string[] { "guard" },
				new string[] { "golem" },
				new string[] { "golem" },
				new string[] { "hydra" },
				new string[] { "evil ent", "tree ent", "ent" },
				new string[] { "evil ent", "tree ent", "ent" },
				new string[] { "evil ent", "tree ent", "ent" },
				new string[] { "guard" },
				new string[] { "guard" },
				new string[] { "guard" },
				new string[] { "jelly", "blob" },
				new string[] { "sharruk", "lich" },
				new string[] { "guard" },
				new string[] { "chieftain" },
				new string[] { "soldier" },
				new string[] { "scout" },
				new string[] { "brother" },
				new string[] { "wizard" },
				new string[] { "demonic snake", "serpent", "snake" },
				new string[] { "hound", "dog" },
				new string[] { "demon" },
				new string[] { "mirabelle", "niece", "girl" }
			};

			for (var i = 0; i < monUids.Length; i++)
			{
				Globals.RtEngine.CreateMonsterSynonyms(monUids[i], synonyms[i]);
			}
		}

		public override void MonsterGetsAggravated(Eamon.Framework.IMonster monster, bool printFinalNewLine = true)
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

		public override void MonsterSmiles(Eamon.Framework.IMonster monster)
		{
			Debug.Assert(monster != null);

			var friendliness = Globals.Engine.GetFriendlinesses(monster.Friendliness);

			Debug.Assert(friendliness != null);

			Globals.Out.Write("{0}{1} {2}{3} at you.",
				Environment.NewLine,
				monster.GetDecoratedName03(true, true, false, false, Globals.Buf),
				friendliness.SmileDesc,
				monster.EvalPlural("s", ""));
		}

		public override void MonsterDies(Eamon.Framework.IMonster OfMonster, Eamon.Framework.IMonster DfMonster)
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

		public virtual bool SpellReagentsInCauldron(Eamon.Framework.IArtifact artifact)
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

		public RtEngine()
		{
			UseMonsterScaledHardinessValues = true;
		}
	}
}
