# Observables.Specialized.Extensions

- interface IObservableDictionary<TKey, TValue>
``` csharp
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
``` 
- class ObservableDictionary<TKey, TValue>
``` csharp
 public class ObservableDictionary<TKey, TValue> : IObservableDictionary<TKey, TValue> { }
 /// observable dictionary with snapshot and update events  
```

``` csharp
            IObservableDictionary<string, string> obsDict = new ObservableDictionary<string, string>();
            obsDict.GetOrAdd("existing", c => "existing");

            IDisposable d = obsDict.Subscribe(
                c =>
                {
                    actualNumberOfEvents++;
                    Console.WriteLine($" {c.EventType} {c.Key} {c.Value} [ {c.StateOfWorld} ]");
                }
            );
            obsDict.GetOrAdd("next", c => "next");
```

- output
```
itemAdded existing existing [ -1 ]
itemAdded next next [ 2 ]
```
