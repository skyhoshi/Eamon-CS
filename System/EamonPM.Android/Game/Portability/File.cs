
// File.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class File : IFile
	{
		public virtual bool Exists(string path)
		{
			return System.IO.File.Exists(NormalizePath(path));
		}

		public virtual void Delete(string path)
		{
			System.IO.File.Delete(NormalizePath(path));
		}

		public virtual void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			System.IO.File.Copy(NormalizePath(sourceFileName), NormalizePath(destFileName), overwrite);
		}

		public virtual string ReadFirstLine(string path, Encoding encoding = null)
		{
			var firstLine = "";

			using (var streamReader = new System.IO.StreamReader(NormalizePath(path), encoding ?? new UTF8Encoding(true)))
			{
				firstLine = streamReader.ReadLine();
			}

			return firstLine;
		}

		public virtual string ReadAllText(string path, Encoding encoding = null)
		{
			return System.IO.File.ReadAllText(NormalizePath(path), encoding ?? new UTF8Encoding(true));
		}

		public virtual void WriteAllText(string path, string contents, Encoding encoding = null)
		{
			System.IO.File.WriteAllText(NormalizePath(path), contents, encoding ?? new UTF8Encoding(true));
		}

		public virtual void AppendAllText(string path, string contents, Encoding encoding = null)
		{
			System.IO.File.AppendAllText(NormalizePath(path), contents, encoding ?? new UTF8Encoding(true));
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
