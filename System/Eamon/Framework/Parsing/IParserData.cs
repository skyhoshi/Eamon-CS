
// IParserData.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;

namespace Eamon.Framework.Parsing
{
	public interface IParserData
	{
		string Name { get; set; }

		string QueryDesc { get; set; }

		IArtifact Artifact { get; set; }

		IMonster Monster { get; set; }

		IList<IArtifact> GetArtifactList { get; set; }

		IList<IMonster> GetMonsterList { get; set; }

		IList<IArtifact> FilterArtifactList { get; set; }

		IList<IMonster> FilterMonsterList { get; set; }

		IList<Func<IArtifact, bool>> ArtifactWhereClauseList { get; set; }

		IList<Func<IMonster, bool>> MonsterWhereClauseList { get; set; }

		Delegates.GetArtifactListFunc GetArtifactListFunc { get; set; }

		Delegates.GetMonsterListFunc GetMonsterListFunc { get; set; }

		Delegates.FilterArtifactListFunc FilterArtifactListFunc { get; set; }

		Delegates.FilterMonsterListFunc FilterMonsterListFunc { get; set; }

		Delegates.FilterRecordListFunc FilterRecordListFunc { get; set; }

		Delegates.RevealEmbeddedArtifactFunc RevealEmbeddedArtifactFunc { get; set; }

		Action ArtifactMatchFunc { get; set; }

		Action MonsterMatchFunc { get; set; }

		Action ArtifactNotFoundFunc { get; set; }

		Action MonsterNotFoundFunc { get; set; }
	}
}
