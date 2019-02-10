
// ISharpSerializer.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.IO;

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface ISharpSerializer
	{
		/// <summary></summary>
		bool IsActive { get; }

		/// <summary></summary>
		/// <param name="data"></param>
		/// <param name="fileName"></param>
		/// <param name="binaryMode"></param>
		void Serialize(object data, string fileName, bool binaryMode = false);

		/// <summary></summary>
		/// <param name="data"></param>
		/// <param name="stream"></param>
		/// <param name="binaryMode"></param>
		void Serialize(object data, Stream stream, bool binaryMode = false);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="binaryMode"></param>
		/// <returns></returns>
		object Deserialize(string fileName, bool binaryMode = false);

		/// <summary></summary>
		/// <param name="stream"></param>
		/// <param name="binaryMode"></param>
		/// <returns></returns>
		object Deserialize(Stream stream, bool binaryMode = false);
	}
}
