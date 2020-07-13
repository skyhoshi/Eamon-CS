﻿
// Engine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

			MacroFuncs.Add(2, () =>
			{
				var buriedCasketArtifact = gADB[35];

				Debug.Assert(buriedCasketArtifact != null);

				return buriedCasketArtifact.InContainer.IsOpen() ? "open" : "closed";
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "magi-torch", "instruction label", "instruction tag", "instructions", "instruction", "label", "tag" } },
				{ 3, new string[] { "small crypt door", "crypt door", "crypt", "door" } },
				{ 4, new string[] { "small tomb door", "tomb door", "tomb", "door" } },
				{ 5, new string[] { "crypt door", "door" } },
				{ 6, new string[] { "bucket" } },
				{ 8, new string[] { "bones" } },
				{ 9, new string[] { "leather bound book", "leather book", "bound book", "book" } },
				{ 10, new string[] { "gravestone", "stone" } },
				{ 11, new string[] { "dragon's treasure", "dragon's hoard", "treasure hoard", "treasure", "hoard" } },
				{ 12, new string[] { "goblet", "cup" } },
				{ 13, new string[] { "chest" } },
				{ 14, new string[] { "old map", "map" } },
				{ 15, new string[] { "ale bottle", "bottle", "ale" } },
				{ 16, new string[] { "gauntlets", "gauntlet" } },
				{ 17, new string[] { "wine", "cask" } },
				{ 18, new string[] { "beholder's treasure", "beholder's hoard", "treasure hoard", "treasure", "hoard" } },
				{ 19, new string[] { "cloak" } },
				{ 20, new string[] { "pieces" } },
				{ 21, new string[] { "pouch with stones", "pouch", "stones" } },
				{ 22, new string[] { "egg" } },
				{ 23, new string[] { "nest" } },
				{ 24, new string[] { "fountain", "basin", "grotesque face", "face" } },
				{ 25, new string[] { "gold pile", "gold coins", "gold", "coins" } },
				{ 26, new string[] { "wood throne", "throne" } },
				{ 27, new string[] { "rune book", "book" } },
				{ 28, new string[] { "rod" } },
				{ 29, new string[] { "mace" } },
				{ 31, new string[] { "sword" } },
				{ 32, new string[] { "dagger" } },
				{ 33, new string[] { "parchment" } },
				{ 34, new string[] { "sword" } },
				{ 35, new string[] { "buried coffin", "casket", "coffin" } },
				{ 36, new string[] { "skeleton", "bones" } },
				{ 37, new string[] { "cross" } },
				{ 38, new string[] { "coil" } },
				{ 43, new string[] { "dead tree" } },
				{ 44, new string[] { "dead forest nettles", "dead blood nettles", "dead bloodnettles", "dead nettles" } },
				{ 45, new string[] { "cloak", "cowl" } },
				{ 46, new string[] { "dead whipweed", "dead weed" } },
				{ 48, new string[] { "dead dragon" } },
				{ 49, new string[] { "dead crimson amoeba", "dead amoeba", "wine" } },
				{ 50, new string[] { "dead saber claws" } },
				{ 51, new string[] { "jade dust", "jade splinters", "jade chunks", "debris" } },
				{ 53, new string[] { "dead crayfish" } },
				{ 54, new string[] { "dead scorpion" } },
				{ 56, new string[] { "dead griffin cubs", "dead griffin babies", "dead baby griffins", "dead griffin baby" } },
				{ 57, new string[] { "Jaldial the lich's remains", "Jaldi'al's remains", "Jaldial's remains", "lich's remains", "lich remains", "remains", "dead Jaldi'al", "dead Jaldial", "dead lich" } },
				{ 58, new string[] { "dead jungle cats", "dead bekkahs", "dead cats" } },
				{ 59, new string[] { "dead Reginald" } },
				{ 60, new string[] { "dead Dubro" } },
				{ 61, new string[] { "dead Joque" } },
				{ 62, new string[] { "dead Trevor" } },
				{ 63, new string[] { "stone coffin", "coffin" } },
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

			var lanternArtifact = gADB[39];

			Debug.Assert(lanternArtifact != null);

			lanternArtifact.LightSource.Field1 = RollDice(1, 81, 299);
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "rat" } },
				{ 10, new string[] { "will-o'-wisp", "will o' the wisp", "will o' wisp", "wisp" } },
				{ 11, new string[] { "crawler" } },
				{ 12, new string[] { "mound" } },
				{ 13, new string[] { "jelly" } },
				{ 20, new string[] { "bloodnettle clump", "bloodnettle", "clump" } },
				{ 21, new string[] { "hood" } },
				{ 22, new string[] { "weed" } },
				{ 23, new string[] { "animated armor", "suit of armor", "armor" } },
				{ 24, new string[] { "dragon" } },
				{ 25, new string[] { "amoeba" } },
				{ 31, new string[] { "possessed sword", "cutlass", "sword" } },
				{ 32, new string[] { "statues" } },
				{ 37, new string[] { "crayfish" } },
				{ 38, new string[] { "weird" } },
				{ 39, new string[] { "scorpion" } },
				{ 41, new string[] { "baby griffins", "griffin cubs", "griffins", "babies", "cubs" } },
				{ 43, new string[] { "jaldial", "lich" } },
				{ 44, new string[] { "jungle cats", "bekkahs", "cats" } },
				{ 50, new string[] { "genie" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null && monster.IsCharacterMonster());

			base.ResetMonsterStats(monster);

			// Ensure monster not paralyzed or clumsy

			gGameState.ParalyzedTargets.Remove(monster.Uid);

			gGameState.ClumsyTargets.Remove(monster.Uid);
		}

		public override void MonsterEmotes(IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(monster != null);

			var rl = RollDice(1, 100, 0);

			// Giant rat

			if (monster.Uid == 1)
			{
				Globals.Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "squeals" : rl > 50 ? "squeaks" : "hisses");
			}

			// Skeleton/Gargoyle

			else if ((monster.Uid == 3 || monster.Uid == 8) && rl > 50)
			{
				Globals.Out.Write("{0}{1} hisses at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Zombie

			else if (monster.Uid == 4 && rl > 50)
			{
				Globals.Out.Write("{0}{1} snarls at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Ghoul/Ghast

			else if (monster.Uid == 6 || monster.Uid == 7)
			{
				Globals.Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "hisses" : rl > 50 ? "snarls" : "growls");
			}

			// Shadow/Specter/Wraith/Dark Hood/Animated suit of armor

			else if (monster.Uid == 9 || monster.Uid == 14 || monster.Uid == 16 || monster.Uid == 21 || monster.Uid == 23)
			{
				Globals.Out.Write("{0}{1} gestures at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Will-o'-the-wisp

			else if (monster.Uid == 10)
			{
				Globals.Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 50 ? "brightly flashes" : "hums");
			}

			// Pocket dragon/Giant crayfish/Giant scorpion

			else if ((monster.Uid == 24 && monster.Friendliness != Friendliness.Neutral) || monster.Uid == 37 || monster.Uid == 39)
			{
				Globals.Out.Write("{0}{1} hisses at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Griffin/Small griffin

			else if (monster.Uid == 40 || (monster.Uid == 41 && monster.Friendliness != Friendliness.Neutral))
			{
				Globals.Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? monster.EvalPlural("screeches", "screech") : rl > 50 ? monster.EvalPlural("squawks", "squawk") : monster.EvalPlural("hisses", "hiss"));
			}

			// Jaldi'al the lich

			else if (monster.Uid == 43)
			{
				Globals.Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "hollowly chuckles" : rl > 50 ? "gestures" : "glares");
			}

			// Jungle bekkah

			else if (monster.Uid == 44)
			{
				Globals.Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? monster.EvalPlural("roars", "roar") : rl > 50 ? monster.EvalPlural("snarls", "snarl") : monster.EvalPlural("hisses", "hiss"));
			}

			// Non-emoting monsters

			else if (Constants.NonEmotingMonsterUids.Contains(monster.Uid))
			{
				Globals.Out.Write("{0}{1} {2} not responsive.", Environment.NewLine, monster.GetTheName(true), monster.EvalPlural("is", "are"));
			}
			else
			{
				base.MonsterEmotes(monster, friendSmile);
			}
		}

		public override void MonsterDies(IMonster OfMonster, IMonster DfMonster)
		{
			Debug.Assert(DfMonster != null);

			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			// Wandering monsters

			if (DfMonster.Uid >= 1 && DfMonster.Uid <= 17)
			{
				if (room.IsLit())
				{
					gOut.Print("{0}As {1} dies, its body {2}.", Environment.NewLine, DfMonster.GetTheName(), room.Zone == 1 ? "dissolves and is absorbed into the ground" : "is enveloped in a cloud of sinister black smoke");
				}
			}

			// Possessed cutlass

			else if (DfMonster.Uid == 31)
			{
				var cutlassArtifact = gADB[34];

				Debug.Assert(cutlassArtifact != null);

				cutlassArtifact.Type = ArtifactType.Weapon;

				cutlassArtifact.Field1 = 25;

				cutlassArtifact.Field2 = 5;

				cutlassArtifact.Field3 = 2;

				cutlassArtifact.Field4 = 8;

				cutlassArtifact.Field5 = 1;
			}

			// Giant crayfish

			else if (DfMonster.Uid == 37)
			{
				gGameState.GiantCrayfishKilled = true;
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

			gGameState.ParalyzedTargets.Remove(DfMonster.Uid);

			gGameState.ClumsyTargets.Remove(DfMonster.Uid);

			base.MonsterDies(OfMonster, DfMonster);
		}

		public override IList<IMonster> GetHostileMonsterList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var monsterList = base.GetHostileMonsterList(monster);

			// Bloodnettle always chooses same victim

			if (monster.Uid == 20 && gGameState.BloodnettleVictimUid != 0)
			{
				if (monsterList.FirstOrDefault(m => m.Uid == gGameState.BloodnettleVictimUid) != null)
				{
					monsterList = monsterList.Where(m => m.Uid == gGameState.BloodnettleVictimUid).ToList();
				}
				else
				{
					gGameState.BloodnettleVictimUid = 0;
				}
			}

			return monsterList;
		}

		public override IList<IMonster> GetEmotingMonsterList(IRoom room, IMonster monster, bool friendSmile = true)
		{
			// Some monsters don't emote

			return base.GetEmotingMonsterList(room, monster, friendSmile).Where(m => !Constants.NonEmotingMonsterUids.Contains(m.Uid) && (friendSmile || !gGameState.ParalyzedTargets.ContainsKey(m.Uid))).ToList();
		}

		public override void CheckToExtinguishLightSource()
		{
			// do nothing
		}

		public override void MoveMonsters(params Func<IMonster, bool>[] whereClauseFuncs)
		{
			base.MoveMonsters(m => !m.IsCharacterMonster() && (m.Seen || m.Friendliness == Friendliness.Friend) && m.Location == gGameState.R3);
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

		public Engine()
		{
			MacroFuncs.Add(3, () =>
			{
				var result = "to bright sunlight";

				if (gGameState != null)
				{
					var room = gRDB[17] as Framework.IRoom;

					Debug.Assert(room != null);

					if (gGameState.IsNightTime())
					{
						result = "into the night";
					}
					else if (room.GetWeatherIntensity() > 1)
					{
						result = "to daylight";
					}
				}

				return result;
			});
		}
	}
}
