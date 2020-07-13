
// BeforePrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Reflection;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforePrintPlayerRoomEventState : State, IBeforePrintPlayerRoomEventState
	{
		public virtual bool ExitEventLoop { get; set; }

		public override void Execute()
		{
			if (!Globals.CommandPromptSeen || ShouldPreTurnProcess())
			{
				var eventTurn = 0L;

				var eventName = "";

				gGameState.BeforePrintPlayerRoomEventHeap.PeekMin(ref eventTurn, ref eventName);

				while (!ExitEventLoop && !string.IsNullOrWhiteSpace(eventName) && eventTurn <= gGameState.CurrTurn)
				{
					gGameState.BeforePrintPlayerRoomEventHeap.RemoveMin(ref eventTurn, ref eventName);

					var methodName = string.Format("{0}{1}", eventName, !eventName.EndsWith("Event") ? "Event" : "");

					var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

					if (methodInfo != null)
					{
						methodInfo.Invoke(this, null);
					}

					eventTurn = 0;

					eventName = "";

					gGameState.BeforePrintPlayerRoomEventHeap.PeekMin(ref eventTurn, ref eventName);
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPrintPlayerRoomState>();
			}

			Globals.NextState = NextState;
		}

		public BeforePrintPlayerRoomEventState()
		{
			Name = "BeforePrintPlayerRoomEventState";
		}
	}
}
