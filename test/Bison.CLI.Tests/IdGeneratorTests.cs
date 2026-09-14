namespace Bison.CLI.Tests;

public class IdGeneratorTests
{
    [Fact]
    public void NextObserveId_WhenFileDoesNotExist_ReturnsOne()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");

        int id = IdGenerator.NextObserveId(path);

        Assert.Equal(1, id);
    }
}