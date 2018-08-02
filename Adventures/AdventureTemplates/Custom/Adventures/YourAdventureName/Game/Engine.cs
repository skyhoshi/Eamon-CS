
// Engine.cs

// Copyright (c) 2014+ by YourAuthorName.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace YourAdventureName.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{

	}
}
