using System;
using Observables.Specialized.Extensions;
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
                c => {
					actualNumberOfEvents++;
					Console.WriteLine($" {c.EventType} {c.Key} {c.Value}");
                }
			);
			obsDict.GetOrAdd("next", c => "next");

            Assert.Equal(expectedNumberOfEvents, actualNumberOfEvents);
        }
    }
}
