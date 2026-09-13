public class UserInterface
{
    public void PrintObservations(IEnumerable<ObserveCheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            string formatted = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).ToString("MM-dd HH:mm:ss");
            Console.WriteLine($"{cheep.Author} @ {cheep.Observation}: {formatted} (id: {cheep.id})");
        }
    }
    public void PrintComments(IEnumerable<CommentCheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            string formatted = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).ToString("MM-dd HH:mm:ss");
            Console.WriteLine($"Comment by {cheep.Author} @ {cheep.Comment}: {formatted} (id: {cheep.id})");
        }
    }
}