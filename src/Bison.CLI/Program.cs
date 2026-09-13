using CsvHelper;
using System.Globalization;
using SimpleDB;

//Refactor this to use CsvHelper instead of manually parsing the CSV file:
string CSVObservePath = "./bison_observe_cli_db.csv";
string CSVCommentPath = "./bison_comment_cli_db.csv";


CSVDatabase<ObserveCheep> dbObserve = new CSVDatabase<ObserveCheep>(CSVObservePath);
CSVDatabase<CommentCheep> dbComment = new CSVDatabase<CommentCheep>(CSVCommentPath);
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
    var id = File.ReadAllLines(CSVObservePath).Length; // Get the current number of lines in the CSV file 
    var newCheep = new ObserveCheep(Environment.UserName, args[1], DateTimeOffset.UtcNow.ToUnixTimeSeconds(), id);
    dbObserve.Store(newCheep);
}
else if (args[0] == "comment")
{
    var id = int.Parse(args[2]);
    var newCheep = new CommentCheep(Environment.UserName, args[1], DateTimeOffset.UtcNow.ToUnixTimeSeconds(), id);
    dbComment.Store(newCheep);
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
public record ObserveCheep(string Author, string Observation, long Timestamp, int id);
public record CommentCheep(string Author, string Comment, long Timestamp, int id);
