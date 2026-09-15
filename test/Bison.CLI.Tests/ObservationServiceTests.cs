using SimpleDB;

namespace Bison.CLI.Tests;

public class ObservationServiceTests
{
    [Fact]
    public void GetByLocation_ReturnsOnlyMatchingObservations()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
        var dbObserve = CSVDatabase<ObserveCheep>.GetInstance(path);
        var service = new ObservationService(dbObserve);

        dbObserve.Store(new ObserveCheep("a", "first bird", 1000, 1, "DR Byen"));
        dbObserve.Store(new ObserveCheep("b", "second bird", 1001, 2, "Fælledparken"));

        var result = service.GetByLocation("DR Byen");

        Assert.Single(result);
        Assert.Equal("first bird", result.First().Observation);
    }
}