using UnityEngine;

/// <summary>
/// Moves in a straight line toward the target.
/// </summary>
public class NaivePathfinder : IPathfinder
{
    Vector3 _target = Vector3.zero;

    public void CalculatePath(Vector3 current, Vector3 target)
    {
        _target = target;
    }

    public Vector3 PopNextNode()
    {
        return _target;
    }
}
