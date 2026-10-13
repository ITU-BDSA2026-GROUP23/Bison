using System;

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

        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT user.username, observation.text, observation.pub_date
            FROM observation
            JOIN user ON observation.author_id = user.user_id
            ORDER BY observation.pub_date DESC
            LIMIT $pageSize OFFSET $offset;
        ";

        command.Parameters.AddWithValue("$pageSize", 32);
        command.Parameters.AddWithValue("$offset", (page - 1) * 32);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var author = reader.GetString(0);
            var message = reader.GetString(1);
            var timestamp = reader.GetInt64(2);

            observations.Add(new Observation{
                Author = new Author{Name = author},
                Text = message,
                TimeStamp = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime
            });
        }

        return observations;
    }

    public List<Observation> GetObservationsFromAuthor(string author, int page)
    {
        var observations = new List<Observation>();

        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT user.username, observation.text, observation.pub_date
            FROM observation
            JOIN user ON observation.author_id = user.user_id
            WHERE user.username = $author
            ORDER BY observation.pub_date DESC
            LIMIT $pageSize OFFSET $offset;
        ";

        command.Parameters.AddWithValue("$author", author);
        command.Parameters.AddWithValue("$pageSize", 32);
        command.Parameters.AddWithValue("$offset", (page - 1) * 32);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var authorName = reader.GetString(0);
            var message = reader.GetString(1);
            var timestamp = reader.GetInt64(2);

            observations.Add(new Observation{
                Author = new Author{Name = authorName},
                Text = message,
                TimeStamp = DateTimeOffset
                    .FromUnixTimeSeconds(timestamp)
                    .UtcDateTime
            });
        }

        return observations;
    }

    public List<Observation> GetObservationsFromId(int id, int page)
    {
        var observations = new List<Observation>();

        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT user.username, observation.text, observation.pub_date
            FROM observation
            JOIN user ON observation.author_id = user.user_id
            WHERE observation.observation_id = $id
            ORDER BY observation.pub_date DESC
            LIMIT $pageSize OFFSET $offset;
        ";

        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$pageSize", 32);
        command.Parameters.AddWithValue("$offset", (page - 1) * 32);

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var authorName = reader.GetString(0);
            var message = reader.GetString(1);
            var timestamp = reader.GetInt64(2);

           observations.Add(new Observation{
                Author = new Author{Name = authorName},
                Text = message,
                TimeStamp = DateTimeOffset
                    .FromUnixTimeSeconds(timestamp)
                    .UtcDateTime
            });
        }

        return observations;
    }

}