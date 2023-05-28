namespace SpringChallenge2023;
using static CodingameTools.CodingameHelper;

public class OutputManager
{
    private readonly HashSet<Beacon> beacons;

    public OutputManager()
    {
        beacons = new();
    }

    public void SendOutput()
    {
        if (beacons.Count > 0)
        {
            Output(string.Join("; ", beacons.Select(x => $"BEACON {x.CellIndex} {(int)x.Strength}")));
            beacons.Clear();
        }
        else
        {
            Output("WAIT");
        }
    }

    [Obsolete("Not adapted to multi-base and not optimal in game")]
    public static void Line(Cell source, Cell destination, int strength)
    {
        Output($"LINE {source.Index} {destination.Index} {strength}");
    }

    public void Beacon(Cell cell, double strength)
    {
        if (strength > 0)
        {
            if (beacons.FirstOrDefault(x => x.CellIndex == cell.Index) is Beacon existing)
            {
                existing.Strength = Math.Max(existing.Strength, strength);
            }
            else
            {
                beacons.Add(new()
                {
                    CellIndex = cell.Index,
                    Strength = strength
                });
            }
        }
    }
}
