using System.Collections.Generic;
using UnityEngine;

public interface IPathfinder
{
    /// <summary>
    /// Determine a series of straight-line movements that can take the object
    /// from <paramref name="current"/> to <paramref name="target"/>
    /// </summary>
    /// 
    /// <returns>
    /// The path to the target.
    /// Will be empty if target is unreachable.
    /// </returns>
    Stack<Vector3> CalculatePath(Vector3 current, IPathfindingTarget target);
}
