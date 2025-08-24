using UnityEngine;
using UnityEngine.UI;

public static class GridLayoutGroupExtentions
{
    /// <summary>
    /// Get the index of a column a given child is in.
    /// </summary>
    public static int ColumnIndexOfChild(this GridLayoutGroup group, int childIndex)
    {
        if (group.constraint != GridLayoutGroup.Constraint.FixedColumnCount)
        {
            Debug.LogError(
                "Calculating horizontal offset for flexible column grids currently not supported.",
                group
            );

            return 0;
        }
        int columnIndex = childIndex % group.constraintCount;
        return columnIndex;
    }
}
