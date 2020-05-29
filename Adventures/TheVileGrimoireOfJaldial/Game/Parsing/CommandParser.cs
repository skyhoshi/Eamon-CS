
// CommandParser.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using TheVileGrimoireOfJaldial.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void ParseName()
		{
			base.ParseName();

			var a = gADB[ObjData == DobjData ? 41 : 42];

			Debug.Assert(a != null);

			a.Name = ObjData == DobjData ? "DECORATION41" : "DECORATION42";

			// Decorations remain in limbo unless the normal Artifact resolution process fails

			a.Location = 0;

			a.Field1 = 0;

			a.Field2 = 0;

			// Examined decorations

			if ((gActorRoom.Uid == 1 || gActorRoom.Uid == 4) && ObjData.Name.Contains("gate", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 1;
			}
			else if ((gActorRoom.Uid == 11 || gActorRoom.Uid == 16 || gActorRoom.Uid == 22) && ObjData.Name.ContainsAny(new string[] { "brook", "stream" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 5;
			}
			else if (gActorRoom.Uid == 12 && ObjData.Name.ContainsAny(new string[] { "pile", "offal" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 6;
			}
			else if (gActorRoom.Uid == 12 && ObjData.Name.Contains("rat", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 7;
			}
			else if (gActorRoom.Uid == 13 && ObjData.Name.ContainsAny(new string[] { "pile", "rocks", "pyramid" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 8;
			}
			else if (gActorRoom.Uid == 8 && ObjData.Name.Contains("elm", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 9;
			}
			else if (gActorRoom.Uid == 19 && ObjData.Name.ContainsAny(new string[] { "hole", "grave" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 10;
			}
			else if (gActorRoom.Uid == 20 && ObjData.Name.ContainsAny(new string[] { "skeleton", "animal", "creature" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 11;
			}
			else if (gActorRoom.Uid == 23 && ObjData.Name.Contains("pine", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 12;
			}
			else if (gActorRoom.Uid == 26 && ObjData.Name.ContainsAny(new string[] { "coffin", "hand", "skeleton" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 13;
			}
			else if (gActorRoom.Uid == 56 && ObjData.Name.ContainsAny(new string[] { "heap", "pile", "bone" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 15;
			}
			else if (gActorRoom.Uid == 62 && ObjData.Name.ContainsAny(new string[] { "fresco", "mural", "painting" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 17;
			}
			else if (gActorRoom.Uid == 62 && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 18;
			}
			else if ((gActorRoom.Uid == 64 || gActorRoom.Uid == 65) && ObjData.Name.Contains("door", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 19;
			}
			else if (gActorRoom.Uid == 65 && ObjData.Name.ContainsAny(new string[] { "fluid", "blood" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 20;
			}
			else if ((gActorRoom.Uid == 64 || gActorRoom.Uid == 65) && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 21;
			}
			else if (gActorRoom.Uid == 66 && ObjData.Name.ContainsAny(new string[] { "skeleton", "leather", "armor" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 22;
			}
			else if (gActorRoom.Uid == 66 && ObjData.Name.Contains("wall", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 23;
			}
			else if (gActorRoom.Uid == 68 && ObjData.Name.Contains("moss", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 24;
			}
			else if (gActorRoom.Uid == 69 && ObjData.Name.Contains("moss", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 25;
			}
			else if (gActorRoom.Uid == 69 && ObjData.Name.Contains("box", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 26;
			}
			else if (gActorRoom.Uid == 71 && ObjData.Name.Contains("alga", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 27;
			}
			else if (gActorRoom.Uid == 70 && ObjData.Name.Contains("groove", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 28;
			}
			else if (gActorRoom.Uid == 71 && ObjData.Name.Contains("bow", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 29;
			}
			else if (gActorRoom.Uid == 71 && ObjData.Name.Contains("hole", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 30;
			}
			else if (gActorRoom.Uid == 72 && ObjData.Name.ContainsAny(new string[] { "cloth", "strip" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 31;
			}
			else if (gActorRoom.Uid == 72 && ObjData.Name.ContainsAny(new string[] { "rock", "pile" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 32;
			}
			else if (gActorRoom.Uid == 73 && ObjData.Name.Contains("mummy", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 33;
			}
			else if (gActorRoom.Uid == 74 && ObjData.Name.ContainsAny(new string[] { "spider", "web" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 34;
			}
			else if (gActorRoom.Uid == 75 && ObjData.Name.ContainsAny(new string[] { "fresco", "mural", "painting" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 35;
			}
			else if (gActorRoom.Uid == 75 && ObjData.Name.ContainsAny(new string[] { "glyph", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 36;
			}
			else if (gActorRoom.Uid == 76 && ObjData.Name.Contains("etching", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 37;
			}
			else if (gActorRoom.Uid == 77 && ObjData.Name.Contains("face", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 38;
			}
			else if (gActorRoom.Uid == 82 && ObjData.Name.ContainsAny(new string[] { "goblin", "body" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 39;
			}
			else if (gActorRoom.Uid == 82 && ObjData.Name.ContainsAny(new string[] { "chain", "armor" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 40;
			}
			else if (gActorRoom.Uid == 84 && ObjData.Name.ContainsAny(new string[] { "shiny", "substance", "slime" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 41;
			}
			else if (gActorRoom.Uid == 84 && ObjData.Name.ContainsAny(new string[] { "boot", "mound", "earth" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 42;
			}
			else if (gActorRoom.Uid == 86 && ObjData.Name.ContainsAny(new string[] { "goblin", "bodies", "body" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 43;
			}
			else if (gActorRoom.Uid == 86 && ObjData.Name.ContainsAny(new string[] { "spoor", "dung" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 44;
			}
			else if (gActorRoom.Uid == 87 && ObjData.Name.ContainsAny(new string[] { "fog", "mist" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 45;
			}
			else if (gActorRoom.Uid == 88 && ObjData.Name.ContainsAny(new string[] { "pick", "marks" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 46;
			}
			else if (gActorRoom.Uid == 89 && ObjData.Name.ContainsAny(new string[] { "tapestries", "tapestry" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 47;
			}
			else if ((gActorRoom.Uid == 90 || gActorRoom.Uid == 93) && ObjData.Name.ContainsAny(new string[] { "mining", "tool" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 48;
			}
			else if (gActorRoom.Uid == 95 && ObjData.Name.ContainsAny(new string[] { "skeletal", "arm" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 49;
			}
			else if (gActorRoom.Uid == 96 && ObjData.Name.ContainsAny(new string[] { "pit", "hole" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 50;
			}
			else if (gActorRoom.Uid == 101 && ObjData.Name.Contains("skeleton", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 51;
			}
			else if (gActorRoom.Uid == 102 && ObjData.Name.ContainsAny(new string[] { "stain", "blood" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 52;
			}
			else if (gActorRoom.Uid == 103 && ObjData.Name.ContainsAny(new string[] { "etching", "carving" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 53;
			}
			else if (gActorRoom.Uid == 104 && ObjData.Name.ContainsAny(new string[] { "face", "mouth", "hole" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 54;
			}
			else if (gActorRoom.Uid == 105 && ObjData.Name.ContainsAny(new string[] { "pile", "bodies", "body" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 55;
			}
			else if (gActorRoom.Uid == 108 && ObjData.Name.ContainsAny(new string[] { "beach", "sea", "ocean" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 56;
			}
			else if (gActorRoom.Uid == 13 && ObjData.Name.Contains("pictograph", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 58;
			}
			else if (gActorRoom.Uid == 100 && ObjData.Name.Contains("rune", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 59;
			}
			else if (gActorRoom.Uid == 110 && ObjData.Name.Contains("message", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 60;
			}
			else if (gActorRoom.IsFenceRoom() && ObjData.Name.Contains("fence", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 2;
			}
			else if (gActorRoom.IsGroundsRoom() && ObjData.Name.ContainsAny(new string[] { "foliage", "trees", "forest", "weeds", "plants", "grass", "lichen", "moss" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 3;
			}
			else if (gActorRoom.IsGroundsRoom() && gActorRoom.Uid != 16 && gActorRoom.Uid != 23 && gActorRoom.Uid != 39 && ObjData.Name.ContainsAny(new string[] { "tombstone", "gravestone" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 4;
			}
			else if (gActorRoom.IsCryptRoom() && ObjData.Name.ContainsAny(new string[] { "floor", "dust" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 14;
			}
			else if (gActorRoom.IsBodyChamberRoom() && ObjData.Name.ContainsAny(new string[] { "body", "bodies", "internment", "opening", "chamber" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 16;
			}
			else if (gActorRoom.IsGroundsRoom() && gActorRoom.Uid != 16 && gActorRoom.Uid != 23 && gActorRoom.Uid != 39 && ObjData.Name.Contains("epitaph", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 57;
			}

			// Read decorations

			if (gActorRoom.Uid == 13 && ObjData.Name.Contains("pictograph", StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 2;
			}
			else if (gActorRoom.Uid == 62 && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 3;
			}
			else if ((gActorRoom.Uid == 64 || gActorRoom.Uid == 65) && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 4;
			}
			else if (gActorRoom.Uid == 75 && ObjData.Name.ContainsAny(new string[] { "glyph", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 5;
			}
			else if (gActorRoom.Uid == 100 && ObjData.Name.Contains("rune", StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 6;
			}
			else if (gActorRoom.Uid == 103 && ObjData.Name.ContainsAny(new string[] { "etching", "carving" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 7;
			}
			else if (gActorRoom.Uid == 110 && ObjData.Name.Contains("message", StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 8;
			}
			else if (gActorRoom.IsGroundsRoom() && gActorRoom.Uid != 16 && gActorRoom.Uid != 23 && gActorRoom.Uid != 39 && ObjData.Name.ContainsAny(new string[] { "tombstone", "gravestone", "epitaph" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 1;
			}

			if (a.Field1 > 0 || a.Field2 > 0)
			{
				a.Name = Globals.CloneInstance(ObjData.Name);

				// Make note of the Decoration so it can be used later if the normal Artifact resolution process fails

				ObjData.Cast<Framework.Parsing.IParserData>().DecorationArtifact = a;
			}
		}

		public override void CheckPlayerCommand(ICommand command, bool afterFinishParsing)
		{
			Debug.Assert(command != null);

			if (afterFinishParsing)
			{
				// Restrict various commands while paralyzed

				if (!(command is ISmileCommand || (command.Type == CommandType.Miscellaneous && !(command is ISpeedCommand || command is IPowerCommand))) && gGameState.ParalyzedTargets.ContainsKey(gGameState.Cm))
				{
					gOut.Print("You can't do that while paralyzed!");

					NextState = Globals.CreateInstance<IStartState>();
				}

				// Restrict GiveCommand and RequestCommand when targeting paralyzed Monster

				else if ((command is IGiveCommand || command is IRequestCommand) && gIobjMonster != null && gGameState.ParalyzedTargets.ContainsKey(gIobjMonster.Uid))
				{
					gOut.Print("You can't do that while {0} {1} paralyzed!", gIobjMonster.GetTheName(), gIobjMonster.EvalPlural("is", "are"));

					NextState = Globals.CreateInstance<IStartState>();
				}

				// Restrict SearchCommand while enemies are present

				else if (command is ISearchCommand && gGameState.GetNBTL(Friendliness.Enemy) > 0)
				{
					command.PrintEnemiesNearby();

					NextState = Globals.CreateInstance<IStartState>();
				}

				// Restrict Commands in the graveyard at night or in heavy fog

				else if ((command is IReadCommand || command is ISearchCommand) && gActorRoom.IsDimLightRoom() && gGameState.Ls <= 0)
				{
					gOut.Print("You'll need a bit more light for that!");

					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
				else
				{
					var waterWeirdMonster = gMDB[38];

					Debug.Assert(waterWeirdMonster != null);

					// Large fountain and water weird

					if (gDobjArtifact != null && gDobjArtifact.Uid != 24 && gDobjArtifact.Uid != 40 && gIobjArtifact != null && gIobjArtifact.Uid == 24)
					{
						if (waterWeirdMonster.IsInRoom(gActorRoom))
						{
							gOut.Print("{0} won't let you get close enough to do that!", waterWeirdMonster.GetTheName(true));

							NextState = Globals.CreateInstance<IMonsterStartState>();
						}
						else if (!gGameState.WaterWeirdKilled)
						{
							gEngine.PrintEffectDesc(100);

							waterWeirdMonster.SetInRoom(gActorRoom);

							NextState = Globals.CreateInstance<IStartState>();
						}
						else
						{
							base.CheckPlayerCommand(command, afterFinishParsing);
						}
					}
					else
					{
						base.CheckPlayerCommand(command, afterFinishParsing);
					}
				}
			}
			else
			{
				base.CheckPlayerCommand(command, afterFinishParsing);
			}
		}
	}
}
