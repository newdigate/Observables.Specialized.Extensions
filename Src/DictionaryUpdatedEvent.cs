using System;
namespace Observables.Specialized.Extensions
{
	public class DictionaryUpdatedEventMessage<TKey, TValue>
	{
		public DictionaryUpdatedEventType EventType { get; set; }
		public TKey Key { get; set; }
		public TValue Value { get; set; }
        public long StateOfWorld { get; set; }

		public DictionaryUpdatedEventMessage()
		{
            StateOfWorld = -1;
		}

		public DictionaryUpdatedEventMessage(DictionaryUpdatedEventType eventType, TKey key, TValue value, long stateOfWorld) : this()
		{
			EventType = eventType;
			Key = key;
			Value = value;
            StateOfWorld = stateOfWorld;
		}
	}

	public enum DictionaryUpdatedEventType
	{
		snapshot = 0,
		itemAdded = 1,
		itemUpdated = 2,
		itemRemoved = 3
	}
}
