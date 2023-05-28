namespace SpringChallenge2023;

public class PathCache
{
    public List<List<Cell>> Paths { get; set; }

    public int ShortestPath { get; set; }

    public PathCache(int shortestPath)
    {
        Paths = new();
        ShortestPath = shortestPath;
    }

    public PathCache Mirror()
    {
        PathCache ret = new(ShortestPath);
        foreach (List<Cell> path in Paths)
        {
            ret.Paths.Add(new(path.Select(x => x.MirroredCell)));
        }
        return ret;
    }

    public PathCache Reverse()
    {
        PathCache ret = new(ShortestPath);
        foreach (List<Cell> path in Paths)
        {
            ret.Paths.Add(new(Enumerable.Reverse(path)));
        }
        return ret;
    }
}
