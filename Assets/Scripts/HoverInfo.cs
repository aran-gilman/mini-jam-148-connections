using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HoverInfo : MonoBehaviour
{
    [SerializeField]
    [TextArea]
    private string _description;
    public string Description => _description;
}
