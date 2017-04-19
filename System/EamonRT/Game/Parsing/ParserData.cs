
// ParserData.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Parsing;
using Eamon.Game.Attributes;

namespace EamonRT.Game.Parsing
{
	[ClassMappings]
	public class ParserData : IParserData
	{
		public virtual string Name { get; set; }

		public virtual string QueryDesc { get; set; }

		public virtual IArtifact Artifact { get; set; }

		public virtual IMonster Monster { get; set; }

		public virtual IList<IArtifact> GetArtifactList { get; set; }

		public virtual IList<IMonster> GetMonsterList { get; set; }

		public virtual IList<IArtifact> FilterArtifactList { get; set; }

		public virtual IList<IMonster> FilterMonsterList { get; set; }

		public virtual IList<Func<IArtifact, bool>> ArtifactWhereClauseList { get; set; }

		public virtual IList<Func<IMonster, bool>> MonsterWhereClauseList { get; set; }

		public virtual Delegates.GetArtifactListFunc GetArtifactListFunc { get; set; }

		public virtual Delegates.GetMonsterListFunc GetMonsterListFunc { get; set; }

		public virtual Delegates.FilterArtifactListFunc FilterArtifactListFunc { get; set; }

		public virtual Delegates.FilterMonsterListFunc FilterMonsterListFunc { get; set; }

		public virtual Delegates.FilterRecordListFunc FilterRecordListFunc { get; set; }

		public virtual Delegates.RevealEmbeddedArtifactFunc RevealEmbeddedArtifactFunc { get; set; }

		public virtual Action ArtifactMatchFunc { get; set; }

		public virtual Action MonsterMatchFunc { get; set; }

		public virtual Action ArtifactNotFoundFunc { get; set; }

		public virtual Action MonsterNotFoundFunc { get; set; }
	}
}
