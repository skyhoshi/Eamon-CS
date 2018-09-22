
// ICommandSignatures.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Parsing;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Commands
{
	public interface ICommandSignatures
	{
		ICommandParser CommandParser { get; set; }

		IMonster ActorMonster { get; set; }

		IRoom ActorRoom { get; set; }

		IGameBase Dobj { get; set; }

		IArtifact DobjArtifact { get; }

		IMonster DobjMonster { get; }

		IGameBase Iobj { get; set; }

		IArtifact IobjArtifact { get; }

		IMonster IobjMonster { get; }

		string[] Synonyms { get; set; }

		long SortOrder { get; set; }

		string Verb { get; set; }

		string Prep { get; set; }

		Enums.CommandType Type { get; set; }

		bool IsNew { get; set; }

		bool IsListed { get; set; }

		bool IsIobjEnabled { get; set; }

		bool IsDarkEnabled { get; set; }

		bool IsPlayerEnabled { get; set; }

		bool IsMonsterEnabled { get; set; }

		void PrintCantVerbObj(IGameBase obj);

		void PrintCantVerbIt(IArtifact artifact);

		void PrintCantVerbThat(IArtifact artifact);

		void PrintDoYouMeanObj1OrObj2(IGameBase obj1, IGameBase obj2);

		void PrintTakingFirst(IArtifact artifact);

		void PrintBestLeftAlone(IArtifact artifact);

		void PrintTooHeavy(IArtifact artifact);

		void PrintMustBeFreed(IArtifact artifact);

		void PrintMustFirstOpen(IArtifact artifact);

		void PrintRemoved(IArtifact artifact);

		void PrintOpened(IArtifact artifact);

		void PrintClosed(IArtifact artifact);

		void PrintReceived(IArtifact artifact);

		void PrintRetrieved(IArtifact artifact);

		void PrintTaken(IArtifact artifact);

		void PrintDropped(IArtifact artifact);

		void PrintNotOpen(IArtifact artifact);

		void PrintAlreadyOpen(IArtifact artifact);

		void PrintWontOpen(IArtifact artifact);

		void PrintWontFit(IArtifact artifact);

		void PrintFull(IArtifact artifact);

		void PrintLocked(IArtifact artifact);

		void PrintBrokeIt(IArtifact artifact);

		void PrintAlreadyBrokeIt(IArtifact artifact);

		void PrintHaveToForceOpen(IArtifact artifact);

		void PrintWearingRemoveFirst(IArtifact artifact);

		void PrintWearingRemoveFirst01(IArtifact artifact);

		void PrintVerbItAll(IArtifact artifact);

		void PrintNoneLeft(IArtifact artifact);

		void PrintOkay(IArtifact artifact);

		void PrintFeelBetter(IArtifact artifact);

		void PrintFeelWorse(IArtifact artifact);

		void PrintTryDifferentCommand(IArtifact artifact);

		void PrintWhyAttack(IArtifact artifact);

		void PrintNotWeapon(IArtifact artifact);

		void PrintNotReadyableWeapon(IArtifact artifact);

		void PrintPolitelyRefuses(IMonster monster);

		void PrintGiveObjToActor(IArtifact artifact, IMonster monster);

		void PrintOpenObjWithKey(IArtifact artifact, IArtifact key);

		void PrintNotEnoughGold();

		void PrintMustFirstReadyWeapon();

		void PrintDontHaveItNotHere();

		void PrintDontHaveIt();

		void PrintDontNeedTo();

		void PrintCantVerbThat();

		void PrintCantVerbHere();

		void PrintBeMoreSpecific();

		void PrintNobodyHereByThatName();

		void PrintNothingHereByThatName();

		void PrintYouSeeNothingSpecial();

		void PrintDontFollowYou();

		void PrintDontBeAbsurd();

		void PrintCalmDown();

		void PrintNoPlaceToGo();

		void PlayerArtifactMatch();

		void PlayerArtifactMatch01();

		void PlayerArtifactMatch02();

		void PlayerMonsterMatch();

		void PlayerMonsterMatch01();

		void PlayerMonsterMatch02();

		void PlayerMonsterMatch03();

		void PlayerResolveArtifact();

		void PlayerResolveMonster();

		void PlayerProcessEvents(long eventType);

		void MonsterProcessEvents(long eventType);

		void PlayerExecute();

		void MonsterExecute();

		void PlayerFinishParsing();

		void MonsterFinishParsing();

		bool IsAllowedInRoom();

		bool ShouldAllowSkillGains();

		string GetPrintedVerb();

		bool IsEnabled(IMonster monster);

		void CopyCommandData(ICommand destCommand, bool includeIobj = true);

		void FinishParsing();
	}
}
