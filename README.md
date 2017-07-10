# Observables.Specialized.Extensions
C# Observable Dictionary

```
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
