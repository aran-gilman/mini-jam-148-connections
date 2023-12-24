using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Structure : MonoBehaviour
{
    public delegate void StructureEventHandler(Structure newStructure);
    public static event StructureEventHandler StructureAdded;

    [SerializeField]
    [Tooltip("Size of the placeable in tiles.")]
    private Vector2Int _size;
    public Vector2Int Size => _size;

    [SerializeField]
    [Tooltip("Which tile is the pivot, with (0, 0) to right and up from the center.")]
    private Vector2Int _pivot;
    public Vector2Int Pivot => _pivot;

    [SerializeField]
    private List<TileBase> _disallowedTerrain;
    public List<TileBase> DisallowedTerrain => _disallowedTerrain;

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

    public IEnumerable<Vector3Int> GetContainedCells(Vector3Int pivotCell)
    {
        List<Vector3Int> containedCells = new List<Vector3Int>();
        for (int y = 0; y < _size.y; ++y)
        {
            for (int x = 0; x < _size.x; ++x)
            {
                containedCells.Add(
                    new Vector3Int(x + _pivot.x, y + _pivot.y) + pivotCell);
            }
        }
        return containedCells;
    }

    private void OnEnable()
    {
        StructureAdded?.Invoke(this);
    }
}
