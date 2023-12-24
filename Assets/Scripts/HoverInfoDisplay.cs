using TMPro;
using UnityEngine;

public class HoverInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _nameArea;

    [SerializeField]
    private TMP_Text _descriptionArea;

    private IHoverInfo _infoSource;
    public IHoverInfo InfoSource
    {
        get => _infoSource;
        set
        {
            _infoSource = value;
            if (_infoSource != null)
            {
                _nameArea.text = _infoSource.DisplayName;
                _descriptionArea.text = _infoSource.Description;
            }
            else
            {
                _nameArea.text = "";
                _descriptionArea.text = "";
            }
        }
    }
}
