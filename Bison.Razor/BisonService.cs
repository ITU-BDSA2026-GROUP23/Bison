public record ObservationViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations();
    public List<ObservationViewModel> GetObservationsFromAuthor(string author);
}


public class ObservationService : IObservationService
{
    private readonly DBFacade _db;
    public ObservationService(DBFacade db)
    {
        _db = db;
    }

    public List<ObservationViewModel> GetObservations()
    {    
        var observations = new List<ObservationViewModel>();
        
        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = @"
        SELECT user.username, observation.text, observation.pub_date
        FROM observation
        JOIN user ON observation.author_id = user.user_id;
        ";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var author = reader.GetString(0);
            var message = reader.GetString(1);
            var timestamp = reader.GetInt64(2);

            observations.Add(new ObservationViewModel(author, message, UnixTimeStampToDateTimeString(timestamp)));
        }
        return observations;
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author)
    {
        var observations = new List<ObservationViewModel>();
        
        using var connection = _db.GetConnection();
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText = @"
            SELECT user.username, observation.text, observation.pub_date
            FROM observation
            JOIN user ON observation.author_id = user.user_id
            WHERE user.username = $author;
        ";

        command.Parameters.AddWithValue("$author", author);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var authorName = reader.GetString(0);
            var message = reader.GetString(1);
            var timestamp = reader.GetInt64(2);

            observations.Add(new ObservationViewModel(authorName, message, UnixTimeStampToDateTimeString(timestamp)));
        }
        // filter by the provided author name
        return observations;
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
