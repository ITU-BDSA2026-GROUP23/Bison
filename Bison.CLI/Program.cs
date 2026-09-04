using CsvHelper;

//Refactor this to use CsvHelper instead of manually parsing the CSV file:
string CSVPath = "./bison_observe_cli_db.csv";

using (var reader = new StreamReader(CSVPath))
using (var csv = new CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
{
    csv.Read();
    csv.ReadHeader();
    while (csv.Read())
    {
        var author = csv.GetField(0);
        var observation = csv.GetField(1).Replace("\"", "");
        var timestamp = csv.GetField(2);

        Console.WriteLine($"{author} @ {timestamp}: {observation}");
    }
}
