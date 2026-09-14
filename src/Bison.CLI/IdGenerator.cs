public static class IdGenerator
{
public static int NextObserveId(string path)
    {
        if (!File.Exists(path))
        {
            return 1;
        }
        return File.ReadAllLines(path).Length;
    }
}