namespace SpringChallenge2023;

public static class Game
{
    public static int TurnsLeft { get; set; }

    public static void Main(string[] args)
    {
        OutputManager outputManager = new();
        Player.Ennemy = new List<EnnemyPlayer>();
        Player.Me = new List<FriendlyPlayer>();
        TurnsLeft = 100;
        string[] inputs;
        int numberOfCells = int.Parse(Console.ReadLine());
        for (int i = 0; i < numberOfCells; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            List<Neighbour> neighbours = new();
            for (int j = 2; j < inputs.Length; j++)
            {
                int cellIndex = int.Parse(inputs[j]);
                if (cellIndex != -1)
                {
                    neighbours.Add(new Neighbour
                    {
                        CellIndex = cellIndex,
                        NeighbourhoodIndex = j - 2
                    });
                }
            }
            Cell newCell = new()
            {
                ResourceType = (CellResourceType)int.Parse(inputs[0]),
                Neighbours = neighbours,
                Index = i
            };
            Board.Cells.Add(newCell);
            if (newCell.ResourceType != CellResourceType.None)
            {
                Board.CellsWithResources.Add(newCell);
                (newCell.ResourceType switch
                {
                    CellResourceType.Egg => Board.CellsWithEggs,
                    CellResourceType.Crystal => Board.CellsWithCrystals,
                    _ => throw new InvalidOperationException("Shouldn't try to add a non-egg/crystal cell to caches")
                }).Add(newCell);
            }
        }

        Board.IsSplitOnTile = Board.Cells.Count % 2 != 0;
        foreach (Cell cell in Board.Cells)
        {
            cell.ComputeMirroredCell();
            cell.ComputeNeighbourhood();
        }

        int numberOfBases = int.Parse(Console.ReadLine());
        inputs = Console.ReadLine().Split(' ');
        for (int i = 0; i < numberOfBases; i++)
        {
            Cell cell = Board.Cells[int.Parse(inputs[i])];
            cell.Base = CellBase.Friendly;
            Player.Me.Add(new FriendlyPlayer(outputManager) { Base = cell });
        }

        inputs = Console.ReadLine().Split(' ');
        for (int i = 0; i < numberOfBases; i++)
        {
            Cell cell = Board.Cells[int.Parse(inputs[i])];
            cell.Base = CellBase.Ennemy;
            Player.Ennemy.Add(new EnnemyPlayer { Base = cell });
        }

        foreach (Cell cell in Board.CellsWithResources)
        {
            foreach (Cell cell2 in Board.Cells.Skip((int)(Board.Cells.Count * 0.9)))
            {
                cell.ComputePathsToCell(cell2);
            }
        }

        while (true)
        {
            Player.Me.ForEach(x => x.TotalAnts = 0);
            Player.Ennemy.ForEach(x => x.TotalAnts = 0);
            foreach (Cell cell in Board.Cells)
            {
                inputs = Console.ReadLine().Split(' ');
                CodingameTools.CodingameHelper.Stopwatch.Start();
                if (cell.ResourceType != CellResourceType.None)
                {
                    cell.Resources = int.Parse(inputs[0]);
                    if (cell.Resources == 0)
                    {
                        (cell.ResourceType switch
                        {
                            CellResourceType.Egg => Board.CellsWithEggs,
                            CellResourceType.Crystal => Board.CellsWithCrystals,
                            _ => throw new InvalidOperationException("Shouldn't try to remove a non-egg/crystal cell from caches")
                        }).Remove(cell);
                        cell.ResourceType = CellResourceType.None;
                        Board.CellsWithResources.Remove(cell);
                    }
                }

                cell.FriendlyAnts = int.Parse(inputs[1]);
                Player.Me.ForEach(x => x.TotalAnts += cell.FriendlyAnts / Player.Me.Count);
                cell.EnnemyAnts = int.Parse(inputs[2]);
                Player.Ennemy.ForEach(x => x.TotalAnts += cell.EnnemyAnts / Player.Ennemy.Count);
            }

            foreach (Cell resourceCell in Board.CellsWithResources)
            {
                resourceCell.DistanceToMe = Board.Cells.Where(x => x.FriendlyAnts > 0).Min(x => x.ComputePathsToCell(resourceCell).ShortestPath);
                resourceCell.DistanceToEnnemy = Board.Cells.Where(x => x.EnnemyAnts > 0).Min(x => x.ComputePathsToCell(resourceCell).ShortestPath);
            }

            Player.Me.ForEach(x => x.Play());
            outputManager.SendOutput();
            TurnsLeft--;
        }
    }
}