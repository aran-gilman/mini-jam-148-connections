using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1.0f;

    [SerializeField]
    [Tooltip("How close the object needs to be for the algorithm to consider it as having reached the target position.")]
    private float _targetReachedTolerance = 0.1f;

    public Vector3 TargetPosition
    {
        get => _targetPosition;
        set
        {
            _targetPosition = value;
            _pathfinder.CalculatePath(transform.position, TargetPosition);
            _nextNode = _pathfinder.PopNextNode();
            _rb.velocity = (_nextNode - transform.position).normalized * _moveSpeed;
        }
    }

    private Rigidbody2D _rb;

    private IPathfinder _pathfinder;
    private Vector3 _nextNode;
    private Vector3 _targetPosition;

    private void Awake()
    {
        _pathfinder = new AStarPathfinder(FindObjectOfType<NavigationMap>());
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        _rb.velocity = Vector3.zero;
        _nextNode = transform.position;
        _targetPosition = transform.position;
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
