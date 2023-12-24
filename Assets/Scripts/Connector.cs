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
    [Tooltip("Connectors that are neither sources nor transitively connected to a source will collapse.")]
    private bool _isSource = false;

    [SerializeField]
    [Tooltip("Child object spawned on awake that displays the connector's range and provides a collider for checking proximity to other connectors.")]
    private GameObject _rangePrefab;

    [SerializeField]
    [Tooltip("Child object spawned on awake that displays a line between connected objects.")]
    private GameObject _linePrefab;

    [SerializeField]
    [Tooltip("Layer(s) to check for other connector colliders.")]
    private LayerMask _connectorLayers;

    // LineRenderers do not appear to respect the sorting order/sorting layers
    // for at least some other types of renderers (e.g. TilemapRenderer). The
    // easiest workaround is to assign them different z-coordinates.
    private static readonly Vector3 _linePositionOffset =
        new Vector3(0.0f, 0.0f, 0.1f);

    private Collider2D _rangeCollider;
    private Dictionary<Connector, LineRenderer> _connectedObjects =
        new Dictionary<Connector, LineRenderer>();

    public static IEnumerable<Connector> GetAvailableConnectors(
        Vector3 position,
        LayerMask connectorLayers)
    {
        return Physics2D.OverlapPointAll(position, connectorLayers.value)
            .Select(c => c.GetComponentInParent<Connector>())
            .Where(c => c != null && c.CanConnect());
    }

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
            line.SetPosition(0, transform.position + _linePositionOffset);
            line.SetPosition(
                line.positionCount - 1, other.transform.position + _linePositionOffset);
        }
    }

    /// <summary>
    /// Disconnects this Connector from <paramref name="other"/> (and vice versa).
    /// </summary>
    /// 
    /// <remarks>
    /// Does not call VerifyConnection() on either Connector; this must be called
    /// separately.
    /// </remarks>
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

    /// <summary>
    /// Verifies that the Connector connects transitively to a source.
    /// 
    /// If it is not connected, then this Connector and all connected
    /// Connectors are destroyed.
    /// </summary>
    public void VerifyConnection()
    {
        HashSet<Connector> openNodes = new HashSet<Connector>
        {
            this
        };
        HashSet<Connector> visited = new HashSet<Connector>();
        if (!IsConnectedToSource(openNodes, visited))
        {
            foreach (Connector connector in visited)
            {
                Destroy(connector.gameObject);
            }
        }
    }

    private static bool IsConnectedToSource(
        HashSet<Connector> openNodes,
        HashSet<Connector> visited)
    {
        // Grab a node from the list of nodes to check, if one exists.
        Connector node = openNodes.FirstOrDefault();

        // If there are no nodes left to check, then we never found a source.
        if (node == null)
        {
            return false;
        }

        openNodes.Remove(node);
        visited.Add(node);

        if (node._isSource)
        {
            return true;
        }

        // Add all connections that have not been visited yet to the set of
        // nodes to check.
        openNodes.UnionWith(
            node._connectedObjects
            .Select(n => n.Key)
            .Where(n => !visited.Contains(n)));

        return IsConnectedToSource(openNodes, visited);
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
    }

    private void OnEnable()
    {
        List<Collider2D> neighbors = new List<Collider2D>(
            Physics2D.OverlapPointAll(
                transform.position,
                _connectorLayers.value));

        IEnumerable<Connector> connections =
            GetAvailableConnectors(transform.position, _connectorLayers)
            .Where(c => c != this)
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

        foreach (Connector other in connections)
        {
            if (other != null)
            {
                other.VerifyConnection();
            }
        }
    }
}
