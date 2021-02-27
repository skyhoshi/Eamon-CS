
// TextSerializer.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.IO;
using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class TextSerializer : ITextSerializer
	{
		public virtual bool IsActive { get; protected set; }

		public virtual void Serialize<T>(T data, string fileName, bool binaryMode = false) where T : class
		{
			using (var fileStream = new FileStream(NormalizePath(fileName), FileMode.Create))
			{
				Serialize(data, fileStream, binaryMode);
			}
		}

		public virtual void Serialize<T>(T data, Stream stream, bool binaryMode = false) where T : class
		{
			var textSerializer = new Polenter.Serialization.SharpSerializer(binaryMode);

			try
			{
				IsActive = true;

				textSerializer.Serialize(data, stream);
			}
			finally
			{
				IsActive = false;
			}
		}

		public virtual T Deserialize<T>(string fileName, bool binaryMode = false) where T : class
		{
			using (var fileStream = new FileStream(NormalizePath(fileName), FileMode.Open))
			{
				return Deserialize<T>(fileStream, binaryMode);
			}
		}

		public virtual T Deserialize<T>(Stream stream, bool binaryMode = false) where T : class
		{
			var textSerializer = new Polenter.Serialization.SharpSerializer(binaryMode);

			T result = default(T);

			try
			{
				IsActive = true;

				result = textSerializer.Deserialize(stream) as T;
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
