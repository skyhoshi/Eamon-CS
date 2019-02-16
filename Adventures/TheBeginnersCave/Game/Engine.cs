
// Engine.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Classes = Eamon.Framework.Primitive.Classes;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		protected virtual long HeldWpnIdx { get; set; }

		protected override void PrintTooManyWeapons()
		{
			Globals.Out.Print("As you leave for the Main Hall, the Knight Marshal reappears and tells you, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				if (Globals.GameState.CastTo<Framework.IGameState>().Trollsfire == 1)
				{
					return string.Format("{0}{0}Trollsfire is alight!", Environment.NewLine);
				}
				else
				{
					return "";
				}
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 3, new string[] { "label", "strange potion", "potion", "bottle" } },
				{ 14, new string[] { "east wall", "wall", "smooth shape", "shape", "secret door", "door", "passage", "tunnel" } },
				{ 15, new string[] { "water", "sea water", "ocean water" } },
				{ 24, new string[] { "broken old boat", "old broken boat", "broken boat", "old boat" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			var pirateMonster = Globals.MDB[8];

			Debug.Assert(pirateMonster != null);

			if (pirateMonster.Weapon == 10)
			{
				Globals.GameState.CastTo<Framework.IGameState>().Trollsfire = 1;
			}
		}

		public override IArtifact ConvertWeaponToArtifact(Classes.ICharacterArtifact weapon)
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

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
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

		public override void MonsterDies(IMonster OfMonster, IMonster DfMonster)
		{
			Debug.Assert(DfMonster != null && !DfMonster.IsCharacterMonster());

			if (DfMonster.Uid == 8)
			{
				var trollsfireArtifact = Globals.ADB[10];

				Debug.Assert(trollsfireArtifact != null);

				var printEffect = trollsfireArtifact.IsCarriedByMonster(DfMonster) || trollsfireArtifact.IsWornByMonster(DfMonster);

				base.MonsterDies(OfMonster, DfMonster);

				if (printEffect)
				{
					var effect = Globals.EDB[3];

					Debug.Assert(effect != null);

					Globals.Out.Write("{0}{0}{1}", Environment.NewLine, effect.Desc);
				}
			}
			else
			{
				base.MonsterDies(OfMonster, DfMonster);
			}
		}
	}
}
