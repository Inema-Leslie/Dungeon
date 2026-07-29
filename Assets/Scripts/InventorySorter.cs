using System.Collections.Generic;

public static class InventorySorter
{
    public static List<string> SortAlphabetically(List<string> items)
    {
        List<string> sorted = new List<string>(items);

        for (int i = 0; i < sorted.Count - 1; i++)
        {
            for (int j = 0; j < sorted.Count - 1 - i; j++)
            {
                if (string.Compare(sorted[j], sorted[j + 1], System.StringComparison.OrdinalIgnoreCase) > 0)
                {
                    (sorted[j], sorted[j + 1]) = (sorted[j + 1], sorted[j]);
                }
            }
        }

        return sorted;
    }
}