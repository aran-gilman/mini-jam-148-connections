using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private Vector3 _currentPointerPosition;

    private void OnPlaceStructure()
    {
        Debug.Log($"OnPlaceStructure() called at position {_currentPointerPosition}");
    }

    private void OnPointWorld(InputValue value)
    {
        _currentPointerPosition = value.Get<Vector2>();
    }
}
