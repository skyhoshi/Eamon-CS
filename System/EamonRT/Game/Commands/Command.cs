
// Command.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Commands;
using Eamon.Framework.Parsing;
using EamonRT.Framework.States;
using EamonRT.Game.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	public abstract class Command : State, ICommand
	{
		public virtual ICommandParser CommandParser { get; set; }

		public virtual IMonster ActorMonster { get; set; }

		public virtual IRoom ActorRoom { get; set; }

		public virtual IGameBase Dobj { get; set; }

		public virtual IArtifact DobjArtifact
		{
			get
			{
				return Dobj as IArtifact;
			}
		}

		public virtual IMonster DobjMonster
		{
			get
			{
				return Dobj as IMonster;
			}
		}

		public virtual IGameBase Iobj { get; set; }

		public virtual IArtifact IobjArtifact
		{
			get
			{
				return Iobj as IArtifact;
			}
		}

		public virtual IMonster IobjMonster
		{
			get
			{
				return Iobj as IMonster;
			}
		}

		public virtual string[] Synonyms { get; set; }

		public virtual long SortOrder { get; set; }

		public virtual string Verb { get; set; }

		public virtual string Prep { get; set; }

		public virtual Enums.CommandType Type { get; set; }

		public virtual bool IsNew { get; set; }

		public virtual bool IsListed { get; set; }

		public virtual bool IsDarkEnabled { get; set; }

		public virtual bool IsPlayerEnabled { get; set; }

		public virtual bool IsMonsterEnabled { get; set; }

		protected virtual void PrintCantVerbObj(IGameBase obj)
		{
			Debug.Assert(obj != null);

			Globals.Out.Print("You can't {0} {1}.", Verb, obj.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		protected virtual void PrintCantVerbIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You can't {0} {1}.", Verb, artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintCantVerbThat(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You can't {0} {1}.", Verb, artifact.EvalPlural("that", "them"));
		}

		protected virtual void PrintDoYouMeanObj1OrObj2(IGameBase obj1, IGameBase obj2)
		{
			Debug.Assert(obj1 != null && obj2 != null);

			Globals.Out.Print("Do you mean \"{0}\" or \"{1}\"?", obj1.GetDecoratedName01(false, false, false, false, Globals.Buf), obj2.GetDecoratedName01(false, false, false, false, Globals.Buf01));
		}

		protected virtual void PrintTakingFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("[Taking {0} first.]", artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintBestLeftAlone(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} {1} best if left alone.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), artifact.EvalPlural("is", "are"));
		}

		protected virtual void PrintTooHeavy(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} {1} too heavy.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), artifact.EvalPlural("is", "are"));
		}

		protected virtual void PrintMustBeFreed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} must be freed.", artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
		}

		protected virtual void PrintMustFirstOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You must first open {0}.", artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintRemoved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} removed.", artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		protected virtual void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} opened.", artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		protected virtual void PrintClosed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} closed.", artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		protected virtual void PrintReceived(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} received.", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		protected virtual void PrintRetrieved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} retrieved.", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		protected virtual void PrintTaken(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} taken.", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		protected virtual void PrintDropped(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} dropped.", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		protected virtual void PrintNotOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} not open.", artifact.EvalPlural("It's", "They're"));
		}

		protected virtual void PrintAlreadyOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} already open.", artifact.EvalPlural("It's", "They're"));
		}

		protected virtual void PrintWontOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} won't open.", artifact.EvalPlural("It", "They"));
		}

		protected virtual void PrintWontFit(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} won't fit.", artifact.EvalPlural("It", "They"));
		}

		protected virtual void PrintFull(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} full.", artifact.EvalPlural("It's", "They're"));
		}

		protected virtual void PrintLocked(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} locked.", artifact.EvalPlural("It's", "They're"));
		}

		protected virtual void PrintBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You broke {0}!", artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintAlreadyBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You already broke {0}!", artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintHaveToForceOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You'll have to force {0} open.", artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintWearingRemoveFirst(IArtifact artifact)
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

		protected virtual void PrintWearingRemoveFirst01(IArtifact artifact)
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

		protected virtual void PrintVerbItAll(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You {0} {1} all.", Verb, artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintNoneLeft(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("There's none left.");
		}

		protected virtual void PrintOkay(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("Okay.");
		}

		protected virtual void PrintFeelBetter(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You feel better!");
		}

		protected virtual void PrintFeelWorse(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("You feel worse!");
		}

		protected virtual void PrintTryDifferentCommand(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("Try a different command.");
		}

		protected virtual void PrintWhyAttack(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("Why would you attack {0}?", artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		protected virtual void PrintNotWeapon(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} a weapon.", artifact.EvalPlural("That isn't", "They aren't"));
		}

		protected virtual void PrintNotReadyableWeapon(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Print("{0} a weapon that you can wield.", artifact.EvalPlural("That isn't", "They aren't"));
		}

		protected virtual void PrintPolitelyRefuses(IMonster monster)
		{
			Debug.Assert(monster != null);

			Globals.Out.Print("{0} politely refuse{1}.",	monster.GetDecoratedName03(true, true, false, false, Globals.Buf), monster.EvalPlural("s", ""));
		}

		protected virtual void PrintGiveObjToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			Globals.Out.Print("You give {0} to {1}.",	artifact.GetDecoratedName03(false, true, false, false, Globals.Buf),	monster.GetDecoratedName03(false, true, false, false, Globals.Buf01));
		}

		protected virtual void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			Globals.Out.Print("You open {0} with {1}.", artifact.EvalPlural("it", "them"), key.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		protected virtual void PrintNotEnoughGold()
		{
			if (Globals.IsRulesetVersion(5))
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

		protected virtual void PrintMustFirstReadyWeapon()
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

		protected virtual void PrintDontHaveItNotHere()
		{
			Globals.Out.Print("You don't have it and it's not here.");
		}

		protected virtual void PrintDontHaveIt()
		{
			Globals.Out.Print("You don't have it.");
		}

		protected virtual void PrintDontNeedTo()
		{
			Globals.Out.Print("You don't need to.");
		}

		protected virtual void PrintCantVerbThat()
		{
			Globals.Out.Print("You can't {0} that.", Verb);
		}

		protected virtual void PrintCantVerbHere()
		{
			Globals.Out.Print("You can't {0} here.", Verb);
		}

		protected virtual void PrintBeMoreSpecific()
		{
			Globals.Out.Print("Try to be more specific.");
		}

		protected virtual void PrintNobodyHereByThatName()
		{
			Globals.Out.Print("Nobody here by that name!");
		}

		protected virtual void PrintNothingHereByThatName()
		{
			Globals.Out.Print("Nothing here by that name!");
		}

		protected virtual void PrintYouSeeNothingSpecial()
		{
			Globals.Out.Print("You see nothing special.");
		}

		protected virtual void PrintDontFollowYou()
		{
			Globals.Out.Print("I don't follow you.");
		}

		protected virtual void PrintDontBeAbsurd()
		{
			Globals.Out.Print("Don't be absurd.");
		}

		protected virtual void PrintCalmDown()
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

		protected virtual void PrintNoPlaceToGo()
		{
			Globals.Out.Print("There's no place to go!");
		}

		protected virtual void PlayerArtifactMatch()
		{
			if (CommandParser.ObjData.FilterArtifactList.Count > 1)
			{
				PrintDoYouMeanObj1OrObj2(CommandParser.ObjData.FilterArtifactList[0], CommandParser.ObjData.FilterArtifactList[1]);

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else if (CommandParser.ObjData.FilterArtifactList.Count < 1)
			{
				CommandParser.ObjData.ArtifactNotFoundFunc();

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				CommandParser.ObjData.RevealEmbeddedArtifactFunc(ActorRoom, CommandParser.ObjData.FilterArtifactList[0]);

				CommandParser.SetArtifact(CommandParser.ObjData.FilterArtifactList[0]);
			}
		}

		protected virtual void PlayerArtifactMatch01()
		{
			if (CommandParser.ObjData.FilterArtifactList.Count > 1)
			{
				PrintDoYouMeanObj1OrObj2(CommandParser.ObjData.FilterArtifactList[0], CommandParser.ObjData.FilterArtifactList[1]);

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else if (CommandParser.ObjData.FilterArtifactList.Count < 1)
			{
				PlayerResolveMonster();
			}
			else
			{
				CommandParser.ObjData.RevealEmbeddedArtifactFunc(ActorRoom, CommandParser.ObjData.FilterArtifactList[0]);

				CommandParser.SetArtifact(CommandParser.ObjData.FilterArtifactList[0]);
			}
		}

		protected virtual void PlayerArtifactMatch02()
		{
			if (CommandParser.ObjData.FilterArtifactList.Count > 1)
			{
				PrintDoYouMeanObj1OrObj2(CommandParser.ObjData.FilterArtifactList[0], CommandParser.ObjData.FilterArtifactList[1]);

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else if (CommandParser.ObjData.FilterArtifactList.Count < 1)
			{
				CommandParser.ObjData.ArtifactNotFoundFunc();

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				CommandParser.ObjData.RevealEmbeddedArtifactFunc(ActorRoom, CommandParser.ObjData.FilterArtifactList[0]);

				CommandParser.SetArtifact(CommandParser.ObjData.FilterArtifactList[0]);
			}
		}

		protected virtual void PlayerMonsterMatch()
		{
			if (CommandParser.ObjData.FilterMonsterList.Count < 1)
			{
				CommandParser.ObjData.MonsterNotFoundFunc();

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				CommandParser.SetMonster(CommandParser.ObjData.FilterMonsterList[0]);
			}
		}

		protected virtual void PlayerMonsterMatch01()
		{
			if (CommandParser.ObjData.FilterMonsterList.Count < 1)
			{
				CommandParser.ObjData.MonsterNotFoundFunc();

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				PrintCantVerbObj(CommandParser.ObjData.FilterMonsterList[0]);

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		protected virtual void PlayerMonsterMatch02()
		{
			if (CommandParser.ObjData.FilterMonsterList.Count < 1)
			{
				CommandParser.ObjData.MonsterNotFoundFunc();

				CommandParser.NextState.Dispose();

				CommandParser.NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				CommandParser.SetMonster(CommandParser.ObjData.FilterMonsterList[0]);
			}
		}

		protected virtual void PlayerMonsterMatch03()
		{
			if (CommandParser.ObjData.FilterMonsterList.Count < 1)
			{
				PlayerResolveArtifact();
			}
			else
			{
				CommandParser.SetMonster(CommandParser.ObjData.FilterMonsterList[0]);
			}
		}

		protected virtual void PlayerResolveArtifact()
		{
			if (CommandParser.GetArtifact() == null)
			{
				if (string.IsNullOrWhiteSpace(CommandParser.ObjData.Name))
				{
					CommandParser.ParseName();
				}

				if (!string.IsNullOrWhiteSpace(CommandParser.ObjData.Name))
				{
					if (CommandParser.ObjData.ArtifactWhereClauseList == null)
					{
						CommandParser.ObjData.ArtifactWhereClauseList = new List<Func<IArtifact, bool>>()
						{
							a => a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom),
							a => a.IsEmbeddedInRoom(ActorRoom)
						};
					}

					if (CommandParser.ObjData.RevealEmbeddedArtifactFunc == null)
					{
						CommandParser.ObjData.RevealEmbeddedArtifactFunc = Globals.Engine.RevealEmbeddedArtifact;
					}

					if (CommandParser.ObjData.GetArtifactListFunc == null)
					{
						CommandParser.ObjData.GetArtifactListFunc = Globals.Engine.GetArtifactList;
					}

					if (CommandParser.ObjData.FilterArtifactListFunc == null)
					{
						CommandParser.ObjData.FilterArtifactListFunc = Globals.Engine.FilterArtifactList;
					}

					if (CommandParser.ObjData.ArtifactMatchFunc == null)
					{
						CommandParser.ObjData.ArtifactMatchFunc = PlayerArtifactMatch;
					}

					if (CommandParser.ObjData.ArtifactNotFoundFunc == null)
					{
						CommandParser.ObjData.ArtifactNotFoundFunc = PrintDontHaveItNotHere;
					}

					CommandParser.ObjData.GetArtifactList = new List<IArtifact>();

					foreach (var whereClauseFuncs in CommandParser.ObjData.ArtifactWhereClauseList)
					{
						CommandParser.ObjData.GetArtifactList = CommandParser.ObjData.GetArtifactListFunc(() => true, whereClauseFuncs);

						Debug.Assert(CommandParser.ObjData.GetArtifactList != null);

						var filterArtifactList = CommandParser.ObjData.FilterArtifactListFunc(CommandParser.ObjData.GetArtifactList, CommandParser.ObjData.Name);

						Debug.Assert(filterArtifactList != null);

						CommandParser.ObjData.FilterArtifactList = filterArtifactList.GroupBy(a => a.Name.ToLower()).Select(a => a.First()).ToList();

						Debug.Assert(CommandParser.ObjData.FilterArtifactList != null);

						if (CommandParser.ObjData.FilterArtifactList.Count > 0)
						{
							break;
						}
					}

					CommandParser.ObjData.ArtifactMatchFunc();
				}
				else
				{
					CommandParser.NextState.Dispose();

					CommandParser.NextState = Globals.CreateInstance<IErrorState>(x =>
					{
						x.ErrorMessage = string.Format("{0}: string.IsNullOrWhiteSpace(CommandParser.{1}.Name)", Name, CommandParser.GetActiveObjData());
					});
				}
			}
		}

		protected virtual void PlayerResolveMonster()
		{
			if (CommandParser.GetMonster() == null)
			{
				if (string.IsNullOrWhiteSpace(CommandParser.ObjData.Name))
				{
					CommandParser.ParseName();
				}

				if (!string.IsNullOrWhiteSpace(CommandParser.ObjData.Name))
				{
					if (CommandParser.ObjData.MonsterWhereClauseList == null)
					{
						CommandParser.ObjData.MonsterWhereClauseList = new List<Func<IMonster, bool>>()
						{
							m => m.IsInRoom(ActorRoom) && m != ActorMonster
						};
					}

					if (CommandParser.ObjData.GetMonsterListFunc == null)
					{
						CommandParser.ObjData.GetMonsterListFunc = Globals.Engine.GetMonsterList;
					}

					if (CommandParser.ObjData.FilterMonsterListFunc == null)
					{
						CommandParser.ObjData.FilterMonsterListFunc = Globals.Engine.FilterMonsterList;
					}

					if (CommandParser.ObjData.MonsterMatchFunc == null)
					{
						CommandParser.ObjData.MonsterMatchFunc = PlayerMonsterMatch;
					}

					if (CommandParser.ObjData.MonsterNotFoundFunc == null)
					{
						CommandParser.ObjData.MonsterNotFoundFunc = PrintNobodyHereByThatName;
					}

					CommandParser.ObjData.GetMonsterList = new List<IMonster>();

					foreach (var whereClauseFuncs in CommandParser.ObjData.MonsterWhereClauseList)
					{
						CommandParser.ObjData.GetMonsterList = CommandParser.ObjData.GetMonsterListFunc(() => true, whereClauseFuncs);

						Debug.Assert(CommandParser.ObjData.GetMonsterList != null);

						CommandParser.ObjData.FilterMonsterList = CommandParser.ObjData.FilterMonsterListFunc(CommandParser.ObjData.GetMonsterList, CommandParser.ObjData.Name);

						Debug.Assert(CommandParser.ObjData.FilterMonsterList != null);

						if (CommandParser.ObjData.FilterMonsterList.Count > 0)
						{
							break;
						}
					}

					CommandParser.ObjData.MonsterMatchFunc();
				}
				else
				{
					CommandParser.NextState.Dispose();

					CommandParser.NextState = Globals.CreateInstance<IErrorState>(x =>
					{
						x.ErrorMessage = string.Format("{0}: string.IsNullOrWhiteSpace(CommandParser.{1}.Name)", Name, CommandParser.GetActiveObjData());
					});
				}
			}
		}

		protected virtual void PlayerExecute()
		{

		}

		protected virtual void MonsterExecute()
		{

		}

		protected virtual void PlayerFinishParsing()
		{

		}

		protected virtual void MonsterFinishParsing()
		{

		}

		protected virtual bool IsAllowedInRoom()
		{
			return true;
		}

		protected virtual bool ShouldAllowSkillGains()
		{
			return true;
		}

		public override bool ShouldPreTurnProcess()
		{
			return true;
		}

		public override void Execute()
		{
			Debug.Assert(ActorMonster != null);

			Debug.Assert(ActorRoom != null);

			if (ActorMonster.IsCharacterMonster())
			{
				if (IsAllowedInRoom())
				{
					PlayerExecute();
				}
				else
				{
					PrintCantVerbHere();

					NextState = Globals.CreateInstance<IStartState>();
				}
			}
			else
			{
				Debug.Assert(IsMonsterEnabled);

				MonsterExecute();
			}

			Globals.NextState = NextState;
		}

		public virtual string GetPrintedVerb()
		{
			return Verb.ToUpper();
		}

		public virtual bool IsEnabled(IMonster monster)
		{
			Debug.Assert(monster != null);

			return monster.IsCharacterMonster() ? IsPlayerEnabled : IsMonsterEnabled;
		}

		public virtual void CopyCommandData(ICommand destCommand, bool includeIobj = true)
		{
			Debug.Assert(destCommand != null);

			destCommand.CommandParser = CommandParser;

			destCommand.ActorMonster = ActorMonster;

			destCommand.ActorRoom = ActorRoom;

			destCommand.Dobj = Dobj;

			if (includeIobj)
			{
				destCommand.Iobj = Iobj;

				destCommand.Prep = Globals.CloneInstance(Prep);
			}
		}

		public virtual void FinishParsing()
		{
			Debug.Assert(CommandParser != null);

			Debug.Assert(ActorMonster != null);

			Debug.Assert(ActorRoom != null);

			if (ActorMonster.IsCharacterMonster())
			{
				PlayerFinishParsing();
			}
			else
			{
				MonsterFinishParsing();
			}
		}

		public Command()
		{
			SortOrder = Int64.MaxValue;

			IsListed = true;

			IsPlayerEnabled = true;

			IsMonsterEnabled = true;
		}
	}
}
