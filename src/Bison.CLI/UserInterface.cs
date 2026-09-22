using System.Globalization;
public class UserInterface

{
    public static string FormatTimestamp(long unixSeconds) =>
    DateTimeOffset.FromUnixTimeSeconds(unixSeconds)
        .ToString("MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
    public void PrintObservations(IEnumerable<ObserveCheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            string formatted = FormatTimestamp(cheep.Timestamp);
            Console.WriteLine($"{cheep.Author} @ {cheep.Observation}: {formatted} (id: {cheep.id}, Location: {cheep.Location})");
        }
    }
    public void PrintComments(IEnumerable<CommentCheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            string formatted = FormatTimestamp(cheep.Timestamp);
            Console.WriteLine($"Comment by {cheep.Author} @ {cheep.Comment}: {formatted} (id: {cheep.id})");
        }
    }
    public void PrintProposals(IEnumerable<ProposalCheep> cheeps)
    {
        foreach (var cheep in cheeps)
        {
            string formatted = FormatTimestamp(cheep.Timestamp);
            Console.WriteLine($"Proposal by {cheep.Author} @ {cheep.TaxonId}: {formatted} (ObservationId: {cheep.ObservationId})");
        }
    }
}