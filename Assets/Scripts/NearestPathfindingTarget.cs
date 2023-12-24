using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NearestPathfindingTarget : IPathfindingTarget
{
    private IEnumerable<Transform> _targets;

    public NearestPathfindingTarget(IEnumerable<Transform> targets)
    {
        _targets = targets;
    }

    public Vector3 NearestPosition(Vector3 point)
    {
        if (_targets.Count() == 0)
        {
            return point;
        }    

        return _targets
            .OrderBy(t => (t.position - point).sqrMagnitude)
            .First()
            .position;
    }
}