using System.Collections.Generic;
using UnityEngine;

public class NearestPathfindingTarget : IPathfindingTarget
{
    private IEnumerable<Transform> _targets;

    public NearestPathfindingTarget(IEnumerable<Transform> targets)
    {
        _targets = targets;
    }
}