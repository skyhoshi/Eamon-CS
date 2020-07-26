
// BeforePrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Diagnostics;
using System.Reflection;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforePrintPlayerRoomEventState : State, IBeforePrintPlayerRoomEventState
	{
		public virtual bool ExitEventLoop { get; set; }

		public virtual void FireEvent(string eventName, object eventParam)
		{
			FireEvent02(new EventData() { EventName = eventName, EventParam = eventParam });
		}

		public virtual void FireEvent02(EventData eventData)
		{
			Debug.Assert(eventData != null && !string.IsNullOrWhiteSpace(eventData.EventName));

			var methodName = string.Format("{0}{1}", eventData.EventName, !eventData.EventName.EndsWith("Event") ? "Event" : "");

			var methodInfo = GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (methodInfo != null)
			{
				methodInfo.Invoke(this, new object[] { eventData.EventParam });
			}
		}

		public override void Execute()
		{
			if (!Globals.CommandPromptSeen || ShouldPreTurnProcess())
			{
				var eventTurn = 0L;

				EventData eventData = null;

				gGameState.BeforePrintPlayerRoomEventHeap.PeekMin(ref eventTurn, ref eventData);

				while (!ExitEventLoop && eventData != null && !string.IsNullOrWhiteSpace(eventData.EventName) && eventTurn <= gGameState.CurrTurn)
				{
					gGameState.BeforePrintPlayerRoomEventHeap.RemoveMin(ref eventTurn, ref eventData);

					FireEvent02(eventData);

					eventTurn = 0;

					eventData = null;

					gGameState.BeforePrintPlayerRoomEventHeap.PeekMin(ref eventTurn, ref eventData);
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
