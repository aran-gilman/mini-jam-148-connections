using System;
using UnityEngine;

/// <summary>
/// Component that allows components on other objects to listen for trigger
/// events on this object.
/// </summary>
public class TriggerCallback : MonoBehaviour
{
    // C# events are used instead of UnityEvents because callbacks set up via
    // inspector tend to be more difficult to trace/debug.
    public event EventHandler<Collider2D> TriggerEntered;
    public event EventHandler<Collider2D> TriggerExited;

    private void OnTriggerEnter2D(Collider2D other)
    {
        TriggerEntered?.Invoke(this, other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        TriggerExited?.Invoke(this, other);
    }
}
