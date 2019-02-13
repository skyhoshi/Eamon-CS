
// IParserData.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework;

namespace EamonRT.Framework.Parsing
{
	/// <summary></summary>
	public interface IParserData
	{
		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		string QueryDesc { get; set; }

		/// <summary></summary>
		IGameBase Obj { get; set; }

		/// <summary></summary>
		IArtifact Artifact { get; }

		/// <summary></summary>
		IMonster Monster { get; }

		/// <summary></summary>
		IList<IArtifact> GetArtifactList { get; set; }

		/// <summary></summary>
		IList<IMonster> GetMonsterList { get; set; }

		/// <summary></summary>
		IList<IArtifact> FilterArtifactList { get; set; }

		/// <summary></summary>
		IList<IMonster> FilterMonsterList { get; set; }

		/// <summary></summary>
		IList<Func<IArtifact, bool>> ArtifactWhereClauseList { get; set; }

		/// <summary></summary>
		IList<Func<IMonster, bool>> MonsterWhereClauseList { get; set; }

		/// <summary></summary>
		Delegates.GetArtifactListFunc GetArtifactListFunc { get; set; }

		/// <summary></summary>
		Delegates.GetMonsterListFunc GetMonsterListFunc { get; set; }

		/// <summary></summary>
		Delegates.FilterArtifactListFunc FilterArtifactListFunc { get; set; }

		/// <summary></summary>
		Delegates.FilterMonsterListFunc FilterMonsterListFunc { get; set; }

		/// <summary></summary>
		Delegates.FilterRecordListFunc FilterRecordListFunc { get; set; }

		/// <summary></summary>
		Delegates.RevealEmbeddedArtifactFunc RevealEmbeddedArtifactFunc { get; set; }

		/// <summary></summary>
		Action ArtifactMatchFunc { get; set; }

		/// <summary></summary>
		Action MonsterMatchFunc { get; set; }

		/// <summary></summary>
		Action ArtifactNotFoundFunc { get; set; }

		/// <summary></summary>
		Action MonsterNotFoundFunc { get; set; }
	}
}
