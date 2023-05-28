namespace SpringChallenge2023;
using System;
using System.Collections.Generic;

public class GenericComparer<T, U> : IComparer<T>
{
    private readonly Func<T, U>[] selectors;

    public GenericComparer(params Func<T, U>[] selectors)
    {
        this.selectors = selectors;
    }

    public int Compare(T x, T y)
    {
        foreach (var selector in selectors)
        {
            int comparisonResult = Comparer<U>.Default.Compare(selector(x), selector(y));
            if (comparisonResult != 0)
            {
                return comparisonResult;
            }
        }

        return 0;
    }
}
