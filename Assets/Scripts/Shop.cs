using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private List<ShopEntry> _entries = new List<ShopEntry>();

    private int _money = 1000;
    [SerializeField]
    private TextMeshProUGUI MoneyText;

    [SerializeField]
    PlayerController PlayerController;

    private int currentSelection = -1;

    private void Awake()
    {
        for(int i = 0; i < _entries.Count; i++)
        {
            _entries[i].PriceTag.text = _entries[i].Cost.ToString();
        }

        MoneyText.text = _money.ToString();
    }

    public void SelectItem(int buttonID)
    {
        ShopEntry selectedItem = _entries[buttonID];

        if(PlayerController.GetCurrentPlaceable() == selectedItem.Structure)
        {
            SwitchSelection(-1);
            return;
        }

        if(_money >= selectedItem.Cost)
        {
            SwitchSelection(buttonID);
        }
    }

    public void BuyItem()
    {
        int cost = _entries[currentSelection].Cost;
        _money -= cost;
        MoneyText.text = _money.ToString();
        if(_money < cost)
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
        }
        else
        {
            PlayerController.SetCurrentPlaceable(_entries[ID].Structure);
            _entries[ID].RegularButton.SetActive(false);
        }

        currentSelection = ID;
    }    


}
