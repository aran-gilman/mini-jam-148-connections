using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    [SerializeField]
    private int _maxConnections = 2;

    [SerializeField]
    private float _connectionRange = 1.0f;

    [SerializeField]
    [Tooltip("Child object spawned on start that displays the connector's range and provides a collider for checking proximity to other connectors.")]
    private GameObject _rangePrefab;
}
