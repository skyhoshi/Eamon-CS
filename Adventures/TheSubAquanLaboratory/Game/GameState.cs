
// GameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long FoodButtonPushes { get; set; }

		public virtual bool Sterilize { get; set; }

		public virtual long Flood { get; set; }

		public virtual long FloodLevel { get; set; }

		public virtual long Elevation { get; set; }

		public virtual bool Energize { get; set; }

		public virtual long EnergyMaceCharge { get; set; }

		public virtual long LaserScalpelCharge { get; set; }

		public virtual bool CabinetOpen { get; set; }

		public virtual bool LockerOpen { get; set; }

		public virtual bool Shark { get; set; }

		public virtual bool FloorAttack { get; set; }

		public virtual long QuestValue { get; set; }

		public virtual bool ReadPlaque { get; set; }

		public virtual bool ReadTerminals { get; set; }

		public virtual long FakeWallExamines { get; set; }

		public virtual bool AlphabetDial { get; set; }

		public virtual bool ReadDisplayScreen { get; set; }

		public virtual long LabRoomsSeen { get; set; }

		public GameState()
		{
			EnergyMaceCharge = 120;

			LaserScalpelCharge = 40;
		}
	}
}
