using CsvHelper;

//Refactor this to use CsvHelper instead of manually parsing the CSV file:
string CSVPath = "./bison_observe_cli_db.csv";
string input = "";
while (input != "exit") {
    input = Console.ReadLine()?.ToLowerInvariant() ?? "";
    if (input == "read")
    {
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

                string formatted = DateTimeOffset.FromUnixTimeSeconds(long.Parse(timestamp)).ToString("MM-dd HH:mm:ss");

                Console.WriteLine($"{author} @ {formatted}: {observation}");
            }
        }
    }
    else if (input == "write")
    {
        string author = Environment.UserName;
        Console.WriteLine("Enter observation:");
        string observation = Console.ReadLine() ?? "";
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        //bool fileExists = File.Exists(CSVPath);
        using (var writer = new StreamWriter(CSVPath, append: true))
        using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.InvariantCulture))
        {
            csv.WriteField(author);
            csv.WriteField(observation);
            csv.WriteField(timestamp);
            csv.NextRecord();
        }

        Console.WriteLine("Observation saved.");
    }
}