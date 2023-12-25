using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves in a straight line toward the target.
/// </summary>
public class NaivePathfinder : IPathfinder
{
    public Stack<Vector3> CalculatePath(Vector3 current, IPathfindingTarget target)
    {
        Stack<Vector3> path = new Stack<Vector3>();
        path.Push(target.NearestPosition(current));
        return path;
    }
}
