using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private void OnPlaceStructure()
    {
        Debug.Log("OnPlaceStructure() called");
    }
}
