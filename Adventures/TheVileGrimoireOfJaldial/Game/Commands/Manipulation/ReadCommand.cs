
// ReadCommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void PlayerExecute()
		{
			// Solitary tombstone

			if (gDobjArtifact?.Uid == 10)
			{
				gOut.Print("You stare at the epitaph intently, trying to read its weather-worn message:");

				gOut.WriteLine();

				gEngine.PrintTitle("Here under this stone lies wise sage Druce,".PadTRight(44, ' '), false);
				gEngine.PrintTitle("Who in his time spoke many truths.".PadTRight(44, ' '), false);
				gEngine.PrintTitle("Just how he died none do now know,".PadTRight(44, ' '), false);
				gEngine.PrintTitle("It's thought he was killed by an unseen foe.".PadTRight(44, ' '), false);

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}

			// Decoration

			else if (gDobjArtifact?.Uid == 41 && gDobjArtifact.Field2 > 0)
			{
				switch (gDobjArtifact.Field2)
				{
					case 1:
					{
						var firstNames = new string[] { "Joala", "Skulian", "Kalsil", "Torlal", "Quald", "Olsaian", "Lianda", "Slobalan", "Yeouil", "Geoamlo" };

						var middleNames = new string[] { "Iliom", "Uilalma", "Polais", "Eliuo", "Malamdi", "Didosal", "Alalkdi", "Jaldian", "Zloana", "Voquilana" };

						var lastNames = new string[] { "Aluionsa", "Opuloanaks", "Yoalamas", "Telaoam", "Dalsodid", "Ralpodkin", "Makdidtoala", "Salaidodona", "Hvolala", "Codlaeido" };

						var epitaphs = new string[]
						{
							"live better in the afterlife than here", "rest in pieces", "aspire to godsent ethics", "rest in peace", "fulfill the prescribed destiny which awaits",
							"die a thousand times more before finally resting", "listen next time with more interest", "show better timing with the next departure", "live forever in our memories",
							"be treated above as life was lived down here"
						};

						gOut.Print("You search out a tombstone, and read the epitaph:");

						gOut.WriteLine();

						gEngine.PrintTitle("HERE LIES", false);

						var fullName = string.Format("{0}  {1}  {2}", firstNames[gEngine.RollDice(1, 10, -1)], middleNames[gEngine.RollDice(1, 10, -1)], lastNames[gEngine.RollDice(1, 10, -1)]);

						gOut.WriteLine();

						gEngine.PrintTitle(fullName, false);

						var birthDate = gEngine.RollDice(1, 501, 999);

						var deathDate = birthDate + gEngine.RollDice(1, 100, 0);

						var dateRange = string.Format("{0}-{1}", birthDate, deathDate);

						gOut.WriteLine();

						gEngine.PrintTitle(dateRange, false);

						var gender = gEngine.RollDice(1, 100, 0) > 50 ? Gender.Male : Gender.Female;

						var epitaph = string.Format("May {0} {1}.", gEngine.EvalGender(gender, "he", "she", "it"), epitaphs[gEngine.RollDice(1, 10, -1)]);

						gOut.WriteLine();

						gEngine.PrintTitle(epitaph, false);

						break;
					}

					case 2:

						gOut.Print("The pictographs read:");

						gOut.WriteLine();

						gEngine.PrintTitle("Here I died many seasons ago.".PadTRight(35, ' '), false);
						gEngine.PrintTitle("My brittle bones beneath your toes.".PadTRight(35, ' '), false);
						gEngine.PrintTitle("What time I had I spent it well.".PadTRight(35, ' '), false);
						gEngine.PrintTitle("Or so I thought - I'm now in...".PadTRight(35, ' '), false);

						break;

					case 3:
					case 4:
					case 5:

						var command = Globals.CreateInstance<IExamineCommand>();

						CopyCommandData(command);

						NextState = command;

						break;

					case 6:

						gOut.Print("Not much left that's legible now - besides which, I don't think you know the tongue in which it is written.");

						break;

					case 7:

						gOut.Print("The etchings seem to deal with a hero of popular folklore.  According to the inscription, a long time ago, a great scourge brought strife to the land.  A peasant was called upon to do the work at which many knights had failed.  A giant had taken up residence nearby, and the man succeeded in slaying the creature.  He was then knighted at a grand ceremony held by the King in his name.");

						break;

					case 8:

						gOut.Print("The message reads:");

						gOut.Print("From the foundations of the earth to the upper reaches of the sky - my ascension is at hand.");

						break;
				}

				if (NextState == null)
				{
					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
			}
			else
			{
				base.PlayerExecute();
			}
		}
	}
}
