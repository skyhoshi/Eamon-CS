
// ISharpSerializer.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.IO;

namespace Eamon.Framework.Portability
{
	public interface ISharpSerializer
	{
		bool IsActive { get; }

		void Serialize(object data, string fileName, bool binaryMode = false);

		void Serialize(object data, Stream stream, bool binaryMode = false);

		object Deserialize(string fileName, bool binaryMode = false);

		object Deserialize(Stream stream, bool binaryMode = false);
	}
}
