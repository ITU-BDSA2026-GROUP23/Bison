using CsvHelper;
using System.Globalization;

//Refactor this to use CsvHelper instead of manually parsing the CSV file:
string CSVPath = "./bison_observe_cli_db.csv";

if (args.Length == 0)
{
    Console.WriteLine("Enter read or write");
    return;
}
if (args[0] == "read")
{
    if (!File.Exists(CSVPath))
    {
        Console.WriteLine("CSV file not found.");
        return;
    }

    using (var reader = new StreamReader(CSVPath))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture)){
        var cheeps = csv.GetRecords<Cheep>().ToList();
        foreach (var cheep in cheeps){
            string formatted = DateTimeOffset
                .FromUnixTimeSeconds(cheep.Timestamp)
                .ToString("MM-dd HH:mm:ss");

            Console.WriteLine($"{cheep.Author} @ {formatted}: {cheep.Observation}");
        }
    }
}
else if (args[0] == "write")
{
    if (args.Length < 2)
    {
        Console.WriteLine("Please provide an observation to write.");
        return;
    }

    var newCheep = new Cheep(Environment.UserName, args[1], DateTimeOffset.UtcNow.ToUnixTimeSeconds());
    bool fileExists = File.Exists(CSVPath);

    using (var writer = new StreamWriter(CSVPath, append: true))
    using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
        if (!fileExists)
        {
            csv.WriteHeader<Cheep>();
            csv.NextRecord();
        }

        csv.WriteRecord(newCheep);
        csv.NextRecord();
    }

    Console.WriteLine("Observation saved.");
}

public record Cheep(string Author, string Observation, long Timestamp);
