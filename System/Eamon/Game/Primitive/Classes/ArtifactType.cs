
// ArtifactType.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class ArtifactType : IArtifactType
	{
		public virtual string Name { get; set; }

		public virtual string WeightEmptyVal { get; set; }

		public virtual string LocationEmptyVal { get; set; }

		public virtual string Field5Name { get; set; }

		public virtual string Field5EmptyVal { get; set; }

		public virtual string Field6Name { get; set; }

		public virtual string Field6EmptyVal { get; set; }

		public virtual string Field7Name { get; set; }

		public virtual string Field7EmptyVal { get; set; }

		public virtual string Field8Name { get; set; }

		public virtual string Field8EmptyVal { get; set; }
	}
}
