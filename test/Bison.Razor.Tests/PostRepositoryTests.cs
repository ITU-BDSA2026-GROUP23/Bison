using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Bison.Razor.Tests;

public class PostRepositoryTests : IDisposable
{
    private readonly SqliteConnection connection;
    private readonly PostRepository repository;

    public PostRepositoryTests()
    {
        // in memory database, shared so the repository can see it
        string database = $"test_{Guid.NewGuid()};Mode=Memory;Cache=Shared";
        connection = new SqliteConnection($"Data Source={database}");
        connection.Open();

        var options = new DbContextOptionsBuilder<BisonDBContext>().UseSqlite(connection).Options;
        using var context = new BisonDBContext(options);
        context.Database.EnsureCreated();
        DbInitializer.SeedDatabase(context);

        repository = new PostRepository(new DBFacade(database));
    }

    public void Dispose()
    {
        connection.Dispose();
    }

    [Fact]
    public void GetObservations_FirstPage_Has32()
    {
        var result = repository.GetObservations(1);

        Assert.Equal(32, result.Count);
    }

    [Fact]
    public void GetObservations_PageTwo_IsDifferentFromPageOne()
    {
        var pageOne = repository.GetObservations(1).Select(o => o.PostId);
        var pageTwo = repository.GetObservations(2).Select(o => o.PostId);

        Assert.Empty(pageOne.Intersect(pageTwo));
    }

    [Fact]
    public void GetObservationsFromAuthor_OnlyGivesThatAuthor()
    {
        var result = repository.GetObservationsFromAuthor("Roger Histand", 1);

        Assert.NotEmpty(result);
        Assert.All(result, o => Assert.Equal("Roger Histand", o.Author.Name));
    }

    [Fact]
    public void GetObservationsFromAuthor_UnknownAuthor_IsEmpty()
    {
        var result = repository.GetObservationsFromAuthor("Nobody", 1);

        Assert.Empty(result);
    }
}