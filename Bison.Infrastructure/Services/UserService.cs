using Bison.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserService : IUserService
{
    private readonly IDbContextFactory<AuthDBContext> _factory;

    public UserService(IDbContextFactory<AuthDBContext> factory)
    {
        _factory = factory;
    }

    public List<string> SearchUsers(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return new List<string>();

        using var context = _factory.CreateDbContext();

        return context.Users
            .AsNoTracking()
            .Where(u => u.UserName != null && u.UserName.ToLower().Contains(query.ToLower()))
            .OrderBy(u => u.UserName)
            .Select(u => u.UserName!)
            .ToList();
    }
}