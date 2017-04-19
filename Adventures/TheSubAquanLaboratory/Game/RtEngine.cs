
// RtEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheSubAquanLaboratory.Framework;
using static TheSubAquanLaboratory.Game.Plugin.PluginContext;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IRtEngine))]
	public class RtEngine : EamonRT.Game.RtEngine, IRtEngine
	{
		public override void AddPoundCharsToArtifactNames()
		{
			base.AddPoundCharsToArtifactNames();

			var artifact = Globals.ADB[26];     // Card slot #2

			Debug.Assert(artifact != null);

			artifact.Name = artifact.Name.TrimEnd('#');

			artifact = Globals.ADB[62];         // Console #2

			Debug.Assert(artifact != null);

			artifact.Name = artifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var artUids = new long[] 
			{
				1, 3, 4, 5, 6, 7, 9, 10, 11, 13, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 30,
				31, 32, 33, 36, 41, 42, 43, 44, 46, 47, 48, 49, 50, 52, 53, 54, 55, 56, 57, 58, 59, 60, 
				61, 63, 64, 65, 66, 67, 68, 69, 70, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82, 83, 84, 85,
				86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99, 100, 101, 102, 103, 104, 105,
				106, 107
			};

			var synonyms = new List<string[]>()
			{
				new string[] { "slot" },
				new string[] { "button" },
				new string[] { "button" },
				new string[] { "desk" },
				new string[] { "plate" },
				new string[] { "phone-like device", "device" },
				new string[] { "plaque" },
				new string[] { "spigot" },
				new string[] { "cabinet" },
				new string[] { "utensil" },
				new string[] { "vat" },
				new string[] { "door" },
				new string[] { "pipe" },
				new string[] { "massive apparatus", "spherical apparatus", "dish washer", "washer" },
				new string[] { "button" },
				new string[] { "button" },
				new string[] { "button" },
				new string[] { "food" },
				new string[] { "magnetic power plant", "fusion power plant", "fusion plant", "power plant", "plant" },
				new string[] { "tank" },
				new string[] { "pool" },
				new string[] { "slot" },
				new string[] { "button" },
				new string[] { "burner" },
				new string[] { "dish" },
				new string[] { "pedestal" },
				new string[] { "balance" },
				new string[] { "spectrograph", "chromatograph" },
				new string[] { "table" },
				new string[] { "box sized device", "device" },
				new string[] { "tool" },
				new string[] { "surgeomat xxi", "type xxi", "surgeomat", "xxi" },
				new string[] { "button" },
				new string[] { "drum" },
				new string[] { "display", "screen" },
				new string[] { "cabinet" },
				new string[] { "computer", "terminal" },
				new string[] { "gear" },
				new string[] { "rack" },
				new string[] { "gun" },
				new string[] { "button" },
				new string[] { "button" },
				new string[] { "Seven" },
				new string[] { "panel" },
				new string[] { "button" },
				new string[] { "button" },
				new string[] { "crystal column", "column" },
				new string[] { "shield panel", "control panel", "panel" },
				new string[] { "installation panel", "defense panel", "panel" },
				new string[] { "dial" },
				new string[] { "elevation button", "increase button", "button" },
				new string[] { "elevation button", "decrease button", "button" },
				new string[] { "rotate button", "turret button", "button" },
				new string[] { "button" },
				new string[] { "button" },
				new string[] { "blaster", "pistol", "gun" },
				new string[] { "blaster", "pistol", "gun" },
				new string[] { "pistol", "gun" },
				new string[] { "scalpel" },
				new string[] { "drill" },
				new string[] { "disruptor", "pistol", "gun" },
				new string[] { "axe" },
				new string[] { "mace" },
				new string[] { "crossbow" },
				new string[] { "card" },
				new string[] { "fake looking back wall", "fake back wall", "fake wall", "back wall", "wall" },
				new string[] { "wall" },
				new string[] { "electric floor", "floor", "tile", "trap" },
				new string[] { "horl choo", "choo" },
				new string[] { "dismantled android", "first android", "android" },
				new string[] { "dismantled android", "second android", "android" },
				new string[] { "dismantled android", "worker android", "android" },
				new string[] { "dismantled android", "thinker android", "android" },
				new string[] { "destroyed android", "thinker android", "android" },
				new string[] { "dead hammerhead", "large hammerhead", "dead shark", "large shark", "hammerhead", "shark" },
				new string[] { "dismembered hammerhead", "small hammerhead", "dismembered shark", "small shark", "hammerhead", "shark" },
				new string[] { "dismantled first android", "dismantled silver android", "first silver android", "silver android", "android" },
				new string[] { "dismantled second android", "dismantled silver android", "second silver android", "silver android", "android" },
				new string[] { "dismantled android", "thinker", "android" },
				new string[] { "dismantled android", "worker", "android" },
				new string[] { "dead humanoid", "dead fish man", "dead fish-man", "fen" },
				new string[] { "dismantled technician", "lab technician", "dismantled android", "lab android", "technician", "android" },
				new string[] { "dismantled android", "warrior android", "android" },
				new string[] { "jules' body", "body" },
				new string[] { "jemmas' body", "body" },
				new string[] { "mutilated body", "body" },
				new string[] { "body" },
				new string[] { "shattered wall", "glass wall", "wall" },
				new string[] { "pile of rubble", "pile", "rubble" },
				new string[] { "shorted out floor", "shorted out trap", "floor trap", "floor", "trap" }
			};

			for (var i = 0; i < artUids.Length; i++)
			{
				Globals.RtEngine.CreateArtifactSynonyms(artUids[i], synonyms[i]);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			var monUids = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22 };

			var synonyms = new List<string[]>()
			{
				new string[] { "choo" },
				new string[] { "first android", "worker android", "worker", "android" },
				new string[] { "second android", "worker android", "worker", "android" },
				new string[] { "worker", "android" },
				new string[] { "thinker", "android" },
				new string[] { "thinker", "android" },
				new string[] { "large hammerhead", "mutated hammerhead", "large shark", "mutated shark", "hammerhead", "shark" },
				new string[] { "small hammerhead", "mutated hammerhead", "small shark", "mutated shark", "hammerhead", "shark" },
				new string[] { "first figure", "silver-clad figure", "first android", "silver-clad android", "silver figure", "silver android", "figure", "android" },
				new string[] { "second figure", "silver-clad figure", "second android", "silver-clad android", "silver figure", "silver android", "figure", "android" },
				new string[] { "thinker", "android" },
				new string[] { "worker", "android" },
				new string[] { "humanoid", "fish man", "fish-man" },
				new string[] { "thinker", "android" },
				new string[] { "warrior", "android" },
				new string[] { "jules" },
				new string[] { "jemmas" },
				new string[] { "red eye", "eye" },
				new string[] { "archie", "panther" },
				new string[] { "worker", "android" },
				new string[] { "thinker", "android" },
				new string[] { "warrior", "android" }
			};

			for (var i = 0; i < monUids.Length; i++)
			{
				Globals.RtEngine.CreateMonsterSynonyms(monUids[i], synonyms[i]);
			}
		}

		public RtEngine()
		{
			StartRoom = 16;
		}
	}
}
