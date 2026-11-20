using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AuthDBContext : IdentityDbContext
{
    public AuthDBContext(DbContextOptions<AuthDBContext> options)
        : base(options)
    {
        
    }
}
