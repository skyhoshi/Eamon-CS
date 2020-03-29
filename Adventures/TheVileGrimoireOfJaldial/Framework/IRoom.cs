
// IRoom.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework
{
	public interface IRoom : Eamon.Framework.IRoom
	{
		bool IsGroundsRoom();

		bool IsFenceRoom();

		bool IsBodyChamberRoom();

		bool IsRainyRoom();

		bool IsFoggyRoom();
	}
}
