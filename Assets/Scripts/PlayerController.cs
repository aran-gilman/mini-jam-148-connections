using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Camera _mainCamera;
    private Vector3 _currentPointerPosition;

    private void OnPlaceStructure()
    {
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(_currentPointerPosition);
        worldPos.z = 0;
        Debug.Log($"OnPlaceStructure() called at position {worldPos}");
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
