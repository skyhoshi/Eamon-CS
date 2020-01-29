
// Engine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, Framework.IEngine
	{
		protected override void PlayerSpellCastBrainOverload(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s));

			Debug.Assert(spell != null);

			gOut.Print("The strain of attempting to cast {0} overloads your brain and you forget it completely.", spell.Name);

			gGameState.SetSa(s, 0);

			gCharacter.SetSpellAbilities(s, 0);
		}

		public override void PrintMonsterAlive(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Cutlass

			if (artifact.Uid != 34)
			{
				base.PrintMonsterAlive(artifact);
			}
		}

		public override void PrintLightOut(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 1)
			{
				gOut.Print("{0} suddenly flickers and then goes out.", artifact.GetTheName(true));
			}
			else
			{
				base.PrintLightOut(artifact);
			}
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				var largeFountainArtifact = gADB[24];

				Debug.Assert(largeFountainArtifact != null);

				return largeFountainArtifact?.DoorGate != null ? "A small staircase leads down into darkness, and a passage leads back southward.  From below, many different noises can be discerned." : "A passage leads back southward.";
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "magi-torch", "instruction label", "instruction tag", "instructions", "instruction", "label", "tag" } },
				{ 3, new string[] { "small crypt door", "crypt door", "crypt", "door" } },
				{ 4, new string[] { "small tomb door", "tomb door", "tomb", "door" } },
				{ 5, new string[] { "crypt door", "door" } },
				{ 6, new string[] { "bucket" } },
				{ 10, new string[] { "gravestone", "stone" } },
				{ 13, new string[] { "chest" } },
				{ 14, new string[] { "old map", "map" } },
				{ 15, new string[] { "bottle" } },
				{ 16, new string[] { "gauntlets", "gauntlet" } },
				{ 17, new string[] { "wine", "cask" } },
				{ 19, new string[] { "cloak" } },
				{ 22, new string[] { "egg" } },
				{ 23, new string[] { "nest" } },
				{ 24, new string[] { "fountain", "basin", "grotesque face", "face" } },
				{ 25, new string[] { "gold pile", "gold coins", "gold", "coins" } },
				{ 28, new string[] { "rod" } },
				{ 33, new string[] { "parchment" } },
				{ 34, new string[] { "sword" } },
				{ 35, new string[] { "buried coffin", "casket", "coffin" } },
				{ 36, new string[] { "skeleton", "bones" } },
				{ 37, new string[] { "cross" } },
				{ 38, new string[] { "coil" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}

			var torchArtifact = gADB[1];

			Debug.Assert(torchArtifact != null);

			gGameState.TorchRounds = RollDice(1, 81, 399);

			torchArtifact.Value = (long)Math.Round(5.0 * ((double)gGameState.TorchRounds / 50.0));

			torchArtifact.LightSource.Field1 = gGameState.TorchRounds;
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			// TODO: complete when monster names locked down

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "rat" } },
				{ 10, new string[] { "will-o'-wisp", "will o' the wisp", "will o' wisp", "wisp" } },
				{ 11, new string[] { "crawler" } },
				{ 12, new string[] { "mound" } },
				{ 13, new string[] { "jelly" } },
				{ 20, new string[] { "bloodnettle" } },
				{ 21, new string[] { "hood" } },
				{ 24, new string[] { "dragon" } },
				{ 25, new string[] { "amoeba" } },
				{ 31, new string[] { "possessed sword", "cutlass", "sword" } },
				{ 37, new string[] { "crayfish" } },
				{ 38, new string[] { "weird" } },
				{ 39, new string[] { "scorpion" } },
				{ 43, new string[] { "jaldial", "lich" } },
				{ 50, new string[] { "genie" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void MonsterDies(IMonster OfMonster, IMonster DfMonster)	// Note: much more to implement here
		{
			Debug.Assert(DfMonster != null);

			// Possessed cutlass

			if (DfMonster.Uid == 31)
			{
				var cutlassArtifact = gADB[34];

				Debug.Assert(cutlassArtifact != null);

				cutlassArtifact.Type = ArtifactType.Weapon;

				cutlassArtifact.Field1 = 25;

				cutlassArtifact.Field2 = 5;

				cutlassArtifact.Field3 = 2;

				cutlassArtifact.Field4 = 6;

				cutlassArtifact.Field5 = 1;
			}

			// Water weird

			else if (DfMonster.Uid == 38)
			{
				gGameState.WaterWeirdKilled = true;
			}

			// Efreeti

			else if (DfMonster.Uid == 50)
			{
				gGameState.EfreetiKilled = true;
			}

			base.MonsterDies(OfMonster, DfMonster);
		}

		public override void CheckToExtinguishLightSource()
		{
			// do nothing
		}

		public virtual bool SaveThrow(Stat stat, long bonus = 0)
		{
			var characterMonster = gMDB[gGameState.Cm];

			Debug.Assert(characterMonster != null);

			var value = 0L;

			switch (stat)
			{
				case Stat.Hardiness:

					// This is the saving throw vs. opening doors, etc

					value = characterMonster.Hardiness + bonus;

					break;

				case Stat.Agility:

					// This is the saving throw vs. avoiding traps, etc

					value = characterMonster.Agility + bonus;

					break;

				case Stat.Intellect:

					// This is the saving throw vs. searching, etc

					value = gCharacter.GetStats(Stat.Intellect) + bonus;

					break;

				default:

					// This is the saving throw vs. death or magic

					value = (long)Math.Round((double)(characterMonster.Agility + gCharacter.GetStats(Stat.Charisma) + characterMonster.Hardiness) / 3.0) + bonus;

					break;
			}

			var rl = RollDice(1, 22, 2);

			return rl <= value;
		}
	}
}
