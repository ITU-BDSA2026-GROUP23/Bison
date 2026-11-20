
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class BisonDBContextFactory
    : IDesignTimeDbContextFactory<BisonDBContext>
{
    public BisonDBContext CreateDbContext(string[] args)
    {
        var currentDirectory = Directory.GetCurrentDirectory();

        var razorDirectory = Directory.Exists(
            Path.Combine(currentDirectory, "Bison.Razor"))
            ? Path.Combine(currentDirectory, "Bison.Razor")
            : currentDirectory;

        var databasePath =
            Environment.GetEnvironmentVariable("BISONDBPATH");

        if (string.IsNullOrWhiteSpace(databasePath))
        {
            databasePath = Path.Combine(
                razorDirectory, "data", "bison.db");
        }

        var options = new DbContextOptionsBuilder<BisonDBContext>()
            .UseSqlite($"Data Source={databasePath}")
            .Options;

        return new BisonDBContext(options);
    }
}
