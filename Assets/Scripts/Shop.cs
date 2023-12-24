using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private List<ShopEntry> _entries = new List<ShopEntry>();

    [SerializeField]
    private TextMeshProUGUI MoneyText;

    [SerializeField]
    PlayerController PlayerController;

    [SerializeField]
    int _startingMoney;

    [SerializeField]
    private HoverInfoDisplay _infoDisplay;

    [SerializeField]
    Transform _battlefield;

    private int currentSelection = -1;

    private void Awake()
    {
        for(int i = 0; i < _entries.Count; i++)
        {
            _entries[i].PriceTag.text = _entries[i].Cost.ToString();
        }

        Global.Money = _startingMoney;
    }

    private void Update()
    {
        MoneyText.text = Global.Money.ToString();
    }

    public void SelectItem(int buttonID)
    {
        ShopEntry selectedItem = _entries[buttonID];

        if(PlayerController.GetCurrentPlaceable() == selectedItem.Structure)
        {
            SwitchSelection(-1);
            return;
        }
        
        SwitchSelection(buttonID);
    }

    public void BuyItem(Vector3 position)
    {
        int cost = _entries[currentSelection].Cost;
        if (Global.Money < cost)
        {
            return;
        }
        Global.Money -= cost;
        Instantiate(
            _entries[currentSelection].Structure,
            position,
            Quaternion.identity,
            _battlefield);

    }

    public void SwitchSelection(int ID)
    {
        if (currentSelection != -1)
        {
            _entries[currentSelection].RegularButton.SetActive(true);
        }

        if (ID == -1)
        {
            PlayerController.SetCurrentPlaceable(null);
            _infoDisplay.InfoSource = null;
        }
        else
        {
            PlayerController.SetCurrentPlaceable(_entries[ID].Structure);
            _entries[ID].RegularButton.SetActive(false);
            _infoDisplay.InfoSource =
                _entries[ID].Structure.GetComponentInChildren<IHoverInfo>();
        }

        currentSelection = ID;
    }    


}
