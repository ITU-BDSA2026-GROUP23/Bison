using SimpleDB;

namespace Bison.CLI.Tests;

public class CommentServiceTests
{
    [Fact]
    public void TryAddComment_OnNonExistingObservation_IsRejected()
    {
       string observePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv"); 
       string commentPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv"); 

       var dbObserve = CSVDatabase<ObserveCheep>.GetInstance(observePath);
       var dbComment = CSVDatabase<CommentCheep>.GetInstance(commentPath);

       var commentService = new CommentService(dbObserve, dbComment);

       bool result = commentService.TryAddComment("emili", "nice bird", 99999, out var error);

       Assert.False(result);
       Assert.NotNull(error);


    }
}

    