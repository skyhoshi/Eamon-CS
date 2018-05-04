
// IGameState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved

namespace TheBeginnersCave.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary>
		/// Gets or sets the Trollsfire property.
		/// </summary>
		/// <value>
		/// If Trollsfire is activated, this is one (1); else it is zero (0).
		/// </value>
		/// <remarks>
		/// The Trollsfire property controls whether the Trollsfire sword is alight (1) or extinguished (0).  It works in 
		/// conjunction with <see cref="Game.Monster.Weapon"/> and <see cref="Commands.ITrollsfireCommand"/> to fully implement
		/// the "Trollsfire effect".  When Trollsfire is activated the string "(alight)" is added to its StateDesc property
		/// and its weapon dice value is set to 10.  When deactivated, the "(alight)" string is removed and the weapon dice
		/// value is set to 6.  The Trollsfire property uses a complex setter to perform these actions automatically.
		/// </remarks>
		long Trollsfire { get; set; }

		long BookWarning { get; set; }

		#endregion
	}
}
