namespace SpringChallenge2023;

public class Neighbour
{
    public Cell Cell { get; set; }

    public int CellIndex { get; init; }

    public int NeighbourhoodIndex { get; init; }

    public override string ToString()
    {
        return Cell.ToString();
    }
}
