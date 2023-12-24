using UnityEngine;

/// <summary>
/// Returns the hover info of the specified object. Useful for UIs that
/// reference prefabs.
/// </summary>
public class PassThroughHoverInfo : MonoBehaviour, IHoverInfo
{
    // We can't directly store IHoverInfo without a fair amount of custom
    // inspector code, so instead store a game object and retrieve the
    // component from it.
    // This is also a bit more flexible in that we can put IHoverInfo on a
    // child object.
    [SerializeField]
    private GameObject _referenceObject;

    public string DisplayName
    {
        get
        {
            if (_referenceObjectInfo != null)
            {
                return _referenceObjectInfo.DisplayName;
            }
            return "";
        }
    }

    public string Description
    {
        get
        {
            if (_referenceObjectInfo != null)
            {
                return _referenceObjectInfo.Description;
            }
            return "";
        }
    }

    private IHoverInfo _referenceObjectInfo;

    private void Awake()
    {
        _referenceObjectInfo = _referenceObject.GetComponentInChildren<IHoverInfo>();
    }
}
