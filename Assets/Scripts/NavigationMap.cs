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
        foreach (Vector3Int neighbor in _neighborVectors)
        {
            CustomTile tile = _terrain.GetTile<CustomTile>(cellPos + neighbor);
            if (tile != null && tile.IsWalkable)
            {
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }
}
