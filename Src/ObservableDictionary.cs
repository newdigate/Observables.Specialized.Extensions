using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Observables.Specialized.Extensions
{
	public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue>
	{
		private readonly ConcurrentDictionary<TKey, TValue> _dictionary;
		private long _stateOfWorld;

		private Action<DictionaryUpdatedEventMessage<TKey, TValue>> _dictionaryUpdatedEventHandler = d => { };

		public ObservableDictionary()
		{
			_stateOfWorld = 0;
			_dictionary = new ConcurrentDictionary<TKey, TValue>();
		}

		TValue IDictionary<TKey, TValue>.this[TKey key] { get => _dictionary[key]; set => _dictionary[key] = value; }

		public int Count => _dictionary.Count;

		public bool IsReadOnly => false;

		public long StateOfWorld => _stateOfWorld;

		ICollection<TKey> IDictionary<TKey, TValue>.Keys => _dictionary.Keys;

		ICollection<TValue> IDictionary<TKey, TValue>.Values => _dictionary.Values;

		int ICollection<KeyValuePair<TKey, TValue>>.Count => _dictionary.Keys.Count;

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => IsReadOnly;

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		#region default implementation
		public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
		{
			Func<TKey, TValue> addValueFactoryWrapper =
				k =>
				{
					TValue v = addValueFactory(k);
					Interlocked.Increment(ref _stateOfWorld);
					_dictionaryUpdatedEventHandler?.Invoke(new DictionaryUpdatedEventMessage<TKey, TValue>(DictionaryUpdatedEventType.itemAdded, k, v));
					return v;
				};

			Func<TKey, TValue, TValue> updateValueFactoryWrapper =
				(k, v) =>
				{
					TValue r = updateValueFactory(k, v);
					Interlocked.Increment(ref _stateOfWorld);
					_dictionaryUpdatedEventHandler?.Invoke(new DictionaryUpdatedEventMessage<TKey, TValue>(DictionaryUpdatedEventType.itemUpdated, k, v));
					return r;
				};

			TValue result = _dictionary.AddOrUpdate(key, addValueFactoryWrapper, updateValueFactoryWrapper);
			return result;
		}

		public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
		{
			Func<TKey, TValue, TValue> updateValueFactoryWrapper =
				(k, v) =>
				{
					TValue r = updateValueFactory(k, v);
					Interlocked.Increment(ref _stateOfWorld);
					_dictionaryUpdatedEventHandler?.Invoke(new DictionaryUpdatedEventMessage<TKey, TValue>(DictionaryUpdatedEventType.itemAdded, k, v));
					return r;
				};

			return _dictionary.AddOrUpdate(key, addValue, updateValueFactoryWrapper);
		}

		public void Clear()
		{
			List<KeyValuePair<TKey, TValue>> values = _dictionary.ToList();
			_dictionary.Clear();
			Interlocked.Increment(ref _stateOfWorld);
			if (_dictionaryUpdatedEventHandler != null)
				values.ForEach(
					value => _dictionaryUpdatedEventHandler?.Invoke(new DictionaryUpdatedEventMessage<TKey, TValue>(DictionaryUpdatedEventType.itemRemoved, value.Key, value.Value)));
		}

		public bool Contains(KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		public bool ContainsKey(TKey key)
		{
			return _dictionary.ContainsKey(key);
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		public TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
		{
			Func<TKey, TValue> valueFactoryWrapper = k =>
			{
				TValue v = valueFactory(k);
				_dictionaryUpdatedEventHandler?.Invoke(new DictionaryUpdatedEventMessage<TKey, TValue>(DictionaryUpdatedEventType.itemAdded, k, v));
				return v;
			};
			return _dictionary.GetOrAdd(key, valueFactoryWrapper);
		}

		public bool Remove(TKey key)
		{
			TValue value;
			bool result = _dictionary.TryRemove(key, out value);
			if (result)
			{
				_dictionaryUpdatedEventHandler?.Invoke(new DictionaryUpdatedEventMessage<TKey, TValue>(DictionaryUpdatedEventType.itemRemoved, key, value));
			}
			return result;
		}

		public void Remove(object key)
		{
			Remove((TKey)Convert.ChangeType(key, typeof(TKey)));
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			return _dictionary.TryGetValue(key, out value);
		}

		public bool TryRemove(TKey key, out TValue value)
		{
			return _dictionary.TryRemove(key, out value);
		}

		public bool TryUpdate(TKey key, TValue newValue, TValue comparisonValue)
		{
			bool success = _dictionary.TryUpdate(key, newValue, comparisonValue);

			return success;
		}

		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			throw new NotImplementedException();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			throw new NotImplementedException();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
		{
			throw new NotImplementedException();
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return _dictionary.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_dictionary).GetEnumerator();
		}

		TValue IObservableDictionary<TKey, TValue>.GetOrAdd(TKey key, Func<TKey, TValue> valueFactory)
		{
			return GetOrAdd(key, valueFactory);
		}

		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			return Remove(key);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			throw new NotImplementedException();
		}

		IDisposable IObservable<DictionaryUpdatedEventMessage<TKey, TValue>>.Subscribe(IObserver<DictionaryUpdatedEventMessage<TKey, TValue>> observer)
		{
			IObservable<DictionaryUpdatedEventMessage<TKey, TValue>> obs =
				Observable
				.FromEvent<DictionaryUpdatedEventMessage<TKey, TValue>>(
					h => _dictionaryUpdatedEventHandler += h,
					h => _dictionaryUpdatedEventHandler -= h);

			IEnumerable<DictionaryUpdatedEventMessage<TKey, TValue>> enumerable = _dictionary.Select(
					k => new DictionaryUpdatedEventMessage<TKey, TValue>(DictionaryUpdatedEventType.itemAdded, k.Key, k.Value));

			List<DictionaryUpdatedEventMessage<TKey, TValue>> list = enumerable.ToList();

			IObservable<DictionaryUpdatedEventMessage<TKey, TValue>> snap =
				list.ToObservable();

			return snap
				.Concat(obs)
				.Subscribe(observer);
		}

		bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
		{
			throw new NotImplementedException();
		}

		bool IObservableDictionary<TKey, TValue>.TryRemove(TKey key, out TValue value)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}