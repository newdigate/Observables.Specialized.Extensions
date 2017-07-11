# Observables.Specialized.Extensions

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
