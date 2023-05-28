namespace SpringChallenge2023;
using static CodingameTools.CodingameHelper;
public class FriendlyPlayer : Player
{
    private readonly OutputManager outputManager;

    private List<Cell> targets;

    private Cell eggTarget;

    private bool harvestEggs;

    public FriendlyPlayer(OutputManager outputManager)
    {
        this.outputManager = outputManager;
        targets = new();
        harvestEggs = true;
    }

    public void Play()
    {
        if (eggTarget == null)
        {
            if (harvestEggs)
            {
                eggTarget = Board.CellsWithEggs.OrderBy(x => x.DistanceToMe).ThenByDescending(x => x.Resources).ThenBy(x => x.Index).First();
            }
        }
        else if (eggTarget.Resources == 0)
        {
            eggTarget = null;
            harvestEggs = false;
            Debug("No longer harvesting eggs");
        }

        IEnumerable<Cell> interestingCells = null;
        if (targets.Count == 0 || targets[0].Resources == 0)
        {
            interestingCells = Board.CellsWithCrystals.OrderBy(x => x.DistanceToEnnemy - x.DistanceToMe, new DistanceDifferenceComparer()).ThenByDescending(x => x.Resources).ThenBy(x => x.DistanceToMe).ThenBy(x => x.Index);
            targets = interestingCells.ToList();
        }

        var optimalPath = Base.ComputePathsToCell(eggTarget ?? targets[0]).Paths.MaxBy(x => x.Sum(x => x.Resources)).ExpandPath();
        int maxAnts = optimalPath.Where(x => x.ResourceType != CellResourceType.None).Max(x => x.Resources);
        foreach (Cell cell in optimalPath)
        {
            outputManager.Beacon(cell, maxAnts);
        }

        if (!harvestEggs && (maxAnts < (double)TotalAnts / optimalPath.Count || TotalAnts % optimalPath.Count > 0))
        {
            int extraAnts = TotalAnts - maxAnts * optimalPath.Count;
            if (extraAnts < 0)
            {
                extraAnts = TotalAnts % optimalPath.Count;
            }

            var secondaryPaths = targets.Where(x => !optimalPath.Contains(x) && x.ResourceType != CellResourceType.None).Select(Base.ComputePathsToCell).FirstOrDefault(x => x.ShortestPath <= extraAnts);
            if (secondaryPaths != null)
            {
                var optimalSecondaryPath = secondaryPaths.Paths.MaxBy(x => x, new GenericComparer<List<Cell>, int>(x => x.Sum(x => x.Resources), x => x.Sum(x => x.FriendlyAnts))).ExpandPath();
                foreach (Cell cell in optimalSecondaryPath)
                {
                    outputManager.Beacon(cell, extraAnts / optimalSecondaryPath.Count);
                }
            }
        }
    }
}