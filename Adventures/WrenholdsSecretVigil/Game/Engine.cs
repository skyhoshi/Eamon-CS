
// Engine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game
{
	[ClassMappings(typeof(Eamon.Framework.IEngine))]
	public class Engine : EamonRT.Game.Engine, Framework.IEngine
	{
		public override void AddPoundCharsToArtifactNames()
		{
			base.AddPoundCharsToArtifactNames();

			var slimeArtifact = Globals.ADB[25];     // Slime #2

			Debug.Assert(slimeArtifact != null);

			slimeArtifact.Name = slimeArtifact.Name.TrimEnd('#');

			var deviceArtifact = Globals.ADB[49];         // Large Green Device #2

			Debug.Assert(deviceArtifact != null);

			deviceArtifact.Name = deviceArtifact.Name.TrimEnd('#');

			var deadGuardArtifact = Globals.ADB[74];         // Dead Drow Guard #2

			Debug.Assert(deadGuardArtifact != null);

			deadGuardArtifact.Name = deadGuardArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "small wooden shovel", "small scoop shovel", "wooden scoop shovel", "small shovel", "wooden shovel", "scoop shovel", "shovel" } },
				{ 2, new string[] { "adventurer" } },
				{ 3, new string[] { "cloth bound diary", "cloth diary", "bound diary", "diary" } },
				{ 4, new string[] { "life orb", "orb" } },
				{ 5, new string[] { "cube" } },
				{ 6, new string[] { "cups" } },
				{ 7, new string[] { "large limb", "tree limb", "limb" } },
				{ 8, new string[] { "villager in chains", "chained villager" } },
				{ 9, new string[] { "jewel bag", "jewels" } },
				{ 11, new string[] { "chained merchant", "dwarf merchant", "merchant" } },
				{ 12, new string[] { "key" } },
				{ 13, new string[] { "axe" } },
				{ 14, new string[] { "shortsword", "sword" } },
				{ 15, new string[] { "rabbit" } },
				{ 16, new string[] { "small box", "wooden box", "box" } },
				{ 17, new string[] { "rock" } },
				{ 18, new string[] { "ring" } },
				{ 19, new string[] { "pea sized crystal ball", "pea-sized ball", "pea sized ball", "crystal ball", "ball" } },
				{ 20, new string[] { "box" } },
				{ 21, new string[] { "key" } },
				{ 22, new string[] { "key" } },
				{ 23, new string[] { "coins" } },
				{ 26, new string[] { "mattress" } },
				{ 27, new string[] { "key" } },
				{ 29, new string[] { "bones" } },
				{ 31, new string[] { "shark", "fish" } },
				{ 32, new string[] { "cleaver" } },
				{ 33, new string[] { "weapon racks", "weapons" } },
				{ 34, new string[] { "key" } },
				{ 35, new string[] { "wine barrels", "wines" } },
				{ 36, new string[] { "cups" } },
				{ 37, new string[] { "ivory safe", "door", "safe" } },
				{ 38, new string[] { "goebel" } },
				{ 39, new string[] { "door" } },
				{ 40, new string[] { "curtain" } },
				{ 41, new string[] { "book" } },
				{ 42, new string[] { "bones" } },
				{ 43, new string[] { "pedestal" } },
				{ 44, new string[] { "large device", "green device", "device" } },
				{ 45, new string[] { "key" } },
				{ 46, new string[] { "animals" } },
				{ 47, new string[] { "key" } },
				{ 49, new string[] { "large device", "green device", "device" } },
				{ 50, new string[] { "magic bow", "elven bow", "bow" } },
				{ 51, new string[] { "large dead death dog", "dead death dog", "dead dog" } },
				{ 52, new string[] { "smoldering fluids", "ash and fluids", "ash", "fluids" } },
				{ 53, new string[] { "ugly dead one-eyed ogre", "ugly dead one eyed ogre", "ugly dead ogre", "dead one-eyed ogre", "dead one eyed ogre", "dead ogre" } },
				{ 54, new string[] { "body" } },
				{ 55, new string[] { "body" } },
				{ 56, new string[] { "dead rat" } },
				{ 57, new string[] { "dead rat" } },
				{ 58, new string[] { "dead zombie" } },
				{ 59, new string[] { "dead zombie" } },
				{ 60, new string[] { "dead zombie" } },
				{ 62, new string[] { "dead patrol" } },
				{ 63, new string[] { "dead zombie" } },
				{ 64, new string[] { "dead soldiers" } },
				{ 65, new string[] { "dead citizens" } },
				{ 66, new string[] { "dead guards" } },
				{ 67, new string[] { "dead guard" } },
				{ 68, new string[] { "dead chef" } },
				{ 69, new string[] { "dead old woman", "dead drow woman", "dead woman" } },
				{ 70, new string[] { "dead diners" } },
				{ 71, new string[] { "dead drows" } },
				{ 72, new string[] { "dead bartender", "dead barkeeper" } },
				{ 73, new string[] { "dead patron" } },
				{ 74, new string[] { "dead guard" } },
				{ 75, new string[] { "dead priest" } },
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
				{ 1, new string[] { "large dog", "death dog", "dog" } },
				{ 2, new string[] { "sorcerer", "wizard", "mage" } },
				{ 3, new string[] { "one eyed ogre", "ogre" } },
				{ 4, new string[] { "breanok" } },
				{ 5, new string[] { "pantwell" } },
				{ 6, new string[] { "rat" } },
				{ 7, new string[] { "rat" } },
				{ 8, new string[] { "black haired zombie", "zombie" } },
				{ 9, new string[] { "leather clad zombie", "zombie" } },
				{ 10, new string[] { "zombie" } },
				{ 11, new string[] { "chain-mail zombie", "chain mail zombie", "chainmail zombie" } },
				{ 12, new string[] { "zombies" } },
				{ 13, new string[] { "zombie" } },
				{ 14, new string[] { "soldiers", "drows" } },
				{ 15, new string[] { "citizens", "drows" } },
				{ 16, new string[] { "guard" } },
				{ 17, new string[] { "guard" } },
				{ 18, new string[] { "chef" } },
				{ 19, new string[] { "old woman", "drow woman", "woman" } },
				{ 20, new string[] { "diners", "drows" } },
				{ 21, new string[] { "drows" } },
				{ 22, new string[] { "bartender" } },
				{ 23, new string[] { "patron" } },
				{ 24, new string[] { "guard" } },
				{ 25, new string[] { "drow priest", "priest" } },
				{ 26, new string[] { "animals" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void ConvertToCarriedInventory(IList<Eamon.Framework.IArtifact> weaponList)
		{
			// Convert large tree limb into artifact type treasure

			var treeLimbArtifact = Globals.ADB[7];

			Debug.Assert(treeLimbArtifact != null);

			ConvertWeaponToGoldOrTreasure(treeLimbArtifact, false);

			base.ConvertToCarriedInventory(weaponList);
		}

		public override void MonsterSmiles(Eamon.Framework.IMonster monster)
		{
			Debug.Assert(monster != null);

			// Large death dog

			if (monster.Uid == 1 && monster.Friendliness == Enums.Friendliness.Friend)
			{
				Globals.Out.Write("{0}{1} wags its tail.", Environment.NewLine, monster.GetDecoratedName03(true, true, false, false, Globals.Buf));
			}
			else
			{
				base.MonsterSmiles(monster);
			}
		}

		public override Eamon.Framework.IArtifact GetBlockedDirectionArtifact(long ro, long r2, Enums.Direction dir)
		{
			Eamon.Framework.IArtifact artifact = null;

			var slimeArtifact1 = Globals.ADB[24];

			Debug.Assert(slimeArtifact1 != null);

			var slimeArtifact2 = Globals.ADB[25];

			Debug.Assert(slimeArtifact2 != null);

			var largeRockArtifact = Globals.ADB[17];

			Debug.Assert(largeRockArtifact != null);

			var lifeOrbArtifact = Globals.ADB[4];

			Debug.Assert(lifeOrbArtifact != null);

			var ac = largeRockArtifact.GetArtifactCategory(Enums.ArtifactType.DoorGate);

			Debug.Assert(ac != null);

			// If slime in room, can't move past it

			if ((r2 == 21 || r2 == 44) && (ro == 21 || ro == 44) && (slimeArtifact1.IsInRoomUid(ro) || slimeArtifact2.IsInRoomUid(ro)))
			{
				artifact = slimeArtifact1;
			}

			// If rock in room, can't move past it

			else if (r2 == 19 && ro == 18 && !largeRockArtifact.IsInLimbo() && ac.GetKeyUid() != -2)
			{
				artifact = largeRockArtifact;
			}

			// And if room == 15, can't get past orb

			else if (ro == 15 && dir == Enums.Direction.South)
			{
				artifact = lifeOrbArtifact;
			}

			return artifact;
		}

		public virtual string GetMonsterCurse(Eamon.Framework.IMonster monster, long effectUid)
		{
			Debug.Assert(monster != null);

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			var curseString = "";

			var rl = RollDice01(1, 100, 0);

			// Say each curse only once

			if (rl < 41 && monster.Friendliness == Enums.Friendliness.Enemy && monster.HasCarriedInventory() && !gameState.GetMonsterCurses(effectUid - 7))
			{
				var effect = Globals.EDB[effectUid];

				Debug.Assert(effect != null);

				curseString = string.Format("{0}{0}{1} says, {2}", Environment.NewLine, monster.GetDecoratedName03(true, true, false, true, Globals.Buf01), effect.Desc);

				gameState.SetMonsterCurses(effectUid - 7, true);
			}

			return curseString;
		}

		public Engine()
		{
			// Note: this is an example of a macro function that will be used by both EamonDD and EamonRT in macro
			// resolution.  It is hardened to check for the existance of Globals.Character, which will only exist
			// in EamonRT (the GameState object, though not used here, is another thing to always check for).

			MacroFuncs.Add(1, () =>
			{
				return Globals.Character != null ? Globals.Character.Name : UnknownName;
			});
		}
	}
}
