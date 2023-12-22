using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // TODO: We will probably want to convert this to a property to support
    // callbacks when this changes, but leave it as a public variable for now
    // to better support testing via the inspector.
    public GameObject CurrentPlaceable;

    [SerializeField]
    private Grid _placementGrid;

    [SerializeField]
    private GameObject _positionPreviewPrefab;

    private Camera _mainCamera;
    private Vector3 _currentPointerPosition;

    private GameObject _positionPreviewObject;

    private void OnPlaceStructure()
    {
        if (CurrentPlaceable != null)
        {
            Vector3 offset = Vector3.zero;
            if (CurrentPlaceable.TryGetComponent(out Placeable placeable))
            {
                offset = placeable.GetLocalPivotPosition(_placementGrid.cellSize);
            }
            Instantiate(
                CurrentPlaceable,
                _currentPointerPosition - offset,
                Quaternion.identity);
        }
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
}
