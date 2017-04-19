
// IFile.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Portability
{
	public interface IFile
	{
		bool Exists(string path);

		void Delete(string path);

		void WriteAllText(string path, string contents);
	}
}
