using CsvHelper;
using System.Globalization;

namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    private readonly string CSVPath;
    public CSVDatabase(string CSVPath)
    {
        this.CSVPath = CSVPath;
    }
    public IEnumerable<T> Read(int? limit = null)
    {
        if (!File.Exists(CSVPath))
        {
            Console.WriteLine("CSV file not found.");
            return Enumerable.Empty<T>();
        }
        using (var reader = new StreamReader(CSVPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>().ToList();
            return records;
        }
    }
    public void Store(T record)
    {
        bool fileExists = File.Exists(CSVPath);

        using (var writer = new StreamWriter(CSVPath, append: true))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture)){
            if (!fileExists)
            {
                csv.WriteHeader<T>();
                csv.NextRecord();
            }
            csv.WriteRecord(record);
            csv.NextRecord();
        }

    Console.WriteLine("Input saved.");
    }
    
    public IEnumerable<T> discussion(int id)
    {
        if (!File.Exists(CSVPath))
        {
            Console.WriteLine("CSV file not found.");
            return Enumerable.Empty<T>();
        }
        using (var reader = new StreamReader(CSVPath))
        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            var records = csv.GetRecords<T>().ToList();
            records = records.Where(r => (int)r.GetType().GetProperty("id").GetValue(r) == id).ToList();
            return records;
        }
    }
}
