
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(long eventType)
		{
			if (eventType == PeAfterRoundEnd)
			{
				Globals.InitiativeMonsterUid = 0;

				if (Globals.EncounterSurprises)
				{
					NextState = Globals.CreateInstance<IPrintPlayerRoomState>();
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}

