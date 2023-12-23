using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Representation of the game world for navigation purposes.
/// </summary>
/// 
/// <remarks>
/// The current implementation is a grid rather than a typical navigation mesh.
/// </remarks>
public class NavigationMap : MonoBehaviour
{
    [SerializeField]
    private Tilemap _terrain;

    private static readonly List<Vector3Int> _neighborVectors =
        new List<Vector3Int>()
        { 
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.right,
            Vector3Int.left,
        };

    public List<Vector3> GetNeighbors(Vector3 position)
    {
        Vector3Int cellPos = _terrain.layoutGrid.WorldToCell(position);
        List<Vector3> neighbors = new List<Vector3>();
        foreach (Vector3Int relativePos in _neighborVectors)
        {
            Vector3Int neighborPos = cellPos + relativePos;
            CustomTile tile = _terrain.GetTile<CustomTile>(neighborPos);
            if (tile != null && tile.IsWalkable)
            {
                neighbors.Add(CellToWorld(neighborPos));
            }
        }
        return neighbors;
    }

    public Vector3 GetNearestNode(Vector3 position)
    {
        return CellToWorld(_terrain.layoutGrid.WorldToCell(position));
    }

    /// <summary>
    /// Whether <paramref name="start"/> and <paramref name="target"/> are
    /// close enough that the route between them does not pass through
    /// additional nodes.
    /// </summary>
    public bool HasDirectRoute(Vector3 start, Vector3 target)
    {
        Vector2 cellSizeXY = new Vector2(
            _terrain.layoutGrid.cellSize.x,
            _terrain.layoutGrid.cellSize.y);
        float nodeSqrDistance = cellSizeXY.sqrMagnitude;
        float startTargetSqrDistance = (target - start).sqrMagnitude;
        return startTargetSqrDistance < nodeSqrDistance;
    }

    private Vector3 CellToWorld(Vector3Int cellPos)
    {
        // Get the coordinates for the center of the tile
        Vector3 pos =
            _terrain.layoutGrid.CellToWorld(cellPos);
        pos += _terrain.layoutGrid.cellSize / 2;
        pos.z = 0;
        return pos;
    }
}
