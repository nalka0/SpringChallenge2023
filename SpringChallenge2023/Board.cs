namespace SpringChallenge2023;

public static class Board
{
    public static readonly List<Cell> Cells = new();

    public static readonly List<Cell> CellsWithResources = new();

    public static readonly List<Cell> CellsWithEggs = new();

    public static readonly List<Cell> CellsWithCrystals = new();

    public static bool IsSplitOnTile { get; set; }
}
