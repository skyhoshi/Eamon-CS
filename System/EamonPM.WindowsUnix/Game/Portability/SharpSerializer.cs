
// SharpSerializer.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.IO;
using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class SharpSerializer : ISharpSerializer
	{
		public virtual bool IsActive { get; protected set; }

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

			try
			{
				IsActive = true;

				sharpSerializer.Serialize(data, stream);
			}
			finally
			{
				IsActive = false;
			}
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

			object result = null;

			try
			{
				IsActive = true;

				result = sharpSerializer.Deserialize(stream);
			}
			finally
			{
				IsActive = false;
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(System.IO.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', System.IO.Path.DirectorySeparatorChar) : null;
		}
	}
}
