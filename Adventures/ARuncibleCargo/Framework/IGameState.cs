
// IGameState.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

namespace ARuncibleCargo.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		long DreamCounter { get; set; }

		long SwarmyCounter { get; set; }

		long CargoOpenCounter { get; set; }

		long CargoInRoom { get; set; }

		long GiveAmazonMoney { get; set; }

		bool[] PookaMet { get; set; }

		bool AmazonMet { get; set; }

		bool BillAndAmazonMeet { get; set; }

		bool PrinceMet { get; set; }

		bool AmazonLilWarning { get; set; }

		bool BillLilWarning { get; set; }

		bool FireEscaped { get; set; }

		bool CampEntered { get; set; }

		bool PaperRead { get; set; }

		bool Explosive { get; set; }

		#endregion

		#region Methods

		bool GetPookaMet(long index);

		void SetPookaMet(long index, bool value);

		#endregion
	}
}
