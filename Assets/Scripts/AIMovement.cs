using System.Linq;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Vector3 TargetPosition { get; set; }

    // TODO: Move target selection to a separate class.
    // This class should only concern itself with getting the object from one
    // point to another.
    private Vector3 FindTarget()
    {
        Transform target =
            FindObjectsByType<Health>(FindObjectsSortMode.None)
            .Select(t => t.GetComponentInParent<FactionAlignment>())
            .Where(t => t != null && t.Faction == FactionAlignment.EFaction.Player)
            .Select(t => t.transform)
            .OrderBy(t => (transform.position - transform.position).sqrMagnitude)
            .FirstOrDefault();

        if (target == null)
        {
            return transform.position;
        }
        return target.position;
    }

    private void OnEnable()
    {
        TargetPosition = FindTarget();
    }
}
