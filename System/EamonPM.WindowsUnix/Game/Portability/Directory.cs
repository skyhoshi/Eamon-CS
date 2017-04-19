
// Directory.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class Directory : IDirectory
	{
		protected virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(System.IO.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', System.IO.Path.DirectorySeparatorChar) : null;
		}

		public virtual bool Exists(string path)
		{
			return System.IO.Directory.Exists(NormalizePath(path));
		}

		public virtual void CreateDirectory(string path)
		{
			System.IO.Directory.CreateDirectory(NormalizePath(path));
		}

		public virtual void SetCurrentDirectory(string path)
		{
			System.IO.Directory.SetCurrentDirectory(NormalizePath(path));
		}
	}
}
