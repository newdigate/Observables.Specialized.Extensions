using System;
using Xunit;

namespace Observables.Specialized.Extensions.Tests
{
    public class ObservableDictionaryFixtures
    {
        [Fact]
        public void TestBasicCase()
        {
            const long expectedNumberOfEvents = 2;
            long actualNumberOfEvents = 0;
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
            Assert.Equal(expectedNumberOfEvents, actualNumberOfEvents);

			IDisposable d2 = obsDict.Subscribe(
				c =>
				{
					actualNumberOfEvents++;
					Console.WriteLine($" -----> {c.EventType} {c.Key} {c.Value} [ {c.StateOfWorld} ]");
				}
		    );

			obsDict.GetOrAdd("next11", c => "next11");
        }
    }
}
