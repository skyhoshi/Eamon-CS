
// EventHeap.cs

// Copyright (c) 2014+ by Michael R. Penner.  All rights reserved.

using System.Collections.Generic;
using System.Linq;

namespace Eamon.Game.Utilities
{
	public class EventHeap
	{
		// Note: need to replace with performant MinHeap implementation if it can be found

		public virtual IDictionary<long, IList<string>> EventDictionary { get; set; }

		public virtual bool IsEmpty()
		{
			return EventDictionary.Count <= 0;
		}

		public virtual void Insert(long key, string value)
		{
			IList<string> listValue;

			if (key >= 0 && !string.IsNullOrWhiteSpace(value))
			{
				if (EventDictionary.TryGetValue(key, out listValue))
				{
					listValue.Add(value);
				}
				else
				{
					listValue = new List<string>();

					listValue.Add(value);

					EventDictionary.Add(key, listValue);
				}
			}
		}

		public virtual void Remove(long key, string value)
		{
			IList<string> listValue;

			if (key >= 0 && !string.IsNullOrWhiteSpace(value))
			{
				if (EventDictionary.TryGetValue(key, out listValue))
				{
					listValue.Remove(value);

					if (listValue.Count <= 0)
					{
						EventDictionary.Remove(key);
					}
				}
			}
		}

		public virtual void RemoveMin(ref long key, ref string value)
		{
			IList<string> listValue;

			if (EventDictionary.Count > 0)
			{
				key = EventDictionary.Aggregate((l, r) => l.Key < r.Key ? l : r).Key;

				listValue = EventDictionary[key];

				value = listValue[0];

				listValue.RemoveAt(0);

				if (listValue.Count <= 0)
				{
					EventDictionary.Remove(key);
				}
			}
		}

		public virtual void PeekMin(ref long key, ref string value)
		{
			IList<string> listValue;

			if (EventDictionary.Count > 0)
			{
				key = EventDictionary.Aggregate((l, r) => l.Key < r.Key ? l : r).Key;

				listValue = EventDictionary[key];

				value = listValue[0];
			}
		}

		public EventHeap()
		{
			EventDictionary = new Dictionary<long, IList<string>>();
		}
	}
}
