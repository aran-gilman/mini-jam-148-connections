using TMPro;
using UnityEngine;

public class HoverInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _nameArea;

    [SerializeField]
    private TMP_Text _descriptionArea;

    public void SetInfo(IHoverInfo infoSource)
    {
        _nameArea.text = infoSource.DisplayName;
        _descriptionArea.text = infoSource.Description;
    }
}
