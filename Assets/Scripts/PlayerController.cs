using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // TODO: We will probably want to convert this to a property to support
    // callbacks when this changes, but leave it as a public variable for now
    // to better support testing via the inspector.
    private GameObject CurrentPlaceable;

    [SerializeField]
    private Grid _placementGrid;

    [SerializeField]
    private GameObject _positionPreviewPrefab;

    [SerializeField]
    private LayerMask _connectorLayer;

    private Camera _mainCamera;
    private Vector3 _currentPointerPosition;

    private GameObject _positionPreviewObject;

    [SerializeField]
    private Shop _shop;

    private void OnPlaceStructure()
    {
        if (CurrentPlaceable != null)
        {
            Vector3 position = _currentPointerPosition;
            if (CurrentPlaceable.TryGetComponent(out Placeable placeable))
            {
                position -= placeable.GetLocalPivotPosition(_placementGrid.cellSize);

                // Allow placement only if there is a Connector nearby.
                if (Connector
                    .GetAvailableConnectors(position, _connectorLayer)
                    .Count() == 0)
                {
                    return;
                }
            }
            Instantiate(
                CurrentPlaceable,
                position,
                Quaternion.identity);
            _shop.BuyItem();
        }
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
        _positionPreviewObject.transform.position = _currentPointerPosition;
    }

    private void Awake()
    {
        // We cache this to avoid the cost of looking up the camera every time.
        _mainCamera = Camera.main;
        _positionPreviewObject = Instantiate(_positionPreviewPrefab, transform);
        _positionPreviewObject.transform.localScale = _placementGrid.cellSize;
    }

    private Vector3 SnapToGrid(Vector3 worldPosition)
    {
        return _placementGrid.CellToWorld(
            _placementGrid.WorldToCell(worldPosition + _placementGrid.cellSize / 2));
    }

    public void SetCurrentPlaceable(GameObject newPlaceable)
    {
        CurrentPlaceable = newPlaceable;
    }

    public GameObject GetCurrentPlaceable()
    {
        return CurrentPlaceable;
    }
}
