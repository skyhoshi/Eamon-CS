﻿
// EventHeap.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Eamon.Game.Utilities
{
	public class EventData
	{
		public virtual string EventName { get; set; }

		public virtual object EventParam { get; set; }
	}

	public class EventHeap
	{
		public virtual IDictionary<long, IList<EventData>> EventDictionary { get; set; }

		public virtual bool IsEmpty()
		{
			return EventDictionary.Count <= 0;
		}

		public virtual void Clear()
		{
			EventDictionary.Clear();
		}

		public virtual void Insert(long key, string eventName)
		{
			Insert02(key, eventName, null);
		}

		public virtual void Insert02(long key, string eventName, object eventParam)
		{
			Insert03(key, new EventData() { EventName = eventName, EventParam = eventParam });
		}

		public virtual void Insert03(long key, EventData value)
		{
			IList<EventData> listValue;

			if (key >= 0 && value != null && !string.IsNullOrWhiteSpace(value.EventName))
			{
				if (EventDictionary.TryGetValue(key, out listValue))
				{
					listValue.Add(value);
				}
				else
				{
					listValue = new List<EventData>();

					listValue.Add(value);

					EventDictionary.Add(key, listValue);
				}
			}
		}

		public virtual IList<KeyValuePair<long, EventData>> FindRegex(string eventNameRegexPattern, bool FindAll)
		{
			var eventList = new List<KeyValuePair<long, EventData>>();

			if (!string.IsNullOrWhiteSpace(eventNameRegexPattern))
			{
				var regex = new Regex(eventNameRegexPattern);

				foreach (var entry in EventDictionary)
				{
					foreach (var eventData in entry.Value)
					{
						if (regex.IsMatch(eventData.EventName))
						{
							eventList.Add(new KeyValuePair<long, EventData>(entry.Key, eventData));

							if (!FindAll)
							{
								goto ExitLoop;
							}
						}
					}
				}

			ExitLoop:

				;
			}

			return eventList;
		}

		public virtual void Remove(long key, string eventName)
		{
			Remove02(key, new EventData() { EventName = eventName });
		}

		public virtual void Remove02(long key, EventData value)
		{
			IList<EventData> listValue;

			if (key >= 0 && value != null && !string.IsNullOrWhiteSpace(value.EventName))
			{
				if (EventDictionary.TryGetValue(key, out listValue))
				{
					var foundValue = listValue.FirstOrDefault(v => v.EventName == value.EventName);

					if (foundValue != null)
					{
						listValue.Remove(foundValue);
					}

					if (listValue.Count <= 0)
					{
						EventDictionary.Remove(key);
					}
				}
			}
		}

		public virtual IList<KeyValuePair<long, EventData>> RemoveRegex(string eventNameRegexPattern, bool removeAll)
		{
			var eventList = FindRegex(eventNameRegexPattern, removeAll);

			foreach (var entry in eventList)
			{
				Remove02(entry.Key, entry.Value);
			}

			return eventList;
		}

		public virtual void RemoveMin(ref long key, ref EventData value)
		{
			IList<EventData> listValue;

			if (EventDictionary.Count > 0)
			{
				key = EventDictionary.First().Key;

				listValue = EventDictionary[key];

				value = listValue[0];

				listValue.RemoveAt(0);

				if (listValue.Count <= 0)
				{
					EventDictionary.Remove(key);
				}
			}
		}

		public virtual void PeekMin(ref long key, ref EventData value)
		{
			IList<EventData> listValue;

			if (EventDictionary.Count > 0)
			{
				key = EventDictionary.First().Key;

				listValue = EventDictionary[key];

				value = listValue[0];
			}
		}

		public EventHeap()
		{
			EventDictionary = new SortedDictionary<long, IList<EventData>>();
		}
	}
}
