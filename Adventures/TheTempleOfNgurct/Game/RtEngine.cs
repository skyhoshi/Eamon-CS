
// RtEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTempleOfNgurct.Framework;
using Enums = Eamon.Framework.Primitive.Enums;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IRtEngine))]
	public class RtEngine : EamonRT.Game.RtEngine, IRtEngine
	{
		public override void InitArtifacts()
		{
			base.InitArtifacts();

			Globals.Engine.MacroFuncs.Add(2, () =>
			{
				var cellDoorArtifact = Globals.ADB[87];

				Debug.Assert(cellDoorArtifact != null);
			
				var ac = cellDoorArtifact.GetArtifactClass(Enums.ArtifactType.DoorGate);

				Debug.Assert(ac != null);

				return ac.GetKeyUid() > 0 ? "locked" : "unlocked";
			});

			Globals.Engine.MacroFuncs.Add(3, () =>
			{
				var cellDoorArtifact = Globals.ADB[88];

				Debug.Assert(cellDoorArtifact != null);
			
				var ac = cellDoorArtifact.GetArtifactClass(Enums.ArtifactType.DoorGate);

				Debug.Assert(ac != null);

				return ac.GetKeyUid() > 0 ? "locked" : "unlocked";
			});

			Globals.Engine.MacroFuncs.Add(4, () =>
			{
				var result = "floor";

				var gameState = Globals.GameState as IGameState;

				var characterRoom = gameState != null && gameState.Ro > 0 ? Globals.RDB[gameState.Ro] : null;

				if (characterRoom != null && characterRoom.Type == Enums.RoomType.Outdoors)
				{
					result = "ground";
				}

				return result;
			});

			var artUids = new long[]
			{
				11, 34, 38, 40, 41, 42, 43, 47, 51, 52, 53, 56, 58,
				62, 63, 68, 69, 70, 72, 73, 74, 75, 76, 78, 80, 81,
				82, 83, 84, 85, 86, 87, 88
			};

			var synonyms = new List<string[]>()
			{
				new string[] { "sword" },
				new string[] { "axe" },
				new string[] { "sword" },
				new string[] { "sword" },
				new string[] { "scimitar" },
				new string[] { "sword" },
				new string[] { "pan" },
				new string[] { "ingots", "ingot" },
				new string[] { "healing potion", "potion" },
				new string[] { "human blood", "blood", "potion" },
				new string[] { "sulphuric acid", "acid", "h2so4", "potion" },
				new string[] { "bars", "bar" },
				new string[] { "stones", "stone" },
				new string[] { "healing potion", "potion" },
				new string[] { "wand" },
				new string[] { "pig" },
				new string[] { "bottle", "label" },
				new string[] { "key" },
				new string[] { "keys" },
				new string[] { "key" },
				new string[] { "dagger" },
				new string[] { "robes", "robe" },
				new string[] { "blanks" },
				new string[] { "dagger" },
				new string[] { "ring" },
				new string[] { "bound slave", "bound girl", "slave girl", "girl" },
				new string[] { "hieroglyphs", "glyphs", "inscriptions", "wall" },
				new string[] { "door" },
				new string[] { "door" },
				new string[] { "door" },
				new string[] { "door" },
				new string[] { "door" },
				new string[] { "door" },
			};

			for (var i = 0; i < artUids.Length; i++)
			{
				CreateArtifactSynonyms(artUids[i], synonyms[i]);
			}

			var artClasses = new Enums.ArtifactType[] { Enums.ArtifactType.Drinkable, Enums.ArtifactType.Edible };

			artUids = new long[] { 51, 62, 68 };

			// Sets up potion/roast pig random heal amounts

			for (var i = 0; i < artUids.Length; i++)
			{
				var healingArtifact = Globals.ADB[artUids[i]];

				Debug.Assert(healingArtifact != null);

				var ac = healingArtifact.GetArtifactClass(artClasses);

				Debug.Assert(ac != null);

				ac.Field5 = Globals.Engine.RollDice01(1, 10, 0);
			}

			// Places fireball wand and ring of regeneration

			var wandArtifact = Globals.ADB[63];

			Debug.Assert(wandArtifact != null);

			wandArtifact.Location = Globals.Engine.RollDice01(1, 28, 28);

			var ringArtifact = Globals.ADB[64];

			Debug.Assert(ringArtifact != null);

			ringArtifact.Location = wandArtifact.Location;
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			Globals.Engine.MacroFuncs.Add(1, () =>
			{
				var characterMonster = Globals.MDB[Globals.GameState.Cm];

				Debug.Assert(characterMonster != null);

				return characterMonster.EvalGender(" sir", " madam", "");
			});

			Globals.Engine.MacroFuncs.Add(5, () =>
			{
				var result = "room";

				var gameState = Globals.GameState as IGameState;

				var characterRoom = gameState != null && gameState.Ro > 0 ? Globals.RDB[gameState.Ro] : null;

				if (characterRoom != null && characterRoom.Type == Enums.RoomType.Outdoors)
				{
					result = "area";
				}

				return result;
			});

			// Sets up random monster rooms

			for (var i = 7; i <= 11; i++)
			{
				var randomMonster = Globals.MDB[i];

				Debug.Assert(randomMonster != null);

				while (true)
				{
					randomMonster.Location = Globals.Engine.RollDice01(1, 56, 3);

					if (randomMonster.Location != 58)
					{
						break;
					}
				}
			}
		}

		public override void ResetMonsterStats(Eamon.Framework.IMonster monster)
		{
			Debug.Assert(monster != null);

			monster.Agility = Globals.Character.GetStats(Enums.Stat.Agility);

			Globals.GameState.Speed = 0;
		}

		public override void ConvertToCarriedInventory(IList<Eamon.Framework.IArtifact> weaponList)
		{
			// Convert fireball wand into artifact type gold

			var wandArtifact = Globals.ADB[63];

			Debug.Assert(wandArtifact != null);

			Globals.Engine.ConvertWeaponToGoldOrTreasure(wandArtifact, true);

			base.ConvertToCarriedInventory(weaponList);
		}

		public override void MonsterSmiles(Eamon.Framework.IMonster monster)
		{
			Debug.Assert(monster != null);

			// Cobra

			if (monster.Uid == 52)
			{
				Globals.Out.Write("{0}{1} hisses at you.", Environment.NewLine, monster.GetDecoratedName03(true, true, false, false, Globals.Buf));
			}
			else
			{
				base.MonsterSmiles(monster);
			}
		}

		public override void MonsterDies(Eamon.Framework.IMonster OfMonster, Eamon.Framework.IMonster DfMonster)
		{
			Debug.Assert(DfMonster != null);

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			if (DfMonster.Uid == 30)
			{
				gameState.KeyRingRoomUid = DfMonster.Location;
			}
			else if (DfMonster.Uid == 56)
			{
				gameState.AlkandaKilled = true;
			}

			var dmgTaken = DfMonster.DmgTaken;

			base.MonsterDies(OfMonster, DfMonster);

			DfMonster.DmgTaken = dmgTaken;
		}

		public virtual bool GetWanderingMonster()
		{
			var found = false;

			var gameState = Globals.GameState as IGameState;

			Debug.Assert(gameState != null);

			while (gameState.DwLoopCounter <= 15)
			{
				if (gameState.WanderingMonster > 26)
				{
					gameState.WanderingMonster = 12;
				}

				var wanderingMonster = Globals.MDB[gameState.WanderingMonster];

				Debug.Assert(wanderingMonster != null);

				if (wanderingMonster.DmgTaken == 0)
				{
					wanderingMonster.Location = gameState.Ro;

					gameState.WanderingMonster++;

					Globals.RtEngine.CheckEnemies();

					found = true;

					break;
				}

				gameState.WanderingMonster++;

				gameState.DwLoopCounter++;
			}

			return found;
		}

		public RtEngine()
		{
			PoundCharPolicy = Enums.PoundCharPolicy.None;
		}
	}
}
