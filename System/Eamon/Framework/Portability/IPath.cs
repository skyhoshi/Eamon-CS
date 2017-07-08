
// IPath.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Portability
{
	public interface IPath
	{
		char DirectorySeparatorChar { get; }

		bool EqualPaths(string path1, string path2);

		string Combine(string path1, string path2);

		string GetDirectoryName(string path);

		string GetExtension(string path);

		string GetFileName(string path);

		string GetFileNameWithoutExtension(string path);
	}
}
