
// IDirectory.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Portability
{
	public interface IDirectory
	{
		bool Exists(string path);

		void Delete(string path, bool recursive);

		void CreateDirectory(string path);

		void SetCurrentDirectory(string path);
	}
}
