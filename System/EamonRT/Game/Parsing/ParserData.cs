﻿
// ParserData.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Parsing;

namespace EamonRT.Game.Parsing
{
	[ClassMappings]
	public class ParserData : IParserData
	{
		public virtual string Name { get; set; }

		public virtual Func<string> QueryDescFunc { get; set; }

		public virtual IGameBase Obj { get; set; }

		public virtual IArtifact Artifact
		{
			get
			{
				return Obj as IArtifact;
			}
		}

		public virtual IMonster Monster
		{
			get
			{
				return Obj as IMonster;
			}
		}

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
