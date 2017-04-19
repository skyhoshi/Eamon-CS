
// IGameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace TheSubAquanLaboratory.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		long FoodButtonPushes { get; set; }

		bool Sterilize { get; set; }

		long Flood { get; set; }

		long FloodLevel { get; set; }

		long Elevation { get; set; }

		bool Energize { get; set; }

		long EnergyMaceCharge { get; set; }

		long LaserScalpelCharge { get; set; }

		bool CabinetOpen { get; set; }

		bool LockerOpen { get; set; }

		bool Shark { get; set; }

		bool FloorAttack { get; set; }

		long QuestValue { get; set; }

		bool ReadPlaque { get; set; }

		bool ReadTerminals { get; set; }

		long FakeWallExamines { get; set; }

		bool AlphabetDial { get; set; }

		bool ReadDisplayScreen { get; set; }

		long LabRoomsSeen { get; set; }
	}
}
