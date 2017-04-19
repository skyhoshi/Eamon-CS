
// ClassMappingsAttribute.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;

namespace Eamon.Game.Attributes
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	public class ClassMappingsAttribute : Attribute
	{
		public Type InterfaceType { get; set; }

		public ClassMappingsAttribute(Type interfaceType)
		{
			InterfaceType = interfaceType;
		}

		public ClassMappingsAttribute()
		{

		}
	}
}
