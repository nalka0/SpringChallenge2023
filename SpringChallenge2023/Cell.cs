namespace SpringChallenge2023;
public class Cell
{
    private readonly Dictionary<Cell, PathCache> cachedPaths;

    public Cell MirroredCell { get; private set; }

    public int? DistanceToEnnemy { get; set; }

    public int? DistanceToMe { get; set; }

    public int Index { get; set; }

    public CellBase Base { get; set; }

    public int FriendlyAnts { get; set; }

    public int EnnemyAnts { get; set; }

    public int Resources { get; set; }

    public CellResourceType ResourceType { get; set; }

    public List<Neighbour> Neighbours { get; init; }

    public Cell()
    {
        cachedPaths = new()
        {
            { this, new(0) }
        };
    }

    public PathCache ComputePathsToCell(Cell cell)
    {
        if (cachedPaths.TryGetValue(cell, out PathCache ret))
        {
            return ret;
        }

        List<Neighbour> tested = Neighbours;
        Dictionary<int, List<Neighbour>> neighboursByDistance = new();
        int distance = 1;
        while (true)
        {
            if (tested.Any(x => x.Cell == cell))
            {
                ret = new PathCache(distance);
                ret.Paths.Add(new() { cell });
                BuildPath(distance - 1, ret, neighboursByDistance, cell);
                ret.Paths.ForEach(x => x.Add(this));
                break;
            }

            neighboursByDistance.Add(distance, tested);
            tested = tested.SelectMany(t => t.Cell.Neighbours.Where(n => neighboursByDistance.Values.All(x => x.All(x => x.Cell != n.Cell)))).ToList();
            distance++;
        }

        cachedPaths.TryAdd(cell, ret);
        cell.cachedPaths.TryAdd(this, ret.Reverse());
        if (cell != MirroredCell)
        {
            var mirroredRet = ret.Mirror();
            MirroredCell.cachedPaths.TryAdd(cell.MirroredCell, mirroredRet);
            cell.MirroredCell.cachedPaths.TryAdd(MirroredCell, mirroredRet.Reverse());
        }

        return ret;
    }

    public void BuildPath(int distance, PathCache cache, Dictionary<int, List<Neighbour>> neighboursByDistance, Cell root)
    {
        if (distance > 0)
        {
            List<Neighbour> interestingNeighbours = root.Neighbours.Where(x => neighboursByDistance[distance].Any(y => y.Cell == x.Cell)).ToList();
            for (int i = 0; i < cache.Paths.Count; i++)
            {
                if (cache.Paths[i][^1] == root && interestingNeighbours.Count > 0)
                {
                    int indexChanges = 0;
                    var removed = cache.Paths[i];
                    cache.Paths.Remove(removed);
                    indexChanges--;
                    foreach (Neighbour neighbour in interestingNeighbours)
                    {
                        cache.Paths.Insert(0, new List<Cell>(removed)
                        {
                            neighbour.Cell
                        });
                        indexChanges++;
                        BuildPath(distance - 1, cache, neighboursByDistance, neighbour.Cell);
                    }
                    i += indexChanges;
                }
            }
        }
    }

    public void ComputeNeighbourhood()
    {
        foreach (Neighbour neighbour in Neighbours)
        {
            neighbour.Cell = Board.Cells[neighbour.CellIndex];
            PathCache cache = new(1);
            cache.Paths.Add(new()
            {
                neighbour.Cell,
                this
            });
            cachedPaths.TryAdd(neighbour.Cell, cache);
        }
    }

    public void ComputeMirroredCell()
    {
        if (Board.IsSplitOnTile)
        {
            if (Index != 0)
            {
                MirroredCell = Board.Cells[Index + (Index % 2 == 0 ? -1 : 1)];
            }
            else
            {
                MirroredCell = this;
            }
        }
        else
        {
            MirroredCell = Board.Cells[Index + (Index % 2 == 0 ? 1 : -1)];
        }
    }

    public override string ToString()
    {
        return $"Cell {Index}";
    }
}
