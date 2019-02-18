
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using System.Collections.Generic;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace TheSubAquanLaboratory.Game.Helpers
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
		protected virtual bool ValidateFoodButtonPushes()
		{
			return Record.FoodButtonPushes >= 0 && Record.FoodButtonPushes <= 2;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateFlood()
		{
			return Record.Flood >= 0 && Record.Flood <= 2;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateFloodLevel()
		{
			return Record.FloodLevel >= 0 && Record.FloodLevel <= 11;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateElevation()
		{
			return Record.Elevation >= 0 && Record.Elevation <= 4;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateEnergyMaceCharge()
		{
			return Record.EnergyMaceCharge >= 0 && Record.EnergyMaceCharge <= 120;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateLaserScalpelCharge()
		{
			return Record.LaserScalpelCharge >= 0 && Record.LaserScalpelCharge <= 40;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateQuestValue()
		{
			return Record.QuestValue >= 0 && Record.QuestValue <= 1250;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateFakeWallExamines()
		{
			return Record.FakeWallExamines >= 0 && Record.FakeWallExamines <= 2;
		}

		/// <summary></summary>
		/// <returns></returns>
		protected virtual bool ValidateLabRoomsSeen()
		{
			return Record.LabRoomsSeen >= 0 && Record.LabRoomsSeen <= 45;
		}

		public GameStateHelper()
		{
			FieldNames.AddRange(new List<string>()
			{
				"FoodButtonPushes",
				"Flood",
				"FloodLevel",
				"Elevation",
				"EnergyMaceCharge",
				"LaserScalpelCharge",
				"QuestValue",
				"FakeWallExamines",
				"LabRoomsSeen",
			});
		}
	}
}
