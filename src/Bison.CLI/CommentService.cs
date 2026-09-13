using SimpleDB;

public class CommentService
{
    private readonly CSVDatabase<ObserveCheep> dbObserve;
    private readonly CSVDatabase<CommentCheep> dbComment;

    public CommentService(CSVDatabase<ObserveCheep> dbObserve, CSVDatabase<CommentCheep> dbComment)
    {
        this.dbObserve = dbObserve;
        this.dbComment = dbComment;
    }

    public bool TryAddComment(string author, string text, int observationId, out string? error)
    {
        bool observationExists = dbObserve.Read().Any(o => o.id == observationId);
        if (!observationExists)
        {
            error = $"Observation {observationId} does not exist.";
            return false;
        }

        var comment = new CommentCheep(author, text, DateTimeOffset.UtcNow.ToUnixTimeSeconds(), observationId);
        dbComment.Store(comment);
        error = null;
        return true;
    }
}