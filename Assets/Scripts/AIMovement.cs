using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AIMovement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 1.0f;

    [SerializeField]
    [Tooltip("How close the object needs to be for the algorithm to consider it as having reached the target position.")]
    private float _targetReachedTolerance = 0.1f;

    private enum EMovementType
    {
        Walking,
        Flying
    }
    [SerializeField]
    private EMovementType _movementType;

    public Vector3 NextNode => _nextNode;

    private Rigidbody2D _rb;

    private IPathfinder _pathfinder;
    private Vector3 _nextNode;
    private bool _hasReachedTarget;
    private Stack<Vector3> _path = new Stack<Vector3>();

    public void SetTarget(IPathfindingTarget target)
    {
        if (target == null)
        {
            _path.Clear();
            _hasReachedTarget = true;
        }
        else
        {
            _path = _pathfinder.CalculatePath(transform.position, target);
            _hasReachedTarget = !_path.TryPop(out _nextNode);
        }
    }

    public bool HasTarget()
    {
        return !_hasReachedTarget;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        switch (_movementType)
        {
            case EMovementType.Walking:
                _pathfinder = new AStarPathfinder(FindObjectOfType<NavigationMap>());
                break;
            case EMovementType.Flying:
                _pathfinder = new NaivePathfinder();
                break;
        }
        _hasReachedTarget = true;
    }

    private void OnDisable()
    {
        _rb.velocity = Vector3.zero;
        _nextNode = transform.position;
    }

    private void Update()
    {
        if (!HasTarget())
        {
            _rb.velocity = Vector3.zero;
            return;
        }

        if (DidReachTarget(transform.position, _nextNode, _targetReachedTolerance))
        {
            if (!_path.TryPop(out _nextNode))
            {
                _nextNode = transform.position;
                _hasReachedTarget = true;
            }
        }

        Vector3 diff = _nextNode - transform.position;
        diff.z = 0;
        _rb.velocity = (diff).normalized * _moveSpeed;
    }

    private static bool DidReachTarget(Vector3 a, Vector3 b, float epsilon)
    {
        // Even though we take Vector3 as input, this is a 2D game, so we can
        // ignore the z
        return Mathf.Abs(a.x - b.x) < epsilon
            && Mathf.Abs(a.y - b.y) < epsilon;
    }
}
