using UnityEngine;

public class Placeable : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Size of the placeable in tiles.")]
    private Vector2Int _size;
    public Vector2Int Size => _size;

    [SerializeField]
    [Tooltip("Which tile is the pivot, with (0, 0) to right and up from the center.")]
    private Vector2Int _pivot;
    public Vector2Int Pivot => _pivot;

    public Vector3 GetLocalPivotPosition(Vector3 cellSize)
    {
        Vector3 pivotPosition = new Vector3(
            cellSize.x * _pivot.x,
            cellSize.y * _pivot.y);
        return pivotPosition + cellSize / 2;
    }
}
