
// IFile.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Portability
{
	public interface IFile
	{
		bool Exists(string path);

		void Delete(string path);

		string ReadFirstLine(string path);

		string ReadAllText(string path);

		void WriteAllText(string path, string contents);

		void AppendAllText(string path, string contents);
	}
}
