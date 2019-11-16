
// Directory.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Linq;
using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class Directory : IDirectory
	{
		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		protected virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(System.IO.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', System.IO.Path.DirectorySeparatorChar) : null;
		}

		public virtual bool Exists(string path)
		{
			return System.IO.Directory.Exists(NormalizePath(path));
		}

		public virtual void Delete(string path, bool recursive)
		{
			/*
			System.IO.Directory.Delete(NormalizePath(path), recursive);
			*/
		}

		public virtual void DeleteEmptySubdirectories(string path, bool recursive)
		{
			foreach (var directory in System.IO.Directory.GetDirectories(NormalizePath(path)))
			{
				if (recursive)
				{
					DeleteEmptySubdirectories(directory, recursive);
				}

				if (!System.IO.Directory.EnumerateFileSystemEntries(directory).Any())
				{
					/*
					System.IO.Directory.Delete(directory, false);
					*/
				}
			}
		}

		public virtual void CreateDirectory(string path)
		{
			System.IO.Directory.CreateDirectory(NormalizePath(path));
		}

		public virtual void SetCurrentDirectory(string path)
		{
			System.IO.Directory.SetCurrentDirectory(NormalizePath(path));
		}

		public virtual string GetCurrentDirectory()
		{
			return System.IO.Directory.GetCurrentDirectory();
		}

		public virtual string[] GetFiles(string path)
		{
			return System.IO.Directory.GetFiles(NormalizePath(path));
		}

		public virtual string[] GetDirectories(string path)
		{
			return System.IO.Directory.GetDirectories(NormalizePath(path));
		}
	}
}
