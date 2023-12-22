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

    private Collider2D _rangeCollider;

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
}
