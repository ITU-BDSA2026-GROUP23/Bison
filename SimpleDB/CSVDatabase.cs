namespace SimpleDB;

public sealed class CSVDatabase<T> : IDatabaseRepository<T>
{
    public IEnumerable<T> Read(int? limit = null)
    {
        return Enumerable.Empty<T>();
    }
    public void Store(T record)
    {
        
    }
    
}
