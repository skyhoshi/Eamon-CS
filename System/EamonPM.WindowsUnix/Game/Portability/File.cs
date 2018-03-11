
// File.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class File : IFile
	{
		protected virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(System.IO.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', System.IO.Path.DirectorySeparatorChar) : null;
		}

		public virtual bool Exists(string path)
		{
			return System.IO.File.Exists(NormalizePath(path));
		}

		public virtual void Delete(string path)
		{
			System.IO.File.Delete(NormalizePath(path));
		}

		public virtual string ReadFirstLine(string path)
		{
			var firstLine = "";

			using (var streamReader = new System.IO.StreamReader(NormalizePath(path)))
			{
				firstLine = streamReader.ReadLine();
			}

			return firstLine;
		}

		public virtual string ReadAllText(string path)
		{
			return System.IO.File.ReadAllText(NormalizePath(path));
		}

		public virtual void WriteAllText(string path, string contents)
		{
			System.IO.File.WriteAllText(NormalizePath(path), contents);
		}

		public virtual void AppendAllText(string path, string contents)
		{
			System.IO.File.AppendAllText(NormalizePath(path), contents);
		}
	}
}
