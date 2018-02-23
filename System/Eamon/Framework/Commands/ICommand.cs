
// ICommand.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Parsing;
using Eamon.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Commands
{
	public interface ICommand : IState
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

		bool IsDarkEnabled { get; set; }

		bool IsPlayerEnabled { get; set; }

		bool IsMonsterEnabled { get; set; }

		string GetPrintedVerb();

		bool IsEnabled(IMonster monster);

		void CopyCommandData(ICommand destCommand, bool includeIobj = true);

		void FinishParsing();
	}
}
