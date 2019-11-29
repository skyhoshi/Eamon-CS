
// CommandImpl.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : ICommandImpl
	{
		protected IMonster _actorMonster;

		public virtual ICommand Command { get; set; }

		public virtual ICommandParser CommandParser { get; set; }

		public virtual IMonster ActorMonster
		{
			get
			{
				return _actorMonster;
			}

			set
			{
				if (Globals.EnableRevealContentOverrides)
				{
					Globals.RevealContentMonster = value;
				}

				_actorMonster = value;
			}
		}

		public virtual IRoom ActorRoom { get; set; }

		public virtual IGameBase Dobj { get; set; }

		public virtual IArtifact DobjArtifact
		{
			get
			{
				return Command.Dobj as IArtifact;
			}
		}

		public virtual IMonster DobjMonster
		{
			get
			{
				return Command.Dobj as IMonster;
			}
		}

		public virtual IGameBase Iobj { get; set; }

		public virtual IArtifact IobjArtifact
		{
			get
			{
				return Command.Iobj as IArtifact;
			}
		}

		public virtual IMonster IobjMonster
		{
			get
			{
				return Command.Iobj as IMonster;
			}
		}

		public virtual string[] Synonyms { get; set; }

		public virtual long SortOrder { get; set; }

		public virtual string Verb { get; set; }

		public virtual IPrep Prep { get; set; }

		public virtual CommandType Type { get; set; }

		public virtual ContainerType ContainerType { get; set; }

		public virtual bool IsNew { get; set; }

		public virtual bool GetCommandCalled { get; set; }

		public virtual bool IsListed { get; set; }

		public virtual bool IsDobjPrepEnabled { get; set; }

		public virtual bool IsIobjEnabled { get; set; }

		public virtual bool IsDarkEnabled { get; set; }

		public virtual bool IsPlayerEnabled { get; set; }

		public virtual bool IsMonsterEnabled { get; set; }

		public virtual void PrintCantVerbObj(IGameBase obj)
		{
			Debug.Assert(obj != null);

			Globals.Out.Print("You can't {0} {1}.", Command.Verb, obj.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		public virtual void PrintCantVerbIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You can't {0} {1}.", Command.Verb, artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintCantVerbThat(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You can't {0} {1}.", Command.Verb, artifact.EvalPlural("that", "them"));
		}

		public virtual void PrintDoYouMeanObj1OrObj2(IGameBase obj1, IGameBase obj2)
		{
			Debug.Assert(obj1 != null && obj2 != null);

			Globals.Out.Print("Do you mean \"{0}\" or \"{1}\"?", obj1.GetDecoratedName01(false, false, false, false, Globals.Buf), obj2.GetDecoratedName01(false, false, false, false, Globals.Buf01));
		}

		public virtual void PrintTakingFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("[Taking {0} first.]", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintRemovingFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("[Removing {0} first.]", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintBestLeftAlone(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} {1} best if left alone.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), artifact.EvalPlural("is", "are"));
		}

		public virtual void PrintTooHeavy(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} {1} too heavy.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), artifact.EvalPlural("is", "are"));
		}

		public virtual void PrintMustBeFreed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} must be freed.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
		}

		public virtual void PrintMustFirstOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You must first open {0}.", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintMustFirstClose(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You must first close {0}.", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintRemoved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} removed.", artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		public virtual void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} opened.", artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		public virtual void PrintClosed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} closed.", artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		public virtual void PrintReceived(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} received.", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		public virtual void PrintRetrieved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} retrieved.", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		public virtual void PrintTaken(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} taken.", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		public virtual void PrintDropped(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} dropped.", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		public virtual void PrintNotOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} not open.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintAlreadyOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} already open.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintWontOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} won't open.", artifact.EvalPlural("It", "They"));
		}

		public virtual void PrintWontFit(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} won't fit.", artifact.EvalPlural("It", "They"));
		}

		public virtual void PrintFull(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} full.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintOutOfSpace(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} out of space.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintLocked(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} locked.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You broke {0}!", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintAlreadyBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You already broke {0}!", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintHaveToForceOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You'll have to force {0} open.", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintWearingRemoveFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (Globals.IsRulesetVersion(5))
			{
				Globals.Out.Print("You're wearing {0}.", artifact.EvalPlural("it", "them"));
			}
			else
			{
				Globals.Out.Print("You're wearing {0}.  Remove {0} first.", artifact.EvalPlural("it", "them"));
			}
		}

		public virtual void PrintWearingRemoveFirst01(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (Globals.IsRulesetVersion(5))
			{
				Globals.Out.Print("You're wearing {0}.", artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
			}
			else
			{
				Globals.Out.Print("You're wearing {0}.  Remove {1} first.", artifact.GetDecoratedName03(false, true, false, false, Globals.Buf), artifact.EvalPlural("it", "them"));
			}
		}

		public virtual void PrintCantWearShieldWithWeapon(IArtifact shield, IArtifact weapon)
		{
			Debug.Assert(shield != null && weapon != null);

			Globals.Out.Print("You can't wear {0} while using {1}.", shield.GetDecoratedName03(false, true, false, false, Globals.Buf), weapon.GetDecoratedName03(false, true, false, false, Globals.Buf01));
		}

		public virtual void PrintContainerNotEmpty(IArtifact artifact, ContainerType containerType, bool isPlural)
		{
			Debug.Assert(artifact != null && Enum.IsDefined(typeof(ContainerType), containerType));

			Globals.Out.Print("{0} {1} {2} {3} {4}.  Remove it first.", 
				artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), 
				artifact.EvalPlural("has", "have"), 
				isPlural ? "some stuff" : "something", 
				Globals.Engine.EvalContainerType(containerType, "inside", "on", "under", "behind"), 
				artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintVerbItAll(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You {0} {1} all.", Command.Verb, artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintNoneLeft(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("There's none left.");
		}

		public virtual void PrintOkay(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("Okay.");
		}

		public virtual void PrintFeelBetter(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You feel better!");
		}

		public virtual void PrintFeelWorse(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You feel worse!");
		}

		public virtual void PrintTryDifferentCommand(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("Try a different command.");
		}

		public virtual void PrintWhyAttack(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("Why would you attack {0}?", artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		public virtual void PrintNotWeapon(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} a weapon.", artifact.EvalPlural("That isn't", "They aren't"));
		}

		public virtual void PrintNotReadyableWeapon(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} a weapon that you can use.", artifact.EvalPlural("That isn't", "They aren't"));
		}

		public virtual void PrintNotWhileCarryingObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You can't do that while carrying {0}.", artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		public virtual void PrintNotWhileWearingObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You can't do that while wearing {0}.", artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		public virtual void PrintCantReadyWeaponWithShield(IArtifact weapon, IArtifact shield)
		{
			Debug.Assert(weapon != null && shield != null);

			Globals.Out.Print("You can't use {0} while wearing {1}.", weapon.GetDecoratedName03(false, true, false, false, Globals.Buf), shield.GetDecoratedName03(false, true, false, false, Globals.Buf01));
		}

		public virtual void PrintPolitelyRefuses(IMonster monster)
		{
			Debug.Assert(monster != null);

			Globals.Out.Print("{0} politely refuse{1}.", monster.GetDecoratedName03(true, true, false, false, Globals.Buf), monster.EvalPlural("s", ""));
		}

		public virtual void PrintGiveObjToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			Globals.Out.Print("You give {0} to {1}.", artifact.GetDecoratedName03(false, true, false, false, Globals.Buf), monster.GetDecoratedName03(false, true, false, false, Globals.Buf01));
		}

		public virtual void PrintObjBelongsToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			Globals.Out.Print("{0} belongs to {1}.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), monster.GetDecoratedName03(false, true, false, false, Globals.Buf01));
		}

		public virtual void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			Globals.Out.Print("You open {0} with {1}.", artifact.EvalPlural("it", "them"), key.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		public virtual void PrintNotEnoughGold()
		{
			if (Globals.IsRulesetVersion(5) || Globals.Character.HeldGold < 0)
			{
				Globals.Out.Print("You aren't carrying that much gold of your own!");
			}
			else
			{
				Globals.Out.Print("You only have {0} gold piece{1}.",
					Globals.Engine.GetStringFromNumber(Globals.Character.HeldGold, false, Globals.Buf),
					Globals.Character.HeldGold != 1 ? "s" : "");
			}
		}

		public virtual void PrintMustFirstReadyWeapon()
		{
			if (Globals.IsRulesetVersion(5))
			{
				Globals.Out.Print("You have no weapon ready!");
			}
			else
			{
				Globals.Out.Print("You must first ready a weapon!");
			}
		}

		public virtual void PrintDontHaveItNotHere()
		{
			Globals.Out.Print("You don't have it and it's not here.");
		}

		public virtual void PrintDontHaveIt()
		{
			Globals.Out.Print("You don't have it.");
		}

		public virtual void PrintDontNeedTo()
		{
			Globals.Out.Print("You don't need to.");
		}

		public virtual void PrintCantDoThat()
		{
			Globals.Out.Print("You can't do that.");
		}

		public virtual void PrintCantVerbThat()
		{
			Globals.Out.Print("You can't {0} that.", Command.Verb);
		}

		public virtual void PrintCantVerbHere()
		{
			Globals.Out.Print("You can't {0} here.", Command.Verb);
		}

		public virtual void PrintBeMoreSpecific()
		{
			Globals.Out.Print("Try to be more specific.");
		}

		public virtual void PrintNobodyHereByThatName()
		{
			Globals.Out.Print("Nobody here by that name!");
		}

		public virtual void PrintNothingHereByThatName()
		{
			Globals.Out.Print("Nothing here by that name!");
		}

		public virtual void PrintYouSeeNothingSpecial()
		{
			Globals.Out.Print("You see nothing special.");
		}

		public virtual void PrintDontFollowYou()
		{
			Globals.Out.Print("I don't follow you.");
		}

		public virtual void PrintDontBeAbsurd()
		{
			Globals.Out.Print("Don't be absurd.");
		}

		public virtual void PrintCalmDown()
		{
			if (Globals.IsRulesetVersion(5, 6, 62))
			{
				Globals.Out.Print("There's nothing to flee from!");
			}
			else
			{
				Globals.Out.Print("Calm down.");
			}
		}

		public virtual void PrintNoPlaceToGo()
		{
			Globals.Out.Print("There's no place to go!");
		}

		public virtual void PlayerArtifactMatch()
		{
			if (Command.CommandParser.ObjData.FilterArtifactList.Count > 1)
			{
				Command.PrintDoYouMeanObj1OrObj2(Command.CommandParser.ObjData.FilterArtifactList[0], Command.CommandParser.ObjData.FilterArtifactList[1]);

				Command.CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else if (Command.CommandParser.ObjData.FilterArtifactList.Count < 1)
			{
				Command.CommandParser.ObjData.ArtifactNotFoundFunc();

				Command.CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				Command.CommandParser.ObjData.RevealEmbeddedArtifactFunc(Command.ActorRoom, Command.CommandParser.ObjData.FilterArtifactList[0]);

				Command.CommandParser.SetArtifact(Command.CommandParser.ObjData.FilterArtifactList[0]);
			}
		}

		public virtual void PlayerArtifactMatch01()
		{
			if (Command.CommandParser.ObjData.FilterArtifactList.Count > 1)
			{
				Command.PrintDoYouMeanObj1OrObj2(Command.CommandParser.ObjData.FilterArtifactList[0], Command.CommandParser.ObjData.FilterArtifactList[1]);

				Command.CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else if (Command.CommandParser.ObjData.FilterArtifactList.Count < 1)
			{
				Command.PlayerResolveMonster();
			}
			else
			{
				Command.CommandParser.ObjData.RevealEmbeddedArtifactFunc(Command.ActorRoom, Command.CommandParser.ObjData.FilterArtifactList[0]);

				Command.CommandParser.SetArtifact(Command.CommandParser.ObjData.FilterArtifactList[0]);
			}
		}

		public virtual void PlayerArtifactMatch02()
		{
			if (Command.CommandParser.ObjData.FilterArtifactList.Count > 1)
			{
				Command.PrintDoYouMeanObj1OrObj2(Command.CommandParser.ObjData.FilterArtifactList[0], Command.CommandParser.ObjData.FilterArtifactList[1]);

				Command.CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else if (Command.CommandParser.ObjData.FilterArtifactList.Count < 1)
			{
				Command.CommandParser.ObjData.ArtifactNotFoundFunc();

				Command.CommandParser.NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				Command.CommandParser.ObjData.RevealEmbeddedArtifactFunc(Command.ActorRoom, Command.CommandParser.ObjData.FilterArtifactList[0]);

				Command.CommandParser.SetArtifact(Command.CommandParser.ObjData.FilterArtifactList[0]);
			}
		}

		public virtual void PlayerMonsterMatch()
		{
			if (Command.CommandParser.ObjData.FilterMonsterList.Count < 1)
			{
				Command.CommandParser.ObjData.MonsterNotFoundFunc();

				Command.CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				Command.CommandParser.SetMonster(Command.CommandParser.ObjData.FilterMonsterList[0]);
			}
		}

		public virtual void PlayerMonsterMatch01()
		{
			if (Command.CommandParser.ObjData.FilterMonsterList.Count < 1)
			{
				Command.CommandParser.ObjData.MonsterNotFoundFunc();

				Command.CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				Command.PrintCantVerbObj(Command.CommandParser.ObjData.FilterMonsterList[0]);

				Command.CommandParser.NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public virtual void PlayerMonsterMatch02()
		{
			if (Command.CommandParser.ObjData.FilterMonsterList.Count < 1)
			{
				Command.CommandParser.ObjData.MonsterNotFoundFunc();

				Command.CommandParser.NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				Command.CommandParser.SetMonster(Command.CommandParser.ObjData.FilterMonsterList[0]);
			}
		}

		public virtual void PlayerMonsterMatch03()
		{
			if (Command.CommandParser.ObjData.FilterMonsterList.Count < 1)
			{
				Command.PlayerResolveArtifact();
			}
			else
			{
				Command.CommandParser.SetMonster(Command.CommandParser.ObjData.FilterMonsterList[0]);
			}
		}

		public virtual void PlayerResolveArtifact()
		{
			if (Command.CommandParser.GetArtifact() == null)
			{
				if (string.IsNullOrWhiteSpace(Command.CommandParser.ObjData.Name))
				{
					Command.CommandParser.ParseName();
				}

				if (!string.IsNullOrWhiteSpace(Command.CommandParser.ObjData.Name))
				{
					if (Command.CommandParser.ObjData.ArtifactWhereClauseList == null)
					{
						Command.CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
						{
							a => a.IsCarriedByCharacter() || a.IsInRoom(Command.ActorRoom),
							a => a.IsEmbeddedInRoom(Command.ActorRoom),
							a => a.IsCarriedByContainerContainerTypeExposedToCharacter(Globals.Engine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(Command.ActorRoom, Globals.Engine.ExposeContainersRecursively)
						};
					}

					if (Command.CommandParser.ObjData.RevealEmbeddedArtifactFunc == null)
					{
						Command.CommandParser.ObjData.RevealEmbeddedArtifactFunc = Globals.Engine.RevealEmbeddedArtifact;
					}

					if (Command.CommandParser.ObjData.GetArtifactListFunc == null)
					{
						Command.CommandParser.ObjData.GetArtifactListFunc = Globals.Engine.GetArtifactList;
					}

					if (Command.CommandParser.ObjData.FilterArtifactListFunc == null)
					{
						Command.CommandParser.ObjData.FilterArtifactListFunc = Globals.Engine.FilterArtifactList;
					}

					if (Command.CommandParser.ObjData.ArtifactMatchFunc == null)
					{
						Command.CommandParser.ObjData.ArtifactMatchFunc = Command.PlayerArtifactMatch;
					}

					if (Command.CommandParser.ObjData.ArtifactNotFoundFunc == null)
					{
						Command.CommandParser.ObjData.ArtifactNotFoundFunc = Command.PrintDontHaveItNotHere;
					}

					Command.CommandParser.ObjData.GetArtifactList = new List<IArtifact>();

					foreach (var whereClauseFuncs in Command.CommandParser.ObjData.ArtifactWhereClauseList)
					{
						Command.CommandParser.ObjData.GetArtifactList = Command.CommandParser.ObjData.GetArtifactListFunc(whereClauseFuncs);

						Debug.Assert(Command.CommandParser.ObjData.GetArtifactList != null);

						var filterArtifactList = Command.CommandParser.ObjData.FilterArtifactListFunc(Command.CommandParser.ObjData.GetArtifactList, Command.CommandParser.ObjData.Name);

						Debug.Assert(filterArtifactList != null);

						Command.CommandParser.ObjData.FilterArtifactList = filterArtifactList.GroupBy(a => a.Name.ToLower()).Select(a => a.First()).ToList();

						Debug.Assert(Command.CommandParser.ObjData.FilterArtifactList != null);

						if (Command.CommandParser.ObjData.FilterArtifactList.Count > 0)
						{
							break;
						}
					}

					Command.CommandParser.ObjData.ArtifactMatchFunc();
				}
				else
				{
					Command.CommandParser.NextState = Globals.CreateInstance<IErrorState>(x =>
					{
						x.ErrorMessage = string.Format("{0}: string.IsNullOrWhiteSpace(Command.CommandParser.{1}.Name)", Command.Name, Command.CommandParser.GetActiveObjData());
					});
				}
			}
		}

		public virtual void PlayerResolveMonster()
		{
			if (Command.CommandParser.GetMonster() == null)
			{
				if (string.IsNullOrWhiteSpace(Command.CommandParser.ObjData.Name))
				{
					Command.CommandParser.ParseName();
				}

				if (!string.IsNullOrWhiteSpace(Command.CommandParser.ObjData.Name))
				{
					if (Command.CommandParser.ObjData.MonsterWhereClauseList == null)
					{
						Command.CommandParser.ObjData.MonsterWhereClauseList = new List<Func<IMonster, bool>>()
						{
							m => m.IsInRoom(Command.ActorRoom) && m != Command.ActorMonster
						};
					}

					if (Command.CommandParser.ObjData.GetMonsterListFunc == null)
					{
						Command.CommandParser.ObjData.GetMonsterListFunc = Globals.Engine.GetMonsterList;
					}

					if (Command.CommandParser.ObjData.FilterMonsterListFunc == null)
					{
						Command.CommandParser.ObjData.FilterMonsterListFunc = Globals.Engine.FilterMonsterList;
					}

					if (Command.CommandParser.ObjData.MonsterMatchFunc == null)
					{
						Command.CommandParser.ObjData.MonsterMatchFunc = Command.PlayerMonsterMatch;
					}

					if (Command.CommandParser.ObjData.MonsterNotFoundFunc == null)
					{
						Command.CommandParser.ObjData.MonsterNotFoundFunc = Command.PrintNobodyHereByThatName;
					}

					Command.CommandParser.ObjData.GetMonsterList = new List<IMonster>();

					foreach (var whereClauseFuncs in Command.CommandParser.ObjData.MonsterWhereClauseList)
					{
						Command.CommandParser.ObjData.GetMonsterList = Command.CommandParser.ObjData.GetMonsterListFunc(whereClauseFuncs);

						Debug.Assert(Command.CommandParser.ObjData.GetMonsterList != null);

						Command.CommandParser.ObjData.FilterMonsterList = Command.CommandParser.ObjData.FilterMonsterListFunc(Command.CommandParser.ObjData.GetMonsterList, Command.CommandParser.ObjData.Name);

						Debug.Assert(Command.CommandParser.ObjData.FilterMonsterList != null);

						if (Command.CommandParser.ObjData.FilterMonsterList.Count > 0)
						{
							break;
						}
					}

					Command.CommandParser.ObjData.MonsterMatchFunc();
				}
				else
				{
					Command.CommandParser.NextState = Globals.CreateInstance<IErrorState>(x =>
					{
						x.ErrorMessage = string.Format("{0}: string.IsNullOrWhiteSpace(Command.CommandParser.{1}.Name)", Command.Name, Command.CommandParser.GetActiveObjData());
					});
				}
			}
		}

		public virtual void PlayerProcessEvents(long eventType)
		{

		}

		public virtual void MonsterProcessEvents(long eventType)
		{

		}

		public virtual void PlayerExecute()
		{

		}

		public virtual void MonsterExecute()
		{

		}

		public virtual void PlayerFinishParsing()
		{

		}

		public virtual void MonsterFinishParsing()
		{

		}

		public virtual bool IsAllowedInRoom()
		{
			return true;
		}

		public virtual bool ShouldAllowSkillGains()
		{
			return true;
		}

		public virtual bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			return true;
		}

		public virtual bool ShouldPreTurnProcess()
		{
			return true;
		}

		public virtual void Execute()
		{
			Debug.Assert(Command.ActorMonster != null);

			Debug.Assert(Command.ActorRoom != null);

			if (Command.ActorMonster.IsCharacterMonster())
			{
				if (Command.IsAllowedInRoom())
				{
					Command.PlayerExecute();
				}
				else
				{
					Command.PrintCantVerbHere();

					Command.NextState = Globals.CreateInstance<IStartState>();
				}
			}
			else
			{
				Debug.Assert(Command.IsMonsterEnabled);

				Command.MonsterExecute();
			}

			Globals.NextState = Command.NextState;
		}

		public virtual string GetPrintedVerb()
		{
			return Command.Verb.ToUpper();
		}

		public virtual bool IsEnabled(IMonster monster)
		{
			Debug.Assert(monster != null);

			return monster.IsCharacterMonster() ? Command.IsPlayerEnabled : Command.IsMonsterEnabled;
		}

		public virtual bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			return true;
		}

		public virtual void CopyCommandData(ICommand destCommand, bool includeIobj = true)
		{
			Debug.Assert(destCommand != null);

			destCommand.CommandParser = Command.CommandParser;

			destCommand.ActorMonster = Command.ActorMonster;

			destCommand.ActorRoom = Command.ActorRoom;

			destCommand.Dobj = Command.Dobj;

			if (includeIobj)
			{
				destCommand.Iobj = Command.Iobj;

				destCommand.Prep = Globals.CloneInstance(Command.Prep);
			}
		}

		public virtual void RedirectToGetCommand<T>(IArtifact artifact, bool printTaking = true) where T : class, ICommand
		{
			Debug.Assert(artifact != null);

			if (printTaking)
			{
				if (artifact.IsCarriedByContainer())
				{
					Command.PrintRemovingFirst(artifact);
				}
				else
				{
					Command.PrintTakingFirst(artifact);
				}
			}

			Command.NextState = Globals.CreateInstance<IGetCommand>(x =>
			{
				x.PreserveNextState = true;
			});

			Command.CopyCommandData(Command.NextState as ICommand);

			Command.NextState.NextState = Globals.CreateInstance<T>(x =>
			{
				x.GetCommandCalled = true;

				x.ContainerType = Command.ContainerType;
			});

			Command.CopyCommandData(Command.NextState.NextState as ICommand);
		}

		public virtual void FinishParsing()
		{
			Debug.Assert(Command.CommandParser != null);

			Debug.Assert(Command.ActorMonster != null);

			Debug.Assert(Command.ActorRoom != null);

			if (Command.ActorMonster.IsCharacterMonster())
			{
				Command.PlayerFinishParsing();
			}
			else
			{
				Command.MonsterFinishParsing();
			}
		}

		public CommandImpl()
		{
			// Here we make an exception to the "always use Command" rule

			SortOrder = Int64.MaxValue;

			IsListed = true;

			IsPlayerEnabled = true;

			IsMonsterEnabled = true;

			ContainerType = (ContainerType)(-1);
		}
	}
}
