using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HoverInfo : MonoBehaviour
{
    [SerializeField]
    private string _displayName;
    public string DisplayName => _displayName;

    [SerializeField]
    [TextArea]
    private string _description;
    public string Description => _description;
}
