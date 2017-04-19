
// IProgram.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using System.Collections.Generic;

namespace Eamon.Framework.Portability
{
	public interface IProgram
	{
		bool EnableStdio { get; set; }

		Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		void Main(string[] args);
	}
}
