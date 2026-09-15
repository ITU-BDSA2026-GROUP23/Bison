using CsvHelper;
using System.Globalization;
using SimpleDB;

//Refactor this to use CsvHelper instead of manually parsing the CSV file:
string CSVObservePath = "./bison_observe_cli_db.csv";
string CSVCommentPath = "./bison_comment_cli_db.csv";


CSVDatabase<ObserveCheep> dbObserve = CSVDatabase<ObserveCheep>.GetInstance(CSVObservePath);
CSVDatabase<CommentCheep> dbComment = CSVDatabase<CommentCheep>.GetInstance(CSVCommentPath);
UserInterface ui = new UserInterface();

if (args.Length == 0)
{
    Console.WriteLine("Enter read or write");
    return;
}
if (args[0] == "read")
{
    ui.PrintObservations(dbObserve.Read());
}
else if (args[0] == "observe")
{
    var id = IdGenerator.NextObserveId(CSVObservePath); // Get the next available id for the new observation
    var newCheep = new ObserveCheep(Environment.UserName, args[1], DateTimeOffset.UtcNow.ToUnixTimeSeconds(), id, args[2]);
    dbObserve.Store(newCheep);
}
else if (args[0] == "comment") //Now we print a clear message and refuse to save when the id doesn't exist.
{
    var id = int.Parse(args[2]);
    var commentService = new CommentService(dbObserve, dbComment);
    if (!commentService.TryAddComment(Environment.UserName, args[1], id, out var error))
    {
        Console.WriteLine(error);
    }
}
else if (args[0] == "discussion")
{
    var id = int.Parse(args[1]);
    ui.PrintComments(dbComment.discussion(id));
}
else
{
    Console.WriteLine("Invalid command. Use 'read', 'observe', or 'comment'.");
}
public record ObserveCheep(string Author, string Observation, long Timestamp, int id, string Location);
public record CommentCheep(string Author, string Comment, long Timestamp, int id);
