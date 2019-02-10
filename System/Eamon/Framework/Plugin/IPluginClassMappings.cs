
// IPluginClassMappings.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Eamon.Framework.Portability;

namespace Eamon.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginClassMappings
	{
		/// <summary></summary>
		IDictionary<Type, Type> ClassMappingsDictionary { get; set; }

		/// <summary></summary>
		ITextReader In { get; set; }

		/// <summary></summary>
		ITextWriter Out { get; set; }

		/// <summary></summary>
		ITextWriter Error { get; set; }

		/// <summary></summary>
		IMutex Mutex { get; set; }

		/// <summary></summary>
		ITransferProtocol TransferProtocol { get; set; }

		/// <summary></summary>
		IDirectory Directory { get; set; }

		/// <summary></summary>
		IFile File { get; set; }

		/// <summary></summary>
		IPath Path { get; set; }

		/// <summary></summary>
		ISharpSerializer SharpSerializer { get; set; }

		/// <summary></summary>
		IThread Thread { get; set; }

		/// <summary></summary>
		MemoryStream CloneStream { get; set; }

		/// <summary></summary>
		string WorkDir { get; set; }

		/// <summary></summary>
		string FilePrefix { get; set; }

		/// <summary></summary>
		long RulesetVersion { get; set; }

		/// <summary></summary>
		bool EnableGameOverrides { get; }

		/// <summary></summary>
		bool EnableStdio { get; set; }

		/// <summary></summary>
		bool IgnoreMutex { get; set; }

		/// <summary></summary>
		bool RunGameEditor { get; set; }

		/// <summary></summary>
		bool DeleteGameStateFromMainHall { get; set; }

		/// <summary></summary>
		Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		/// <summary></summary>
		/// <param name="ex"></param>
		/// <param name="stackTraceFile"></param>
		/// <param name="errorMessage"></param>
		void HandleException(Exception ex, string stackTraceFile, string errorMessage);

		/// <summary></summary>
		void ResolvePortabilityClassMappings();

		/// <summary></summary>
		/// <param name="args"></param>
		void ProcessArgv(string[] args);

		/// <summary></summary>
		/// <returns></returns>
		RetCode LoadPluginClassMappings();

		/// <summary></summary>
		/// <param name="plugin"></param>
		/// <returns></returns>
		RetCode LoadPluginClassMappings01(Assembly plugin);

		/// <summary></summary>
		/// <param name="ifaceType"></param>
		/// <param name="initialize"></param>
		/// <returns></returns>
		T CreateInstance<T>(Type ifaceType, Action<T> initialize = null) where T : class;

		/// <summary></summary>
		/// <param name="initialize"></param>
		/// <returns></returns>
		T CreateInstance<T>(Action<T> initialize = null) where T : class;

		/// <summary></summary>
		/// <param name="source"></param>
		/// <returns></returns>
		T CloneInstance<T>(T source) where T : class;

		/// <summary></summary>
		/// <param name="object1"></param>
		/// <param name="object2"></param>
		/// <returns></returns>
		bool CompareInstances<T>(T object1, T object2) where T : class;

		/// <summary></summary>
		/// <param name="versions"></param>
		/// <returns></returns>
		bool IsRulesetVersion(params long[] versions);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		string GetPrefixedFileName(string fileName);
	}
}
