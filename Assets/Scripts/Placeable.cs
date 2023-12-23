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
        float pivotX = cellSize.x * _pivot.x;
        if (_size.x % 2 == 0)
        {
            pivotX += cellSize.x / 2;
        }

        float pivotY = cellSize.y * _pivot.y;
        if (_size.y % 2 == 0)
        {
            pivotY += cellSize.y / 2;
        }

        return new Vector3(pivotX, pivotY);
    }
}
