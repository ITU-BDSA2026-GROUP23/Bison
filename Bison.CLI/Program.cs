using CsvHelper;

//Refactor this to use CsvHelper instead of manually parsing the CSV file:
string CSVPath = "./bison_observe_cli_db.csv";
StreamReader reader = new StreamReader(CSVPath);
reader.ReadLine(); // Skip the header line
while(!reader.EndOfStream)
{
    string line = reader.ReadLine();
    string author = line.Split(',')[0]; 
    string observation = line.Split(',')[1].Replace("\"", ""); 
    string timestamp = line.Split(',')[2];  

    Console.WriteLine($"{author} @ {timestamp}: {observation}");
}
