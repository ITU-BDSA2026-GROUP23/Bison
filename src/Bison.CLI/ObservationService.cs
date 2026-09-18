using SimpleDB;

public class ObservationService
{
    private readonly CSVDatabase<ObserveCheep> dbObserve;
    public ObservationService(CSVDatabase<ObserveCheep> dbObserve)
    {
        this.dbObserve = dbObserve;
    }
    public IEnumerable<ObserveCheep> GetByLocation(string location) =>
        dbObserve.Read().Where(o => o.Location == location);
}
    