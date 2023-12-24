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

        if(Global.Money >= selectedItem.Cost)
        {
            SwitchSelection(buttonID);
        }
    }

    public void BuyItem()
    {
        int cost = _entries[currentSelection].Cost;
        Global.Money -= cost;
        if(Global.Money < cost)
        {
            SwitchSelection(-1);
        }
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
