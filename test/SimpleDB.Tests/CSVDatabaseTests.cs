namespace SimpleDB.Tests;

public record TestCheep(string Author, string Text, long Timestamp, int id);
public class CSVDatabaseTests
{
    [Fact]
    public void StoredRecord_CanBeReadBack()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
        var db = new CSVDatabase<TestCheep>(path);
        var cheep = new TestCheep("emili", "test observation", 1000, 1);

        db.Store(cheep);
        var result = db.Read().Single();

        Assert.Equal(cheep, result);
    }

    [Fact]
    public void Discussion_ReturnsOnlyMatchingId()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
        var db = new CSVDatabase<TestCheep>(path);

        db.Store(new TestCheep("a", "first", 1000, 1));
        db.Store(new TestCheep("b", "second", 1001, 2));

        var result = db.discussion(1);

        Assert.Single(result);
        Assert.Equal("first", result.First().Text);
    }
}