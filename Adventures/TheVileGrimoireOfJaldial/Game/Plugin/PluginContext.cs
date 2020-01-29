
// PluginContext.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;

namespace TheVileGrimoireOfJaldial.Game.Plugin
{
	public static class PluginContext
	{
		public static Framework.Plugin.IPluginConstants Constants
		{
			get
			{
				return (Framework.Plugin.IPluginConstants)EamonRT.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static Framework.Plugin.IPluginClassMappings ClassMappings
		{
			get
			{
				return (Framework.Plugin.IPluginClassMappings)EamonRT.Game.Plugin.PluginContext.ClassMappings;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.ClassMappings = value;
			}
		}

		public static Framework.Plugin.IPluginGlobals Globals
		{
			get
			{
				return (Framework.Plugin.IPluginGlobals)EamonRT.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Globals = value;
			}
		}

		public static ITextWriter gOut
		{
			get
			{
				return EamonRT.Game.Plugin.PluginContext.gOut;
			}
		}

		public static Framework.IEngine gEngine
		{
			get
			{
				return (Framework.IEngine)EamonRT.Game.Plugin.PluginContext.gEngine;
			}
		}

		public static IRecordDb<Eamon.Framework.IRoom> gRDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IRoom>)EamonRT.Game.Plugin.PluginContext.gRDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IArtifact> gADB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IArtifact>)EamonRT.Game.Plugin.PluginContext.gADB;
			}
		}

		public static IRecordDb<Eamon.Framework.IEffect> gEDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IEffect>)EamonRT.Game.Plugin.PluginContext.gEDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IMonster>)EamonRT.Game.Plugin.PluginContext.gMDB;
			}
		}

		public static Framework.IMonster gActorMonster
		{
			get
			{
				return (Framework.IMonster)EamonRT.Game.Plugin.PluginContext.gActorMonster;
			}
		}

		public static Framework.IRoom gActorRoom
		{
			get
			{
				return (Framework.IRoom)EamonRT.Game.Plugin.PluginContext.gActorRoom;
			}
		}

		public static Framework.IArtifact gDobjArtifact
		{
			get
			{
				return (Framework.IArtifact)EamonRT.Game.Plugin.PluginContext.gDobjArtifact;
			}
		}

		public static Framework.IMonster gDobjMonster
		{
			get
			{
				return (Framework.IMonster)EamonRT.Game.Plugin.PluginContext.gDobjMonster;
			}
		}

		public static Framework.IArtifact gIobjArtifact
		{
			get
			{
				return (Framework.IArtifact)EamonRT.Game.Plugin.PluginContext.gIobjArtifact;
			}
		}

		public static Framework.IMonster gIobjMonster
		{
			get
			{
				return (Framework.IMonster)EamonRT.Game.Plugin.PluginContext.gIobjMonster;
			}
		}

		public static Framework.Parsing.ICommandParser gCommandParser
		{
			get
			{
				return (Framework.Parsing.ICommandParser)EamonRT.Game.Plugin.PluginContext.gCommandParser;
			}
		}

		public static Framework.IGameState gGameState
		{
			get
			{
				return (Framework.IGameState)EamonRT.Game.Plugin.PluginContext.gGameState;
			}
		}

		public static Eamon.Framework.ICharacter gCharacter
		{
			get
			{
				return (Eamon.Framework.ICharacter)EamonRT.Game.Plugin.PluginContext.gCharacter;
			}
		}
	}
}
