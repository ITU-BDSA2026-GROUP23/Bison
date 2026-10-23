
using System;
using System.Collections.Generic;
using Models;

public class PostRepository : IPostRepository
{
    private readonly DBFacade _db;

    public PostRepository(DBFacade db)
    {
        _db = db;
    }

    public List<Observation> GetObservations(int page)
{
    var observations = new List<Observation>();
    var observationTaxonIds = new Dictionary<int, int>();

    using var connection = _db.GetConnection();
    connection.Open();

    using var command = connection.CreateCommand();

    command.CommandText = """
        SELECT p.PostId, a.Name, p.Text, p.TimeStamp, p.TaxonId
        FROM Posts p
        JOIN Authors a ON p.AuthorId = a.AuthorId
        WHERE p.PostType = 'Observation'
        ORDER BY p.TimeStamp DESC
        LIMIT $limit OFFSET $offset
        """;

    command.Parameters.AddWithValue("$limit", 32);
    command.Parameters.AddWithValue("$offset", (Math.Max(1, page) - 1) * 32);

    using (var reader = command.ExecuteReader())
    {
        while (reader.Read())
        {
            var observation = new Observation
            {
                PostId = reader.GetInt32(0),
                Author = new Author { Name = reader.GetString(1) },
                Text = reader.GetString(2),
                TimeStamp = DateTime.Parse(reader.GetString(3))
            };

            observations.Add(observation);

            if (!reader.IsDBNull(4))
            {
                observationTaxonIds[observation.PostId] = reader.GetInt32(4);
            }
        }
    }

    // Load the taxonomy
    var taxons = new Dictionary<int, Taxon>();
    var parentIds = new Dictionary<int, int?>();

    using var taxonCommand = connection.CreateCommand();
    taxonCommand.CommandText = """
        SELECT TaxonId, dwc_TaxonID, VernacularName, ParentTaxonId
        FROM Taxons
        """;

    using (var reader = taxonCommand.ExecuteReader())
    {
        while (reader.Read())
        {
            var id = reader.GetInt32(0);

            taxons[id] = new Taxon
            {
                TaxonId = id,
                dwc_TaxonID = reader.GetString(1),
                VernacularName = reader.IsDBNull(2) ? null : reader.GetString(2)
            };

            parentIds[id] = reader.IsDBNull(3) ? null : reader.GetInt32(3);
        }
    }

    // Connect each taxon to its parent
    foreach (var (id, parentId) in parentIds)
    {
        if (parentId.HasValue && taxons.TryGetValue(parentId.Value, out var parent))
        {
            taxons[id].Parent = parent;
        }
    }

    // Assign each observation its taxon
    foreach (var observation in observations)
    {
        if (observationTaxonIds.TryGetValue(observation.PostId, out var taxonId)
            && taxons.TryGetValue(taxonId, out var taxon))
        {
            observation.Taxon = taxon;
        }
    }

    return observations;
}

    public List<Observation> GetObservationsFromAuthor(string author, int page)
    {
        var observations = new List<Observation>();

        using var connection = _db.GetConnection();
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT p.PostId, a.Name, p.Text, p.TimeStamp
            FROM Posts p
            JOIN Authors a ON p.AuthorId = a.AuthorId
            WHERE p.PostType = 'Observation'
              AND a.Name = $author
            ORDER BY p.TimeStamp DESC
            LIMIT $limit OFFSET $offset
            """;

        command.Parameters.AddWithValue("$author", author);
        command.Parameters.AddWithValue("$limit", 32);
        command.Parameters.AddWithValue("$offset", (Math.Max(1, page) - 1) * 32);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            observations.Add(new Observation
            {
                PostId = reader.GetInt32(0),
                Author = new Author { Name = reader.GetString(1) },
                Text = reader.GetString(2),
                TimeStamp = DateTime.Parse(reader.GetString(3))
            });
        }

        return observations;
    }

    public List<Observation> GetObservationsFromId(int id, int page)
    {
        var observations = new List<Observation>();

        using var connection = _db.GetConnection();
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT p.PostId, a.Name, p.Text, p.TimeStamp
            FROM Posts p
            JOIN Authors a ON p.AuthorId = a.AuthorId
            WHERE p.PostType = 'Observation'
              AND p.PostId = $id
            """;

        command.Parameters.AddWithValue("$id", id);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            observations.Add(new Observation
            {
                PostId = reader.GetInt32(0),
                Author = new Author { Name = reader.GetString(1) },
                Text = reader.GetString(2),
                TimeStamp = DateTime.Parse(reader.GetString(3))
            });
        }

        return observations;
    }

    public List<Comment> GetCommentsFromObservation(int observationId)
    {
        var coments = new List<Comment>();

        using var connection = _db.GetConnection();
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT p.PostId, a.Name, p.Text, p.TimeStamp
            FROM Posts p
            JOIN Authors a ON p.AuthorId = a.AuthorId
            WHERE p.PostType = 'Comment'
              AND p.ObservationPostId = $id
            ORDER BY p.TimeStamp DESC
            """;

        command.Parameters.AddWithValue("$id", observationId);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            coments.Add(new Comment
            {
                PostId = reader.GetInt32(0),
                Author = new Author { Name = reader.GetString(1) },
                Text = reader.GetString(2),
                TimeStamp = DateTime.Parse(reader.GetString(3))
            });
        }

        return coments;
    }

    public List<Proposal> GetProposalsFromObservation(int observationId)
    {
        var proposals = new List<Proposal>();

        using var connection = _db.GetConnection();
        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = """
            SELECT p.PostId, a.Name, p.Text, p.TimeStamp
            FROM Posts p
            JOIN Authors a ON p.AuthorId = a.AuthorId
            WHERE p.PostType = 'Proposal'
              AND p.Proposal_ObservationPostId = $id
            ORDER BY p.TimeStamp DESC
            """;

        command.Parameters.AddWithValue("$id", observationId);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            proposals.Add(new Proposal
            {
                PostId = reader.GetInt32(0),
                Author = new Author { Name = reader.GetString(1) },
                Text = reader.GetString(2),
                TimeStamp = DateTime.Parse(reader.GetString(3))
            });
        }

        return proposals;
    }
}
