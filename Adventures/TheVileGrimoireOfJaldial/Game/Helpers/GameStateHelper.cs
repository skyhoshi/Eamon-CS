
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace TheVileGrimoireOfJaldial.Game.Helpers
{
	[ClassMappings]
	public class GameStateHelper : Eamon.Game.Helpers.GameStateHelper, IGameStateHelper
	{
		public virtual new Framework.IGameState Record
		{
			get
			{
				return (Framework.IGameState)base.Record;
			}

			set
			{
				if (base.Record != value)
				{
					base.Record = value;
				}
			}
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateFoggyRoomWeatherIntensity()
		{
			return Record.FoggyRoomWeatherIntensity >= 0 && Record.FoggyRoomWeatherIntensity <= 4;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidatePlayerResurrections()
		{
			return Record.PlayerResurrections >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidatePlayerHardinessPointsDrained()
		{
			return Record.PlayerHardinessPointsDrained >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateBloodnettleVictimUid()
		{
			return Record.BloodnettleVictimUid >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateEfreetiSummons()
		{
			return Record.EfreetiSummons >= 0 && Record.EfreetiSummons <= 4;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateLightningBolts()
		{
			return Record.LightningBolts >= 0 && Record.LightningBolts <= 7;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateIceBolts()
		{
			return Record.IceBolts >= 0 && Record.IceBolts <= 7;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateMentalBlasts()
		{
			return Record.MentalBlasts >= 0 && Record.MentalBlasts <= 3;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateMysticMissiles()
		{
			return Record.MysticMissiles >= 0 && Record.MysticMissiles <= 5;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateFireBalls()
		{
			return Record.FireBalls >= 0 && Record.FireBalls <= 7;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateClumsySpells()
		{
			return Record.ClumsySpells >= 0 && Record.ClumsySpells <= 4;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateTorchRounds()
		{
			return Record.TorchRounds >= 400 && Record.TorchRounds <= 480;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateMinute()
		{
			return Record.Minute >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateHour()
		{
			return Record.Hour >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateDay()
		{
			return Record.Day >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeatherIntensity()
		{
			return Record.WeatherIntensity >= 0 && Record.WeatherIntensity <= 4;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateWeatherDuration()
		{
			return Record.WeatherDuration >= 0;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"FoggyRoomWeatherIntensity",
				"PlayerResurrections",
				"PlayerHardinessPointsDrained",
				"BloodnettleVictimUid",
				"EfreetiSummons",
				"LightningBolts",
				"IceBolts",
				"MentalBlasts",
				"MysticMissiles",
				"FireBalls",
				"ClumsySpells",
				"TorchRounds",
				"Minute",
				"Hour",
				"Day",
				"WeatherIntensity",
				"WeatherDuration"
			});
		}
	}
}
