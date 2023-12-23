using UnityEngine;

public interface IPathfinder
{
    /// <summary>
    /// Determine a series of straight-line movements that can take the object
    /// from <paramref name="current"/> to <paramref name="target"/>
    /// </summary>
    void CalculatePath(Vector3 current, Vector3 target);

    /// <summary>
    /// Returns the next node and pops it from the path node stack.
    /// </summary>
    Vector3 PopNextNode();
}
