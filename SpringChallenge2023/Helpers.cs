namespace SpringChallenge2023;

public static class Helpers
{
    public static List<Cell> ExpandPath(this List<Cell> path)
    {
        HashSet<Cell> expansion = new();
        List<Cell> pathEnds = new(path);
        List<Cell> expanders;
        do
        {
            expanders = pathEnds.SelectMany(x => x.Neighbours.Where(x => x.Cell.ResourceType != CellResourceType.None && !path.Contains(x.Cell) && !expansion.Contains(x.Cell)).Select(x => x.Cell)).Distinct().ToList();
            pathEnds.Clear();
            foreach (var expander in expanders)
            {
                expansion.Add(expander);
                pathEnds.Add(expander);
            }
        }
        while (expanders.Count > 0);
        if (expansion.Count > 0)
        {
            return new(expansion.Distinct().Concat(path));
        }
        else
        {
            return path;
        }
    }
}