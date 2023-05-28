namespace SpringChallenge2023;

public class DistanceDifferenceComparer : IComparer<int?>
{
    public int Compare(int? x, int? y)
    {
        if (x == y)
        {
            return 0;
        }
        else if (x == null || y == null)
        {
            throw new InvalidOperationException("Shouldn't have to compare nulls");
        }
        else if (x == 0)
        {
            return -1;
        }
        else if (y == 0)
        {
            return 1;
        }
        else if (x > 0)
        {
            if (y >= 0)
            {
                return x.Value.CompareTo(y);
            }
            else
            {
                return -1;
            }
        }
        else
        {
            if (y <= 0)
            {
                return (int)(y - x);
            }
            else
            {
                return 1;
            }
        }
    }
}
