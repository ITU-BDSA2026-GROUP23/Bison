using Microsoft.EntityFrameworkCore;
using Models;
public class BisonDBContext : DbContext
{
    public BisonDBContext(DbContextOptions<BisonDBContext> options) : base(options)
    {
    }

    
    public DbSet<Author> Authors { get; set; } = null!;
    public DbSet<Post> Posts { get; set; } = null!;
    public DbSet<Taxon> Taxons { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasKey(a => a.AuthorId);
        modelBuilder.Entity<Post>().HasKey(p => p.PostId);
        modelBuilder.Entity<Taxon>().HasKey(t => t.TaxonId);

        modelBuilder.Entity<Post>()
            .HasDiscriminator<string>("PostType")
            .HasValue<Observation>("Observation")
            .HasValue<Comment>("Comment")
            .HasValue<Proposal>("Proposal");
    }
}