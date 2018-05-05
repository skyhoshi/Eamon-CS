
// Engine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;
using Classes = Eamon.Framework.Primitive.Classes;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game
{
	[ClassMappings(typeof(Eamon.Framework.IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		protected virtual long HeldWpnIdx { get; set; }

		public override void AddPoundCharsToArtifactNames()
		{
			base.AddPoundCharsToArtifactNames();

			// Entrance/exit gates

			var artifact = Globals.ADB[19];

			Debug.Assert(artifact != null);

			artifact.Seen = true;

			artifact.Name = artifact.Name.TrimEnd('#');

			artifact = Globals.ADB[20];

			Debug.Assert(artifact != null);

			artifact.Seen = true;

			artifact.Name = artifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var gateValues = new string[] { "green arch", "vine covered arch", "arch", "vines", "vine", "wrought iron gate", "gate", "words" };

			var synonyms = new Dictionary<long, string[]>()
			{
				// Hidden bridge

				{ 17, new string[] { "giant green blanket", "green blanket", "giant blanket", "giant green", "blanket", "large shape", "shape" } },

				// Entrance/exit gates

				{ 19, gateValues },
				{ 20, gateValues },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			// Set Group Spooks to 0 for Spook routine

			var monster = Globals.MDB[9];

			Debug.Assert(monster != null);

			monster.GroupCount = 0;

			monster.InitGroupCount = 0;

			monster.OrigGroupCount = 0;

			var gameState = Globals.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			monster = Globals.MDB[4];

			Debug.Assert(monster != null);

			if (Globals.Character.Gender == Enums.Gender.Female)
			{
				// Queen's gift

				gameState.QueenGiftEffectUid = 6;

				gameState.QueenGiftArtifactUid = 15;

				// Sir Grummor is always kind to the ladies!

				monster.Friendliness = Enums.Friendliness.Friend;

				monster.OrigFriendliness = (Enums.Friendliness)200;
			}
		}

		public override Eamon.Framework.IArtifact ConvertWeaponToArtifact(Classes.ICharacterWeapon weapon)
		{
			var artifact = base.ConvertWeaponToArtifact(weapon);

			var gameState = Globals.GameState as Framework.IGameState;

			var i = FindIndex(Globals.Character.Weapons, x => x == weapon);

			if (i != gameState.UsedWpnIdx)
			{
				if (artifact.IsCarriedByCharacter())
				{
					gameState.Wt -= artifact.Weight;
				}

				artifact.SetInLimbo();

				gameState.SetHeldWpnUids(HeldWpnIdx++, artifact.Uid);
			}

			return artifact;
		}

		public override void ConvertToCarriedInventory(IList<Eamon.Framework.IArtifact> weaponList)
		{
			var gameState = Globals.GameState as Framework.IGameState;

			for (var i = 0; i < gameState.HeldWpnUids.Length; i++)
			{
				if (gameState.GetHeldWpnUids(i) > 0)
				{
					var artifact = Globals.ADB[gameState.GetHeldWpnUids(i)];

					Debug.Assert(artifact != null);

					artifact.SetCarriedByCharacter();

					gameState.Wt += artifact.Weight;
				}
			}

			base.ConvertToCarriedInventory(weaponList);
		}

		public override void MonsterDies(Eamon.Framework.IMonster OfMonster, Eamon.Framework.IMonster DfMonster)
		{
			Debug.Assert(DfMonster != null && !DfMonster.IsCharacterMonster());

			// Repetitive Spooks' counter reset

			if (DfMonster.Uid == 9)
			{
				var resetGroupCount = DfMonster.GroupCount == 1;

				base.MonsterDies(OfMonster, DfMonster);

				if (resetGroupCount)
				{
					DfMonster.GroupCount = 0;

					DfMonster.InitGroupCount = 0;

					DfMonster.OrigGroupCount = 0;
				}
			}
			else
			{
				base.MonsterDies(OfMonster, DfMonster);
			}
		}
	}
}
