using CsvHelper;
using System.Globalization;
using SimpleDB;

//Refactor this to use CsvHelper instead of manually parsing the CSV file:
string CSVPath = "./bison_observe_cli_db.csv";

CSVDatabase<Cheep> db = new CSVDatabase<Cheep>(CSVPath);
UserInterface ui = new UserInterface();

if (args.Length == 0)
{
    Console.WriteLine("Enter read or write");
    return;
}
if (args[0] == "read")
{
    ui.PrintObservations(db.Read());
}
else if (args[0] == "write")
{
    var newCheep = new Cheep(Environment.UserName, args[1], DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    db.Store(newCheep);
}

public record Cheep(string Author, string Observation, long Timestamp);
