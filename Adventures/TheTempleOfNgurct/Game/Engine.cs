
// Engine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, Framework.IEngine
	{
		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(2, () =>
			{
				var cellDoorArtifact = Globals.ADB[87];

				Debug.Assert(cellDoorArtifact != null);
			
				var ac = cellDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				return ac.GetKeyUid() > 0 ? "locked" : "unlocked";
			});

			MacroFuncs.Add(3, () =>
			{
				var cellDoorArtifact = Globals.ADB[88];

				Debug.Assert(cellDoorArtifact != null);
			
				var ac = cellDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				return ac.GetKeyUid() > 0 ? "locked" : "unlocked";
			});

			MacroFuncs.Add(4, () =>
			{
				var result = "floor";

				var gameState = Globals.GameState as Framework.IGameState;

				var characterRoom = gameState != null && gameState.Ro > 0 ? Globals.RDB[gameState.Ro] : null;

				if (characterRoom != null && characterRoom.Type == RoomType.Outdoors)
				{
					result = "ground";
				}

				return result;
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 11, new string[] { "sword" } },
				{ 34, new string[] { "axe" } },
				{ 38, new string[] { "sword" } },
				{ 40, new string[] { "sword" } },
				{ 41, new string[] { "scimitar" } },
				{ 42, new string[] { "sword" } },
				{ 43, new string[] { "pan" } },
				{ 47, new string[] { "ingots", "ingot" } },
				{ 51, new string[] { "healing potion", "potion" } },
				{ 52, new string[] { "human blood", "blood", "potion" } },
				{ 53, new string[] { "sulphuric acid", "acid", "h2so4", "potion" } },
				{ 56, new string[] { "bars", "bar" } },
				{ 58, new string[] { "stones", "stone" } },
				{ 62, new string[] { "healing potion", "potion" } },
				{ 63, new string[] { "wand" } },
				{ 68, new string[] { "pig" } },
				{ 69, new string[] { "bottle", "label" } },
				{ 70, new string[] { "key" } },
				{ 72, new string[] { "keys" } },
				{ 73, new string[] { "key" } },
				{ 74, new string[] { "dagger" } },
				{ 75, new string[] { "robes", "robe" } },
				{ 76, new string[] { "blanks" } },
				{ 78, new string[] { "dagger" } },
				{ 80, new string[] { "ring" } },
				{ 81, new string[] { "bound slave", "bound girl", "slave girl", "girl" } },
				{ 82, new string[] { "hieroglyphs", "glyphs", "inscriptions", "wall" } },
				{ 83, new string[] { "door" } },
				{ 84, new string[] { "door" } },
				{ 85, new string[] { "door" } },
				{ 86, new string[] { "door" } },
				{ 87, new string[] { "door" } },
				{ 88, new string[] { "door" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}

			var artTypes = new ArtifactType[] { ArtifactType.Drinkable, ArtifactType.Edible };

			var artUids = new long[] { 51, 62, 68 };

			// Sets up potion/roast pig random heal amounts

			for (var i = 0; i < artUids.Length; i++)
			{
				var healingArtifact = Globals.ADB[artUids[i]];

				Debug.Assert(healingArtifact != null);

				var ac = healingArtifact.GetArtifactCategory(artTypes);

				Debug.Assert(ac != null);

				ac.Field1 = RollDice(1, 10, 0);
			}

			// Places fireball wand and ring of regeneration

			var wandArtifact = Globals.ADB[63];

			Debug.Assert(wandArtifact != null);

			wandArtifact.Location = RollDice(1, 28, 28);

			var ringArtifact = Globals.ADB[64];

			Debug.Assert(ringArtifact != null);

			ringArtifact.Location = wandArtifact.Location;
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			// Note: since these macro functions are installed by InitMonsters, we can assume this is
			// being executed by EamonRT so there is no need to overharden the functions.  But, it is
			// still recommended!

			MacroFuncs.Add(1, () =>
			{
				var characterMonster = Globals.MDB[Globals.GameState.Cm];

				Debug.Assert(characterMonster != null);

				return characterMonster.EvalGender(" sir", " madam", "");
			});

			MacroFuncs.Add(5, () =>
			{
				var result = "room";

				var gameState = Globals.GameState as Framework.IGameState;

				var characterRoom = gameState != null && gameState.Ro > 0 ? Globals.RDB[gameState.Ro] : null;

				if (characterRoom != null && characterRoom.Type == RoomType.Outdoors)
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
					randomMonster.Location = RollDice(1, 56, 3);

					if (randomMonster.Location != 58)
					{
						break;
					}
				}
			}
		}

		public override void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null);

			monster.Agility = Globals.Character.GetStats(Stat.Agility);

			Globals.GameState.Speed = 0;
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			// Convert fireball wand into artifact type gold

			var wandArtifact = Globals.ADB[63];

			Debug.Assert(wandArtifact != null);

			ConvertWeaponToGoldOrTreasure(wandArtifact, true);

			base.ConvertToCarriedInventory(weaponList);
		}

		public override void MonsterSmiles(IMonster monster)
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

		public override void MonsterDies(IMonster OfMonster, IMonster DfMonster)
		{
			Debug.Assert(DfMonster != null);

			var gameState = Globals.GameState as Framework.IGameState;

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

		public virtual IList<IMonster> GetTrapMonsterList(long numMonsters, long roomUid)
		{
			var monsters = GetRandomMonsterList(numMonsters, m => m.IsCharacterMonster() || (m.Seen && m.IsInRoomUid(roomUid)));

			Debug.Assert(monsters != null);

			return monsters;
		}

		public virtual void ApplyTrapDamage(Action<IState> setNextStateFunc, IMonster monster, long numDice, long numSides, bool omitArmor)
		{
			Debug.Assert(setNextStateFunc != null && monster != null);

			var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
			{
				x.SetNextStateFunc = setNextStateFunc;

				x.DfMonster = monster;

				x.OmitArmor = omitArmor;
			});

			combatSystem.ExecuteCalculateDamage(numDice, numSides);
		}

		public virtual bool GetWanderingMonster()
		{
			var found = false;

			var gameState = Globals.GameState as Framework.IGameState;

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

					found = true;

					break;
				}

				gameState.WanderingMonster++;

				gameState.DwLoopCounter++;
			}

			return found;
		}

		public Engine()
		{
			MacroFuncs.Add(6, () =>
			{
				var result = "(the specifics are left to your imagination)";

				var gameState = Globals.GameState as Framework.IGameState;

				if (gameState != null && gameState.MatureContent)
				{
					result = Encoding.UTF8.GetString(Convert.FromBase64String("LS0gZWF0aW5nIGh1bWFuIGJhYmllcywgcmFwaW5nIHdvbWVuLCBhbmQgc28gZm9ydGg="));
				}

				return result;
			});

			MacroFuncs.Add(7, () =>
			{
				var result = "You fool!  You just climbed down into the excrement duct!";

				var gameState = Globals.GameState as Framework.IGameState;

				if (gameState != null && gameState.MatureContent)
				{
					result = Encoding.UTF8.GetString(Convert.FromBase64String("WW91IHN0dXBpZCBqZXJrISAgWW91IGp1c3QgY2xpbWJlZCBkb3duIGludG8gdGhlIHNoaXQgaG9sZSE="));
				}

				return result;
			});

			PoundCharPolicy = PoundCharPolicy.None;
		}
	}
}
