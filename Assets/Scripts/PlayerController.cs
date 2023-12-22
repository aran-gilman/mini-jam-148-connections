using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    // TODO: We will probably want to convert this to a property to support
    // callbacks when this changes, but leave it as a public variable for now
    // to better support testing via the inspector.
    public GameObject CurrentPlaceable;

    private Camera _mainCamera;
    private Vector3 _currentPointerPosition;

    private void OnPlaceStructure()
    {
        if (CurrentPlaceable != null)
        {
            Vector3 worldPos = _mainCamera.ScreenToWorldPoint(_currentPointerPosition);
            worldPos.z = 0;
            Instantiate(CurrentPlaceable, worldPos, Quaternion.identity);
        }
    }

    private void OnPointWorld(InputValue value)
    {
        _currentPointerPosition = value.Get<Vector2>();
    }

    private void Awake()
    {
        // We cache this to avoid the cost of looking up the camera every time.
        _mainCamera = Camera.main;
    }
}
