
// Room.cs

// Copyright (c) 2014-2017 by Michael R. Penner.  All rights reserved

using System;
using Eamon.Game.Attributes;
using ARuncibleCargo.Framework;

namespace ARuncibleCargo.Game
{
	[ClassMappings(typeof(Eamon.Framework.IRoom))]
	public class Room : Eamon.Game.Room, IRoom
	{
		protected override string GetObviousExits()
		{
			return IsWaterRoom() ? string.Format("{0}Obvious directions:  ", Environment.NewLine) : base.GetObviousExits();
		}

		public virtual bool IsWaterRoom()
		{
			return Uid >= 97 && Uid <= 101;
		}
	}
}
