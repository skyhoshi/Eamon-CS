
// ICommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;

namespace EamonRT.Framework.Parsing
{
	/// <summary></summary>
	public interface ICommandParser
	{
		/// <summary></summary>
		StringBuilder InputBuf { get; set; }

		/// <summary></summary>
		string LastInputStr { get; set; }

		/// <summary></summary>
		string LastHimNameStr { get; set; }

		/// <summary></summary>
		string LastHerNameStr { get; set; }

		/// <summary></summary>
		string LastItNameStr { get; set; }

		/// <summary></summary>
		string LastThemNameStr { get; set; }

		/// <summary></summary>
		string[] Tokens { get; set; }

		/// <summary></summary>
		long CurrToken { get; set; }

		/// <summary></summary>
		long PrepTokenIndex { get; set; }

		/// <summary></summary>
		IPrep Prep { get; set; }

		/// <summary></summary>
		IMonster ActorMonster { get; set; }

		/// <summary></summary>
		IRoom ActorRoom { get; set; }

		/// <summary></summary>
		IGameBase Dobj { get; set; }

		/// <summary></summary>
		IArtifact DobjArtifact { get; }

		/// <summary></summary>
		IMonster DobjMonster { get; }

		/// <summary></summary>
		IGameBase Iobj { get; set; }

		/// <summary></summary>
		IArtifact IobjArtifact { get; }

		/// <summary></summary>
		IMonster IobjMonster { get; }

		/// <summary></summary>
		IParserData DobjData { get; set; }

		/// <summary></summary>
		IParserData IobjData { get; set; }

		/// <summary></summary>
		IParserData ObjData { get; set; }

		/// <summary></summary>
		IState NextState { get; set; }

		/// <summary></summary>
		ICommand NextCommand { get; }

		/// <summary></summary>
		void PlayerArtifactMatch();

		/// <summary></summary>
		void PlayerArtifactMatch01();

		/// <summary></summary>
		void PlayerArtifactMatch02();

		/// <summary></summary>
		void PlayerMonsterMatch();

		/// <summary></summary>
		void PlayerMonsterMatch01();

		/// <summary></summary>
		void PlayerMonsterMatch02();

		/// <summary></summary>
		void PlayerMonsterMatch03();

		/// <summary></summary>
		void PlayerResolveArtifact();

		/// <summary></summary>
		void PlayerResolveArtifactProcessWhereClauseList();

		/// <summary></summary>
		void PlayerResolveMonster();

		/// <summary></summary>
		void PlayerResolveMonsterProcessWhereClauseList();

		/// <summary></summary>
		void PlayerFinishParsing();

		/// <summary></summary>
		void MonsterFinishParsing();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldStripTrailingPunctuation();

		/// <summary></summary>
		void FinishParsing();

		/// <summary></summary>
		/// <returns></returns>
		string GetActiveObjData();

		/// <summary></summary>
		/// <param name="artifact"></param>
		void SetArtifact(IArtifact artifact);

		/// <summary></summary>
		/// <param name="monster"></param>
		void SetMonster(IMonster monster);

		/// <summary></summary>
		/// <returns></returns>
		IArtifact GetArtifact();

		/// <summary></summary>
		/// <returns></returns>
		IMonster GetMonster();

		/// <summary></summary>
		void Clear();

		/// <summary></summary>
		void ParseName();

		/// <summary></summary>
		/// <param name="obj"></param>
		/// <param name="objDataName"></param>
		/// <param name="artifact"></param>
		/// <param name="monster"></param>
		void SetLastNameStrings(IGameBase obj, string objDataName, IArtifact artifact, IMonster monster);

		/// <summary></summary>
		/// <param name="afterFinishParsing"></param>
		void CheckPlayerCommand(bool afterFinishParsing);

		/// <summary></summary>
		void Execute();
	}
}
