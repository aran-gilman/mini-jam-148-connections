using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Grid _placementGrid;

    [SerializeField]
    private GameObject _positionPreviewPrefab;

    [SerializeField]
    private LayerMask _connectorLayer;

    [SerializeField]
    private Tilemap _terrain;

    [SerializeField]
    private Shop _shop;

    [SerializeField]
    private Transform _battlefield;

    [SerializeField]
    private WinAndLose _winAndLose;

    [SerializeField]
    private HoverInfoDisplay _hoverInfoDisplay;

    [SerializeField]
    private LayerMask _hoverInfoLayer;

    private Camera _mainCamera;

    private Vector3 _currentPointerPosition;
    private Vector3 _positionOffset;
    private GameObject _previewObject;
    private SpriteRenderer _previewRenderer;
    private GameObject _currentPlaceable;

    private void OnPlaceStructure()
    {
        if (_currentPlaceable == null)
        {
            return;
        }

        Vector3 position = _currentPointerPosition - _positionOffset;

        // Allow placement only if there is a Connector nearby.
        if (Connector
            .GetAvailableConnectors(position, _connectorLayer)
            .Count() == 0)
        {
            return;
        }

        if (_currentPlaceable.TryGetComponent(out Structure structure)
            && IsBlocked(structure))
        {
            return;
        }

        Instantiate(
            _currentPlaceable,
            position,
            Quaternion.identity,
            _battlefield);
        _shop.BuyItem();
    }

    private void OnCancelSelection()
    {
        _shop.SwitchSelection(-1);
    }

    private void OnPointWorld(InputValue value)
    {
        Vector3 screenPos = value.Get<Vector2>();
        _currentPointerPosition = _mainCamera.ScreenToWorldPoint(screenPos);
        _currentPointerPosition.z = 0;
        _currentPointerPosition = SnapToGrid(_currentPointerPosition);
        _previewObject.transform.position = _currentPointerPosition - _positionOffset;
    }

    private void OnContinueOrRestart()
    {
        _winAndLose.EndScene();
    }

    private bool IsBlocked(Structure placeable)
    {
        Vector3Int pivotCell = _placementGrid.WorldToCell(_currentPointerPosition);
        IEnumerable<Vector3Int> containedCells = placeable.GetContainedCells(pivotCell);

        // First check whether placeable is blocked by the terrain.
        foreach (Vector3Int placementCell in containedCells)
        {
            // For now, assume that the placement grid cells are
            // smaller than or equal in size to the terrain grid cells.
            Vector3 worldPos = _placementGrid.CellToWorld(placementCell);
            Vector3Int terrainCell = _terrain.layoutGrid.WorldToCell(worldPos);
            CustomTile tile = _terrain.GetTile<CustomTile>(terrainCell);
            if (tile == null || !tile.IsWalkable)
            {
                return true;
            }
        }

        // If not blocked by the terrain, check for existing structures.
        foreach (Structure other in FindObjectsOfType<Structure>())
        {
            Vector3Int otherPivot =
                _placementGrid.WorldToCell(other.transform.position);
            IEnumerable<Vector3Int> otherCells =
                other.GetContainedCells(otherPivot);
            if (otherCells.Intersect(containedCells).Count() > 0)
            {
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        // We cache this to avoid the cost of looking up the camera every time.
        _mainCamera = Camera.main;
        _previewObject = Instantiate(_positionPreviewPrefab, transform);
        _previewRenderer = _previewObject.GetComponent<SpriteRenderer>();
    }

    private Vector3 SnapToGrid(Vector3 worldPosition)
    {
        return _placementGrid.CellToWorld(
            _placementGrid.WorldToCell(worldPosition + _placementGrid.cellSize / 2));
    }

    public void SetCurrentPlaceable(GameObject newPlaceable)
    {
        _currentPlaceable = newPlaceable;
        if (_currentPlaceable != null
            && _currentPlaceable.TryGetComponent(out SpriteRenderer placeableSprite))
        {
            _previewObject.SetActive(true);
            _previewRenderer.sprite = placeableSprite.sprite;

            if (_currentPlaceable.TryGetComponent(out Structure structure))
            {
                _positionOffset =
                    structure.GetLocalPivotPosition(_placementGrid.cellSize);
            }
            else
            {
                _positionOffset = Vector3.zero;
            }
        }
        else
        {
            _previewObject.SetActive(false);
        }
    }

    public GameObject GetCurrentPlaceable()
    {
        return _currentPlaceable;
    }
}
