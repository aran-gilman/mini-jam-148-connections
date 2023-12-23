using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1.0f;

    [SerializeField]
    [Tooltip("How close the object needs to be for the algorithm to consider it as having reached the target position.")]
    private float _targetReachedTolerance = 0.1f;

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
        if (DidReachTarget(
            transform.position, TargetPosition, _targetReachedTolerance))
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        if (DidReachTarget(transform.position, _nextNode, _targetReachedTolerance))
        {
            _nextNode = _pathfinder.PopNextNode();
            _rb.velocity = (_nextNode - transform.position).normalized * _moveSpeed;
        }
    }

    private static bool DidReachTarget(Vector3 a, Vector3 b, float epsilon)
    {
        // Even though we take Vector3 as input, this is a 2D game, so we can
        // ignore the z
        return Mathf.Abs(a.x - b.x) < epsilon
            && Mathf.Abs(a.y - b.y) < epsilon;
    }
}
