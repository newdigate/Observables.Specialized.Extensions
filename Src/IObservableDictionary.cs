using System;
using System.Collections;
using System.Collections.Generic;
namespace Observables.Specialized.Extensions
{
	public interface IObservableDictionary<TKey, TValue> :
		ICollection<KeyValuePair<TKey, TValue>>,
		IEnumerable<KeyValuePair<TKey, TValue>>,
		IEnumerable,
		IDictionary<TKey, TValue>,
		IObservable<DictionaryUpdatedEventMessage<TKey, TValue>>
	{
		bool TryRemove(TKey key, out TValue value);
		TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);
		long StateOfWorld { get; }
	}
}