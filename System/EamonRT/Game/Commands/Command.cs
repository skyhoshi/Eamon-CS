
// Command.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public virtual IArtifact DobjArtifact { get; set; }

		public virtual IMonster DobjMonster { get; set; }

		public virtual IArtifact IobjArtifact { get; set; }

		public virtual IMonster IobjMonster { get; set; }

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

		protected virtual void PrintCantVerbObj(IHaveListedName obj)
		{
			Debug.Assert(obj != null);

			Globals.Out.Write("{0}You can't {1} {2}.{0}", Environment.NewLine, Verb, obj.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		protected virtual void PrintCantVerbIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}You can't {1} {2}.{0}", Environment.NewLine, Verb, artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintCantVerbThat(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}You can't {1} {2}.{0}", Environment.NewLine, Verb, artifact.EvalPlural("that", "them"));
		}

		protected virtual void PrintDoYouMeanObj1OrObj2(IHaveListedName obj1, IHaveListedName obj2)
		{
			Debug.Assert(obj1 != null && obj2 != null);

			Globals.Out.Write("{0}Do you mean \"{1}\" or \"{2}\"?{0}", Environment.NewLine, obj1.GetDecoratedName01(false, false, false, false, Globals.Buf), obj2.GetDecoratedName01(false, false, false, false, Globals.Buf01));
		}

		protected virtual void PrintTakingFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}[Taking {1} first.]", Environment.NewLine, artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintBestLeftAlone(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} {2} best if left alone.{0}", Environment.NewLine, artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), artifact.EvalPlural("is", "are"));
		}

		protected virtual void PrintTooHeavy(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} {2} too heavy.{0}", Environment.NewLine, artifact.GetDecoratedName03(true, true, false, false, Globals.Buf), artifact.EvalPlural("is", "are"));
		}

		protected virtual void PrintMustBeFreed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} must be freed.{0}", Environment.NewLine, artifact.GetDecoratedName03(true, true, false, false, Globals.Buf));
		}

		protected virtual void PrintMustFirstOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}You must first open {1}.", Environment.NewLine, artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintRemoved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} removed.{0}", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		protected virtual void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} opened.{0}", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
		}

		protected virtual void PrintClosed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}{1} closed.{0}", Environment.NewLine, artifact.GetDecoratedName01(true, false, false, false, Globals.Buf));
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

			Globals.Out.WriteLine("{0}{1} not open.", Environment.NewLine, artifact.EvalPlural("It's", "They're"));
		}

		protected virtual void PrintAlreadyOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}{1} already open.", Environment.NewLine, artifact.EvalPlural("It's", "They're"));
		}

		protected virtual void PrintWontOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}{1} won't open.", Environment.NewLine, artifact.EvalPlural("It", "They"));
		}

		protected virtual void PrintWontFit(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}{1} won't fit.", Environment.NewLine, artifact.EvalPlural("It", "They"));
		}

		protected virtual void PrintFull(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}{1} full.", Environment.NewLine, artifact.EvalPlural("It's", "They're"));
		}

		protected virtual void PrintLocked(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}{1} locked.", Environment.NewLine, artifact.EvalPlural("It's", "They're"));
		}

		protected virtual void PrintBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}You broke {1}!", Environment.NewLine, artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintHaveToForceOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}You'll have to force {1} open.", Environment.NewLine, artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintWearingRemoveFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}You're wearing {1}.  Remove {1} first.", Environment.NewLine, artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintWearingRemoveFirst01(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}You're wearing {1}.  Remove {2} first.", Environment.NewLine, artifact.GetDecoratedName03(false, true, false, false, Globals.Buf), artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintVerbItAll(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}You {1} {2} all.{0}", Environment.NewLine, Verb, artifact.EvalPlural("it", "them"));
		}

		protected virtual void PrintTryDifferentCommand(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}Try a different command.", Environment.NewLine);
		}

		protected virtual void PrintWhyAttack(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.Write("{0}Why would you attack {1}?{0}", Environment.NewLine, artifact.GetDecoratedName03(false, true, false, false, Globals.Buf));
		}

		protected virtual void PrintNotWeapon(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Globals.Out.WriteLine("{0}{1} a weapon.", Environment.NewLine, artifact.EvalPlural("That isn't", "They aren't"));
		}

		protected virtual void PrintPolitelyRefuses(IMonster monster)
		{
			Debug.Assert(monster != null);

			Globals.Out.Write("{0}{1} politely refuse{2}.{0}",	Environment.NewLine, monster.GetDecoratedName03(true, true, false, false, Globals.Buf), monster.EvalPlural("s", ""));
		}

		protected virtual void PrintGiveObjToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			Globals.Out.Write("{0}You give {1} to {2}.{0}",	Environment.NewLine,	artifact.GetDecoratedName03(false, true, false, false, Globals.Buf),	monster.GetDecoratedName03(false, true, false, false, Globals.Buf01));
		}

		protected virtual void PrintMustFirstReadyWeapon()
		{
			Globals.Out.WriteLine("{0}You must first ready a weapon!", Environment.NewLine);
		}

		protected virtual void PrintDontHaveItNotHere()
		{
			Globals.Out.WriteLine("{0}You don't have it and it's not here.", Environment.NewLine);
		}

		protected virtual void PrintDontHaveIt()
		{
			Globals.Out.WriteLine("{0}You don't have it.", Environment.NewLine);
		}

		protected virtual void PrintDontNeedTo()
		{
			Globals.Out.WriteLine("{0}You don't need to.", Environment.NewLine);
		}

		protected virtual void PrintCantVerbThat()
		{
			Globals.Out.WriteLine("{0}You can't {1} that.", Environment.NewLine, Verb);
		}

		protected virtual void PrintCantVerbHere()
		{
			Globals.Out.WriteLine("{0}You can't {1} here.", Environment.NewLine, Verb);
		}

		protected virtual void PrintBeMoreSpecific()
		{
			Globals.Out.WriteLine("{0}Try to be more specific.", Environment.NewLine);
		}

		protected virtual void PrintNobodyHereByThatName()
		{
			Globals.Out.WriteLine("{0}Nobody here by that name!", Environment.NewLine);
		}

		protected virtual void PrintNothingHereByThatName()
		{
			Globals.Out.WriteLine("{0}Nothing here by that name!", Environment.NewLine);
		}

		protected virtual void PrintYouSeeNothingSpecial()
		{
			Globals.Out.WriteLine("{0}You see nothing special.", Environment.NewLine);
		}

		protected virtual void PrintDontFollowYou()
		{
			Globals.Out.WriteLine("{0}I don't follow you.", Environment.NewLine);
		}

		protected virtual void PrintDontBeAbsurd()
		{
			Globals.Out.WriteLine("{0}Don't be absurd.", Environment.NewLine);
		}

		protected virtual void PrintCalmDown()
		{
			Globals.Out.WriteLine("{0}Calm down.", Environment.NewLine);
		}

		protected virtual void PrintNoPlaceToGo()
		{
			Globals.Out.WriteLine("{0}There's no place to go!", Environment.NewLine);
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
						CommandParser.ObjData.RevealEmbeddedArtifactFunc = Globals.RtEngine.RevealEmbeddedArtifact;
					}

					if (CommandParser.ObjData.GetArtifactListFunc == null)
					{
						CommandParser.ObjData.GetArtifactListFunc = Globals.Engine.GetArtifactList;
					}

					if (CommandParser.ObjData.FilterArtifactListFunc == null)
					{
						CommandParser.ObjData.FilterArtifactListFunc = Globals.RtEngine.FilterArtifactList;
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

						CommandParser.ObjData.FilterArtifactList = CommandParser.ObjData.FilterArtifactListFunc(CommandParser.ObjData.GetArtifactList, CommandParser.ObjData.Name);

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
						CommandParser.ObjData.FilterMonsterListFunc = Globals.RtEngine.FilterMonsterList;
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

			destCommand.DobjArtifact = DobjArtifact;

			destCommand.DobjMonster = DobjMonster;

			if (includeIobj)
			{
				destCommand.IobjArtifact = IobjArtifact;

				destCommand.IobjMonster = IobjMonster;

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
