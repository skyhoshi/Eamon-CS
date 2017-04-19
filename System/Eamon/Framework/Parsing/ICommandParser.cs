
// ICommandParser.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System.Text;
using Eamon.Framework.States;

namespace Eamon.Framework.Parsing
{
	public interface ICommandParser
	{
		StringBuilder InputBuf { get; set; }

		string LastInputStr { get; set; }

		string[] Tokens { get; set; }

		long CurrToken { get; set; }

		long PrepTokenIndex { get; set; }

		IMonster ActorMonster { get; set; }

		IRoom ActorRoom { get; set; }

		IParserData DobjData { get; set; }

		IParserData IobjData { get; set; }

		IParserData ObjData { get; set; }

		IState NextState { get; set; }

		string GetActiveObjData();

		void SetArtifact(IArtifact artifact);

		void SetMonster(IMonster monster);

		IArtifact GetArtifact();

		IMonster GetMonster();

		void Clear();

		void ParseName();

		void Execute();
	}
}
