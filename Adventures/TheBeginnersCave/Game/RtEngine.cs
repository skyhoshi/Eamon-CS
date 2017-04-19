
// RtEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using TheBeginnersCave.Framework;
using Classes = Eamon.Framework.Primitive.Classes;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IRtEngine))]
	public class RtEngine : EamonRT.Game.RtEngine, IRtEngine
	{
		protected virtual long HeldWpnIdx { get; set; }

		protected override void PrintTooManyWeapons()
		{
			Globals.Out.Write("{0}As you leave for the Main Hall, the Knight Marshal reappears and tells you, \"You have too many weapons to keep them all, four is the legal limit.\"{0}", Environment.NewLine);
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			Globals.Engine.MacroFuncs.Add(1, () =>
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

			Globals.RtEngine.CreateArtifactSynonyms(3, "label", "strange potion", "potion", "bottle");

			Globals.RtEngine.CreateArtifactSynonyms(14, "east wall", "wall", "smooth shape", "shape", "secret door", "door", "passage", "tunnel");

			Globals.RtEngine.CreateArtifactSynonyms(15, "water", "sea water", "ocean water");

			Globals.RtEngine.CreateArtifactSynonyms(24, "broken old boat", "old broken boat", "broken boat", "old boat");
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			var monster = Globals.MDB[8];

			Debug.Assert(monster != null);

			if (monster.Weapon == 10)
			{
				Globals.GameState.CastTo<Framework.IGameState>().Trollsfire = 1;
			}
		}

		public override IArtifact ConvertWeaponToArtifact(Classes.ICharacterWeapon weapon)
		{
			var artifact = base.ConvertWeaponToArtifact(weapon);

			var gameState = Globals.GameState as Framework.IGameState;

			var i = Globals.Engine.FindIndex(Globals.Character.Weapons, x => x == weapon);

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

		public override void MonsterDies(Eamon.Framework.IMonster OfMonster, Eamon.Framework.IMonster DfMonster)
		{
			Debug.Assert(DfMonster != null && !DfMonster.IsCharacterMonster());

			if (DfMonster.Uid == 8)
			{
				var artifact = Globals.ADB[10];

				Debug.Assert(artifact != null);

				var printEffect = artifact.IsCarriedByMonster(DfMonster) || artifact.IsWornByMonster(DfMonster);

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
