public class UserInterface
{
    public void PrintObservations(IEnumerable<Cheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            string formatted = DateTimeOffset.FromUnixTimeSeconds(cheep.Timestamp).ToString("MM-dd HH:mm:ss");
            Console.WriteLine($"{cheep.Author} @ {cheep.Observation}: {formatted}");
        }
    }
}