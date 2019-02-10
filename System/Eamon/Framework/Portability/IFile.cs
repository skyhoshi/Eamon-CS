
// IFile.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface IFile
	{
		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool Exists(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		void Delete(string path);

		/// <summary></summary>
		/// <param name="sourceFileName"></param>
		/// <param name="destFileName"></param>
		/// <param name="overwrite"></param>
		void Copy(string sourceFileName, string destFileName, bool overwrite);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string ReadFirstLine(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string ReadAllText(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="contents"></param>
		void WriteAllText(string path, string contents);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="contents"></param>
		void AppendAllText(string path, string contents);
	}
}
