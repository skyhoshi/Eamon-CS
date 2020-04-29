
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
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

		public virtual bool GiantCrayfishKilled { get; set; }

		public virtual bool WaterWeirdKilled { get; set; }

		public virtual bool EfreetiKilled { get; set; }

		public virtual bool AmoebaAppeared { get; set; }

		public virtual bool ExitDirNames { get; set; }

		public virtual bool FoggyRoom { get; set; }

		public virtual bool[] SecretDoors { get; set; }

		public virtual long FoggyRoomWeatherIntensity { get; set; }

		public virtual long PlayerResurrections { get; set; }

		public virtual long BloodnettleVictimUid { get; set; }

		public virtual long EfreetiSummons { get; set; }

		public virtual long LightningBolts { get; set; }

		public virtual long IceBolts { get; set; }

		public virtual long MentalBlasts { get; set; }

		public virtual long MysticMissiles { get; set; }

		public virtual long FireBalls { get; set; }

		public virtual long ClumsySpells { get; set; }

		public virtual long TorchRounds { get; set; }

		public virtual long Minute { get; set; }

		public virtual long Hour { get; set; }

		public virtual long Day { get; set; }

		public virtual long WeatherIntensity { get; set; }

		public virtual long WeatherDuration { get; set; }

		public virtual WeatherType WeatherType { get; set; }

		public virtual IDictionary<long, IList<long>> ClumsyTargets { get; set; }

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

		public virtual bool IsFoggyHours()
		{
			return Hour < 10 || Hour > 20;
		}

		public virtual bool GetSecretDoors(long index)
		{
			return SecretDoors[index];
		}

		public virtual void SetSecretDoors(long index, bool value)
		{
			SecretDoors[index] = value;
		}

		public virtual void SetFoggyRoom(Framework.IRoom room)
		{
			Debug.Assert(room != null);

			FoggyRoom = room.IsGroundsRoom() && IsFoggy() && gEngine.RollDice(1, 100, 0) > 40;

			FoggyRoomWeatherIntensity = FoggyRoom ? gEngine.RollDice(1, WeatherIntensity, 0) : 0;
		}

		public GameState()
		{
			SecretDoors = new bool[13];

			Hour = 6;

			Minute = 55;

			ClumsyTargets = new Dictionary<long, IList<long>>();
		}
	}
}
