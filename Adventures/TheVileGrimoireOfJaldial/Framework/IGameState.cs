
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Collections.Generic;
using TheVileGrimoireOfJaldial.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		/// <summary></summary>
		bool GriffinAngered { get; set; }

		/// <summary></summary>
		bool GiantCrayfishKilled { get; set; }

		/// <summary></summary>
		bool WaterWeirdKilled { get; set; }

		/// <summary></summary>
		bool EfreetiKilled { get; set; }

		/// <summary></summary>
		bool AmoebaAppeared { get; set; }

		/// <summary></summary>
		bool ExitDirNames { get; set; }

		/// <summary></summary>
		bool FoggyRoom { get; set; }

		/// <summary></summary>
		bool[] SecretDoors { get; set; }

		/// <summary></summary>
		long FoggyRoomWeatherIntensity { get; set; }

		/// <summary></summary>
		long PlayerResurrections { get; set; }

		/// <summary></summary>
		long BloodnettleVictimUid { get; set; }

		/// <summary></summary>
		long EfreetiSummons { get; set; }

		/// <summary></summary>
		long LightningBolts { get; set; }

		/// <summary></summary>
		long IceBolts { get; set; }

		/// <summary></summary>
		long MentalBlasts { get; set; }

		long MysticMissiles { get; set; }

		long FireBalls { get; set; }

		long ClumsySpells { get; set; }

		/// <summary></summary>
		long TorchRounds { get; set; }

		/// <summary></summary>
		long Minute { get; set; }

		/// <summary></summary>
		long Hour { get; set; }

		/// <summary></summary>
		long Day { get; set; }

		/// <summary></summary>
		long WeatherIntensity { get; set; }

		/// <summary></summary>
		long WeatherDuration { get; set; }

		/// <summary></summary>
		WeatherType WeatherType { get; set; }

		IDictionary<long, IList<long>> ClumsyTargets { get; set; }

		/// <summary></summary>
		bool IsNightTime();

		/// <summary></summary>
		bool IsDayTime();

		/// <summary></summary>
		bool IsRaining();

		/// <summary></summary>
		bool IsFoggy();

		/// <summary></summary>
		bool IsFoggyHours();

		bool GetSecretDoors(long index);

		void SetSecretDoors(long index, bool value);

		void SetFoggyRoom(IRoom room);
	}
}
