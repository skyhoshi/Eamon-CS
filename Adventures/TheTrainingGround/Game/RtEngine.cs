
// RtEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Diagnostics;
using Eamon.Game.Attributes;
using TheTrainingGround.Framework;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IRtEngine))]
	public class RtEngine : EamonRT.Game.RtEngine, IRtEngine
	{
		public override void AddPoundCharsToArtifactNames()
		{
			base.AddPoundCharsToArtifactNames();

			// Obsidian scroll case

			var artifact = Globals.ADB[51];

			Debug.Assert(artifact != null);

			artifact.Seen = true;

			artifact.Name = artifact.Name.TrimEnd('#');

			// Graffiti

			for (var i = 46; i <= 50; i++)
			{
				artifact = Globals.ADB[i];

				Debug.Assert(artifact != null);

				artifact.Seen = true;

				artifact.Name = artifact.Name.TrimEnd('#');
			}
		}

		public override void PrintMonsterAlive(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Obsidian scroll case

			if (artifact.Uid != 30)
			{
				base.PrintMonsterAlive(artifact);
			}
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			// Graffiti

			for (var i = 46; i <= 50; i++)
			{
				CreateArtifactSynonyms(i, "walls", "wall", "writing", "writings", "marks", "markings");
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			// Kobold6 is mentioned in Kobold5's description

			var monster = Globals.MDB[11];

			Debug.Assert(monster != null);

			monster.Seen = true;
		}

		public override void RevealDisguisedMonster(Eamon.Framework.IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			base.RevealDisguisedMonster(artifact);

			// Replace obsidian scroll case with dummy

			if (artifact.Uid == 30)
			{
				var scrollCaseArtifact = Globals.ADB[51];

				Debug.Assert(scrollCaseArtifact != null);

				scrollCaseArtifact.SetInRoomUid(Globals.GameState.Ro);
			}
		}
	}
}
