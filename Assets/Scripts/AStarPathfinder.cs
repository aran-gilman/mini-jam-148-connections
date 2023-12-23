using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : IPathfinder
{
    private Stack<Vector3> _path;
    private Vector3 _target;

    public void CalculatePath(Vector3 current, Vector3 target)
    {
        _path = new Stack<Vector3>();
        _path.Push(target);
    }

    public Vector3 PopNextNode()
    {
        if (_path.TryPop(out Vector3 node))
        {
            return node;
        }
        return _target;
    }
}
