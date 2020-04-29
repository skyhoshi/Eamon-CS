
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using TheVileGrimoireOfJaldial.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(long eventType)
		{
			if (!Globals.EncounterSurprises)
			{
				if (eventType == PeBeforePlayerRoomPrint && ShouldPreTurnProcess())
				{
					var characterMonster = gMDB[gGameState.Cm];

					Debug.Assert(characterMonster != null);

					var room = characterMonster.GetInRoom() as Framework.IRoom;

					Debug.Assert(room != null);

					var cloakAndCowlArtifact = gADB[44];

					Debug.Assert(cloakAndCowlArtifact != null);

					// Dark hood and small glade

					if (cloakAndCowlArtifact.IsInLimbo())
					{
						var darkHoodMonster = gMDB[21];

						Debug.Assert(darkHoodMonster != null);

						if (darkHoodMonster.IsInLimbo() && gGameState.IsNightTime())
						{
							darkHoodMonster.SetInRoomUid(23);

							if (darkHoodMonster.IsInRoom(room))
							{
								gOut.Print("A mysterious figure suddenly appears, seemingly out of thin air.");
							}
						}
					}

					// Day/night cycle logic

					gGameState.Minute += 5;

					if (gGameState.Minute >= 60)
					{
						gGameState.Hour++;

						gGameState.Minute = 0;
					}

					if (gGameState.Hour >= 24)
					{
						gGameState.Day++;

						gGameState.Hour = 0;
					}

					if (room.IsGroundsRoom())
					{
						if (gGameState.Hour == 6 && gGameState.Minute == 0)
						{
							gOut.Print("A dull grey light creeps into the eastern parts of the sky.");
						}
						else if (gGameState.Hour == 7 && gGameState.Minute == 0)
						{
							if (room.GetWeatherIntensity() > 1)
							{
								gOut.Print("The grey light in the east is brighter now, as a new day dawns.");
							}
							else
							{
								gOut.Print("The sun peeks over the treetops, casting grey shadows through the graveyard.");
							}
						}
						else if (gGameState.Hour == 8 && gGameState.Minute == 0)
						{
							if (room.GetWeatherIntensity() > 1)
							{
								gOut.Print("The pale morning half-light casts a pall over the graveyard.");
							}
							else
							{
								gOut.Print("The sun climbs higher in the sky, warming the cold air.");
							}
						}
						else if (gGameState.Hour == 12 && gGameState.Minute == 0)
						{
							if (room.GetWeatherIntensity() > 1)
							{
								gOut.Print("The hazy mid-day light fully illuminates the landscape.");
							}
							else
							{
								gOut.Print("The hot afternoon sun is directly above you now.");
							}
						}
						else if (gGameState.Hour == 18 && gGameState.Minute == 0)
						{
							if (room.GetWeatherIntensity() > 1)
							{
								gOut.Print("As the evening half-light fades, a gloom settles over the cemetery.");
							}
							else
							{
								gOut.Print("The sun is setting, casting long shadows across the ground.");
							}
						}
						else if (gGameState.Hour == 19 && gGameState.Minute == 0)
						{
							if (room.GetWeatherIntensity() > 1)
							{
								gOut.Print("Your vision dims with the last remnants of twilight.");
							}
							else
							{
								gOut.Print("The sun is gone now, and your path is lit by moonlight.");
							}
						}
						else if (gGameState.Hour == 0 && gGameState.Minute == 0)
						{
							if (room.GetWeatherIntensity() > 1)
							{
								gOut.Print("An oppressive silence hangs over the land at this late-night hour.");
							}
							else
							{
								gOut.Print("Stars twinkle brightly from the dark heavens above.");
							}
						}
					}

					// Weather cycle logic

					var expiredWeather = false;

					if (gGameState.WeatherDuration > 0)
					{
						gGameState.WeatherDuration -= 5;

						if (gGameState.WeatherDuration < 0)
						{
							gGameState.WeatherDuration = 0;
						}
					}

					if (gGameState.WeatherDuration <= 0 && gGameState.WeatherType != WeatherType.None)
					{
						var rl = gEngine.RollDice(1, 99, 0);

						if (rl >= 1 && rl <= 33)
						{
							if (--gGameState.WeatherIntensity <= 0)
							{
								if (room.IsRainyRoom())
								{
									gOut.Print("The rain disappears as quickly as it came.");
								}
								else if (room.IsFoggyRoom())
								{
									gOut.Print("What little of the fog is left dissipates immediately.");
								}

								gGameState.FoggyRoomWeatherIntensity = 0;

								gGameState.FoggyRoom = false;

								gGameState.WeatherType = WeatherType.None;

								expiredWeather = true;
							}
							else
							{
								if (room.IsRainyRoom())
								{
									gOut.Print("The rain lets up a little bit.");
								}
								else if (room.IsFoggyRoom())
								{
									gOut.Print("The fog doesn't look as thick now.");

									if (--gGameState.FoggyRoomWeatherIntensity <= 0)
									{
										gGameState.FoggyRoom = false;
									}
								}

								gGameState.WeatherDuration += gEngine.RollDice(1, 100, 0);
							}
						}
						else if (rl >= 34 && rl <= 75)
						{
							gGameState.WeatherDuration += gEngine.RollDice(1, 100, 0);
						}
						else
						{
							if (gGameState.WeatherIntensity < 4)
							{
								if (room.IsRainyRoom())
								{
									gOut.Print("The rain is coming down a bit harder now.");
								}
								else if (room.IsFoggyRoom())
								{
									gOut.Print("The fog is harder to see through now.");

									gGameState.FoggyRoomWeatherIntensity++;
								}

								gGameState.WeatherIntensity++;
							}

							gGameState.WeatherDuration += gEngine.RollDice(1, 100, 0);
						}
					}

					// Random events

					Globals.EventRoll = gEngine.RollDice(1, 120, 0);

					// Flavor effects

					if (Globals.EventRoll <= 5)
					{
						var idx = gEngine.RollDice(1, 8, -1);

						if (room.IsGroundsRoom())
						{
							var rl = gEngine.RollDice(1, 9, 0);
							 
							if (rl <= 5)
							{
								if (rl >= 3 || !room.IsRainyRoom())
								{
									var effect = gEDB[rl];

									Debug.Assert(effect != null);

									gOut.Print("{0}", rl == 3 && room.IsRainyRoom() ? "You notice the storm clouds swiftly rolling by overhead." : effect.Desc);
								}
							}

							// Distant graveyard sounds

							else if (rl == 6)
							{
								var distantSounds = new string[]
								{
									"what seems to be loud footfalls.", "a shrill scream - possibly human, but probably not.", "the sounds of wildlife - crickets, and bullfrogs.",
									"the muffled beat of a drum.", "the whisper of wind through the trees.", "a loud crashing sound.", "the sounds of battle!", "a peal of thunder."
								};

								gOut.Print("You hear, in the distance, {0}", distantSounds[(int)idx]);
							}

							// Nearby graveyard sounds

							else if (rl == 7)
							{
								var nearbySounds = new string[]
								{
									"what sounds like footsteps.", "a strange hissing sound.", "the faint sounds of sobbing.", "the rustling of leaves.", "the chirping of birds.",
									"quiet laughter.", "a faint humming sound.", "a faint clicking sound."
								};

								gOut.Print("You hear, very close by, {0}", nearbySounds[(int)idx]);
							}

							// Graveyard aromas

							else if (rl == 8)
							{
								var aromas = new string[]
								{
									"meat frying on an open skillet.", "the putrid odor of decaying flesh.", "the aroma of vanilla.", "the odor of offal.", "a refreshing pine scent, carried by the wind.",
									"what can only be described as ancient death.", "a cloying sweet aroma, that of flowers.", "the reeking odor of swamp methane."
								};

								gOut.Print("You smell {0}", aromas[(int)idx]);
							}

							// Graveyard sightings

							else
							{
								var sightings = new string[]
								{
									"a stark figure, who disappears behind a tombstone.", string.Format("a bolt of lightning far to the {0}.", gEngine.GetRandomElement(new string[] { "north", "south", "east", "west" })),
									"ephemeral wisps of steam rising from the damp earth.", string.Format("a {0}, which piques your interest for a fleeting moment.", gEngine.GetRandomElement(new string[] { "bush", "tree", "rock", "tombstone" })),
									string.Format("a rare species of {0}, found only here.", gEngine.GetRandomElement(new string[] { "warbler", "gecko", "lichen", "mandrake root" })), "an unintelligible symbol scratched into the ground.",
									"a tombstone with an oddly familiar name on it.", string.Format("a {0} flying high overhead.", gEngine.GetRandomElement(new string[] { "crow", "vulture", "dragon", "raven" }))
								};

								gOut.Print("You see {0}", sightings[(int)idx]);
							}
						}
						else if (room.IsCryptRoom())
						{
							var rl = gEngine.RollDice(1, 3, 0);

							// Distant underground sounds

							if (rl == 1)
							{
								var distantSounds = new string[] 
								{ 
									"a hollow booming sound that echoes down the passage.", "the quiet thud of footsteps.", "a soft creaking sound.", "a strange whirring sound.", 
									"the whistle of wind down a passageway.", "the quiet rattling of chains.", "a faint clicking sound that echoes down the passage.",
									"a blood-curdling scream!"
								};

								gOut.Print("You hear, in the distance, {0}", distantSounds[(int)idx]);
							}

							// Nearby underground sounds

							else if (rl == 2)
							{
								var nearbySounds = new string[] 
								{ 
									"a quiet conversation.", "a coughing sound, which quickly dies away.", "a wheezing sound.", "footfalls.", "cackling laughter.", 
									"the sound of water dripping from the ceiling.", "a faint moaning which echoes down the corridors.", "a faint thud."
								};

								gOut.Print("You hear, very close by, {0}", nearbySounds[(int)idx]);
							}

							// Underground aromas

							else
							{
								var aromas = new string[] 
								{ 
									"a fresh breeze that blows down the hallway.", "a putrid odor, probably unseen offal somewhere.", "the air, which grows increasingly stale.",
									"a rotting odor, quite unpleasant.", "yourself; you've been sweating again!", "the strong aroma of vanilla.", "the reek of decaying flesh.",
									"a cloying aroma, probably plant life somewhere nearby."
								};

								gOut.Print("You smell {0}", aromas[(int)idx]);
							}
						}
					}

					// Weather pattern starters

					else if (!expiredWeather && Globals.EventRoll >= 6 && Globals.EventRoll <= 15)
					{
						if (gGameState.WeatherType == WeatherType.None)
						{
							var rl = gEngine.RollDice(1, 2, 0);

							if (rl == 1 || !gGameState.IsFoggyHours())
							{
								if (room.IsGroundsRoom())
								{
									gOut.Print("Suddenly, the clouds open up, and it begins to rain.");
								}

								gGameState.WeatherType = WeatherType.Rain;

								gGameState.WeatherDuration = gEngine.RollDice(1, 100, 0);

								gGameState.WeatherIntensity = 1;
							}
							else
							{
								if (room.IsGroundsRoom())
								{
									gOut.Print("As you watch, patches of fog begin to roll across the landscape.");
								}

								gGameState.WeatherType = WeatherType.Fog;

								gGameState.WeatherDuration = gEngine.RollDice(1, 60, 0);

								gGameState.WeatherIntensity = 1;

								while (room.IsGroundsRoom() && !room.IsFoggyRoom())
								{
									gGameState.SetFoggyRoom(room);
								}
							}
						}
					}

					// Encounters

					else if ((room.IsGroundsRoom() && Globals.EventRoll >= 16 && Globals.EventRoll <= 20) || (room.IsCryptRoom() && Globals.EventRoll >= 16 && Globals.EventRoll <= 23))
					{
						var enemyEncounter = gEngine.GetMonsterList(m => m.Uid <= 17 && m.IsInRoom(room) && m.Friendliness == Friendliness.Enemy).FirstOrDefault();

						if (enemyEncounter == null)
						{
							IList<IMonster> encounterList = null;

							if (room.IsGroundsRoom())
							{
								encounterList = gEngine.GetMonsterList(m => m.Uid <= 17 && m.IsInLimbo() && (gGameState.IsNightTime() || m.Uid <= 6));
							}
							else
							{
								var invalidMonsterUids = new long[] { 2, 5, 10, 12 };

								encounterList = gEngine.GetMonsterList(m => m.Uid <= 17 && m.IsInLimbo() && !invalidMonsterUids.Contains(m.Uid));
							}

							if (encounterList.Count > 0)
							{
								var idx = gEngine.RollDice(1, encounterList.Count, -1);

								var monster = encounterList[(int)idx];

								if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
								{
									gOut.Print("{0} hears the sounds of battle, and comes wandering by.", room.EvalLightLevel("Something", monster.GetArticleName(true)));
								}
								else if (room.IsFoggyRoom())
								{
									gOut.Print("{0} suddenly materializes out of the fog.", monster.GetArticleName(true));
								}
								else if (room.IsRainyRoom() && room.GetWeatherIntensity() > 2)
								{
									gOut.Print("{0} suddenly materializes out of the rain.", monster.GetArticleName(true));
								}
								else if (room.IsGroundsRoom())
								{
									gOut.Print("{0} wanders into the area!", monster.GetArticleName(true));
								}
								else
								{
									gOut.Print("{0} wanders into the room!", room.EvalLightLevel("Something", monster.GetArticleName(true)));
								}

								monster.GroupCount = monster.OrigGroupCount;

								monster.InitGroupCount = monster.OrigGroupCount;

								monster.DmgTaken = 0;

								monster.SetInRoom(room);

								var saved = gEngine.SaveThrow(Stat.Agility);

								if (!saved)
								{
									gOut.Print("You have been taken by surprise!");

									Globals.InitiativeMonsterUid = monster.Uid;

									Globals.EncounterSurprises = true;

									NextState = Globals.CreateInstance<IMonsterStartState>();

									GotoCleanup = true;
								}
							}
						}
					}

					// Random encounters not in player room go poof

					var monsterList = gEngine.GetMonsterList(m => m.Uid <= 17 && !m.IsInLimbo() && !m.IsInRoom(room));

					foreach (var monster in monsterList)
					{
						var rl = gEngine.RollDice(1, 100, 0);

						if (rl > 50)
						{
							monster.SetInLimbo();
						}
					}
				}

				base.ProcessEvents(eventType);
			}
			else
			{
				Globals.EncounterSurprises = false;
			}
		}
	}
}
