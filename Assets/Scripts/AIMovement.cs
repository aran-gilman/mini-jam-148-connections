using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1.0f;

    public Vector3 TargetPosition { get; set; }

    private Rigidbody2D _rb;

    private IPathfinder _pathfinder;
    private Vector3 _nextNode;

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

    private void Awake()
    {
        _pathfinder = new NaivePathfinder();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        TargetPosition = FindTarget();
        _pathfinder.CalculatePath(transform.position, TargetPosition);
        _nextNode = _pathfinder.PopNextNode();
        _rb.velocity = (_nextNode - transform.position).normalized * _moveSpeed;
    }

    private void Update()
    {
        if (VectorApproximately(transform.position, TargetPosition))
        {
            return;
        }

        if (VectorApproximately(transform.position, _nextNode))
        {
            _nextNode = _pathfinder.PopNextNode();
            _rb.velocity = (_nextNode - transform.position).normalized * _moveSpeed;
        }
    }

    // TODO: Move this to a utility function file
    private static bool VectorApproximately(Vector3 a, Vector3 b)
    {
        return Mathf.Approximately(a.x, b.x)
            && Mathf.Approximately(a.y, b.y)
            && Mathf.Approximately(a.z, b.z);
    }
}
