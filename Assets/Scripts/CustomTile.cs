using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomTile : TileBase
{
    [SerializeField]
    private Sprite _sprite;

    public override void GetTileData(
        Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = _sprite;
    }
}
