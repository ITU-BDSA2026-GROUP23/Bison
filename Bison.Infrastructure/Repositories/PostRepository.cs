
using System;
using System.Collections.Generic;

using Models;
using Bison.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

public class PostRepository : IPostRepository
{
    private readonly IDbContextFactory<BisonDBContext> _factory;

    public PostRepository(IDbContextFactory<BisonDBContext> factory)
    {
        _factory = factory;
    }

    public List <Observation> GetObservations(int page)
    {
        return LoadObservations(page);
    }

    public List <Observation> GetAllObservations()
    {
        return LoadObservations(null);
    }
    
    private List<Observation> LoadObservations(int? page)
    {
        using var context = _factory.CreateDbContext();
        context.Taxons.Load();

        IQueryable<Observation> query = context.Posts
            .OfType<Observation>()
            .Include(o => o.Author)
            .Include(o => o.Taxon)
            .OrderByDescending(o => o.TimeStamp);

        if (page.HasValue){
            query = query
                .Skip((Math.Max(1, page.Value) - 1) * 32)
                .Take(32);
        }
        return query.ToList();
    }

    public List<Observation> GetObservationsFromAuthor(string author, int page)
    {
        using var context = _factory.CreateDbContext();
        return context.Posts
            .OfType<Observation>()
            .Include(o => o.Author)
            .Where(o => o.Author.Name == author)
            .OrderByDescending(o => o.TimeStamp)
            .Skip((Math.Max(1, page) - 1) * 32)
            .Take(32)
            .ToList();
    }

    public List<Observation> GetObservationsFromId(int id, int page)
    {
        using var context = _factory.CreateDbContext();
        return context.Posts
            .OfType<Observation>()
            .Include(o => o.Author)
            .Where(o => o.PostId == id)
            .ToList();
    }

    public List<Comment> GetCommentsFromObservation(int observationId)
    {
        using var context = _factory.CreateDbContext();
        return context.Posts
            .OfType<Comment>()
            .Include(c => c.Author)
            .Where(c => c.Observation.PostId == observationId)
            .OrderByDescending(c => c.TimeStamp)
            .ToList();
    }

    public List<Proposal> GetProposalsFromObservation(int observationId)
    {
        using var context = _factory.CreateDbContext();
        return context.Posts
            .OfType<Proposal>()
            .Include(p => p.Author)
            .Include(p => p.Taxon)
            .Where(p => p.Observation.PostId == observationId)
            .OrderByDescending(p => p.TimeStamp)
            .ToList();
    }

    public void AddObservation(string text, string identityUserID, string email)
    {
        using var context = _factory.CreateDbContext();
        var author = context.Authors.FirstOrDefault(a => a.IdentityUserID == identityUserID);
        if (author == null)
        {
            author = new Author
            {
                IdentityUserID = identityUserID,
                Email = email,
                Name = email
            };
            context.Authors.Add(author);        }

        var observation = new Observation
        {
            Text = text,
            Author = author,
            TimeStamp = DateTime.Now
        };
        context.Posts.Add(observation);
        context.SaveChanges();
    }
}
