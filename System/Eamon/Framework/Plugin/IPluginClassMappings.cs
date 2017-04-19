
// IPluginClassMappings.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Eamon.Framework.Portability;

namespace Eamon.Framework.Plugin
{
	public interface IPluginClassMappings
	{
		IDictionary<Type, Type> ClassMappingsDictionary { get; set; }

		ITextReader In { get; set; }

		ITextWriter Out { get; set; }

		ITextWriter Error { get; set; }

		IMutex Mutex { get; set; }

		ITransferProtocol TransferProtocol { get; set; }

		IDirectory Directory { get; set; }

		IFile File { get; set; }

		IPath Path { get; set; }

		ISharpSerializer SharpSerializer { get; set; }

		IThread Thread { get; set; }

		MemoryStream CloneStream { get; set; }

		string WorkDir { get; set; }

		bool EnableStdio { get; set; }

		bool IgnoreMutex { get; set; }

		bool RunGameEditor { get; set; }

		bool DeleteGameStateFromMainHall { get; set; }

		Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		void HandleException(Exception ex, string stackTraceFile, string errorMessage);

		void ResolvePortabilityClassMappings();

		void ProcessArgv(string[] args);

		RetCode LoadPluginClassMappings();

		RetCode LoadPluginClassMappings01(Assembly plugin);

		T CreateInstance<T>(Type ifaceType, Action<T> initialize = null) where T : class;

		T CreateInstance<T>(Action<T> initialize = null) where T : class;

		T CloneInstance<T>(T source) where T : class;

		bool CompareInstances<T>(T object1, T object2) where T : class;
	}
}
