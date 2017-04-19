
// RtEngine.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework;
using Classes = Eamon.Framework.Primitive.Classes;
using Enums = Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game
{
	[ClassMappings(typeof(EamonRT.Framework.IRtEngine))]
	public class RtEngine : EamonRT.Game.RtEngine, IRtEngine
	{
		protected override void PrintTooManyWeapons()
		{
			Globals.Out.Write("{0}As you enter the Main Hall, Lord William Crankhandle approaches you and says, \"You have too many weapons to keep them all, four is the legal limit.\"{0}", Environment.NewLine);
		}

		protected override void PrintDeliverGoods()
		{
			Globals.Out.Write("{0}As Sam Slicker, the local buyer of treasure is vacationing, you grant yourself the gold he would have given you.{0}", Environment.NewLine);
		}

		protected override void PrintGoodsPayment(bool goodsExist, long payment)
		{
			Globals.Out.Write("{0}{1}You take {2} gold pieces total.{0}", Environment.NewLine, goodsExist ? Environment.NewLine : "", payment);
		}

		protected override void PrintMonsterAlive(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Bill in oven, Lil in cell

			if (artifact.Uid != 82 && artifact.Uid != 135)
			{
				base.PrintMonsterAlive(artifact);
			}
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			Globals.Engine.MacroFuncs.Add(1, () =>
			{
				var artifact = Globals.ADB[129];

				Debug.Assert(artifact != null);

				var monster = Globals.MDB[37];

				Debug.Assert(monster != null);

				if (artifact.IsCarriedByMonster(monster))
				{
					return ", dropping the \"Runcible Cargo\" at your feet as she disappears.";
				}
				else
				{
					return ".";
				}
			});

			Globals.Engine.MacroFuncs.Add(2, () =>
			{
				if (Globals.GameState != null)
				{
					var artifact = Globals.ADB[Globals.GameState.Ro == 20 ? 136 : 17];

					Debug.Assert(artifact != null);

					var ac = artifact.GetArtifactClass(Enums.ArtifactType.DoorGate);

					Debug.Assert(ac != null);

					return ac.Field6 != 0 ? "  You've locked yourself out." : "";
				}
				else
				{
					return "";
				}
			});

			Globals.Engine.MacroFuncs.Add(3, () =>
			{
				if (Globals.GameState != null)
				{
					var monster = Globals.MDB[37];

					Debug.Assert(monster != null);

					return monster.Friendliness > Enums.Friendliness.Enemy ? "Larcenous Lil prepares to attack!" : "";
				}
				else
				{
					return "";
				}
			});

			var artUids = new long[]
			{
				25, 37, 69, 73, 74, 78, 80, 89, 90, 95, 118, 129, 136, 
			};

			var synonyms = new List<string[]>()
			{
				new string[] { "lantern" },
				new string[] { "bureau", "drawers" },
				new string[] { "window" },
				new string[] { "sign" },
				new string[] { "window" },
				new string[] { "refrigerator", "fridge" },
				new string[] { "oven" },
				new string[] { "sign" },
				new string[] { "sign" },
				new string[] { "lantern" },
				new string[] { "desks" },
				new string[] { "cargo" },
				new string[] { "rear door", "shop door", "door" },
			};

			for (var i = 0; i < artUids.Length; i++)
			{
				Globals.RtEngine.CreateArtifactSynonyms(artUids[i], synonyms[i]);
			}

			// Signs (Sam's/weathered/supported/station)

			var artifact01 = Globals.ADB[16];

			Debug.Assert(artifact01 != null);

			artifact01.Seen = true;

			artifact01 = Globals.ADB[73];

			Debug.Assert(artifact01 != null);

			artifact01.Seen = true;

			artifact01 = Globals.ADB[89];

			Debug.Assert(artifact01 != null);

			artifact01.Seen = true;

			artifact01 = Globals.ADB[90];

			Debug.Assert(artifact01 != null);

			artifact01.Seen = true;

			// (Barney) Rubble, Maintenance grate, Sewer grate

			Globals.DoubleDoors.Add(Globals.CreateInstance<Classes.IArtifactLinkage>(x => 
			{
				x.RoomUid = 12;
				x.ArtifactUid1 = 18;
				x.ArtifactUid2 = 139;
			}));

			Globals.DoubleDoors.Add(Globals.CreateInstance<Classes.IArtifactLinkage>(x =>
			{
				x.RoomUid = 13;
				x.ArtifactUid1 = 139;
				x.ArtifactUid2 = 18;
			}));

			Globals.DoubleDoors.Add(Globals.CreateInstance<Classes.IArtifactLinkage>(x =>
			{
				x.RoomUid = 13;
				x.ArtifactUid1 = 24;
				x.ArtifactUid2 = 140;
			}));

			Globals.DoubleDoors.Add(Globals.CreateInstance<Classes.IArtifactLinkage>(x =>
			{
				x.RoomUid = 14;
				x.ArtifactUid1 = 140;
				x.ArtifactUid2 = 24;
			}));

			Globals.DoubleDoors.Add(Globals.CreateInstance<Classes.IArtifactLinkage>(x =>
			{
				x.RoomUid = 17;
				x.ArtifactUid1 = 26;
				x.ArtifactUid2 = 138;
			}));

			Globals.DoubleDoors.Add(Globals.CreateInstance<Classes.IArtifactLinkage>(x =>
			{
				x.RoomUid = 29;
				x.ArtifactUid1 = 138;
				x.ArtifactUid2 = 26;
			}));
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			// Can't sell Sam Slicker's shop key

			var artifact = Globals.ADB[9];

			Debug.Assert(artifact != null);

			artifact.SetInLimbo();

			base.ConvertToCarriedInventory(weaponList);
		}

		public override void MonsterGetsAggravated(Eamon.Framework.IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			// Keep Pookas Neutral

			if (monster.Uid > 3)
			{
				base.MonsterGetsAggravated(monster, printFinalNewLine);

				// Sync friendliness of Commander & troops, Prince & Guards

				if (monster.Uid == 27 || monster.Uid == 28)
				{
					var monster01 = Globals.MDB[monster.Uid == 27 ? 28 : 27];

					Debug.Assert(monster01 != null);

					base.MonsterGetsAggravated(monster01, printFinalNewLine);
				}
				else if (monster.Uid == 38 || monster.Uid == 39)
				{
					var monster01 = Globals.MDB[monster.Uid == 38 ? 39 : 38];

					Debug.Assert(monster01 != null);

					base.MonsterGetsAggravated(monster01, printFinalNewLine);
				}
			}
		}

		public RtEngine()
		{
			AddPoundCharsToCharOwnedArtifactNames = true;
		}
	}
}
