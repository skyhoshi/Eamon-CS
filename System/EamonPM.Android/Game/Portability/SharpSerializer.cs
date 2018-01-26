
// SharpSerializer.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.IO;
using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class SharpSerializer : ISharpSerializer
	{
		protected virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(System.IO.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', System.IO.Path.DirectorySeparatorChar) : null;
		}

		public virtual void Serialize(object data, string fileName, bool binaryMode = false)
		{
			using (var fileStream = new FileStream(NormalizePath(fileName), FileMode.Create))
			{
				Serialize(data, fileStream, binaryMode);
			}
		}

		public virtual void Serialize(object data, Stream stream, bool binaryMode = false)
		{
			var sharpSerializer = new Polenter.Serialization.SharpSerializer(binaryMode);

			sharpSerializer.Serialize(data, stream);
		}

		public virtual object Deserialize(string fileName, bool binaryMode = false)
		{
			using (var fileStream = new FileStream(NormalizePath(fileName), FileMode.Open))
			{
				return Deserialize(fileStream, binaryMode);
			}
		}

		public virtual object Deserialize(Stream stream, bool binaryMode = false)
		{
			var sharpSerializer = new Polenter.Serialization.SharpSerializer(binaryMode);

			return sharpSerializer.Deserialize(stream);
		}
	}
}
