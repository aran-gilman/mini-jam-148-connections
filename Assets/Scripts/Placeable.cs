using UnityEngine;

public class Placeable : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Size of the placeable in tiles.")]
    private Vector2Int _size;
    public Vector2Int Size => _size;

    [SerializeField]
    [Tooltip("Which tile is the pivot, with (0, 0) being the bottom left.")]
    private Vector2Int _pivot;
    public Vector2Int Pivot => _pivot;
}
