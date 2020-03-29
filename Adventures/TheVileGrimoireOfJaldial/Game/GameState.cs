
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using TheVileGrimoireOfJaldial.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual bool GriffinAngered { get; set; }

		public virtual bool EfreetiKilled { get; set; }

		public virtual bool WaterWeirdKilled { get; set; }

		public virtual bool AmoebaAppeared { get; set; }

		public virtual bool ExitDirNames { get; set; }

		public virtual bool FoggyRoom { get; set; }

		public virtual long PlayerResurrections { get; set; }

		public virtual long BloodnettleVictimUid { get; set; }

		public virtual long EfreetiSummons { get; set; }

		public virtual long TorchRounds { get; set; }

		public virtual long Minute { get; set; }

		public virtual long Hour { get; set; }

		public virtual long Day { get; set; }

		public virtual long WeatherIntensity { get; set; }

		public virtual long WeatherDuration { get; set; }

		public virtual WeatherType WeatherType { get; set; }

		public virtual bool IsNightTime()
		{
			return !IsDayTime();
		}

		public virtual bool IsDayTime()
		{
			return Hour > 6 && Hour < 19;
		}

		public virtual bool IsRaining()
		{
			return WeatherType == WeatherType.Rain;
		}

		public virtual bool IsFoggy()
		{
			return WeatherType == WeatherType.Fog;
		}

		public GameState()
		{
			Hour = 7;
		}
	}
}
