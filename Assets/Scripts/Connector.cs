using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class Connector : MonoBehaviour
{
    [SerializeField]
    private int _maxConnections = 2;

    [SerializeField]
    private float _connectionRange = 1.0f;

    [SerializeField]
    [Tooltip("Child object spawned on awake that displays the connector's range and provides a collider for checking proximity to other connectors.")]
    private GameObject _rangePrefab;

    [SerializeField]
    [Tooltip("Child object spawned on awake that displays a line between connected objects.")]
    private GameObject _linePrefab;

    private Collider2D _rangeCollider;
    private Dictionary<Connector, LineRenderer> _connectedObjects = new Dictionary<Connector, LineRenderer>();

    private ContactFilter2D _connectorFilter;

    /// <summary>
    /// Whether this Connector can still make more connections.
    /// </summary>
    public bool CanConnect()
    {
        return _connectedObjects.Count < _maxConnections;
    }

    /// <summary>
    /// Connect this Connector to <paramref name="other"/> (and vice versa).
    /// 
    /// The only check performed by this method is whether the objects are
    /// already connected to each other. Any additional checks for validity
    /// must be done by the caller before calling this method.
    /// </summary>
    public void Connect(Connector other)
    {
        if (_connectedObjects.TryAdd(other, null) &&
            other._connectedObjects.TryAdd(this, null))
        {
            LineRenderer line = Instantiate(
                _linePrefab, transform.position, Quaternion.identity)
                .GetComponent<LineRenderer>();
            _connectedObjects[other] = line;
            other._connectedObjects[this] = line;

            line.useWorldSpace = true;
            line.SetPosition(0, transform.position);
            line.SetPosition(line.positionCount - 1, other.transform.position);
        }
    }

    /// <summary>
    /// Disconnects this Connector from <paramref name="other"/> (and vice versa).
    /// </summary>
    public void Disconnect(Connector other)
    {
        if (_connectedObjects.TryGetValue(other, out LineRenderer line))
        {
            _connectedObjects.Remove(other);
            other._connectedObjects.Remove(this);
            if (line != null)
            {
                Destroy(line.gameObject);
            }
        }
    }

    private void Awake()
    {
        Assert.IsNotNull(_rangePrefab, "RangePrefab must be non-null.");
        GameObject go = Instantiate(_rangePrefab, transform);
        _rangeCollider = go.GetComponent<Collider2D>();
        Assert.IsNotNull(_rangeCollider, "RangePrefab must have a Collider2D component.");

        // The range is a radius and the scale is effectively the diameter,
        // so set the scale to double the range.
        go.transform.localScale = new Vector3(
            _connectionRange * 2.0f, _connectionRange * 2.0f, 1.0f);

        // TODO: Restrict filtering to just connector layer
        _connectorFilter = _connectorFilter.NoFilter();
    }

    private void OnEnable()
    {
        List<Collider2D> neighbors = new List<Collider2D>();
        _rangeCollider.OverlapCollider(_connectorFilter, neighbors);

        IEnumerable<Connector> connections = neighbors
            .Select(c => c.GetComponentInParent<Connector>())
            .Where(c => c.CanConnect())
            .OrderBy(c => (transform.position - c.transform.position).sqrMagnitude)
            .Take(_maxConnections);

        foreach (Connector connector in connections)
        {
            Connect(connector);
        }
    }

    private void OnDisable()
    {
        List<Connector> connections = new List<Connector>(_connectedObjects.Keys);
        foreach (Connector other in connections)
        {
            Disconnect(other);
        }
    }
}
